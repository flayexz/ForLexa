/ <summary>
        /// Callback to do the collision resolution and shot evaluation
        /// </summary>
        /// <param name="vcam">The virtual camera being processed</param>
        /// <param name="stage">The current pipeline stage</param>
        /// <param name="state">The current virtual camera state</param>
        /// <param name="deltaTime">The current applicable deltaTime</param>
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            VcamExtraState extra = null;
            if (stage == CinemachineCore.Stage.Body)
            {
                extra = GetExtraState<VcamExtraState>(vcam);
                extra.targetObscured = false;
                extra.colliderDisplacement = 0;
                if (extra.debugResolutionPath != null)
                    extra.debugResolutionPath.RemoveRange(0, extra.debugResolutionPath.Count);
            }

            // Move the body before the Aim is calculated
            if (stage == CinemachineCore.Stage.Body)
            {
                if (m_AvoidObstacles)
                {
                    Vector3 displacement = Vector3.zero;
                    displacement = PreserveLignOfSight(ref state, ref extra);
                    if (m_MinimumOcclusionTime > Epsilon)
                    {
                        float now = CinemachineCore.CurrentTime;
                        if (displacement.sqrMagnitude < Epsilon)
                            extra.occlusionStartTime = 0;
                        else
                        {
                            if (extra.occlusionStartTime <= 0)
                                extra.occlusionStartTime = now;
                            if (now - extra.occlusionStartTime < m_MinimumOcclusionTime)
                                displacement = extra.m_previousDisplacement;
                        }
                    }

                    // Apply distance smoothing
                    if (m_SmoothingTime > Epsilon)
                    {
                        Vector3 pos = state.CorrectedPosition + displacement;
                        Vector3 dir = pos - state.ReferenceLookAt;
                        float distance = dir.magnitude;
                        if (distance > Epsilon)
                        {
                            dir /= distance;
                            if (!displacement.AlmostZero())
                                extra.UpdateDistanceSmoothing(distance, m_SmoothingTime);
                            distance = extra.ApplyDistanceSmoothing(distance, m_SmoothingTime);
                            displacement += (state.ReferenceLookAt + dir * distance) - pos;
                        }
                    }

                    float damping = m_Damping;
                    if (displacement.AlmostZero())
                        extra.ResetDistanceSmoothing(m_SmoothingTime);
                    else
                        damping = m_DampingWhenOccluded;
                    if (damping > 0 && deltaTime >= 0 && VirtualCamera.PreviousStateIsValid)
                    {
                        Vector3 delta = displacement - extra.m_previousDisplacement;
                        delta = Damper.Damp(delta, damping, deltaTime);
                        displacement = extra.m_previousDisplacement + delta;
                    }
                    extra.m_previousDisplacement = displacement;
                    Vector3 correction = RespectCameraRadius(state.CorrectedPosition + displacement, ref state);
                    if (damping > 0 && deltaTime >= 0 && VirtualCamera.PreviousStateIsValid)
                    {
                        Vector3 delta = correction - extra.m_previousDisplacementCorrection;
                        delta = Damper.Damp(delta, damping, deltaTime);
                        correction = extra.m_previousDisplacementCorrection + delta;
                    }
                    displacement += correction;
                    extra.m_previousDisplacementCorrection = correction;
                    state.PositionCorrection += displacement;
                    extra.colliderDisplacement += displacement.magnitude;
                }
            }
            // Rate the shot after the aim was set
            if (stage == CinemachineCore.Stage.Aim)
            {
                extra = GetExtraState<VcamExtraState>(vcam);
                extra.targetObscured = IsTargetOffscreen(state) || CheckForTargetObstructions(state);

                // GML these values are an initial arbitrary attempt at rating quality
                if (extra.targetObscured)
                    state.ShotQuality *= 0.2f;
                if (extra.colliderDisplacement > 0)
                    state.ShotQuality *= 0.8f;

                float nearnessBoost = 0;
                const float kMaxNearBoost = 0.2f;
                if (m_OptimalTargetDistance > 0 && state.HasLookAt)
                {
                    float distance = Vector3.Magnitude(state.R