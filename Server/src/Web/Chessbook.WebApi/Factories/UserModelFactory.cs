﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Chessbook.Common;
using Chessbook.Core;
using Chessbook.Data.Models;
using Chessbook.Data.Models.Media;
using Chessbook.Services;
using Chessbook.Services.Data.Services.Entities;
using Chessbook.Services.Data.Services.Media;
using Chessbook.Services.Notifications.Settings;
using Chessbook.Services.Common;
using Chessbook.Services.Helpers;
using Chessbook.Web.Areas.Admin.Models.Customers;
using Chessbook.Web.Api.Areas.Admin.Models.Users;
using Chessbook.Web.Framework.Models.Extensions;
using Chessbook.Services.Data;
using Chessbook.Services.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Chessbook.Web.Api.Factories
{
    public class UserModelFactory : IUserModelFactory
    {
        private readonly IPictureService pictureService;
        private readonly IPostsService postsService;
        private readonly IGenericAttributeService genericAttributeService;
        private readonly IDateTimeHelper dateTimeHelper;
        private readonly IUserService userService;
        private readonly ISettingsService settingsService;
        private readonly INotificationsSettingsService notificationsSettingsService;
        private readonly IUserNotificationSettingModelFactory userNotificationSettingModelFactory;
        private readonly IRelationshipService relationshipService;
        private readonly IWorkContext workContext;
        private readonly IWebHostEnvironment env;

        private readonly string SiteHttps;

        public UserModelFactory(IPictureService pictureService, IPostsService postsService,
             IGenericAttributeService genericAttributeService, IDateTimeHelper dateTimeHelper,
             IUserService userService, ISettingsService settingsService,
             INotificationsSettingsService notificationsSettingsService,
             IUserNotificationSettingModelFactory userNotificationSettingModelFactory,
             IRelationshipService relationshipService, IWorkContext workContext,
             IWebHostEnvironment env)
        {
            this.pictureService = pictureService;
            this.postsService = postsService;
            this.genericAttributeService = genericAttributeService;
            this.dateTimeHelper = dateTimeHelper;
            this.userService = userService;
            this.settingsService = settingsService;
            this.notificationsSettingsService = notificationsSettingsService;
            this.userNotificationSettingModelFactory = userNotificationSettingModelFactory;
            this.relationshipService = relationshipService;
            this.workContext = workContext;
            this.env = env;

            this.SiteHttps = this.env.IsDevelopment() ? "https://localhost:5001" : "https://chessbook.me";
        }

        /// <summary>
        /// Prepare paged customer list model
        /// </summary>
        /// <param name="searchModel">Customer search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the customer list model
        /// </returns>
        public virtual async Task<CustomerListModel> PrepareCustomerListModelAsync(CustomerSearchModel searchModel, bool forAdmin = false)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            // get parameters to filter customers
            int.TryParse(searchModel.SearchDayOfBirth, out var dayOfBirth);
            int.TryParse(searchModel.SearchMonthOfBirth, out var monthOfBirth);

            // get customers
            var customers = await this.userService.GetAllCustomersAsync(customerRoleIds: searchModel.SelectedCustomerRoleIds.ToArray(),
                email: searchModel.SearchEmail,
                username: searchModel.SearchUsername,
                firstName: searchModel.SearchFirstName,
                dayOfBirth: dayOfBirth,
                monthOfBirth: monthOfBirth,
                ipAddress: searchModel.SearchIpAddress,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            // prepare list model
            var model = await new CustomerListModel().PrepareToGridAsync(searchModel, customers, () =>
            {
                return customers.SelectAwait(async customer =>
                {
                    var customerModel = await this.PrepareCustomerModelAsync(new CustomerModel(), customer, forAdmin);

                    return customerModel;
                });
            });

            return model;
        }

        public async Task<CustomerModel> PrepareCustomerModelAsync(CustomerModel model, Customer customer, bool excludeProperties = false)
        {
            var loggedInUser = await this.workContext.GetCurrentCustomerAsync();
            // relationship between the logged user and the current model user
            var relationship = await this.relationshipService.GetByUsersId(loggedInUser.Id, customer.Id);

            if (customer != null)
            {
                // fill in model values from the entity
                model ??= new CustomerModel();

                model.Id = customer.Id;

                if (excludeProperties)
                {
                    model.Email = customer.Email;
                    model.LastIpAddress = customer.LastIpAddress;
                }

                // whether to fill in some of properties
                // if (!excludeProperties)
                model.DisplayName = customer.DisplayName;
                model.ScreenName = customer.ScreenName;
                model.Active = customer.Active;
                model.FollowedBy = customer.FollowedBy;
                model.BlockedBy = relationship.BlockedBy;
                model.Blocking = relationship.Blocking;
                model.FollowersCount = customer.FollowersCount;
                model.FollowingCount = customer.FollowingCount;
                model.County = await this.genericAttributeService.GetAttributeAsync<string>(customer, NopCustomerDefaults.CountyAttribute);
                model.Description = customer.Description;
                // model.CountryId = await this.genericAttributeService.GetAttributeAsync<int>(customer, NopCustomerDefaults.CountryIdAttribute);
                model.CreatedOn = await dateTimeHelper.ConvertToUserTimeAsync(customer.CreatedOn, DateTimeKind.Utc);
                model.LastActivityDate = await this.dateTimeHelper.ConvertToUserTimeAsync(customer.LastActivityDateUtc, DateTimeKind.Utc);
                if (customer.LastLoginDateUtc.HasValue)
                {
                    model.LastLoginDate = await this.dateTimeHelper.ConvertToUserTimeAsync(customer.LastLoginDateUtc.Value, DateTimeKind.Utc);
                }
                model.WebsiteLink = customer.WebsiteLink;
                model.TwitterLink = customer.TwitterLink;
                model.TwitchLink = customer.TwitchLink;
                model.YoutubeLink = customer.YoutubeLink;
                model.FacebookLink = customer.FacebookLink;
                model.InstagramLink = customer.InstagramLink;
            }
            else
            {
                // whether to fill in some of properties
                if (!excludeProperties)
                {
                    // precheck Registered Role as a default role while creating a new customer through admin
                    var registeredRole = await this.userService.GetCustomerRoleBySystemNameAsync(NopCustomerDefaults.RegisteredRoleName);
                    if (registeredRole != null)
                    {
                        // model.SelectedCustomerRoleIds.Add(registeredRole.Id);
                    }
                }
            }

            // set default values for the new model
            if (customer == null)
            {
                model.Active = true;
            }

            var userCurrent = await this.userService.GetCustomerByIdAsync(model.Id);

            // settings
            model.Settings = await this.settingsService.GetById(userCurrent.Id);

            // avatar
            var avatarPictureId = await this.genericAttributeService.GetAttributeAsync<int>(userCurrent, NopCustomerDefaults.AvatarPictureIdAttribute);
            var profilePictureUrl = await this.pictureService.GetPictureUrlAsync(avatarPictureId, 400, true, defaultPictureType: PictureType.Avatar);
            model.ProfileImageUrlHttps = this.SiteHttps + profilePictureUrl;

            // banner
            var profileBannerPictureId = await this.genericAttributeService.GetAttributeAsync<int>(userCurrent, NopCustomerDefaults.ProfileBannerIdAttribute);
            var profileBannerUrl = await this.pictureService.GetPictureUrlAsync(profileBannerPictureId, 1500, true, defaultPictureType: PictureType.Banner);
            model.ProfileBannerURL = this.SiteHttps + profileBannerUrl;

            // posts
            model.StatusesCount = await this.postsService.GetPostsCountByUserId(model.Id);

            // notification settings
            var userNotificationSettings = await this.notificationsSettingsService.GetByUserId(model.Id);

            if (userNotificationSettings != null)
            {
                model.NotificationSettings = await this.userNotificationSettingModelFactory.PrepareUserNotificationSettingModelAsync(userNotificationSettings);
            }

            // user roles
            var roleNames = (await this.userService.GetCustomerRolesAsync(customer)).Select(role => role.Name);

            var roles = new List<int>();
            foreach (var name in roleNames)
            {

                // 0,1,2 are kept sync with the client enum
                if (name.ToLower() == "administrators") roles.Add(0);
                else if (name.ToLower() == "moderators") roles.Add(1);
                else if (name.ToLower() == "registered") roles.Add(2);
            }

            model.Roles = roles;

            return model;

        }

        /// <summary>
        /// Prepare the customer avatar model
        /// </summary>
        /// <param name="model">Customer avatar model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the customer avatar model
        /// </returns>
        public virtual async Task<string> PrepareCustomerAvatarModelAsync(int userId)
        {
            var userCurrent = await this.userService.GetCustomerByIdAsync(userId);

            var avatarPictureId = await this.genericAttributeService.GetAttributeAsync<int>(userCurrent, NopCustomerDefaults.AvatarPictureIdAttribute);
            var avatarUrl = await this.pictureService.GetPictureUrlAsync(avatarPictureId, 400, true, defaultPictureType: PictureType.Avatar);

            return this.SiteHttps + avatarUrl;
        }

    }
}
