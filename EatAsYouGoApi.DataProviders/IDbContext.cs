using System.Data.Entity;
using EatAsYouGoApi.DataLayer.DataModels;

namespace EatAsYouGoApi.DataLayer
{
    public interface IDbContext
    {
        DbSet<MenuItem> MenuItems { get; set; }

        DbSet<Restaurant> Restaurants { get; set; }

        DbSet<Deal> Deals { get; set; }

        DbSet<Address> Addresses { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<Group> Groups { get; set; }

        DbSet<Permission> Permissions { get; set; }
    }
}