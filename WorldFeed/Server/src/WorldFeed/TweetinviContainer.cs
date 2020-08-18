﻿namespace WorldFeed
{
    using System;

    using WorldFeed.Common.InjectWorldFeed;
    using WorldFeed.Common.Public.Events;

    /// <summary>
    /// For super users only. Change Tweetinvi internal mechanisms.
    /// </summary>
    public static class TweetinviContainer
    {
        public static ITweetinviContainer Container;

        /// <summary>
        /// Event raised before the registration completes so that you can override registered classes.
        /// Doing so allow you to force Tweetinvi to use your own set of class instead of the one designed by the application.
        /// </summary>
        public static event EventHandler<TweetinviContainerEventArgs> BeforeRegistrationComplete;

        static TweetinviContainer()
        {
            Container = new Injectinvi.TweetinviContainer();
            Container.BeforeRegistrationCompletes += ContainerOnBeforeRegistrationCompletes;
        }

        private static void ContainerOnBeforeRegistrationCompletes(object sender, TweetinviContainerEventArgs args)
        {
            var handlers = BeforeRegistrationComplete;
            if (handlers != null)
            {
                handlers.Invoke(sender, args);
            }

            Container.BeforeRegistrationCompletes -= ContainerOnBeforeRegistrationCompletes;
        }

        private static readonly object _resolveLock = new object();

        public static void AddModule(ITweetinviModule module)
        {
            lock (_resolveLock)
            {
                var updatedContainer = new WorldFeed.Injectinvi.TweetinviContainer(Container);
                module.Initialize(updatedContainer);
                updatedContainer.Initialize();

                Container = updatedContainer;
            }
        }

        /// <summary>
        /// Allow you to retrieve any class used by Tweetinvi by specifying its interface.
        /// Resolving a component is roughly equivalent to calling "new" to instantiate a class.
        /// That’s really, really oversimplifying it, but from an analogy perspective it’s fine
        /// </summary>
        public static T Resolve<T>()
        {
            lock (_resolveLock)
            {
                if (!Container.IsInitialized)
                {
                    Container.Initialize();
                }

                return Container.Resolve<T>();
            }
        }
    }
}
