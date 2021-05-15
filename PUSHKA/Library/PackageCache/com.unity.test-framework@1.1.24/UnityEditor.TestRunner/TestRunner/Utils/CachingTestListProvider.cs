using System;
using Unity.Cloud.Collaborate.Models.Structures;
using JetBrains.Annotations;

namespace Unity.Cloud.Collaborate.Models
{
    internal interface IMainModel : IModel
    {
        /// <summary>
        /// Signal when the local state switches between conflicted or not.
        /// </summary>
        event Action<bool> ConflictStatusChange;

        /// <summary>
        /// Signal when an operation with progress has started or stopped.
        /// </summary>
        event Action<bool> OperationStatusChange;

        /// <summary>
        /// Signal with incremental details of the operation in progress.
        /// </summary>
        event Action<IProgressInfo> OperationProgressChange;

        /// <summary>
        /// Signal when an error has occurred.
        /// </summary>
        event Action<IErrorInfo> ErrorOccurred;

        /// <summary>
        /// Signal when the error has cleared.
        /// </summary>
        event Action ErrorCleared;

        /// <summary>
        /// Signal whether or not the there are remote revisions to be fetched.
        /// </summary>
        event Action<bool> RemoteRevisionsAvailabilityChange;

        /// <summary>
        /// Signal when the state of the back button is updated. For example: clearing it or showing a new one.
        /// The string included is the new label for the back navigation button. If that value is null, clear the back
        /// navigation.
        /// </summary>
        event Action<string> BackButtonStateUpdated;

        /// <summary>
        /// Returns true if there are remote revisions available.
        /// </summary>
        bool Remo