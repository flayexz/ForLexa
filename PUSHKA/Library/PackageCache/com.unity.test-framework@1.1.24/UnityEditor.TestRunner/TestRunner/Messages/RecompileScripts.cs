pageSize, pageSize, OnReceivedHistoryPage);
        }

        /// <inheritdoc />
        public void RequestSingleRevision(string revisionId)
        {
            // Only one request at a time.
            if (!AddRequest(k_RequestEntry)) return;

            SavedRevisionId = string.Empty;
            SelectedRevisionId = revisionId;
            m_Provider.RequestHistoryEntry(revisionId, OnReceivedHistoryEntry);
        }

        /// <inheritdoc />
        public void RequestEntryNumber()
        {
            // Only one request at a time.
            if (!AddRequest(k_RequestEntryNumber)) return;

            m_Provider.RequestHistoryCount(OnReceivedHistoryEntryCount);
        }

        /// <inheritdoc />
        public void RequestUpdateTo(string revisionId)
        {
            m_Provider.RequestUpdateTo(revisionId);
        }

        /// <inheritdoc />
        public void RequestRestoreTo(string revisionId)
        {
            m_Provider.RequestRestoreTo(revisionId);
        }

        /// <inheritdoc />
        public void RequestGoBackTo(string revisionId)
        {
            m_Provider.RequestGoBackTo(revisionId);
        }

        /// <inheritdoc />
        public bool SupportsRevert => m_Provider.SupportsRevert;

        /// <inheritdoc />
        public void RequestRevert(string revisionId, IReadOnlyList<string> files)
        {
            m_Provider.RequestRevert(revisionId, files);
        }

        /// <summary>
        /// Add a started request.
        /// </summary>
        /// <param name="requestId">Id of the request to add.</param>
        /// <returns>False if the request already exists.</returns>
        bool AddRequest([NotNull] string requestId)
        {
            if (m_Requests.Contains(requestId)) return false;
            m_Requests.Add(requestId);
            // Signal background activity if this is the only thing running.
            if (m_Requests.Count == 1)
                BusyStatusUpdated?.Invoke(true);
            return true;
        }

        /// <summary>
        /// Remove a finished request.
        /// </summary>
        /// <param name="requestId">Id of the request to remove.</param>
        void RemoveRequest([NotNull] string requestId)
        {
            Assert.IsTrue(m_Requests.Contains(requestId), $"Expects request to have first been made for it to have been finished: {requestId}");
            m_Requests.Remove(requestId);
            // Signal no background activity if no requests in progress
            if (m_Requests.Count == 0)
                BusyStatusUpdated?.Invoke(false);
        }

        /// <inheritdoc />
        public bool Busy => m_Requests.Count != 0;

        /// <inheritdoc />
        public int PageNumber { get; set; }

        /// <inheritdoc />
        public string SelectedRevisionId { get; private set; }

        /// <inheritdoc />
        public string SavedRevisionId { get; private set; }

        /// <inheritdoc />
        public bool IsRevisionSelected => !string.IsNullOrEmpty(SelectedRevisionId);
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Cloud.Collaborate.Models.Structures;

namespace Unity.Cloud.Collaborate.Models
{
    internal interface IChangesModel : IModel
    {
        /// <summary>
        /// Event triggered when an updated change list is available.
        /// </summary>
        event Action UpdatedChangeList;

        /// <summary>
        /// Event triggered when an updated selection of change list is available.
        /// </summary>
        event Action OnUpdatedSelectedChanges;

        /// <summary>
        /// Event triggered when the busy status changes.
        /// </summary>
        event Action<bool> BusyStatusUpdated;

        /// <summary>
        /// Stored revision summary.
        /// </summary>
        [NotNull]
        string SavedRevisionSummary { get; set; }

        /// <summary>
        /// Stored search query.
        