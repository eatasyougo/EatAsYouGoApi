using System.Collections.Generic;
using EatAsYouGoApi.Dtos;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAllUsers(bool showActiveUsersOnly = false);

        UserDto GetUserById(long userId);

        UserDto GetUserByEmail(string email);
        
        UserDto AddNewUser(UserDto userDto);

        void RemoveUser(long userId);

        UserDto UpdateUser(UserDto userDto);

        UserDto GetUserByEmailAndPassword(string email, string password, bool includeGroups);
    }
}
