using Main.Class;
using Main.Interfaces;
using Main.Models;
using Main.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Main.Controllers
{
    // HomeController handles the main logic for rendering views and user authentication
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Constructor that initializes the logger and email sender
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Displays the main page, redirecting to Dashboard if the user is authenticated
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Authenticated") == "true")
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        // Displays the registration page, redirecting to Dashboard if the user is authenticated
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("Authenticated") == "true")
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var types = TypeUsersClass.GetUsers();

            return View(types);
        }

        // Handles the registration of a new user
        [HttpPost]
        public JsonResult CreateAccount([FromBody] UserRegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                // Create a new user using the data from the model
                UsersClass user = new()
                {
                    Nome = model.Nome,
                    Morada = model.Morada,
                    Telefone = int.Parse(model.Telefone),
                    Email = model.Email,
                    Password = model.Password,
                    Pin = model.Pin,
                    TipoUser = model.TipoUser,
                };

                // Attempt to insert the user into the database and capture the result
                (bool success, string message) = UsersClass.InsertUser(user);

                if (success)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message });
                }
            }

            // If there are validation errors
            return Json(new { success = false, message = "Error registering the user." });
        }

        // Gets the local IPv4 address of the machine
        public static string GetLocalIPv4Address()
        {
            foreach (var networkInterface in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.Name.Contains("Ethernet") && networkInterface.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                {
                    foreach (var ipAddress in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        // Search for IPv4 addresses
                        if (ipAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ipAddress.Address.ToString(); // Return the IPv4 address
                        }
                    }
                }
            }
            return "IP not found"; // Return a default message if no IP is found
        }

        /// <summary>
        /// Handles user login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Mark users as inactive if needed
                UsersClass.MarkInactiveUsers();

                // Get the local IPv4 address of the machine
                string? ip = GetLocalIPv4Address();

                // Validate the login credentials
                var (success, message, email, firstJoin, nome, tipo, cliente) = UsersClass.ValidateLogin(model.Email, model.Password, ip);

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                // Set session variables for authenticated users
                HttpContext.Session.SetString("Authenticated", "true");
                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("nome", nome);
                HttpContext.Session.SetString("tipo", tipo);

                return Json(new { success = true, firstJoin, cliente });
            }

            return Json(new { success = false, message = "Invalid login credentials." });
        }        
        
        /// <summary>
        /// Handles user VERIFY PIN
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerifyPin([FromBody] VerifyModel model)
        {
            if (ModelState.IsValid)
            {

				// Get the local IPv4 address of the machine
				string? ip = GetLocalIPv4Address();

				var (success, message) = UsersClass.ValidatePin(model.Pin, model.Email, ip);

                if (!success)
                {
                    return Json(new { success = false, message });
                }


                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid login credentials." });
        }       
        
        /// <summary>
        /// Handles user update password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePassword([FromBody] PasswordModel model)
        {
            if (ModelState.IsValid)
            {

				// Get the local IPv4 address of the machine
				string? ip = GetLocalIPv4Address();

				var (success, message) = UsersClass.UpdatePassword(model.Password, model.Email, ip);

                if (!success)
                {
                    return Json(new { success = false, message });
                }


                return Json(new { success = true, message });
            }

            return Json(new { success = false, message = "Invalid login credentials." });
        }

        // Displays the error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Displays a custom 404 error page
        public IActionResult Error404()
        {
            return View();
        }

        // Error handling action for different status codes
        [Route("Home/Error/{statusCode}")]
        public IActionResult HandleError(int statusCode)
        {
            // Check for 404 status code
            if (statusCode == 404)
            {
                return View("Error404"); // Return the custom 404 view
            }

            return View("Error"); // Return a generic error view for other status codes
        }
    }
}
