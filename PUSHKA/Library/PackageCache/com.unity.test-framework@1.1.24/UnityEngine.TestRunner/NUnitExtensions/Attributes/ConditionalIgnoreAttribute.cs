parents are live.
        /// </summary>
        /// <param name="vcam">The camera to test whether it is live</param>
        /// <param name="dominantChildOnly">If truw, will only return true if this vcam is the dominat live child</param>
        /// <returns>True if the camera is live (directly or indirectly)
        /// or part of a blend in progress.</returns>
        public bool IsLive(ICinemachineCamera vcam, bool dominantChildOnly = false)
        {
            if (SoloCamera == vcam)
                return true;
            if (mCurrentLiveCameras.Uses(vcam))
                return true;

            ICinemachineCamera parent = vcam.ParentCamera;
            while (parent != null && parent.IsLiveChild(vcam, dominantChildOnly))
            {
                if (SoloCamera == parent || mCurrentLiveCameras.Uses(parent))
                    return true;
                vcam = parent;
                parent = vcam.ParentCamera;
            }
            return false;
        }

        /// <summary>
        /// The current state applied to the unity camera (may be the result of a blend)
        /// </summary>
        public CameraState CurrentCameraState { get; private set; }

        /// <summary>
        /// Get the highest-priority Enabled ICinemachineCamera
        /// that is visible to my camera.  Culling Mask is used to test visibility.
        /// </summary>
        private ICinemachineCamera TopCameraFromPriorityQueue()
        {
            CinemachineCore core = CinemachineCore.Instance;
            Camera outputCamera = OutputCamera;
            int mask = outputCamera == null ? ~0 : outputCamera.cullingMask;
            int numCameras = core.VirtualCameraCount;
            for (int i = 0; i < numCameras; ++i)
            {
                var cam = core.GetVirtualCamera(i);
                GameObject go = cam != null ? cam.gameObject : null;
                if (go != null && (mask & (1 << go.layer)) != 0)
                    return cam;
            }
            return null;
        }

        /// <summary>
        /// Create a blend curve for blending from one ICinemachineCamera to another.
        /// If there is a specific blend defined for these cameras it will be used, otherwise
        /// a default blend will be created, which could be a cut.
        /// </summary>
        private CinemachineBlendDefinition LookupBlend(
            ICinemachineCamera fromKey, ICinemachineCamera toKey)
        {
            // Get the blend curve that's most appropriate for these cameras
            CinemachineBlendDefinition blend = m_DefaultBlend;
            if (m_CustomBlends != null)
            {
                string fromCameraName = (fromKey != null) ? fromKey.Name : string.Empty;
                string toCameraName = (toKey != null) ? toKey.Name : string.Empty;
                blend = m_CustomBlends.GetBlendForVirtualCameras(
                        fromCameraName, toCameraName, blend);
            }
            if (CinemachineCore.GetBlendOverride != null)
                blend = CinemachineCore.GetBlendOverride(fromKey, toKey, blend, this);
            return blend;
        }

        /// <summary> Apply a cref="CameraState"/> to the game object</summary>
        private void PushStateToUnityCamera(CameraState state)
        {
            CurrentCameraState = state;
            if ((state.BlendHint & CameraState.BlendHintValue.NoPosition) =