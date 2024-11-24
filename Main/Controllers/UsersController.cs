using Main.Class;
using Main.Models.Apagar;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    // Controller to handle user-related actions
    public class UsersController : Controller
    {
        // Action to display a list of logs
        public IActionResult Index()
        {
            // Redirect to the home page if the user is not authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home");
            }

            // Get all logs of the users
            var logs = LogsUserClass.GetAllLogs().OrderByDescending(c => c.Id);

            // Return the view with the logs
            return View(logs);
        }

        // Action to display a list of users
        public IActionResult UsersList()
        {
            // Check if the user is authenticated, otherwise redirect to the home page
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home");
            }

            // Get all users
            var users = UsersClass.GetAllUsers();

            // Return the view with the list of users
            return View(users);
        }

        // Action to display the registration form for a new user
        public IActionResult Registar()
        {
            // Check if the user is authenticated, otherwise redirect to the home page
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home");
            }

            // Get the list of user types
            var types = TypeUsersClass.GetUsers();

            // Return the view with the list of user types
            return View(types);
        }

        // Action to display the user's profile
        public IActionResult Perfil()
        {
            // Check if the user is authenticated, otherwise redirect to the home page
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home");
            }

            // Return the view for the profile page
            return View();
        }

        // Action to inactivate a user
        [HttpPost]
        public JsonResult Inative([FromBody] ApagarModel model)
        {
            // Check if the model state is valid before proceeding
            if (ModelState.IsValid)
            {
                // Attempt to inactivate the user with the given ID
                bool sucesso = UsersClass.Inative(model.Id);

                // Return the result as JSON indicating whether the operation was successful
                if (sucesso)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao inativar. Tente novamente." });
                }
            }

            // If the model state is invalid, return an error message
            return Json(new { success = false, message = "Erro ao inativar." });
        }

    }
}
