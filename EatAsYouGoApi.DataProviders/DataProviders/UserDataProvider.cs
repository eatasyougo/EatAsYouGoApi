using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using EatAsYouGoApi.DataLayer.DataModels;
using EatAsYouGoApi.DataLayer.DataProviders.Interfaces;

namespace EatAsYouGoApi.DataLayer.DataProviders
{
    public class UserDataProvider : IUserDataProvider
    {
        private readonly IDbContextFactory<EaygDbContext> _dbContextFactory;

        public UserDataProvider(IDbContextFactory<EaygDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IEnumerable<User> GetAllUsers(bool showActiveUsersOnly = false)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var users = dbContext.Users.Include(user => user.UserGroups);
                if (showActiveUsersOnly)
                    users = users.Where(user => user.IsActive);

                return users.ToList();
            }
        }

        public User GetUserById(long userId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Users.Include(user => user.UserGroups)
                    .FirstOrDefault(user => user.UserId == userId);
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                return dbContext.Users.Include(user => user.UserGroups)
                    .FirstOrDefault(user => user.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
        }

        public User AddNewUser(User user, ICollection<Group> groups)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var userWithSameEmailExists = dbContext.Users.FirstOrDefault(
                    u => u.Email.Trim().Equals(user.Email.Trim(), StringComparison.OrdinalIgnoreCase));

                if (userWithSameEmailExists != null)
                    throw new DataException($"User with same email - {user.Email.Trim()} already exists");

                if (user.UserGroups == null)
                    user.UserGroups = new List<UserGroup>();

                foreach (var group in groups)
                {
                    var userGroup = new UserGroup {User = user};

                    var groupFound = dbContext.Groups.FirstOrDefault(g =>
                        g.GroupName.Trim().Equals(group.GroupName.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (groupFound == null)
                        dbContext.Groups.Add(group);
                    else
                        group.GroupId = groupFound.GroupId;

                    userGroup.Group = group;
                    user.UserGroups.Add(userGroup);
                }

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                return user;
            }
        }

        public void RemoveUser(long userId)
        {
            using (var dbContext = _dbContextFactory.Create())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                    throw new DataException($"User with id - {userId} not found");

                user.IsActive = false;
                dbContext.SaveChanges();
            }
        }

        public User UpdateUser(User user, ICollection<Group> groups)
        {
            // When updating user and groups in many-to-many relationship, we need to save user and group first to get the Ids
            // Then add the UserGroup link and save it to the database
            using (var dbContext = _dbContextFactory.Create())
            {
                var userToUpdate = dbContext.Users
                    .Include(u => u.UserGroups)
                    .FirstOrDefault(u => u.UserId == user.UserId);

                if (userToUpdate == null)
                    throw new DataException($"User with id - {user.UserId} not found");

                // Clear old groups and add new groups
                if (groups != null && groups.Any())
                {
                    userToUpdate.UserGroups.Clear();
                    foreach (var group in groups)
                    {
                        var groupFound = dbContext.Groups.FirstOrDefault(g =>
                            g.GroupName.Trim().Equals(group.GroupName.Trim(), StringComparison.OrdinalIgnoreCase));

                        if (groupFound == null)
                            dbContext.Groups.Add(group);
                        else
                            group.GroupId = groupFound.GroupId;
                    }
                }

                // add users
                dbContext.Entry(userToUpdate).CurrentValues.SetValues(user);

                // save users and groups first to generate ids
                dbContext.SaveChanges();

                // now add the link between the user and the groups
                if (groups == null || !groups.Any())
                    return userToUpdate;

                foreach (var g in groups)
                {
                    userToUpdate.UserGroups.Add(
                        new UserGroup
                        {
                            GroupId = g.GroupId,
                            UserId = user.UserId
                        });
                }

                // finally save the link in the linking table
                dbContext.SaveChanges();

                return userToUpdate;
            }
        }
    }
}