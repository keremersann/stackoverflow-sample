using AutoMapper;
using StackOverFlowClone.DomainModels;
using StackOverFlowClone.Repositories;
using StackOverFlowClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.ServiceLayer
{
    public interface IUsersService
    {
        int InsertUser(RegisterViewModel model);
        void UpdateUserDetails(EditUserDetailsViewModel model);
        void UpdateUserPassword(EditUserPasswordViewModel model);
        void DeleteUser(int userId);
        List<UserViewModel> GetUsers();
        UserViewModel GetUsersById(int id);
        UserViewModel GetUsersByEmail(string email);
        UserViewModel GetUsersByEmailAndPassword(string email, string password);
    }
    public class UsersService : IUsersService
    {
        IUsersRepository ur;
        public UsersService()
        {
            ur = new UsersRepository();
        }
        public int InsertUser(RegisterViewModel rvm)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterViewModel, User>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            User u = mapper.Map<User>(rvm);
            u.PasswordHash = SHA256HashGenerator.GenerateHash(rvm.Password);
            ur.InsertUser(u);
            return ur.GetLatestUserId();
        }
        public void UpdateUserDetails(EditUserDetailsViewModel eudvm)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EditUserDetailsViewModel, User>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            User u = mapper.Map<User>(eudvm);
            ur.UpdateUserDetails(u);
        }
        public void UpdateUserPassword(EditUserPasswordViewModel eupvm)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EditUserPasswordViewModel, User>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            User u = mapper.Map<User>(eupvm);
            u.PasswordHash = SHA256HashGenerator.GenerateHash(eupvm.Password);
            ur.UpdateUserPassword(u);
        }
        public void DeleteUser(int userId)
        {
            ur.DeleteUser(userId);
        }
        public List<UserViewModel> GetUsers()
        {
            List<User> users = ur.GetUsers();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserViewModel>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            List<UserViewModel> uvm = mapper.Map<List<User>, List<UserViewModel>>(users);
            return uvm;
        }
        public UserViewModel GetUsersByEmail(string email)
        {
            User user = ur.GetUsersByEmail(email).FirstOrDefault();
            UserViewModel uvm = null;
            if (user != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(user);
            }
            return uvm;
        }
        public UserViewModel GetUsersById(int id)
        {
            User user = ur.GetUsersByUserId(id).FirstOrDefault();
            UserViewModel uvm = null;
            if (user != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(user);
                uvm.Password = user.PasswordHash;
            }
            return uvm;
        }
        public UserViewModel GetUsersByEmailAndPassword(string email, string password)
        {
            User user = ur.GetUsersByEmailAndPassword(email, SHA256HashGenerator.GenerateHash(password)).FirstOrDefault();
            UserViewModel uvm = null;
            if (user != null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<User, UserViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                uvm = mapper.Map<User, UserViewModel>(user);
            }
            return uvm;
        }
    }
}
