using System;
using JetBrains.Annotations;
using Unity.Cloud.Collaborate.Models.Api;
using Unity.Cloud.Collaborate.Models.Structures;
using Unity.Cloud.Collaborate.UserInterface;
using UnityEngine.Assertions;

namespace Unity.Cloud.Collaborate.Models
{
    internal class MainModel : IMainModel
    {
        [NotNull]
        readonly ISourceControlProvider m_Provider;

        /// <inheritdoc />
        public event Action<bool> ConflictStatusChange;

        /// <inheritdoc />
        public event Action<bool> OperationStatusChange;

        /// <inheritdoc />
        public event Action<IProgressInfo> OperationProgressChange;

        /// <inheritdoc />
        public event Action<IErrorInfo> ErrorOccur