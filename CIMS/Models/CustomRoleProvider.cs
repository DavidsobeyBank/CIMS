using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;


namespace CIMS.Models
{
    public class CustomRoleProvider: RoleProvider
    {
        public override string ApplicationName { get ; set ; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (CIMS_NEWEntities db = new CIMS_NEWEntities())
            {
                username = GetANumber(username);
                User user = db.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase) || u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));
                try
                {
                    var roles = from ur in user.UserRoles
                                from r in db.Roles
                                where ur.RoleID == r.RoleID
                                select r.RoleName;

                    if (roles != null)
                        return roles.ToArray();
                    else
                        return new string[] { };
                }
                catch(Exception)
                {
                    return new string[] { };
                }
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (CIMS_NEWEntities db = new CIMS_NEWEntities())
            {
                User user = db.Users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.CurrentCultureIgnoreCase) || u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));

                var roles = from ur in user.UserRoles
                            from r in db.Roles
                            where ur.RoleID == r.RoleID
                            select r.RoleName;
                if (user != null)
                    return roles.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
                else
                    return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        public string GetANumber(string username)
        {
            string[] sArray = username.Split('\\');
            return sArray.Last<string>();
        }
    }
}