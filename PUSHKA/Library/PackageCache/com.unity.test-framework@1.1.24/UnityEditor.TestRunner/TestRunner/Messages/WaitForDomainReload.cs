using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Cloud.Collaborate.Models.Structures;

namespace Unity.Cloud.Collaborate.Models
{
    internal interface IHistoryModel : IModel
    {
        /// <summary>
        /// Event triggered when the history list has been updated.
        /// </summary>
        event Action HistoryListUpdated;

        /// <summary>
        /// Event triggered when the requested page of revisions is received.
        /// </summary>
        event Action<IReadOnlyList<IHistoryEntry>> HistoryListReceived;

        /// <summary>
        /// Event triggered when the requested revision is received.
        /// </summary>
        event Action<IHistoryEntry> SelectedRevisionReceived;

        /// <summary>
        /// Event triggered when the busy status changes.
        /// </summary>
        event Action<bool> BusyStatusUpdated;

        /// <summary>
        /// Event triggered when the requested entry count is received.
        /// </summary>
        event Action<int?> EntryCountUpdated;

        /// <summary>
        /// Whether or not the model is busy with a request.
        /// </summary>
        bool Busy { get; }

        /// <summary>
        /// Current page number.
        /// </summary>
        int PageNumber { get; set; }

        /// <summary>
        /// Currently selected revision id.
        /// </summary>
        [NotNull]
        string SelectedRevisionId { get; }

        /// <summary>
        /// Revision saved before domain reload.
        /// </summary>
        [NotNull]
        string SavedRevisionId { get; }

        /// <summary>
        /// True if a revision is currently selected.
        /// </summary>
        bool IsRevisionSelected { get; }

        /// <summary>
        /// Request the current page of given size. Result returns via the HistoryListReceived event.
        /// </summary>
        /// <param name="pageSize"></param>
        void RequestPageOfRevisions(int pageSize);

        /// <summary>
        /// Request the revision with the given id. Result returned via the SelectedRevisionReceived event.
        /// </summary>
        /// <param name="revisionId"></param>
        void RequestSingleRevision([NotNull] string revisionId);

        /// <summary>
        /// Request the count of entries. Result returned via the EntryCountUpdated event.
        /// </summary>
        void RequestEntryNumber();

        /// <summary>
 