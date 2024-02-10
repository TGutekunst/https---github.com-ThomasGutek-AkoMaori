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

        public Product GetProductByID(int id)
        {
            Product product = _dbContext.Products.FirstOrDefault(e => e.Id == id);
            return product;
        }
        

        public IEnumerable<Product> GetItemsByName(string term)
        {
            IEnumerable<Product> products = _dbContext.Products.Where(e => (e.Name.ToLower()).Contains(term.ToLower()));
            return products;
        }

        public Comment GetCommentByID(int id)
        {
            Comment comment = _dbContext.Comments.FirstOrDefault(e => e.Id == id);
            return comment;
        }

        public Comment AddWriteComment(Comment comment)
        {
            EntityEntry<Comment> e = _dbContext.Comments.Add(comment);
            Comment c = e.Entity;
            _dbContext.SaveChanges();
            return c;
        }
        public IEnumerable<Comment> GetAllComment()
        {
            IEnumerable<Comment> comments = _dbContext.Comments.ToList<Comment>();

            return comments;
        }
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
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
        public bool ValidLogin(string userName, string password)
        {
            User c = _dbContext.Users.FirstOrDefault(e => e.UserName == userName && e.Password == password);
            if (c == null)
                return false;
            else
                return true;
        }
        public bool ValidLoginOrganizer(string userName, string password)
        {
            Organizer c = _dbContext.Organizers.FirstOrDefault(e => e.Name == userName && e.Password == password);
            if (c == null)
                return false;
            else
                return true;
        }
        public Event AddEvent(Event newEvent)
        {
            EntityEntry<Event> e = _dbContext.Events.Add(newEvent);
            Event c = e.Entity;
            _dbContext.SaveChanges();
            return c;
        }
        public int numEvent()
        {
            IEnumerable<Event> events = _dbContext.Events.ToList<Event>();
            int num = events.Count();
            return num;
        }
        public Event GetEvent(int id)
        {
            Event dbEvent = _dbContext.Events.FirstOrDefault(e => e.Id == id);
            return dbEvent;

        }

    }
}