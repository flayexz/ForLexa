 the file at the given path.
        /// </summary>
        /// <param name="entry">Entry to discard.</param>
        void RequestDiscard([NotNull] IChangeEntry entry);

        /// <summary>
        /// Request discard of the given list of files.
        /// </summary>
        /// <param name="entries">List of entries to discard.</param>
        void RequestBulkDiscard([NotNull] IReadOnlyList<IChangeEntry> entries);

        /// <summary>
        /// Request publish with the given message and list of files.
        /// </summary>
        /// <param name="message">Message for the revision.</param>
        /// <param name="changes">Changes to publish.</param>
        void RequestPublish([NotNull] string message, [NotNull] IReadOnlyList<IChangeEntry> changes);

        /// <summary>
        /// Show the difference between both version of a conflicted file.
        /// </summary>
        /// <param name="path">Path of the file to show.</param>
        void RequestShowConflictedDifferences([NotNull] string path);

        /// <summary>
        /// Request to choose merge for the provided conflict.
        /// </summary>
        /// <param name="path">Path of the file to choose merge for.</param>
        void RequestChooseMerge([NotNull] string path);

        /// <summary>
        /// Request to choose mine for the provided conflict.
        /// </summary>
        /// <param name="paths">Paths of the files to choose mine for.</param>
        void RequestChooseMine([NotNull] string[] paths);

        /// <summary>
        /// Request to choose remote for the