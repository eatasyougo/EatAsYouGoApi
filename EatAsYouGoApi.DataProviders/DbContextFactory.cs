using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EatAsYouGoApi.DataLayer
{
    public class DbContextFactory<T>: IDbContextFactory<T> where T : DbContext
    {
        public T Create()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
