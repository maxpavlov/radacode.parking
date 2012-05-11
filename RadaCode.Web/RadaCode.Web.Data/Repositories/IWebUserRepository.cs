using System;
using System.Collections.Generic;
using System.Linq;
using RadaCode.Web.Data.Entities;

namespace RadaCode.Web.Data.Repositories
{
    public interface IWebUserRepository
    {
        IQueryable<WebUser> GetAllUsers();

        WebUser GetUser(Guid userId);

        WebUser GetUser(string userName);

        int GetNumberOfUsersActiveAfter(DateTime afterWhen);

        int CountUsersWithName(string nameToMatch);
        IQueryable<WebUser> UsersWithNamePattern(string nameToMatch);

        int TotalUsersCount();

        IQueryable<WebUser> GetUsersAsQueryable();

        IQueryable<WebUser> GetUsersInRole(string roleName);
        IQueryable<WebUser> GetUsersInRole(Guid roleId);
        IQueryable<WebUser> GetUsersInRole(WebUserRole role);
        IQueryable<WebUserRole> GetAllRoles();

        WebUserRole GetRole(Guid id);

        WebUserRole GetRole(string name);

        IList<WebUserRole> GetRolesForUser(string userName);
        IList<WebUserRole> GetRolesForUser(Guid userId);
        IList<WebUserRole> GetRolesForUser(WebUser user);

        WebUser CreateUser(string username, string password, string email);

        void DeleteUser(WebUser user);
        void DeleteUser(string userName);


        void AddRole(WebUserRole role);
        void AddRole(string roleName);

        void AddRoleToUser(Guid userId, string roleName);
        void AddRoleToUser(string userName, string roleName);
        void AddRoleToUser(WebUser user, WebUserRole role);

        void DeleteRole(WebUserRole role);
        void DeleteRole(string roleName);

        void SaveChanges();

        bool UserExists(WebUser user);
        bool RoleExists(WebUserRole role);
        bool UserNameTaken(string userName);
    }
}
