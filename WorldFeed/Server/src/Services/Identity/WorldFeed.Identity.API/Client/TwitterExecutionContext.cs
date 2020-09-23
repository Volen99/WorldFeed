﻿namespace WorldFeed.Identity.API.Client
{
    using System;

    using WorldFeed.Common.Events;
    using WorldFeed.Common.Settings;
    using WorldFeed.Identity.API.Application.Requesters;
    using WorldFeed.Identity.API.Infrastructure.Inject;

    public class TwitterExecutionContext : TweetinviSettings, ITwitterExecutionContext
    {
        public TwitterExecutionContext()
        {
            RequestFactory = () => throw new InvalidOperationException($"You cannot run contextual operations without configuring the {nameof(this.RequestFactory)} of the ExecutionContext");
        }

        public TwitterExecutionContext(ITwitterExecutionContext context)
            : base(context)
        {
            RequestFactory = context.RequestFactory;
            Container = context.Container;
            Events = context.Events;
        }

        public Func<ITwitterRequest> RequestFactory { get; set; }

        public IIdentityContainer Container { get; set; }

        public ITwitterClientEvents Events { get; set; }
    }
}
