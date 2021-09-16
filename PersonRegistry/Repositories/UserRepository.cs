using PersonRegistry.Data;
using PersonRegistry.Models;
using PersonRegistry.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace PersonRegistry.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PersonRegistryContext _context;

        public UserRepository(PersonRegistryContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.User.ToList();
        }

        public void State(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
        
        public void Add(User user)
        {
            _context.User.Add(user);
        }

        public void SaveChanges()
        {
           _context.SaveChanges();
        }

        public User Find(string id)
        {
            return _context.User.Find(id);
        }

        public void Remove(User user)
        {
            _context.User.Remove(user);
        }
        
        public bool UserExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public string IdGenerator()
        {
            Random random = new Random();
            string id = string.Empty;

            id += (char)random.Next(97, 123);
            id += random.Next(0, 10);
            id += (char)random.Next(97, 123);
            id += random.Next(0, 10);
            id += (char)random.Next(97, 123);

            return id + '-' + id + '-' + id + '-' + id + '-' + id;
        }

    }
}