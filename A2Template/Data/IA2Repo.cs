using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using A2.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace A2.Data
{
    public interface IA2Repo
    {

        //public Product GetProductByID(int id);
        //public ActionResult<String> Register(User user);
        public IEnumerable<Product> GetAllProducts();
        public Product GetProductByID(int id);
        public Comment GetCommentByID(int id);
        public IEnumerable<Product> GetItemsByName(string name);
        public Comment AddWriteComment(Comment comment);
        public IEnumerable<Comment> GetAllComment();
        public void SaveChanges();
       
        public User UserInDbbool(User user);
        public User AddUser(User user);

        public bool ValidLogin(string userName, string password);

        public bool ValidLoginOrganizer(string userName, string password);

        public Event AddEvent(Event newEvent);
        public int numEvent();

        public Event GetEvent(int id);




        //public IEnumerable<Customer> GetAllCustomers();
        //public bool ValidLogin(string userName, string password);
        //public Customer GetCustomerByEmail(string e);

    }
}
