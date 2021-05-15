ass BrainFrame
        {
            public int id;
            public CinemachineBlend blend = new CinemachineBlend(null, null, null, 0, 0);
            public bool Active { get { return blend.IsValid; } }

            // Working data - updated every frame
            public CinemachineBlend workingBlend = new CinemachineBlend(null, null, null, 0, 0);
            public BlendSourceVirtualCamera workingBlendSource = new BlendSourceVirtualCamera(null);

            // Used by Timeline Preview for overriding the current value of deltaTime
            public float deltaTimeOverride;
        }

        // Current game state is always frame 0, overrides are subsequent frames
        private List<BrainFrame> mFrameStack = new List<BrainFrame>();
        private int mNextFrameId = 1;

        /// Get the frame index corresponding to the ID
        private int GetBrainFrame(int withId)
        {
            int count = mFrameStack.Count;
            for (int i = mFrameStack.Count - 1; i > 0; --i)
                if (mFrameStack[i].id == withId)
                    return i;
            // Not found - add it
            mFrameStack.Add(new BrainFrame() { id = withId });
            return mFrameStack.Count - 1;
        }

        // Current Brain State - result of all frames.  Blend camB is "current" camera always
        CinemachineBlend mCurrentLiveCameras = new CinemachineBlend(null, null, null, 0, 0);
        
        // To avoid GC memory alloc every frame
        private static readonly AnimationCurve mDefaultLinearAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        /// <summary>
        /// This API is specifically for Timeline.  Do not use it.
        /// Override the current camera and current blend.  This setting will trump
        /// any in-game logic that sets virtual camera priorities and Enabled states.
        /// This is the main API for the timeline.
        /// </summary>
        /// <param name="overrideId">Id to represent a specific client.  An internal
        /// stack is maintained, with the most recent non-empty override taking precenence.
        /// This id must be > 0.  If you pass -1, a new id will be created, and returned.
        /// Use that id for subsequent calls.  Don't forget to
        /// call ReleaseCameraOverride after all overriding is finished, to
        /// free the OverideStack resources.</param>
        /// <param name="camA"> The camera to set, corresponding to weight=0</param>
        /// <param name="camB"> The camera