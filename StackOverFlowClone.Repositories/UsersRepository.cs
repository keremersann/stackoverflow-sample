using StackOverFlowClone.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Repositories
{

    public interface IUsersRepository
    {
        void InsertUser(User u);
        void UpdateUserDetails(User u);
        void UpdateUserPassword(User u);
        void DeleteUser(int userId);
        List<User> GetUsers();
        List<User> GetUsersByEmail(string email);
        List<User> GetUsersByEmailAndPassword(string email, string passwordHash);
        List<User> GetUsersByUserId(int userId);
        int GetLatestUserId();
    }
    public class UsersRepository : IUsersRepository
    {
        StackOverflowDatabaseDBContext db;

        public UsersRepository()
        {
            db = new StackOverflowDatabaseDBContext();
        }
        public void InsertUser(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();
        }
        public void UpdateUserPassword(User u)
        {
            User userToBeUpdated = db.Users.Where(user => u.UserID == user.UserID).FirstOrDefault();
            if (userToBeUpdated != null) 
            {
                userToBeUpdated.PasswordHash = u.PasswordHash;
                db.SaveChanges();
            }
        }
        public void UpdateUserDetails(User u)
        {
            User userToBeUpdated = db.Users.Where(user => u.UserID == user.UserID).FirstOrDefault();
            if (userToBeUpdated != null)
            {
                userToBeUpdated.Name = u.Name;
                userToBeUpdated.Mobile = u.Mobile;
                db.SaveChanges();
            }
        }
        public void DeleteUser(int userId)
        {
            User userToBeDeleted = db.Users.Where(user => user.UserID == userId).FirstOrDefault();
            if(userToBeDeleted != null)
            {
                db.Users.Remove(userToBeDeleted);
                db.SaveChanges();
            }
        }
        public List<User> GetUsers()
        {
            return db.Users.Where(user => user.IsAdmin == false).OrderBy(user => user.Name).ToList();
        }
        public List<User> GetUsersByEmail(string email)
        {
            return db.Users.Where(user => user.Email == email).ToList();
        }
        public List<User> GetUsersByEmailAndPassword(string email, string passwordHash)
        {
            return db.Users.Where(user => user.Email == email && user.PasswordHash == passwordHash).ToList();
        }
        public List<User> GetUsersByUserId(int userId)
        {
            return db.Users.Where(user => user.UserID == userId).ToList();
        }
        public int GetLatestUserId()
        {
            return db.Users.Select(user => user.UserID).Max();   
        }
    }
}
