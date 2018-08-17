﻿using System.Data.Entity.Infrastructure;
using System.Web.Http;
using EatAsYouGoApi.DataLayer;
using EatAsYouGoApi.DataLayer.DataProviders;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;
using EatAsYouGoApi.Services;
using EatAsYouGoApi.Services.Interfaces;
using Unity;
using Unity.Lifetime;

namespace EatAsYouGoApi.Core
{
    public class UnityRegistrations
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IDbContext, EaygDbContext>(new ContainerControlledLifetimeManager());
            container.RegisterType<IRestaurantDataProvider, RestaurantDataProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMenuItemDataProvider, MenuItemDataProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserDataProvider, UserDataProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGroupDataProvider, GroupDataProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDealDataProvider, DealDataProvider>(new ContainerControlledLifetimeManager());
            container.RegisterType<IRestaurantService, RestaurantService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMenuItemService, MenuItemService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGroupService, GroupService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDealService, DealService>(new ContainerControlledLifetimeManager());
            container.RegisterType(typeof (IDbContextFactory<>), typeof (DbContextFactory<>), new TransientLifetimeManager());
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}