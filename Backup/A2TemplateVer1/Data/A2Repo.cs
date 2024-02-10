using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using A2.Models;
using A2.Data;



namespace A2.Data
{
    public class A2Repo : IA2Repo
    {

        private readonly A2DbContext _dbContext;
        //private readonly IA1Repo _repository;
        public A2Repo(A2DbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<Product> GetAllProducts()
        {

            IEnumerable<Product> products = _dbContext.Products.ToList<Product>();

            return products;
        }

        public User UserInDbbool(User user)
        {
            //userInDb = _repository.UserInDbbool(user);
            User dbUser = _dbContext.Users.FirstOrDefault(e => e.UserName == user.UserName);

            //IEnumerable<User> user = _dbContext.Users.ToList<User>();

            return dbUser;
        }
        public User AddUser(User user)
        {
            EntityEntry<User> e = _dbContext.Users.Add(user);
            User u = e.Entity;
            _dbContext.SaveChanges();
            return u;
        }
    }
}