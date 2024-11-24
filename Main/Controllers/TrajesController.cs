using Main.Class;
using Main.Models.Apagar;
using Main.Models.Trajes;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    public class TrajesController : Controller
    {
        // Index action that loads the list of "Trajes" (outfits)
        public IActionResult Index()
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch the list of outfits from the database and return the view
            var trajes = TrajesClass.GetTrajes();
            return View(trajes);
        }

        // Action for registering a new outfit
        public IActionResult Registar()
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch the list of warehouses and create the view model
            var armazem = ArmazemClass.GetWarehouses();
            var viewModel = new TrajesViewParcialModel
            {
                Armazem = armazem,
                IsEdit = false // This indicates that it is a registration (not an edit)
            };
            return View(viewModel);
        }

        // Action for editing an existing outfit
        public IActionResult Editar(int id)
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch the list of warehouses and the specific outfit to edit
            var armazem = ArmazemClass.GetWarehouses();
            var traje = TrajesClass.GetTraje(id);
            var viewModel = new TrajesViewParcialModel
            {
                Armazem = armazem,
                Trajes = traje,
                IsEdit = true // This indicates that it is an edit operation
            };
            return View(viewModel);
        }

        // Action to delete an outfit
        [HttpPost]
        public JsonResult Delete([FromBody] ApagarModel model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Attempt to delete the outfit and return a JSON response
                bool sucesso = TrajesClass.DeleteTraje(model.Id);
                if (sucesso)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Error deleting. Try again." });
                }
            }

            // Return a failure response if the model state is not valid
            return Json(new { success = false, message = "Error deleting." });
        }

        // Action to register a new outfit, handling file upload for the outfit photo
        [HttpPost]
        public async Task<JsonResult> Register([FromForm] TrajesModel model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Handle the file upload if the photo is provided
                if (model.Foto != null && model.Foto.Length > 0)
                {
                    // Generate a unique name for the photo
                    var nomeFoto = Guid.NewGuid().ToString() + Path.GetExtension(model.Foto.FileName);

                    // Set the file path where the photo will be saved
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "trajesPhotos", nomeFoto);

                    // Save the file to the server
                    using (var fileStream = new FileStream(caminho, FileMode.Create))
                    {
                        await model.Foto.CopyToAsync(fileStream);
                    }

                    model.FotoNome = nomeFoto; // Save the photo file name in the model
                }

                // Create a new Trajes object and populate its fields from the model
                TrajesClass traje = new()
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Valor = double.Parse(model.Valor),
                    Armazem = model.Armazem,
                    Foto = model.FotoNome,
                    Quantidade = model.Quantidade,
                    Especificacao = model.Especificacao,
                    Tipo = model.Tipo,
                    Ref = model.Ref
                };

                // Attempt to insert the new outfit and return a JSON response
                (bool sucesso, string message) = TrajesClass.InsertTraje(traje);
                if (sucesso)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message });
                }
            }

            // Return a failure response if the model state is not valid
            return Json(new { success = false, message = "Error registering." });
        }

        // Action to edit an existing outfit, handling file upload for the outfit photo
        [HttpPost]
        public async Task<JsonResult> Edit([FromForm] TrajesModel model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Handle the file upload if the photo is provided
                if (model.Foto != null && model.Foto.Length > 0)
                {
                    // Generate a unique name for the photo
                    var nomeFoto = Guid.NewGuid().ToString() + Path.GetExtension(model.Foto.FileName);

                    // Set the file path where the photo will be saved
                    var caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "trajesPhotos", nomeFoto);

                    // Save the file to the server
                    using (var fileStream = new FileStream(caminho, FileMode.Create))
                    {
                        await model.Foto.CopyToAsync(fileStream);
                    }

                    model.FotoNome = nomeFoto; // Save the photo file name in the model
                }

                // Create a new Trajes object and populate its fields from the model
                TrajesClass traje = new()
                {
                    Id = model.Id,
                    Nome = model.Nome,
                    Valor = double.Parse(model.Valor),
                    Armazem = model.Armazem,
                    Foto = model.FotoNome,
                    Quantidade = model.Quantidade,
                    Especificacao = model.Especificacao,
                    Tipo = model.Tipo,
                    Ref = model.Ref,
                };

                // Attempt to update the outfit and return a JSON response
                (bool sucesso, string message) = TrajesClass.UpdateTraje(traje);
                if (sucesso)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message });
                }
            }

            // Return a failure response if the model state is not valid
            return Json(new { success = false, message = "Error updating." });
        }

        // Action to inactivate an outfit
        [HttpPost]
        public JsonResult Inative([FromBody] ApagarModel model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Attempt to inactivate the outfit and return a JSON response
                bool sucesso = TrajesClass.Inative(model.Id);
                if (sucesso)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Error inactivating. Try again." });
                }
            }

            // Return a failure response if the model state is not valid
            return Json(new { success = false, message = "Error inactivating." });
        }
    }
}
