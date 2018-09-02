using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;

namespace EatAsYouGoApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDataProvider _userDataProvider;

        public UserService(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        public IEnumerable<UserDto> GetAllUsers(bool showActiveUsersOnly = false)
        {
            return _userDataProvider.GetAllUsers(showActiveUsersOnly).Select(Mapper.Map<User, UserDto>).ToList();
        }

        public UserDto GetUserById(long userId)
        {
            var user = _userDataProvider.GetUserById(userId);
            return Mapper.Map<User, UserDto>(user);
        }

        public UserDto GetUserByEmail(string email)
        {
            var user = _userDataProvider.GetUserByEmail(email);
            return Mapper.Map<User, UserDto>(user);
        }

        public UserDto AddNewUser(UserDto userDto)
        {
            var user = Mapper.Map<UserDto, User>(userDto);
            if(userDto.Groups == null || userDto.Groups.Count == 0)
                throw new InvalidOperationException("User must belong to at least one Group. Cannot add User.");

            var groups = userDto.Groups.Select(Mapper.Map<GroupDto, Group>).ToList();
            var newUser = _userDataProvider.AddNewUser(user, groups);

            var newGroups = newUser.UserGroups.Select(ug => ug.Group).ToList();
            var newUserDto = Mapper.Map<User, UserDto>(newUser);
            newUserDto.Groups = newGroups.Select(Mapper.Map<Group, GroupDto>).ToList();

            return newUserDto;
        }

        public void RemoveUser(long userId)
        {
            _userDataProvider.RemoveUser(userId);
        }

        public UserDto UpdateUser(UserDto userDto)
        {
            var user = Mapper.Map<UserDto, User>(userDto);
            if (userDto.Groups == null || userDto.Groups.Count == 0)
                throw new InvalidOperationException("User must belong to at least one Group. Cannot update User.");

            var groups = userDto.Groups.Select(Mapper.Map<GroupDto, Group>).ToList();
            var updatedUser = _userDataProvider.UpdateUser(user, groups);

            var updatedGroups = updatedUser.UserGroups.Select(ug => ug.Group).ToList();
            var updatedUserDto = Mapper.Map<User, UserDto>(updatedUser);
            updatedUserDto.Groups = updatedGroups.Select(Mapper.Map<Group, GroupDto>).ToList();

            return updatedUserDto;
        }

        public UserDto GetUserByEmailAndPassword(string email, string password)
        {
            var user = _userDataProvider.GetUserByEmailAndPassword(email, password);
            return Mapper.Map<User, UserDto>(user);
        }
    }
}