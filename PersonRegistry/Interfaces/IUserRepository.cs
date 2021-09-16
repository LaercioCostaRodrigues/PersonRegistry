using PersonRegistry.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


namespace PersonRegistry.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        void State(User user);
        bool UserExists(string id);
        void Add(User user);
        void SaveChanges();
        User Find(string id);
        void Remove(User user);
        string IdGenerator();
    }
}