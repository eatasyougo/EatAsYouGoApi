using System.Collections.Generic;
using EatAsYouGoApi.Dtos;

namespace EatAsYouGoApi.Services.Interfaces
{
    public interface IGroupService
    {
        IEnumerable<GroupDto> GetAllGroups(bool showActiveGroupsOnly);

        GroupDto GetGroupById(int groupId);

        GroupDto AddNewGroup(GroupDto groupDto);

        void RemoveGroup(int groupId);

        GroupDto UpdateGroup(GroupDto groupDto);
    }
}