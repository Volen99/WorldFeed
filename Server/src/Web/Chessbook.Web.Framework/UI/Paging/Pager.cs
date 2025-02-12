﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

using Chessbook.Core;
using Chessbook.Core.Infrastructure;

namespace Chessbook.Web.Framework.UI.Paging
{
    /// <summary>
    /// Renders a pager component from an IPageableModel datasource.
    /// </summary>
    public partial class Pager : IHtmlContent
    {
        #region Properties

        /// <summary>
        /// ViewContext
        /// </summary>
        public ViewContext ViewContext { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// Page query string prameter name
        /// </summary>
        private string _queryParam = "page";

        /// <summary>
        /// A value indicating whether to show Total summary
        /// </summary>
        private bool _showTotalSummary = false;

        /// <summary>
        /// A value indicating whether to show pager items
        /// </summary>
        private bool _showPagerItems = true;

        /// <summary>
        /// A value indicating whether to show the first item
        /// </summary>
        private bool _showFirst = true;

        /// <summary>
        /// A value indicating whether to the previous item
        /// </summary>
        private bool _showPrevious = true;

        /// <summary>
        /// A value indicating whether to show the next item
        /// </summary>
        private bool _showNext = true;

        /// <summary>
        /// A value indicating whether to show the last item
        /// </summary>
        private bool _showLast = true;

        /// <summary>
        /// A value indicating whether to show individual page
        /// </summary>
        private bool _showIndividualPages = true;

        /// <summary>
        /// A value indicating whether to render empty query string parameters (without values)
        /// </summary>
        private bool _renderEmptyParameters = true;

        /// <summary>
        /// Number of individual page items to display
        /// </summary>
        private int _individualPagesDisplayedCount = 5;

        /// <summary>
        /// Boolean parameter names
        /// </summary>
        private IList<string> _booleanParameterNames = new List<string>();

        /// <summary>
        /// First page css class name
        /// </summary>
        private string _firstPageCssClass = "first-page";

        /// <summary>
        /// Previous page css class name
        /// </summary>
        private string _previousPageCssClass = "previous-page";

        /// <summary>
		/// Current page css class name
		/// </summary>
        private string _currentPageCssClass = "current-page";

        /// <summary>
        /// Individual page css class name
        /// </summary>
        private string _individualPageCssClass = "individual-page";

        /// <summary>
		/// Next page css class name
		/// </summary>
        private string _nextPageCssClass = "next-page";

        /// <summary>
		/// Last page css class name
		/// </summary>
        private string _lastPageCssClass = "last-page";

        /// <summary>
		/// Main ul css class name
		/// </summary>
        private string _mainUlCssClass = string.Empty;

        private readonly IPageableModel _model;

        #endregion

        #region Ctor

        public Pager(IPageableModel model, ViewContext context)
        {
            _model = model;
            ViewContext = context;
        }

        #endregion

        #region Methods

        #region Fields setters

        /// <summary>
        /// Set 
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager QueryParam(string value)
        {
            _queryParam = value;
            return this;
        }

        /// <summary>
        /// Set a value indicating whether to show Total summary
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowTotalSummary(bool value)
        {
            _showTotalSummary = value;
            return this;
        }

        /// <summary>
        /// Set a value indicating whether to show pager items
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowPagerItems(bool value)
        {
            _showPagerItems = value;
            return this;
        }

        /// <summary>
        /// Set a value indicating whether to show the first item
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowFirst(bool value)
        {
            _showFirst = value;
            return this;
        }

        /// <summary>
        /// Set a value indicating whether to the previous item
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowPrevious(bool value)
        {
            _showPrevious = value;
            return this;
        }

        /// <summary>
        /// Set a  value indicating whether to show the next item
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowNext(bool value)
        {
            _showNext = value;
            return this;
        }

        /// <summary>
        /// Set a value indicating whether to show the last item
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowLast(bool value)
        {
            _showLast = value;
            return this;
        }

        /// <summary>
        /// Set number of individual page items to display
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager ShowIndividualPages(bool value)
        {
            _showIndividualPages = value;
            return this;
        }

        /// <summary>
        /// Set a value indicating whether to render empty query string parameters (without values)
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager RenderEmptyParameters(bool value)
        {
            _renderEmptyParameters = value;
            return this;
        }

        /// <summary>
        /// Set number of individual page items to display
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager IndividualPagesDisplayedCount(int value)
        {
            _individualPagesDisplayedCount = value;
            return this;
        }

        /// <summary>
        /// little hack here due to ugly MVC implementation
        /// find more info here: http://www.mindstorminteractive.com/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
        /// </summary>
        /// <param name="paramName">Parameter name</param>
        /// <returns>Pager</returns>
        public Pager BooleanParameterName(string paramName)
        {
            _booleanParameterNames.Add(paramName);
            return this;
        }

        /// <summary>
        /// Set first page pager css class name
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager FirstPageCssClass(string value)
        {
            _firstPageCssClass = value;
            return this;
        }

        /// <summary>
        /// Set previous page pager css class name
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager PreviousPageCssClass(string value)
        {
            _previousPageCssClass = value;
            return this;
        }

        /// <summary>
        /// Set current page pager css class name
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager CurrentPageCssClass(string value)
        {
            _currentPageCssClass = value;
            return this;
        }

        /// <summary>
        /// Set individual page pager css class name
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager IndividualPageCssClass(string value)
        {
            _individualPageCssClass = value;
            return this;
        }

        /// <summary>
        /// Set next page pager css class name
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager NextPageCssClass(string value)
        {
            _nextPageCssClass = value;
            return this;
        }

        /// <summary>
        /// Set last page pager css class name
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager LastPageCssClass(string value)
        {
            _lastPageCssClass = value;
            return this;
        }

        /// <summary>
        /// Set main ul css class name
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Pager</returns>
        public Pager MainUlCssClass(string value)
        {
            _mainUlCssClass = value;
            return this;
        }

        #endregion

        /// <summary>
        /// Write control
        /// </summary>
        /// <param name="writer">Writer</param>
        /// <param name="encoder">Encoder</param>
        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            writer.Write(ToString());
        }


