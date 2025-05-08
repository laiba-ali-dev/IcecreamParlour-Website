using ice_cream.DB;
using ice_cream.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For async methods like FirstOrDefaultAsync
using System.Linq; // For LINQ methods like FirstOrDefault


namespace ice_cream.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly mydbcontext context;

        IWebHostEnvironment env;
        public AdminController(ILogger<AdminController> logger, mydbcontext context, IWebHostEnvironment env)
        {
            _logger = logger;
            this.context = context;
            this.env = env;
        }
      
        public IActionResult Adminregister()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Adminregister(Adminregister user)
        {
            if (ModelState.IsValid)
            {
                // Check if user already exists by email
                var existingUser = await context.adminregister.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(user);
                }

                // Add the new user to the database
                await context.adminregister.AddAsync(user);
                await context.SaveChangesAsync();

                TempData["Success"] = "Registered Successfully";
                return RedirectToAction("AdminLogin"); // Redirect to Login page after successful registration
            }
            return View(user); // If validation fails, return to the same page with error messages
        }


        public IActionResult AdminLogin()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                return RedirectToAction("AdminDashboard");

            }
            return View();

        }


        [HttpPost]
        public async Task <IActionResult> AdminLogin(Adminregister user)
        {
            var Myuser = await context.adminregister.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (Myuser != null)
            {
                HttpContext.Session.SetString("AdminSession", Myuser.Email);
                HttpContext.Session.SetString("Adminnamesession", Myuser.Firstname);
                HttpContext.Session.SetString("AdminId", Myuser.AdminId.ToString());


                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Login Failed..";
            }
            return View();
        }

        public IActionResult AdminLogout()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                HttpContext.Session.Remove("AdminSession");
                HttpContext.Session.Remove("Adminnamesession");
                HttpContext.Session.Remove("AdminId");

                return RedirectToAction("AdminLogin");

            }
            return View();
        }




        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mysession = HttpContext.Session.GetString("AdminSession").ToString();
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                ViewBag.Myidsession = HttpContext.Session.GetString("AdminId").ToString();



            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
            return View();

        }



     //CRUD FOR ICECREAM ADDED , UPDATED , DELETED

        public IActionResult AddProducts()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult AddProducts(IcecreamsViewModel emp)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }

            string fileName = "";
            if (emp.Photo != null)
            {
                var ext = Path.GetExtension(emp.Photo.FileName);
                var size = emp.Photo.Length;
                if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                {

                    if (size <= 1000000)//1 MB
                    {

                        string folder = Path.Combine(env.WebRootPath, "images");
                        fileName = Guid.NewGuid().ToString() + "_" + emp.Photo.FileName;
                        string filePath = Path.Combine(folder, fileName);
                        emp.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                        Icecreams p = new Icecreams()
                        {
                            Icecreamname = emp.Icecreamname,
                            Icecreamprice = emp.Icecreamprice,
                            
                            Image = fileName,
                            
                        };
                        context.icecreams.Add(p);
                        context.SaveChanges();
                        TempData["Success"] = "Icecream Added..";
                        return RedirectToAction("Productslist");
                    }
                    else
                    {
                        TempData["Size_Error"] = "Image must be less than 1 MB";
                    }
                }
                else
                {
                    TempData["Ext_Error"] = "Only PNG, JPG, JPEG iamges allowed";
                }

            }

            return View();
        }


        public IActionResult Productslist()
        {

            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                var data = context.icecreams.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        public async Task<IActionResult> EditProducts(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                if (Id == null || context.icecreams == null)
                {
                    return NotFound();
                }
                var data = await context.icecreams.FindAsync(Id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProducts(int? Id, Icecreams emp)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }

            if (Id != emp.IcecreamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                context.icecreams.Update(emp);
                TempData["update_success"] = "updated..";
                await context.SaveChangesAsync();
                return RedirectToAction("Productslist");
            }

            return View();
        }


        public async Task<IActionResult> DeleteProducts(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                if (Id == null || context.icecreams == null)
                {
                    return NotFound();
                }
                var data = await context.icecreams.FirstOrDefaultAsync(x => x.IcecreamId == Id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost, ActionName("DeleteProducts")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)

            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }
            var data = await context.icecreams.FindAsync(Id);
            if (data != null)
            {
                context.icecreams.Remove(data);
            }
            await context.SaveChangesAsync();
            TempData["delete_success"] = "deleted..";

            return RedirectToAction("ProductsList");

        }

        //CRUD FOR ICECREAM ADDED , UPDATED , DELETED



        // CRUD FOR RECPIES START






        public IActionResult AddRecpies()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult AddRecpies(RecipesViewModel emp)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }

            string fileName = "";
            if (emp.Photo != null)
            {
                var ext = Path.GetExtension(emp.Photo.FileName);
                var size = emp.Photo.Length;
                if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                {

                    if (size <= 1000000)//1 MB
                    {

                        string folder = Path.Combine(env.WebRootPath, "recipeimages");
                        fileName = Guid.NewGuid().ToString() + "_" + emp.Photo.FileName;
                        string filePath = Path.Combine(folder, fileName);
                        emp.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                        Recipes p = new Recipes()
                        {
                            Recipename = emp.Recipename,
                            Recipedescription = emp.Recipedescription,
                            Recipeingredients = emp.Recipeingredients,
                            Image = fileName,
                            CreatedDate = emp.CreatedDate,

                            CreatedBy = emp.CreatedBy

                        };
                        context.recipes.Add(p);
                        context.SaveChanges();
                        TempData["Success"] = "recipe Added..";
                        return RedirectToAction("RecipeList");
                    }
                    else
                    {
                        TempData["Size_Error"] = "Image must be less than 1 MB";
                    }
                }
                else
                {
                    TempData["Ext_Error"] = "Only PNG, JPG, JPEG iamges allowed";
                }

            }

            return View();
        }

        public IActionResult RecipeList()
        {

            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                var data = context.recipes.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        public async Task<IActionResult> EditRecipe(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                if (Id == null || context.recipes == null)
                {
                    return NotFound();
                }
                var data = await context.recipes.FindAsync(Id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipe(int? Id, Recipes emp)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }

            if (Id != emp.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                context.recipes.Update(emp);
                TempData["update_success"] = "updated..";
                await context.SaveChangesAsync();
                return RedirectToAction("RecipeList");
            }

            return View();
        }


        public async Task<IActionResult> DeleteRecipe(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                if (Id == null || context.recipes == null)
                {
                    return NotFound();
                }
                var data = await context.recipes.FirstOrDefaultAsync(x => x.RecipeId == Id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost, ActionName("DeleteRecipe")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteRecipeConfirmed(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)

            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }
            var data = await context.recipes.FindAsync(Id);
            if (data != null)
            {
                context.recipes.Remove(data);
            }
            await context.SaveChangesAsync();
            TempData["delete_success"] = "deleted..";

            return RedirectToAction("RecipeList");

        }



        // CRUD FOR RECPIES END

        //CRUD FOR BOOKS START


        //CRUD FOR BOOKS END 


        //USERS LIST START

        public IActionResult Userlist()
        {

            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                var data = context.userregister.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }

        }

        //USERS LIST END



        //BOOKS CRUD START 

        public IActionResult AdminBooks()
        {

            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                var data = context.books.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost]
        public IActionResult ChangeAvailability(int bookId, bool isAvailable)
        {
            var book = context.books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null)
            {
                book.IsAvailable = isAvailable;
                context.SaveChanges();
                return Ok();
            }
            return NotFound();
        }


        public IActionResult AddBooks()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult AddBooks(BooksViewModel emp)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }

            string fileName = "";
            if (emp.Photo != null)
            {
                var ext = Path.GetExtension(emp.Photo.FileName);
                var size = emp.Photo.Length;
                if (ext.Equals(".png") || ext.Equals(".jpg") || ext.Equals(".jpeg"))
                {

                    if (size <= 1000000)//1 MB
                    {

                        string folder = Path.Combine(env.WebRootPath, "bookimages");
                        fileName = Guid.NewGuid().ToString() + "_" + emp.Photo.FileName;
                        string filePath = Path.Combine(folder, fileName);
                        emp.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                        Books p = new Books()
                        {
                            Booktittle = emp.Booktittle,
                            Author = emp.Author,
                            Price = emp.Price,
                            Stock = emp.Stock,
                            Image = fileName,
                            IsAvailable = emp.IsAvailable,
                            CreatedDate = emp.CreatedDate

                           

                        };
                        context.books.Add(p);
                        context.SaveChanges();
                        TempData["Success"] = "Books Added..";
                        return RedirectToAction("AdminBooks");
                    }
                    else
                    {
                        TempData["Size_Error"] = "Image must be less than 1 MB";
                    }
                }
                else
                {
                    TempData["Ext_Error"] = "Only PNG, JPG, JPEG iamges allowed";
                }

            }

            return View();
        }



        public async Task<IActionResult> EditBooks(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                if (Id == null || context.books == null)
                {
                    return NotFound();
                }
                var data = await context.books.FindAsync(Id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBooks(int? Id, Books emp)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }

            if (Id != emp.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                context.books.Update(emp);
                TempData["update_success"] = "updated..";
                await context.SaveChangesAsync();
                return RedirectToAction("AdminBooks");
            }

            return View();
        }


        public async Task<IActionResult> DeleteBooks(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                if (Id == null || context.books == null)
                {
                    return NotFound();
                }
                var data = await context.books.FirstOrDefaultAsync(x => x.BookId == Id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost, ActionName("DeleteBooks")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteBooksConfirmed(int? Id)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)

            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();
                return RedirectToAction("AdminLogin");
            }
            var data = await context.books.FindAsync(Id);
            if (data != null)
            {
                context.books.Remove(data);
            }
            await context.SaveChangesAsync();
            TempData["delete_success"] = "deleted..";

            return RedirectToAction("AdminBooks");

        }



        //BOOKS CRUD END 


        public IActionResult orderedbooks()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                // Exclude both "Delivered" and "Cancelled" orders
                var data = context.booksorder
                                  .Where(o => o.Status != "Delivered" && o.Status != "Cancelled")
                                  .ToList();

                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost]
        public IActionResult MarkAsDelivered(int id)
        {
            var order = context.booksorder.Find(id);
            if (order != null)
            {
                order.Status = "Delivered";
                context.SaveChanges();
            }
            return RedirectToAction("orderedbooks");
        }


        [HttpPost]
        public IActionResult MarkAsCancelled(int id)
        {
            var order = context.booksorder.Find(id);
            if (order != null)
            {
                order.Status = "Cancelled";
                context.SaveChanges();
            }
            return RedirectToAction("OrderedBooks");
        }


        public IActionResult DeliveredBooks()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                // Fetch only "Delivered" orders
                var deliveredData = context.booksorder
                                            .Where(o => o.Status == "Delivered")
                                            .ToList();

                return View(deliveredData);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        public IActionResult CancelledBooks()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                // Fetch only "Delivered" orders
                var deliveredData = context.booksorder
                                            .Where(o => o.Status == "Cancelled")
                                            .ToList();

                return View(deliveredData);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        public IActionResult orderedicecreams()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                // Exclude both "Delivered" and "Cancelled" orders
                var data = context.icecreamorder
                                  .Where(o => o.Status != "Delivered" && o.Status != "Cancelled")
                                  .ToList();

                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        [HttpPost]
        public IActionResult IcecreamMarkAsDelivered(int id)
        {
            var order = context.icecreamorder.Find(id);
            if (order != null)
            {
                order.Status = "Delivered";
                context.SaveChanges();
            }
            return RedirectToAction("orderedicecreams");
        }


        [HttpPost]
        public IActionResult IcecreamMarkAsCancelled(int id)
        {
            var order = context.icecreamorder.Find(id);
            if (order != null)
            {
                order.Status = "Cancelled";
                context.SaveChanges();
            }
            return RedirectToAction("orderedicecreams");
        }


        public IActionResult DeliveredIcecream()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                // Fetch only "Delivered" orders
                var deliveredData = context.icecreamorder
                                            .Where(o => o.Status == "Delivered")
                                            .ToList();

                return View(deliveredData);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }

        public IActionResult CancelledIcecreams()
        {
            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                // Fetch only "Delivered" orders
                var deliveredData = context.icecreamorder
                                            .Where(o => o.Status == "Cancelled")
                                            .ToList();

                return View(deliveredData);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }


        public IActionResult feedbacklist()
        {

            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                var data = context.feedback.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }
        public IActionResult contactuslist()
        {

            if (HttpContext.Session.GetString("AdminSession") != null)
            {
                ViewBag.Mynamesession = HttpContext.Session.GetString("Adminnamesession").ToString();

                var data = context.contactus.ToList();
                return View(data);
            }
            else
            {
                return RedirectToAction("AdminLogin");
            }
        }



    }

}
