using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Cloud.Collaborate.Models.Api;
using Unity.Cloud.Collaborate.Models.Structures;
using Unity.Cloud.Collaborate.UserInterface;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.Cloud.Collaborate.Models
{
    internal class HistoryModel : IHistoryModel
    {
        [NotNull]
        readonly ISourceControlProvider m_Provider;

        [NotNull]
        readonly HashSet<string> m_Requests;
        const string k_RequestPage = "request-page";
        const string k_RequestEntry = "request-entry";
        const string k_RequestEntryNumber = "request-entry-number";

        /// <inheritdoc />
        public event Action HistoryListUpdated;

        /// <inheritdoc />
        public event Action<IReadOnlyList<IHistoryEntry>> HistoryListReceived;

        /// <inheritdoc />
        public event Action<IHistoryEntry> SelectedRevisionReceived;

        /// <inheritdoc />
        public event Action<bool> BusyStatusUpdated;

        /// <inheritdoc />
        public event Action<int?> EntryCountUpdated;

        /// <inheritdoc />
        public event Action StateChanged;

        public HistoryModel([NotNull] ISourceControlProvider provider)
        {
            m_Provider = provider;
            m_Requests = new HashSet<string>();
            SelectedRevisionId = string.Empty;
            SavedRevisionId = string.Empty;
        }

        /// <inheritdoc />
        public void OnStart()
        {
            // Setup events
            m_Provider.UpdatedHistoryEntries += OnUpdatedHistoryEntries;
        }

        /// <inheritdoc />
        public void OnStop()
        {
            // Clean up.
            m_Provider.UpdatedHistoryEntries -= OnUpdatedHistoryEntries;
        }

        /// <inheritdoc />
        public void RestoreState(IWindowCache cache)
        {
            // Populate data.
            PageNumber = cache.HistoryPageNumber;
            SavedRevisionId = cache.SelectedHistoryRevision;

            StateChanged?.Invoke();
        }

        /// <inheritdoc />
        public void SaveState(IWindowCache cache)
        {
 