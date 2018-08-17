using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;

namespace EatAsYouGoApi.DataLayer.DataProviders
{
    public class GroupDataProvider : IGroupDataProvider
    {
        private readonly IDbContextFactory<EaygDbContext> _dbContextFactory;

        public GroupDataProvider(IDbContextFactory<EaygDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<Group> GetAllGroups(bool showActiveGroupsOnly)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var groups = dbContext.Groups
                    .Include(group => group.UserGroups)
                    .Include(group => group.GroupPermissions);

                if (showActiveGroupsOnly)
                    groups = groups.Where(g => g.IsActive);

                foreach (var group in groups)
                {
                    GetUsersAndPermissionsForAGroup(@group, dbContext);
                }

                return groups.ToList();
            }
        }

        public Group GetGroupById(int groupId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var group = dbContext.Groups
                    .Include(g => g.UserGroups)
                    .Include(g => g.GroupPermissions)
                    .FirstOrDefault(g => g.GroupId == groupId);

                GetUsersAndPermissionsForAGroup(group, dbContext);

                return group;
            }
        }

        public Group AddNewGroup(Group group, ICollection<User> users, ICollection<Permission> permissions)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var groupWithSimilarNameExists = dbContext.Groups.FirstOrDefault(
                    g => g.GroupName.Trim().Equals(g.GroupName.Trim(), StringComparison.OrdinalIgnoreCase));

                if (groupWithSimilarNameExists != null)
                    throw new DataException("Group with same name already exists");

                if (group.UserGroups == null)
                    group.UserGroups = new List<UserGroup>();

                // add users to groups if any
                foreach (var user in users)
                {
                    var userGroup = new UserGroup {Group = group};

                    var userFound = dbContext.Users.FirstOrDefault(u =>
                        u.Email.Trim().Equals(user.Email.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (userFound == null)
                        dbContext.Users.Add(user);
                    else
                        user.UserId = userFound.UserId;

                    userGroup.User = user;
                    group.UserGroups.Add(userGroup);
                }

                if (group.GroupPermissions == null)
                    group.GroupPermissions = new List<GroupPermission>();

                // add permissions if any
                foreach (var permission in permissions)
                {
                    var groupPermission = new GroupPermission {Group = group};

                    var permissionFound = dbContext.Permissions.FirstOrDefault(p =>
                        p.PermissionType.Trim()
                            .Equals(permission.PermissionType.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (permissionFound == null)
                        dbContext.Permissions.Add(permission);
                    else
                        permission.PermissionId = permissionFound.PermissionId;

                    groupPermission.Permission = permission;
                    group.GroupPermissions.Add(groupPermission);
                }

                dbContext.Groups.Add(group);
                dbContext.SaveChanges();

                return group;
            }

        }

        public void RemoveGroup(int groupId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var group = dbContext.Groups.FirstOrDefault(g => g.GroupId == groupId);

                if (group == null)
                    throw new DataException($"Group with id - {groupId} not found");

                group.IsActive = false;

                dbContext.SaveChanges();
            }
        }

        public Group UpdateGroup(Group @group, ICollection<User> users, ICollection<Permission> permissions)
        {
            // When updating user and groups in many-to-many relationship, we need to save user and group first to get the Ids
            // Then add the UserGroup link and save it to the database
            using (var dbContext = _dbContextFactory.Create())
            {
                var groupToUpdate = dbContext.Groups
                    .Include(g => g.UserGroups)
                    .Include(g => g.GroupPermissions)
                    .FirstOrDefault(g => g.GroupId == group.GroupId);

                if (groupToUpdate == null)
                    throw new DataException($"Group with id - {group.GroupId} not found");

                // Clear old users and add new users
                if (users != null && users.Any())
                {
                    groupToUpdate.UserGroups.Clear();
                    foreach (var user in users)
                    {
                        var userFound = dbContext.Users.FirstOrDefault(u =>
                            u.Email.Trim().Equals(user.Email.Trim(), StringComparison.OrdinalIgnoreCase));

                        if (userFound == null)
                            dbContext.Users.Add(user);
                        else
                            user.UserId = userFound.UserId;
                    }
                }

                // Clear old permissions and add new permissions
                if (permissions != null && permissions.Any())
                {
                    groupToUpdate.GroupPermissions.Clear();
                    foreach (var permission in permissions)
                    {
                        var permissionFound = dbContext.Permissions.FirstOrDefault(p =>
                            p.PermissionType.Trim()
                                .Equals(permission.PermissionType.Trim(), StringComparison.OrdinalIgnoreCase));

                        if (permissionFound == null)
                            dbContext.Permissions.Add(permission);
                        else
                            permission.PermissionId = permissionFound.PermissionId;
                    }
                }

                // add group
                dbContext.Entry(groupToUpdate).CurrentValues.SetValues(group);

                // save users and groups first to generate ids
                dbContext.SaveChanges();

                // now add the link between the user and the groups
                if (users != null && users.Any())
                {
                    foreach (var user in users)
                    {
                        groupToUpdate.UserGroups.Add(
                            new UserGroup
                            {
                                GroupId = group.GroupId,
                                UserId = user.UserId
                            });
                    }
                }

                // now add the link between the groups and the permissions
                if (permissions != null && permissions.Any())
                {
                    foreach (var permission in permissions)
                    {
                        groupToUpdate.GroupPermissions.Add(
                            new GroupPermission
                            {
                                GroupId = group.GroupId,
                                PermissionId = permission.PermissionId
                            });
                    }
                }

                // finally save the link in the linking table
                dbContext.SaveChanges();

                return groupToUpdate;
            }
        }

        private static void GetUsersAndPermissionsForAGroup(Group @group, EaygDbContext dbContext)
        {
            foreach (var userGroup in @group.UserGroups)
            {
                userGroup.User = dbContext.Users.FirstOrDefault(u => u.UserId == userGroup.UserId);
                userGroup.Group = @group;
            }

            foreach (var groupPermission in @group.GroupPermissions)
            {
                groupPermission.Permission =
                    dbContext.Permissions.FirstOrDefault(p => p.PermissionId == groupPermission.PermissionId);
                groupPermission.Group = @group;
            }
        }
    }
}