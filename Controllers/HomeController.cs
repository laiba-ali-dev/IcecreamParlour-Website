using ice_cream.DB;
using ice_cream.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ice_cream.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly mydbcontext context;

        IWebHostEnvironment env;
        public HomeController(ILogger<HomeController> logger, mydbcontext context, IWebHostEnvironment env)
        {
            _logger = logger;
            this.context = context;
            this.env = env;
        }

    

      

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Userregister user)
        {
            if (ModelState.IsValid)
            {
                if (user.PaymentStatus == "Paid")
                {
                    // Calculate Expiry Date
                    if (user.SubscriptionType == "Monthly")
                    {
                        user.ExpiryDate = user.PaymentDate?.AddMonths(1);
                    }
                    else if (user.SubscriptionType == "Yearly")
                    {
                        user.ExpiryDate = user.PaymentDate?.AddYears(1);
                    }

                    await context.userregister.AddAsync(user);
                    await context.SaveChangesAsync();
                    TempData["Success"] = "Registered Successfully. You can now login.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Error"] = "Payment is required to complete the registration.";
                    return View(user);
                }
            }
            return View(user);
        }



        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Index");

            }
            return View();

        }


        [HttpPost]
        public IActionResult Login(Userregister user)
        {
            var Myuser = context.userregister.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (Myuser != null)
            {
                HttpContext.Session.SetString("UserSession", Myuser.Email);
                HttpContext.Session.SetString("UsernameSession", Myuser.Firstname);
                HttpContext.Session.SetString("UserId", Myuser.RegisterId.ToString());


                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Login Failed..";
            }
            return View();

             
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Login"); // Redirect to login page
        }


        public IActionResult Index()
        {
            ViewBag.UsernameSession = HttpContext.Session.GetString("UsernameSession");
            return View();
        }




        public IActionResult Products()
        { // Retrieve UsernameSession from session, if exists
            ViewBag.Mynamesession = HttpContext.Session.GetString("UsernameSession");

            // Fetch all products from the database
            var data = context.icecreams.ToList();

            return View(data);
        }



        public IActionResult Recipes()
        {
            ViewBag.Mynamesession = HttpContext.Session.GetString("UsernameSession");

            // Fetch all products from the database
            var data = context.recipes.ToList();

            return View(data);
        }

      
        public IActionResult Books()
        {
            ViewBag.Mynamesession = HttpContext.Session.GetString("UsernameSession");

            // Fetch all products from the database
            var data = context.books.ToList();

            return View(data);
        }


        // GET Method
        [HttpGet]
        public IActionResult Orderbooks(int id)
        {
            var book = context.books.FirstOrDefault(b => b.BookId == id); // Books table se data lena
            if (book == null)
            {
                return NotFound(); // Agar book nahi milti
            }

            // Form pre-fill karna
            var order = new BooksOrder
            {
                Booktitle = book.Booktittle, // Books table se Booktitle lena
                TotalAmount = book.Price     // Books table se TotalAmount lena
            };

            return View(order); // View ko pre-filled data ke sath return karna
        }

        // POST Method
        //[HttpPost]
        //public IActionResult Orderbooks(BooksOrder model)
        //{
        //    if (ModelState.IsValid) // Validation check karna
        //    {
        //        // Save data to database
        //        var order = new BooksOrder
        //        {
        //            Name = model.Name,                // User se liya gaya Name
        //            Email = model.Email,              // User se liya gaya Email
        //            Phone = model.Phone,              // User se liya gaya Phone
        //            Address = model.Address,          // User se liya gaya Address
        //            TotalAmount = model.TotalAmount,  // Books table se fetched TotalAmount
        //            Paying = model.Paying,            // User se liya gaya Payment method
        //            Booktitle = model.Booktitle,      // Books table se fetched Booktitle
        //            Status = "Pending"

        //        };

        //        context.booksorder.Add(order); // Data context mein add karna
        //        context.SaveChanges(); // Database mein changes save karna

        //        // Success message ya kisi aur page par redirect
        //        return RedirectToAction("Books"); // Example: Success page par redirect
        //    }

        //    // Agar validation fail ho jaye to form wapas dikhaye
        //    return View(model);
        //}

        [HttpPost]
        public IActionResult Orderbooks(BooksOrder model)
        {
            if (ModelState.IsValid) // Validation check karna
            {
                // Save data to database
                var order = new BooksOrder
                {
                    Name = model.Name,                // User se liya gaya Name
                    Email = model.Email,              // User se liya gaya Email
                    Phone = model.Phone,              // User se liya gaya Phone
                    Address = model.Address,          // User se liya gaya Address
                    TotalAmount = model.TotalAmount,  // Books table se fetched TotalAmount
                    Paying = model.Paying,            // User se liya gaya Payment method
                    Booktitle = model.Booktitle,      // Books table se fetched Booktitle
                    Status = "pending"
                };

                context.booksorder.Add(order); // Data context mein add karna
                context.SaveChanges(); // Database mein changes save karna

                // Success message ya kisi aur page par redirect
                return RedirectToAction("Books"); // Example: Success page par redirect
            }

            // Agar validation fail ho jaye to form wapas dikhaye
            return View(model);
        }




        // GET Method
        [HttpGet]
        public IActionResult Ordericecreams(int id)
        {
            var icecream = context.icecreams.FirstOrDefault(b => b.IcecreamId == id); // icecream table se data lena
            if (icecream == null)
            {
                return NotFound(); // Agar icecream nahi milti
            }

            // Form pre-fill karna
            var order = new Icecreamorder
            {
                Icecreamname = icecream.Icecreamname, // icecream table se name lena
                TotalAmount = icecream.Icecreamprice     // icecream table se TotalAmount lena
            };

            return View(order); // View ko pre-filled data ke sath return karna
        }

        // POST Method
        [HttpPost]
        public IActionResult Ordericecreams(Icecreamorder model)
        {
            if (ModelState.IsValid) // Validation check karna
            {
                // Save data to database
                var order = new Icecreamorder
                {
                    Icecreamname =model.Icecreamname,
                    Name = model.Name,                // User se liya gaya Name
                    Email = model.Email,              // User se liya gaya Email
                    Phone = model.Phone,              // User se liya gaya Phone
                    Address = model.Address,          // User se liya gaya Address
                    TotalAmount = model.TotalAmount,  // Books table se fetched TotalAmount
                    Paying = model.Paying,            // User se liya gaya Payment method
                    Status = "pending"
                };

                context.icecreamorder.Add(order); // Data context mein add karna
                context.SaveChanges(); // Database mein changes save karna

                // Success message ya kisi aur page par redirect
                return RedirectToAction("Products"); // Example: Success page par redirect
            }

            // Agar validation fail ho jaye to form wapas dikhaye
            return View(model);
        }





        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs(Contactus contact)
        {
            if (ModelState.IsValid)
            {
                // Save data to the database
                await context.contactus.AddAsync(contact);
                await context.SaveChangesAsync();

                TempData["Success"] = "Your message has been submitted successfully!";
                return RedirectToAction("ContactUs"); // Redirect to clear the form
            }

            // If validation fails, display the form with errors
            TempData["Error"] = "There was an error submitting your message. Please check your input.";
            return View(contact);
        }


        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Feedback(Feedback feedb)
        {
            if (ModelState.IsValid)
            {
                // Save data to the database
                await context.feedback.AddAsync(feedb);
                await context.SaveChangesAsync();

                TempData["Success"] = "Your feedback has been submitted successfully!";
                return RedirectToAction("Feedback"); // Redirect to clear the form
            }

            // If validation fails, display the form with errors
            TempData["Error"] = "There was an error submitting your feedback. Please check your input.";
            return View(feedb);
        }

        public IActionResult About()
        {
            return View();
        }
       

       

       



    
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}