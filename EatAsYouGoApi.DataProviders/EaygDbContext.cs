﻿using System.Data.Entity;
using EatAsYouGoApi.DataLayer.DataModels;

namespace EatAsYouGoApi.DataLayer
{
    public class EaygDbContext : DbContext, IDbContext
    {
        public EaygDbContext(): base("name=EatAsYouGo")
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Deal> Deals { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<GroupPermission> GroupPermissions { get; set; }

        public DbSet<DealMenuItem> DealMenuItems { get; set; }
    }
}