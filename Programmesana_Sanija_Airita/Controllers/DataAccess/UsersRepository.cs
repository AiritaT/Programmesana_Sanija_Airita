using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Programmesana_Sanija_Airita.Models;
namespace Programmesana_Sanija_Airita.Controllers.DataAccess
{
    public class UsersRepository : ConnectionClass
    {

        public UsersRepository() : base()
        {

        }

        public void AddUser(User u)
        {
            Entity.Users.Add(u);
            Entity.SaveChanges();
        }
        public void AllocateRoleToUser(User u, Role r)
        {
        u.Roles.Add(r);
        Entity.SaveChanges();
        }
        public User GetUserByUsername(string username)
        {
            return Entity.Users.SingleOrDefault(x => x.Username == username);
        }
        public bool DoesUsernameExist(string username)
        {
            if(Entity.Users.SingleOrDefault(x=>x.Username == username) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool Login(string username, string password)
        {
            if (Entity.Users.SingleOrDefault(x => x.Username == username && x.Password == password) == null)
                return false;
            else return true;
        }
        public IQueryable<User> GetUsers()
        {
            return Entity.Users;
        }
       /* public void DeleteUser(User user)
        {
            Entity.Users.Remove(user);
            Entity.SaveChanges();
        }*/
        public Role GetDefaultRole()
        {
            return Entity.Roles.SingleOrDefault(x => x.DefaultRole == true);
        }
    }
  
}