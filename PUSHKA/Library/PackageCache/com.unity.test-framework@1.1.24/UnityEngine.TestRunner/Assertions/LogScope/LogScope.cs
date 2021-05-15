d for a camera override client.
        /// See SetCameraOverride.
        /// </summary>
        /// <param name="overrideId">The ID to released.  This is the value that
        /// was returned by SetCameraOverride</param>
        public void ReleaseCameraOverride(int overrideId)
        {
            for (int i = mFrameStack.Count - 1; i > 0; --i)
            {
                if (mFrameStack[i].id == overrideId)
                {
                    mFrameStack.RemoveAt(i);
                    return;
                }
            }
        }

        ICinemachineCamera mActiveCameraPreviousFrame;
        private void ProcessActiveCamera(float deltaTime)
        {
            var activeCamera = ActiveVirtualCamera;
            if (activeCamera != null)
            {
                // Has the current camera changed this frame?
                if (activeCamera != mActiveCameraPreviousFrame)
                {
                    // Notify incoming camera of transition
                    activeCamera.OnTransitionFromCamera(
                        mActiveCameraPreviousFrame, DefaultWorldUp, deltaTime);
                    if (m_CameraActivatedEvent != null)
                        m_CameraActivatedEvent.Invoke(activeCamera, mActiveCameraPreviousFrame);

                    // If we're cutting without a blend, send an event
                    if (!IsBlending || (mActiveCameraPreviousFrame != null
                                && !ActiveBlend.Uses(mActiveCameraPreviousFrame)))
                    {
                        if (m_CameraCutEvent != null)
                            m_CameraCutEvent.Invoke(this);
                        if (CinemachineCore.CameraCutEvent != null)
                            CinemachineCore.CameraCutEvent.Invoke(this);
                    }
                    // Re-update in case it's inactive
                    activeCamera.UpdateCameraState(DefaultWorldUp, deltaTime);
                }
                // Apply the vcam state to the Unity camera
                PushStateToUnityCamera(
                    SoloCamera != null ? SoloCamera.State : mCurrentLiveCameras.State);
            }
            mActiveCameraPreviousFrame = activeCamera;
        }

        private void UpdateFrame0(float deltaTime)
        {
            // Update the in-game frame (frame 0)
            BrainFrame frame = mFrameStack[0];

            // Are we transitioning cameras?
            var activeCamera = TopCameraFromPriorityQueue();
            var outGoingCamera = frame.blend.CamB;
            if (activeCamera != outGoingCamera)
            {
                // Do we need to create a game-play blend?
                if ((UnityEngine.Object)activeCamera != null
                    && (UnityEngine.Object)outGoingCamera != null && deltaTime >= 0)
                {
                    // Create a blend (curve will be null if a cut)
                    var blendDef = LookupBlend(outGoingCamera, activeCamera);
                    if (blendDef.BlendCurve != null && blendDef.BlendTime > 0)
                    {
                        if (frame.blend.IsComplete)
                            frame.blend.CamA = outGoingCamera;  // new blend
                        else
                        {
                            // Special case: if backing out of a blend-in-progress
                            // with the same blend in reverse, adjust the belnd time
                            if (frame.blend.CamA == activeCamera
                                && frame.blend.CamB == outGoingCamera
                                && frame.blend.Duration <= blendDef.BlendTime)
                            {
                                blendDef.m_Time = frame.blend.TimeInBlend;
                            }

                            // Chain to existing blend
                            frame.blend.CamA = new BlendSourceVirtualCamera(
                                new CinemachineBlend(
                                    frame.blend.CamA, frame.blend.CamB,
                                    frame.blend.BlendCurve, frame.blend.Duration,
                                    frame.blend.TimeInBlend));
                        }
                    }
                    frame.blend.BlendCurve = blendDef.BlendCurve;
                    frame.blend.Duration = blendDef.BlendTime;
                    frame.blend.TimeInBlend = 0;
                }
                // Set the current active camera
                frame.blend.CamB = activeCamera;
            }

            // Advance the current blend (if any)
            if (frame.blend.CamA != null)
            {
                frame.blend.TimeInBlend += (deltaTime >= 0) ? deltaTime : frame.blend.Duration;
                if (frame.blend.IsComplete)
                {
                    // No more blend
                    frame.blend.CamA = null;
                    frame.blend.BlendCurve = null;
                    frame.blend.Duration = 0;
                    frame.blend.TimeInBlend = 0;
                }
            }
        }

        /// <summary>
        /// Used internally to compute the currrent blend, taking into account
        /// the in-game camera and all the active overrides.  Caller may optionally
        /// exclude n topmost overrides.
        /// </summary>
        /// <param name="outputBlend">Receives the nested blend</param>
        /// <param name="numTopLayersToExclude">Optionaly exclude the last number 
        /// of overrides from the blend</param>
        public void ComputeCurrentBlend(
            ref CinemachineBlend outputBlend, int numTopLayersToExclude)
        {
            // Resolve the current working frame states in the stack
            int lastActive = 0;
            int topLayer = Mathf.Max(1, mFrameStack.Count - numTopLayersToExclude);
            for (int i = 0; i < topLayer; ++i)
            {
                BrainFrame frame = mFrameStack[i];
                if (i == 0 || frame.Active)
                {
                    frame.workingBlend.CamA = frame.blend.CamA;
                    frame.workingBlend.CamB = frame.blend.CamB;
                    frame.workingBlend.BlendCurve = frame.blend.BlendCurve;
                    frame.workingBlend.Duration = frame.blend.Duration;
                    frame.workingBlend.TimeInBlend = frame.blend.TimeInBlend;
                    if (i > 0 && !frame.blend.IsComplete)
                    {
                        if (frame.workingBlend.CamA == null)
                        {
                            if (mFrameStack[lastActive].blend.IsComplete)
                                frame.workingBlend.CamA = mFrameStack[lastActive].blend.CamB;
                            else
                            {
                                frame.workingBlendSource.Blend = mFrameStack[lastActive].workingBlend;
                                frame.workingBlend.CamA = frame.workingBlendSource;
                            }
                        }
                        else if (frame.workingBlend.CamB == null)
                        {
                            if (mFrameStack[lastActive].blend.IsComplete)
                                frame.workingBlend.CamB = mFrameStack[lastActive].blend.CamB;
         