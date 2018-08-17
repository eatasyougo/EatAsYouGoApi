using System.Collections.Generic;
using EatAsYouGoApi.DataLayer.DataModels;

namespace EatAsYouGoApi.DataLayer.DataProviders.Interfaces
{
    public interface IUserDataProvider
    {
        IEnumerable<User> GetAllUsers(bool showActiveUsersOnly = false);

        User GetUserById(long userId); 

        User GetUserByEmail(string email);

        User AddNewUser(User user, ICollection<Group> groups);

        void RemoveUser(long userId);

        User UpdateUser(User user, ICollection<Group> groups);
    }
}