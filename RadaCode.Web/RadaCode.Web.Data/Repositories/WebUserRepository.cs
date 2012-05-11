using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using RadaCode.Web.Data.EF;
using RadaCode.Web.Data.Entities;

namespace RadaCode.Web.Data.Repositories
{
    public class WebUserRepository : IWebUserRepository
    {
        private WebStoreContext _db; 

        public WebUserRepository(WebStoreContext context)
        {
            _db = context;
            _db.WebUsers.ToList();
        }

        public IQueryable<WebUser> GetAllUsers()
        {
            return _db.WebUsers;
        }

        public WebUser GetUser(Guid userId)
        {
            return _db.WebUsers.SingleOrDefault(usr => usr.Id == userId);
        }

        public WebUser GetUser(string userName)
        {
            return _db.WebUsers.SingleOrDefault(usr => usr.UserName == userName);
        }

        public int GetNumberOfUsersActiveAfter(DateTime afterWhen)
        {
            return _db.WebUsers.Where(usr => usr.LastActivityDate > afterWhen).Count();
        }

        public int CountUsersWithName(string nameToMatch)
        {
            return _db.WebUsers.Where(usr => usr.UserName.Contains(nameToMatch)).Count();
        }

        public IQueryable<WebUser> UsersWithNamePattern(string nameToMatch)
        {
            return _db.WebUsers.Where(usr => usr.UserName.Contains(nameToMatch)).OrderBy(usr => usr.UserName);
        }

        public int TotalUsersCount()
        {
            return _db.WebUsers.Count();
        }

        public IQueryable<WebUser> GetUsersAsQueryable()
        {
            return _db.WebUsers.OrderBy(usr => usr.UserName);
        }

        public IQueryable<WebUser> GetUsersInRole(string roleName)
        {
            return GetUsersInRole(GetRole(roleName));
        }

        public IQueryable<WebUser> GetUsersInRole(Guid roleId)
        {
            return GetUsersInRole(GetRole(roleId));
        }

        public IQueryable<WebUser> GetUsersInRole(WebUserRole role)
        {
            if (!RoleExists(role))
                throw new ArgumentException("MissingRole");

            return _db.WebUserRoles.SingleOrDefault(rl => rl.Id == role.Id).Users.AsQueryable();
        }

        public IQueryable<WebUserRole> GetAllRoles()
        {
            return _db.WebUserRoles;
        }

        public WebUserRole GetRole(Guid id)
        {
            return _db.WebUserRoles.SingleOrDefault(rl => rl.Id == id);
        }

        public WebUserRole GetRole(string name)
        {
            return _db.WebUserRoles.SingleOrDefault(rl => rl.RoleName == name);
        }

        public IList<WebUserRole> GetRolesForUser(string userName)
        {
            return _db.WebUsers.SingleOrDefault(usr => usr.UserName == userName).Roles;
        }

        public IList<WebUserRole> GetRolesForUser(Guid userId)
        {
            return _db.WebUsers.SingleOrDefault(usr => usr.Id == userId).Roles;
        }

        public IList<WebUserRole> GetRolesForUser(WebUser user)
        {
            return GetRolesForUser(user.Id);
        }

        private void AddUser(WebUser user)
        {
            if (UserExists(user))
                throw new ArgumentException("UserAlreadyExists");

            _db.WebUsers.Add(user);
        }

        public WebUser CreateUser(string userName, string password, string email)
        {
            if (string.IsNullOrEmpty(userName.Trim()))
                throw new ArgumentException("The user name provided is invalid. Please check the value and try again.");
            if (string.IsNullOrEmpty(password.Trim()))
                throw new ArgumentException("The password provided is invalid. Please enter a valid password value.");
            if (_db.WebUsers.Any(user => user.UserName == userName))
                throw new ArgumentException("Username already exists. Please enter a different user name.");

            var newUser = new WebUser()
                              {
                                  UserName = userName,
                                  Password = password,
                                  Email = email,
                                  CreateDate = DateTime.UtcNow,
                                  IsLockedOut = false,
                                  LastActivityDate = SqlDateTime.MinValue.Value,
                                  LastLoginDate = SqlDateTime.MinValue.Value,
                                  LastLockoutDate = SqlDateTime.MinValue.Value,
                                  LastPasswordChangedDate = SqlDateTime.MinValue.Value,
                                  LastPasswordFailureDate = SqlDateTime.MinValue.Value,
                                  PasswordVerificationTokenExpirationDate = SqlDateTime.MinValue.Value
            };

            try
            {
                AddUser(newUser);
            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The authentication provider returned an error. Please verify your entry and try again. " +
                    "If the problem persists, please contact your system administrator.");
            }

            _db.SaveChanges();

            return newUser;
        }

        public void DeleteUser(WebUser user)
        {
            if (!UserExists(user))
                throw new ArgumentException("MissingUser");

            _db.WebUsers.Remove(user);
        }

        public void DeleteUser(string userName)
        {
            DeleteUser(GetUser(userName));
        }

        public void AddRole(WebUserRole role)
        {
            if (RoleExists(role))
                throw new ArgumentException("RoleAlreadyExists");

            _db.WebUserRoles.Add(role);
        }

        public void AddRole(string roleName)
        {
            var role = new WebUserRole()
            {
                RoleName = roleName
            };

            AddRole(role);
        }

        public void AddRoleToUser(Guid userId, string roleName)
        {
            var usr = _db.WebUsers.SingleOrDefault(u => u.Id == userId);
            var rl = _db.WebUserRoles.SingleOrDefault(r => r.RoleName == roleName);
            AddRoleToUser(usr, rl);
        }

        public void AddRoleToUser(string userName, string roleName)
        {
            var usr = _db.WebUsers.SingleOrDefault(u => u.UserName == userName);
            var rl = _db.WebUserRoles.SingleOrDefault(r => r.RoleName == roleName);
            AddRoleToUser(usr, rl);
        }

        public void AddRoleToUser(WebUser user, WebUserRole role)
        {
            user.Roles.Add(role);
        }

        public void DeleteRole(WebUserRole role)
        {
            if (!RoleExists(role))
                throw new ArgumentException("Role doesn't exist");

            _db.WebUserRoles.Remove(role);
        }

        public void DeleteRole(string roleName)
        {
            DeleteRole(GetRole(roleName));
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public bool UserExists(WebUser user)
        {
            if (user == null)
                return false;
 
            return (_db.WebUsers.SingleOrDefault(u => u.Id == user.Id || u.UserName == user.UserName) != null);
        }

        public bool UserNameTaken(string userName)
        {
            if (userName == String.Empty) return true;

            return (_db.WebUsers.SingleOrDefault(u => u.UserName == userName) != null);
        }

        public bool RoleExists(WebUserRole role)
        {
            if (role == null)
                return false;

            return (_db.WebUserRoles.SingleOrDefault(r => r.Id == role.Id || r.RoleName == role.RoleName) != null);
        }
    }
}