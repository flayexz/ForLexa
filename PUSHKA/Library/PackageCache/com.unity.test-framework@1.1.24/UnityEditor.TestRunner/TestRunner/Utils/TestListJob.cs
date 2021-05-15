tTabIndex { get; set; }

        /// <inheritdoc />
        public IHistoryModel ConstructHistoryModel()
        {
            return m_HistoryModel;
        }

        /// <inheritdoc />
        public IChangesModel ConstructChangesModel()
        {
            return m_ChangesModel;
        }

        /// <inheritdoc />
        public void ClearError()
        {
            m_Provider.ClearError();
        }

        /// <inheritdoc />
        public void RequestSync()
        {
            m_Provider.RequestSync();
        }

        /// <inheritdoc />
        public void RequestCancelJob()
        {
            m_Provider.RequestCancelJob();
        }

        /// <inheritdoc />
        public (string id, string text, Action backAction)? GetBackNavigation()
        {
            return m_BackNavigation;
        }

        /// <inheritdoc />
        public void RegisterBackNavigation(string id, string text, Action backAction)
        {
            Assert.IsTrue(m_BackNavigation == null, "There should only be one back navigation registered at a time.");
            m_BackNavigation = (id, text, 