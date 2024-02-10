
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A2.Dtos;
using A2.Models;
using A2.Data;
using A2.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Collections;


namespace A2.Controllers
{
    [Route("webapi")]
    [ApiController]
    public class A2Controller : Controller
    {
        private readonly IA2Repo _repository;


        public A2Controller(IA2Repo repository)
        {
            _repository = repository;
        }
         [HttpGet("GetVersion")]
        public ActionResult<string> GetVersion(){
            return Ok("1.0.0 (Ngongotahā) by zjes252");

        }


        [HttpGet("Logo")]
        public ActionResult Logo()
        {
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, "Logos");
            string fileName = Path.Combine(imgDir, "Logo" + ".png");
            string respHeader = "image/png";
            return PhysicalFile(fileName, respHeader);
        }
        [HttpGet("AllItems")]
        public ActionResult<IEnumerable<Product>> AllItems()
        {
            IEnumerable<Product> products = _repository.GetAllProducts();
            return Ok(products);

        }

        // GET /Items/{term}
        [HttpGet("Items/{term}")]
        public ActionResult<IEnumerable<Product>> Items(string term)
        {
            IEnumerable<Product> products = _repository.GetItemsByName(term);

            return Ok(products);

        }



        [HttpGet("ItemImage/{id}")]
        public ActionResult ItemImage(string id)
        {
            string path = Directory.GetCurrentDirectory();
            string imgDir = Path.Combine(path, "ItemsImages");
            string fileName1 = Path.Combine(imgDir, id + ".png");
            string fileName2 = Path.Combine(imgDir, id + ".jpg");
            string fileName3 = Path.Combine(imgDir, id + ".gif");
            string fileName4 = Path.Combine(imgDir, id + ".svg");
            string fileName5 = Path.Combine(imgDir, "default.png");
            string respHeader = "";
            string fileName = "";

            if (System.IO.File.Exists(fileName2))
            {
                respHeader = "image/jpeg";
                fileName = fileName2;

            }
            else if (System.IO.File.Exists(fileName1))
            {

                respHeader = "image/png";
                fileName = fileName1;

            }
            else if (System.IO.File.Exists(fileName3))
            {
                respHeader = "image/gif";
                fileName = fileName3;

            }
            else if (System.IO.File.Exists(fileName4))
            {
                
                respHeader = "image/svg+xml";
                fileName = fileName4;
                
                //return < svg width = "100" height = "100" >{ { PhysicalFile(fileName5, "image/png") } }</ svg > ;
            }
            else
            {
                return PhysicalFile(fileName5, "image/png");
            }

            return PhysicalFile(fileName, respHeader);
        }

        // GET /GetComment/{id}
        [HttpGet("GetComment/{id}")]
        public ActionResult<Comment> GetComment(int id)
        {
            Comment comment = _repository.GetCommentByID(id);

            if (comment == null)
                return BadRequest("Comment " + id.ToString() + " does not exist.");

            else
            {
                return Ok(comment);
            }
            

        }
        [HttpPost("WriteComment")]
        public ActionResult<Comment> WriteComment(CommentInputDto comment)

        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("yyyyMMddTHHmmssZ");
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Comment c = new Comment {UserComment = comment.UserComment, Name = comment.Name };
            c.IP = ipAddress;
            c.Time = formattedDateTime;
            Comment addedComment = _repository.AddWriteComment(c);
            return CreatedAtAction(nameof(GetComment), new { id = addedComment.Id }, addedComment);
        }
        // GET /webapi/Comments/{num}
        [HttpGet("Comments/{num}")]
        public ActionResult<IEnumerable<Comment>> Comments(int num=5)
        {
            
            IEnumerable<Comment> comments = _repository.GetAllComment().Reverse();
            IEnumerable<Comment> toComments = new List<Comment>();
            int itemInList = comments.Count();

            if (itemInList >= num)
            {
                toComments = comments.Take(num);
            }
            else if (itemInList < num)
            {
                toComments = comments;
            }
            return Ok(toComments);

        }
        [HttpPost("Register")]
        public ActionResult<String> Register(User user)
        {
            User userInDb = _repository.UserInDbbool(user);

            if (userInDb == null)
            {
                User u = new User { UserName = user.UserName, Password = user.Password, Address = user.Address };
                //c.IP = ipAddress;
                //c.Time = formattedDateTime;
                User addedUser = _repository.AddUser(u);
                //CreatedAtAction(nameof(u), new { userName = addedUser.UserName }, addedUser);
                return Ok("User successfully registered.");
            }
            return Ok("UserName " + user.UserName + " is not available.");


            //DateTime dt = DateTime.ParseExact(inputString, formatString, System.Globalization.CultureInfo.InvariantCulture);
        }

        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "UserOnly")]
        [HttpGet("PurchaseItem/{id}")]
        public ActionResult<PurchaseOutput> PurchaseItem(int id)
        {
            //PurchaseOutput p = new PurchaseOutput { }
            //return Ok(PurchaseOutput)
            Product product = _repository.GetProductByID(id);

            ClaimsIdentity ci = HttpContext.User.Identities.FirstOrDefault();
            Claim c = ci.FindFirst("user");
            string username = c.Value;

            //User user = _repository.GetUserByInputID();
            if (product == null)
                return BadRequest("Product " + id + " not found");
                //return BadRequest("Product " + id + " not found");
            else
            {

                PurchaseOutput p = new PurchaseOutput { UserName = username, ProductID = product.Id };
                return Ok(p);


                //DateTime dt = DateTime.ParseExact(inputString, formatString, System.Globalization.CultureInfo.InvariantCulture);
            }
        }


        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("AddEvent")]
        public ActionResult<string> AddEvent(EventInput newEvent)
        {
            DateTime parsedDate;
            string format = "yyyyMMddTHHmmssZ";
            bool validStart = DateTime.TryParseExact(newEvent.Start, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            bool validEnd = DateTime.TryParseExact(newEvent.End, format, null, System.Globalization.DateTimeStyles.None, out parsedDate);
            var myData = new
            {
                message = "Bad request",
                errorCode = 400,
                detail = "The format of Start and End should be yyyyMMddTHHmmssZ.",
            };
            if (validStart == false)
            {
                return BadRequest(myData);
            }
            if (validEnd == false)
            {
                return BadRequest(myData);
            }
            if (validStart && validEnd == false)
            {
                return BadRequest(myData);
            }
            Event eventInput = new Event { Start = newEvent.Start, End = newEvent.End, Summary = newEvent.Summary, Description = newEvent.Description, Location = newEvent.Location };
            Event newEventInput = _repository.AddEvent(eventInput);
            return Ok("Success");

        }
        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "AdminAndUser")]
        [HttpGet("EventCount")]
        public ActionResult<int> EventCount()
        {
            int eventCount = _repository.numEvent();
            return eventCount;

        }
        [Authorize(AuthenticationSchemes = "Authentication")]
        [Authorize(Policy = "AdminAndUser")]
        [HttpGet("Event/{id}")]
        public ActionResult Event(int id)
        {
            Event selectedEvent = _repository.GetEvent(id);
            if (selectedEvent == null)
            {
                //return BadRequest("The value '" + id + "' is not valid.");
                return BadRequest("Event " + id + " does not exist.");
            }
            Response.Headers.Add("Content-Type", "text/calendar");
            return Ok(selectedEvent);

        }


    }

}
    










//response.header.add("Content-Type", "text/calender"):