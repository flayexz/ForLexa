ght be different from the
        /// virtual camera that owns the collider, in the event that the camera has children</param>
        /// <returns>True if the virtual camera has been displaced due to collision or
        /// target obstruction</returns>
        public float GetCameraDisplacementDistance(ICinemachineCamera vcam)
        {
            return GetExtraState<VcamExtraState>(vcam).colliderDisplacement;
        }
        
        private void OnValidate()
        {
            m_DistanceLimit = Mathf.Max(0, m_DistanceLimit);
            m_MinimumOcclusionTime = Mathf.Max(0, m_MinimumOcclusionTime);
            m_CameraRadius = Mathf.Max(0, m_CameraRadius);
            m_MinimumDistanceFromTarget = Mathf.Max(0.01f, m_MinimumDistanceFromTarget);
            m_OptimalTargetDistance = Mathf.Max(0, m_OptimalTargetDistance);
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        protected override void OnDestroy()
        {
            DestroyCollider();
            base.OnDestroy();
        }

        /// This must be small but greater than 0 - reduces false results due to precision
        const float PrecisionSlush = 0.001f;

        /// <summary>
        /// Per-vcam extra state info
     