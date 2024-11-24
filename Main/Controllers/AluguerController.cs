using Main.Class;
using Main.Models.Aluguer;
using Main.Models.Apagar;
using Main.Models.Clientes;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    // Controller responsible for handling rental (aluguer) related actions.
    public class AluguerController : Controller
    {
        // Action to display all rental records if the user is authenticated.
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to Home if the user is not authenticated.
                return RedirectToAction("Index", "Home");
            }
            var aluguer = AluguerClass.GetAlugueres(); // Fetch the list of rentals.
            return View(aluguer); // Return the view with the list of rentals.
        }

        // Action to display the rental registration form with necessary data for a new rental.
        public IActionResult Registar()
        {
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to Home if the user is not authenticated.
                return RedirectToAction("Index", "Home");
            }

            // Fetch necessary data for rental creation: clients, events, and costumes.
            var clientes = ClientesClass.GetClients();
            var eventos = EventosClass.GetEventos();
            var trajes = TrajesClass.GetTrajes();

            // Create a ViewModel for the view with the required data.
            var viewModel = new ViewParcialAluguerModel
            {
                Eventos = eventos,
                Cliente = clientes,
                Trajes = trajes,
                IsEdit = false // Set to false as it's a new registration.
            };
            return View(viewModel); // Return the view with the ViewModel.
        }

        // Action to display the edit form for an existing rental.
        public IActionResult Editar(int id)
        {
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to Home if the user is not authenticated.
                return RedirectToAction("Index", "Home");
            }

            // Fetch necessary data for editing the rental: clients, events, and costumes.
            var clientes = ClientesClass.GetClients();
            var eventos = EventosClass.GetEventos();
            var trajes = TrajesClass.GetTrajes();
            var aluguer = AluguerClass.GetAluguer(id); // Fetch the rental to edit by its ID.

            // Create a ViewModel for the view with the required data.
            var viewModel = new ViewParcialAluguerModel
            {
                Eventos = eventos,
                Cliente = clientes,
                Trajes = trajes,
                Aluguer = aluguer, // Set the rental data for editing.
                IsEdit = true // Set to true as it's an edit operation.
            };
            return View(viewModel); // Return the view with the ViewModel.
        }

        // Action to display the list of costumes available for a specific rental.
        public IActionResult Trajes(int id)
        {
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to Home if the user is not authenticated.
                return RedirectToAction("Index", "Home");
            }
            var trajes = TrajesClass.GetTrajesAluguer(id); // Fetch the list of costumes for the rental.

            return View(trajes); // Return the view with the list of costumes.
        }

        // POST action to delete a client (called when an AJAX request to delete a client is made).
        [HttpPost]
        public JsonResult Delete([FromBody] ApagarModel model)
        {
            if (ModelState.IsValid)
            {
                bool sucesso = AluguerClass.DeleteAluguer(model.Id); // Try to delete the client.

                if (sucesso)
                {
                    return Json(new { success = true }); // Return success if the deletion was successful.
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao apagar. Tente novamente." }); // Return error message if deletion failed.
                }
            }

            return Json(new { success = false, message = "Erro ao apagar." }); // Return error if model state is invalid.
        }

        // POST action to register a new rental (called when an AJAX request to register a rental is made).
        [HttpPost]
        public JsonResult Register([FromBody] AluguerModel model)
        {
            if (ModelState.IsValid)
            {
                AluguerClass aluguer = new()
                {
                    DataEntrega = DateTime.Parse(model.DataEntrega!), // Parse the delivery date from the model.
                    Cliente = model.Cliente, // Set the client.
                    Evento = model.Evento, // Set the event.
                };

                // Insert the new rental and get the result.
                (bool sucesso, int? lastInsertedId) = AluguerClass.InsertAluguer(aluguer);

                if (sucesso)
                {
                    // If there are selected costumes, add them to the rental.
                    if (model.TrajesId != null && model.TrajesId.Length > 0)
                    {
                        foreach (var trajeId in model.TrajesId)
                        {
                            TrajesAluguerClass trajeAluguer = new()
                            {
                                Traje = trajeId,
                                Alugar = (int)lastInsertedId! // Set the rental ID.
                            };

                            bool sucessoTraje = TrajesAluguerClass.InsertRental(trajeAluguer); // Try to insert the costume rental.

                            if (!sucessoTraje)
                            {
                                return Json(new { success = false, message = "Erro ao registrar um ou mais trajes." }); // Return error if costume registration fails.
                            }
                        }
                    }

                    return Json(new { success = true }); // Return success if the rental was registered successfully.
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao registrar o aluguel." }); // Return error if rental registration fails.
                }
            }

            return Json(new { success = false, message = "Dados inválidos." }); // Return error if model state is invalid.
        }

        // POST action to edit an existing rental (called when an AJAX request to edit a rental is made).
        [HttpPost]
        public JsonResult Edit([FromBody] AluguerModel model)
        {
            if (ModelState.IsValid)
            {
                AluguerClass aluguer = new()
                {
                    Id = model.Id, // Set the rental ID.
                    DataEntrega = DateTime.Parse(model.DataEntrega!), // Parse the new delivery date.
                    Cliente = model.Cliente, // Set the new client.
                    Evento = model.Evento, // Set the new event.
                };

                bool sucessoAluguer = AluguerClass.UpdateAluguer(aluguer); // Try to update the rental.

                if (sucessoAluguer)
                {
                    // Remove any old costumes from the rental.
                    bool sucessoRemoverTrajes = TrajesAluguerClass.DeleteTrajesByAluguerId(model.Id);

                    if (!sucessoRemoverTrajes)
                    {
                        return Json(new { success = false, message = "Erro ao remover trajes antigos." }); // Return error if removing old costumes fails.
                    }

                    // Add the new costumes to the rental.
                    if (model.TrajesId != null && model.TrajesId.Length > 0)
                    {
                        foreach (var trajeId in model.TrajesId)
                        {
                            TrajesAluguerClass trajeAluguer = new()
                            {
                                Traje = trajeId,
                                Alugar = aluguer.Id // Set the rental ID for the new costume.
                            };

                            bool sucessoTraje = TrajesAluguerClass.InsertRental(trajeAluguer); // Try to insert the new costume rental.

                            if (!sucessoTraje)
                            {
                                return Json(new { success = false, message = "Erro ao atualizar um ou mais trajes." }); // Return error if updating costumes fails.
                            }
                        }
                    }

                    return Json(new { success = true }); // Return success if the rental was updated successfully.
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o aluguel." }); // Return error if rental update fails.
                }
            }

            return Json(new { success = false, message = "Dados inválidos." }); // Return error if model state is invalid.
        }

        // POST action to finalize a rental
        [HttpPost]
        public JsonResult Finalize([FromBody] ApagarModel model)
        {
            if (ModelState.IsValid)
            {
                bool sucesso = AluguerClass.Inative(model.Id); // Try to finalize the rental.

                if (sucesso)
                {
                    return Json(new { success = true }); // Return success if the rental was finalized.
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao finalizar. Tente novamente." }); // Return error if finalizing the rental fails.
                }
            }

            return Json(new { success = false, message = "Erro ao finalizar." }); // Return error if model state is invalid.
        }

        // POST action to edit an existing rental (called when an AJAX request to edit a rental is made).
        [HttpPost]
        public JsonResult Copy([FromBody] AluguerCopyModel model)
        {
            if (ModelState.IsValid)
            {
                AluguerClass aluguer = new()
                {
                    Id = model.Id,
                    DataEntrega = DateTime.Parse(model.DataEntrega!),
                };

                bool sucessoAluguer = AluguerClass.CopyAluguer(aluguer); // Try to copy the rental.

                if (sucessoAluguer)
                {
                    return Json(new { success = true }); // Return success if the rental was copy successfully.
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar o aluguel." }); // Return error if rental copy fails.
                }
            }

            return Json(new { success = false, message = "Dados inválidos." }); // Return error if model state is invalid.
        }

    }
}
