﻿using Chessbook.Services.Tasks;

namespace Chessbook.Services.Common
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public class KeepAliveTask : IScheduleTask
    {
        #region Fields

        private readonly StoreHttpClient _storeHttpClient;

        #endregion

        #region Ctor

        public KeepAliveTask(StoreHttpClient storeHttpClient)
        {
            _storeHttpClient = storeHttpClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            await _storeHttpClient.KeepAliveAsync();
        }

        #endregion
    }
}
