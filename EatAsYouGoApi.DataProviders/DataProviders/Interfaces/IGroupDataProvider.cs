using System.Collections.Generic;
using EatAsYouGoApi.DataLayer.DataModels;

namespace EatAsYouGoApi.DataLayer.DataProviders.Interfaces
{
    public interface IGroupDataProvider
    {
        IEnumerable<Group> GetAllGroups(bool showActiveGroupsOnly);

        Group GetGroupById(int groupId);

        Group AddNewGroup(Group group, ICollection<User> users, ICollection<Permission> permissions);

        void RemoveGroup(int groupId);

        Group UpdateGroup(Group group, ICollection<User> users, ICollection<Permission> permissions);
    }
}