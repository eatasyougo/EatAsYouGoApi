using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;
using EatAsYouGoApi.Dtos;
using EatAsYouGoApi.Services.Interfaces;

namespace EatAsYouGoApi.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupDataProvider _groupDataProvider;

        public GroupService(IGroupDataProvider groupDataProvider)
        {
            _groupDataProvider = groupDataProvider;
        }

        public IEnumerable<GroupDto> GetAllGroups(bool showActiveGroupsOnly)
        {
            var groups = _groupDataProvider.GetAllGroups(showActiveGroupsOnly);
            var groupDtos = groups.Select(g =>
            {
                var groupDto = Mapper.Map<Group, GroupDto>(g);
                groupDto.Users = g.UserGroups.Select(ug => Mapper.Map<User, UserDto>(ug.User)).ToList();
                groupDto.Permissions = g.GroupPermissions.Select(gp => Mapper.Map<Permission, PermissionDto>(gp.Permission)).ToList();
                return groupDto;
            }).ToList();

            return groupDtos;
        }

        public GroupDto GetGroupById(int groupId)
        {
            var group = _groupDataProvider.GetGroupById(groupId);
            var groupDto = Mapper.Map<Group, GroupDto>(group);
            groupDto.Users = group.UserGroups.Select(ug => Mapper.Map<User, UserDto>(ug.User)).ToList();
            groupDto.Permissions = group.GroupPermissions
                .Select(gp => Mapper.Map<Permission, PermissionDto>(gp.Permission))
                .ToList();

            return groupDto;
        }

        public GroupDto AddNewGroup(GroupDto groupDto)
        {
            var group = Mapper.Map<GroupDto, Group>(groupDto);
            var users = groupDto.Users.Select(Mapper.Map<UserDto, User>).ToList();
            var permissions = groupDto.Permissions.Select(Mapper.Map<PermissionDto, Permission>).ToList();
            var newGroup = _groupDataProvider.AddNewGroup(group, users, permissions);

            var newUsers = newGroup.UserGroups.Select(ug => ug.User).ToList();
            var newPermissions = newGroup.GroupPermissions.Select(gp => gp.Permission).ToList();
            var newGroupDto = Mapper.Map<Group, GroupDto>(newGroup);
            newGroupDto.Users = newUsers.Select(Mapper.Map<User, UserDto>).ToList();
            newGroupDto.Permissions = newPermissions.Select(Mapper.Map<Permission, PermissionDto>).ToList();

            return newGroupDto;
        }

        public void RemoveGroup(int groupId)
        {
            _groupDataProvider.RemoveGroup(groupId);
        }

        public GroupDto UpdateGroup(GroupDto groupDto)
        {
            var group = Mapper.Map<GroupDto, Group>(groupDto);
            var users = groupDto.Users.Select(Mapper.Map<UserDto, User>).ToList();
            var permissions = groupDto.Permissions.Select(Mapper.Map<PermissionDto, Permission>).ToList();

            var updatedGroup = _groupDataProvider.UpdateGroup(group, users, permissions);

            var updatedUsers = updatedGroup.UserGroups.Select(ug => ug.User).ToList();
            var updatedPermissions = updatedGroup.GroupPermissions.Select(ug => ug.Permission).ToList();
            var updatedGroupDto = Mapper.Map<Group, GroupDto>(updatedGroup);
            updatedGroupDto.Users = updatedUsers.Select(Mapper.Map<User, UserDto>).ToList();
            updatedGroupDto.Permissions = updatedPermissions.Select(Mapper.Map<Permission, PermissionDto>).ToList();

            return updatedGroupDto;
        }
    }
}