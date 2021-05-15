using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Cloud.Collaborate.Models.Structures;

namespace Unity.Cloud.Collaborate.Presenters
{
    internal interface IHistoryPresenter : IPresenter
    {
        /// <summary>
        /// Request a move to the previous page. Ensures page number never goes below 0.
        /// </summary>
        void PrevPage();

        /// <summary>
        /// Request a move to the next page. Ensures page number doesn't go beyond the max number of pages.
        /// </summary>
        void NextPage();

        /// <summary>
        /// Set the revision id to request.
        /// </summary>
        [NotNull]
        string SelectedRevisionId { set; }

        /// <summary>
        /// Request to update the state of the project to a provided revision. If revision is in the past, then the
        /// state of the project at that point simply will be applied on top of the current without impacting history.
     