        #endregion


        /// <summary>
        /// Get first individual page index
        /// </summary>
        /// <returns>Page index</returns>
        protected virtual int GetFirstIndividualPageIndex()
        {
            if ((_model.TotalPages < _individualPagesDisplayedCount) || ((_model.Start - (_individualPagesDisplayedCount / 2)) < 0))
                return 0;

            if ((_model.Start + (_individualPagesDisplayedCount / 2)) >= _model.TotalPages)
                return _model.TotalPages - _individualPagesDisplayedCount;

            return _model.Start - (_individualPagesDisplayedCount / 2);
        }

        /// <summary>
        /// Get last individual page index
        /// </summary>
        /// <returns>Page index</returns>
        protected virtual int GetLastIndividualPageIndex()
        {
            var num = _individualPagesDisplayedCount / 2;
            if ((_individualPagesDisplayedCount % 2) == 0)
                num--;

            if ((_model.TotalPages < _individualPagesDisplayedCount) || ((_model.Start + num) >= _model.TotalPages))
                return _model.TotalPages - 1;

            if ((_model.Start - (_individualPagesDisplayedCount / 2)) < 0)
                return _individualPagesDisplayedCount - 1;

            return _model.Start + num;
        }

        /// <summary>
        /// Create default URL
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <returns>URL</returns>
        protected virtual string CreateDefaultUrl(int pageNumber)
        {
            var routeValues = new RouteValueDictionary();

            var parametersWithEmptyValues = new List<string>();
            foreach (var key in ViewContext.HttpContext.Request.Query.Keys.Where(key => key != null))
            {
                var value = ViewContext.HttpContext.Request.Query[key].ToString();
                if (_renderEmptyParameters && string.IsNullOrEmpty(value))
                {
                    //we store query string parameters with empty values separately
                    //we need to do it because they are not properly processed in the UrlHelper.GenerateUrl method (dropped for some reasons)
                    parametersWithEmptyValues.Add(key);
                }
                else
                {
                    if (_booleanParameterNames.Contains(key, StringComparer.InvariantCultureIgnoreCase))
                    {
                        //little hack here due to ugly MVC implementation
                        //find more info here: http://www.mindstorminteractive.com/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
                        if (!string.IsNullOrEmpty(value) && value.Equals("true,false", StringComparison.InvariantCultureIgnoreCase))
                            value = "true";
                    }

                    routeValues[key] = value;
                }
            }

            if (pageNumber > 1)
                routeValues[_queryParam] = pageNumber;
            else
            {
                //SEO. we do not render pageindex query string parameter for the first page
                if (routeValues.ContainsKey(_queryParam))
                    routeValues.Remove(_queryParam);
            }

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            var url = webHelper.GetThisPageUrl(false);
            foreach (var routeValue in routeValues) 
                url = webHelper.ModifyQueryString(url, routeValue.Key, routeValue.Value?.ToString());

            if (_renderEmptyParameters && parametersWithEmptyValues.Any())
                foreach (var key in parametersWithEmptyValues) 
                    url = webHelper.ModifyQueryString(url, key);

            return url;
        }

    }
}
