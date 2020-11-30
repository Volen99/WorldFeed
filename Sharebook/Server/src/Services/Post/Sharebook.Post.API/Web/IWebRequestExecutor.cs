﻿namespace Sharebook.Post.API.Application.Web
{
    using System.Threading.Tasks;
    using Sharebook.Common.Web;
    using Sharebook.Storage.Application.Requesters;


    /// <summary>
    /// Generate a Token that can be used to perform OAuth queries
    /// </summary>
    public interface IWebRequestExecutor
    {
        /// <summary>
        /// Execute a TwitterQuery and return the resulting json data.
        /// </summary>
        Task<ITwitterResponse> ExecuteQueryAsync(ITwitterRequest request, ITwitterClientHandler handler = null);

        /// <summary>
        /// Execute a multipart TwitterQuery and return the resulting json data.
        /// </summary>
        Task<ITwitterResponse> ExecuteMultipartQueryAsync(ITwitterRequest request);
    }
}
