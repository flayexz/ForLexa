ull && m_Window.treeView != null)
                m_Window.treeView.CalculateRowRects();
        }

        // Only one track within a 'track' hierarchy can be armed
        public void ArmForRecord(TrackAsset track)
        {
            m_ArmedTracks[TimelineUtility.GetSceneReferenceTrack(track)] = track;
            if (track != null && !recording)
                recording = true;
            if (!recording)
                return;

            track.OnRecordingArmed(editSequence.director);
            CalculateRowRects();
        }

        public void UnarmForRecord(TrackAsset track)
        {
            m_ArmedTracks.Remove(TimelineUtility.GetSceneReferenceTrack(track));
            if (m_ArmedTracks.Count == 0)
                recording = false;
            track.OnRecordingUnarmed(editSequence.director);
        }

        public void UpdateRecordingState()
        {
            if (recording)
            {
                foreach (var track in m_ArmedTracks.Values)
                {
                    if (track != null)
                        track.OnRecordingTimeChanged(editSequence.director);
                }
            }
        }

        public bool IsTrackRecordable(TrackAsset track)
        {
            // A track with animated parameters can always be recorded to
            return IsArmedForRecord(track) || track.HasAnyAnimatableParameters();
        }

        public bool IsArmedForRecord(TrackAsset track)
        {
            return track == GetArmedTrack(track);
        }

        public TrackAsset GetArmedTrack(TrackAsset track)
        {
            TrackAsset outTrack;
            m_ArmedTracks.TryGetValue(TimelineUtility.GetSceneReferenceTrack(track), out outTrack);
            return outTrack;
        }

        void CheckRecordingState()
        {
            // checks for deleted tracks, and makes sure the recording state matches
            if (m_ArmedTracks.Any(t => t.Value == null))
            {
                m_ArmedTracks = m_ArmedTracks.Where(t => t.Value != null).ToDictionary(t => t.Key, t => t.Value);
                if (m_ArmedTracks.Count == 0)
                    recording = false;
            }
        }

        void OnCurrentDirectorWillChange()
        {
            if (ignorePreview)
                return;

            SynchronizeViewModelTime(editSequence);
            Stop();
            rebuildGraph = true; // needed for asset previews
        }

        public void GatherProperties(PlayableDirector director)
        {
            if (director == null || Application.isPlaying)
                return;

            var asset = director.playableAsset as TimelineAsset;
            if (asset != null && !asset.editorSettings.scenePreview)
                return;

            if (!previewMode)
            {
                AnimationMode.StartAnimationMode(previewDriver);

                OnStartPreview(director);

                AnimationPropertyContextualMenu.Instance.SetResponder(new TimelineRecordingContextualResponder(this));
                if (!previewMode)
                    return;
                EnsureWindowTimeConsistency();
            }

            if (asset != null)
            {
                m_PropertyCollector.Reset();
                m_PropertyCollector.PushActiveGameObject(null); // avoid overflow on unbound tracks
                asset.GatherProperties(director, m_PropertyCollector);
            }
        }

        void OnStartPreview(PlayableDirector director)
        {
            previewedDirectors = TimelineUtility.GetAllDirectorsInHierarchy(director).ToList();

            if (previewedDirectors == null)
                return;

            m_PreviewedAnimators = TimelineUtility.GetBindingsFromDirectors<Animator>(previewedDirectors).ToList();

            m_PreviewedComponents = new List<IAnimationWindowPreview>();
            foreach (var animator in m_PreviewedAnimators)
            {
                m_PreviewedComponents.AddRange(animator.GetComponents<IAnimationWindowPreview>());
            }
            foreach (var previewedComponent in m_PreviewedComponents)
            {
                previewedComponent.StartPreview();
            }
        }

        void OnStopPreview()
        {
            if (m_PreviewedComponents != null)
            {
                foreach (var previewComponent in m_PreviewedComponents)
                {
                    if (previewComponent != null)
                    {
                        previewComponent.StopPreview();
                    }
                }
                m_PreviewedComponents = null;
            }

            if (m_PreviewedAnimators != null)
            {
                foreach (var previewAnimator in m_PreviewedAnimators)
                {
                    if (previewAnimator != null)
                    {
                        previewAnimator.UnbindAllHandles();
                    }
                }
                m_PreviewedAnimators = null;
            }
        }

        internal void ProcessStartFramePendingUpdates()
        {
            if (m_OnStartFrameUpdates != null)
                m_OnStartFrameUpdates.RemoveAll(callback => callback.Invoke(this, Event.current));
        }

        internal void ProcessEndFramePendingUpdates()
        {
            if (m_OnEndFrameUpdates != null)
                m_OnEndFrameUpdates.RemoveAll(callback => callback.Invoke(this, Event.current));
        }

        public void AddStartFrameDelegate(PendingUpdateDelegate updateDelegate)
        {
            if (m_OnStartFrameUpdates == null)
                m_OnStartFrameUpdates = new List<PendingUpdateDelegate>();
            if (m_OnStartFrameUpdates.Contains(updateDelegate))
                return;
            m_OnStartFrameUpdates.Add(updateDelegate);
        }

        public void AddEndFrameDelegate(PendingUpdateDelegate updateDelegate)
        {
            if (m_OnEndFrameUpdates == null)
                m_OnEndFrameUpdates = new List<PendingUpdateDelegate>();
            if (m_OnEndFrameUpdates.Contains(updateDelegate))
                return;
            m_OnEndFrameUpdates.Add(updateDelegate);
        }

        internal void InvokeWindowOnGuiStarted(Event evt)
        {
            if (windowOnGuiStarted != null)
                windowOnGuiStarted.Invoke(this, evt);
        }

        internal void InvokeWindowOnGuiFinished(Event evt)
        {
            if (windowOnGuiFinished != null)
                windowOnGuiFinished.Invoke(this, evt);
        }

        public void UpdateRootPlayableDuration(double duration)
        {
            if (editSequence.director != null)
            {
                if (editSequence.director.playableGraph.IsValid())
                {
                    if (editSequence.director.playableGraph.GetRootPlayableCount() > 0)
                    {
                        var rootPlayable = editSequence.director.playableGraph.GetRootPlayable(0);
                        if (rootPlayable.IsValid())
                            rootPlayable.SetDuration(duration);
                    }
                }
            }
        }

        public void InvokeTimeChangeCallback()
        {
            if (OnTimeChange != null)
                OnTimeChange.Invoke();
        }

        Vector2 ValidatePlayRange(Vector2 range)
        {
            if (range == TimelineAssetViewModel.NoPlayRangeSet)
                return range;

            float minimumPlayRangeTime = 0.01f / Mathf.Max(1.0f, referenceSeque