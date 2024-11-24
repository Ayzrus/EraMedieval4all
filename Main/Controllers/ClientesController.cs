using Main.Class;
using Main.Models.Apagar;
using Main.Models.Clientes;
using Main.Models.Trajes;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    public class ClientesController : Controller
    {
        // Action to show the list of clients
        public IActionResult Index()
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to the home page if not authenticated
                return RedirectToAction("Index", "Home");
            }

            // Get the list of clients
            var clientes = ClientesClass.GetClients();
            return View(clientes);
        }

        // Action to register a new client (GET)
        public IActionResult Registar()
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to the home page if not authenticated
                return RedirectToAction("Index", "Home");
            }

            // Prepare the model for the client registration view
            var viewModel = new ClientesViewParcialModel
            {
                IsEdit = false // Indicating that this is for new registration, not editing
            };
            return View(viewModel);
        }

        // Action to edit an existing client (GET)
        public IActionResult Editar(int id)
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to the home page if not authenticated
                return RedirectToAction("Index", "Home");
            }

            // Get the client details to edit
            var clientes = ClientesClass.GetClient(id);
            var viewModel = new ClientesViewParcialModel
            {
                Cliente = clientes,
                IsEdit = true // Indicating that this is an edit operation
            };
            return View(viewModel);
        }

        // Action to delete a client (POST)
        [HttpPost]
        public JsonResult Delete([FromBody] ApagarModel model)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Attempt to delete the client
                bool sucesso = ClientesClass.DeleteClient(model.Id);

                // Return JSON response indicating success or failure
                if (sucesso)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Error while deleting. Please try again." });
                }
            }

            // If there are validation errors
            return Json(new { success = false, message = "Error while deleting." });
        }

        // Action to register a new client (POST)
        [HttpPost]
        public JsonResult Register([FromBody] ClientesModel model)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Create a new client from the model data
                ClientesClass cliente = new()
                {
                    Nif = model.Nif,
                    Nome = model.Nome,
                    Morada = model.Morada,
                    Email = model.Email,
                    Telefone = model.Telefone,
                };

                // Attempt to insert the client into the database
                bool sucesso = ClientesClass.InsertClient(cliente, out string errorMessage);

                // Return JSON response indicating success or failure
                if (sucesso)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = errorMessage });
                }
            }

            // If the model is invalid
            return Json(new { success = false, message = "Error while registering." });
        }

        // Action to edit an existing client (POST)
        [HttpPost]
        public JsonResult Edit([FromBody] ClientesModel model)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                // Create a new client from the model data for update
                ClientesClass cliente = new()
                {
                    Id = model.Id,
                    Nif = model.Nif,
                    Nome = model.Nome,
                    Morada = model.Morada,
                    Email = model.Email,
                    Telefone = model.Telefone,
                };

                // Attempt to update the client
                (bool sucesso, string message) = ClientesClass.UpdateClient(cliente);

                // Return JSON response indicating success or failure
                if (sucesso)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message });
                }
            }

            // If the model is invalid
            return Json(new { success = false, message = "Error while updating." });
        }

    }
}
