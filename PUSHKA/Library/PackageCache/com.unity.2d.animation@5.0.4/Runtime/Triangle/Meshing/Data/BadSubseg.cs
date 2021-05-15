      range.y = Mathf.Min(range.x + minimumPlayRangeTime, (float)editSequence.duration);

            return range;
        }

        void EnsureWindowTimeConsistency()
        {
            if (masterSequence.director != null && masterSequence.viewModel != null && !ignorePreview)
                masterSequence.time = masterSequence.viewModel.windowTime;
        }

        void SynchronizeSequencesAfterPlayback()
        {
            // Synchronizing editSequence will synchronize all view models up to the master
            SynchronizeViewModelTime(editSequence);
        }

        static void SynchronizeViewModelTime(ISequenceState state)
        {
            if (state.director == null || state.viewModel == null)
                return;

            var t = state.time;
            state.time = t;
        }

        // because we may be evaluating outside the duration of the root playable
        //  we explicitly set the time - this causes the graph to not 'advance' the time
        //  because advancing it can force it to change due to wrapping to the duration
        // This can happen if the graph is force evaluated outside it's duration
        // case 