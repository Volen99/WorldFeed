﻿namespace WorldFeed.Post.API.Infrastructure.Inject
{
    using WorldFeed.Common.Models.Enums;
    using WorldFeed.Post.API.Controllers;
    using WorldFeed.Post.API.Infrastructure.Inject.Contracts;
    using WorldFeed.Post.Application.QueryExecutors;
    using WorldFeed.Post.Application.QueryGenerators;
    using WorldFeed.Post.Helpers;

    public class PostControllersModule : IPostModule
    {
        public void Initialize(ITrendContainer container)
        {
            InitializeControllers(container);
            InitializeQueryExecutors(container);
            InitializeQueryGenerators(container);
            InitializeHelpers(container);
        }

        private void InitializeControllers(ITrendContainer container)
        {
            container.RegisterType<ITimelineController, TimelineController>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetController, TweetController>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeQueryExecutors(ITrendContainer container)
        {
            container.RegisterType<ITimelineQueryExecutor, TimelineQueryExecutor>();
            container.RegisterType<ITweetQueryExecutor, TweetQueryExecutor>();
        }

        private void InitializeQueryGenerators(ITrendContainer container)
        {
            container.RegisterType<ITimelineQueryGenerator, TimelineQueryGenerator>(RegistrationLifetime.InstancePerApplication);
            container.RegisterType<ITweetQueryGenerator, TweetQueryGenerator>(RegistrationLifetime.InstancePerApplication);

            container.RegisterType<IQueryParameterGenerator, QueryParameterGenerator>(RegistrationLifetime.InstancePerApplication);
        }

        private void InitializeHelpers(ITrendContainer container)
        {
            container.RegisterType<ITweetHelper, TweetHelper>();
        }
    }
}
