using Main.Class;
using Main.Models.Apagar;
using Main.Models.Clientes;
using Main.Models.Eventos;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    public class EventosController : Controller
    {
        // Action to display a list of events
        public IActionResult Index()
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home"); // Redirect to the home page if not authenticated
            }
            var eventos = EventosClass.GetEventos(); // Retrieve events
            return View(eventos); // Return the events view with the list of events
        }

        // Action to display a list of event organizers
        public IActionResult Organizador()
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home"); // Redirect to the home page if not authenticated
            }

            var tipoOrg = TipoOrganizadorClass.GetAllTipoOrganizadores(); // Retrieve event organizers' types
            return View(tipoOrg); // Return the view with the organizer types
        }

        // Action to display the event registration page
        public IActionResult Registar()
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home"); // Redirect to the home page if not authenticated
            }

            var org = OrganizadorClass.GetOrganizadores(); // Get organizers for the registration
            var viewModel = new EventosViewParcialModel
            {
                Organizadores = org, // Pass the list of organizers to the view model
                IsEdit = false // Indicate that this is a registration, not an edit
            };
            return View(viewModel); // Return the view with the event registration form
        }

        // Action to display the event editing page
        public IActionResult Editar(int id)
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                return RedirectToAction("Index", "Home"); // Redirect to the home page if not authenticated
            }

            var org = OrganizadorClass.GetOrganizadores(); // Get organizers for the event edit form
            var evento = EventosClass.GetEvento(id); // Get the event details for editing
            var viewModel = new EventosViewParcialModel
            {
                Organizadores = org, // Pass the list of organizers
                Evento = evento, // Pass the event data to the view model
                IsEdit = true // Indicate that this is an edit, not a new registration
            };
            return View(viewModel); // Return the view with the event edit form
        }

        // POST action to delete an event
        [HttpPost]
        public JsonResult Delete([FromBody] ApagarModel model)
        {
            if (ModelState.IsValid)
            {
                bool sucesso = EventosClass.DeleteEvento(model.Id); // Try to delete the event by ID

                if (sucesso)
                {
                    return Json(new { success = true }); // Return success if deletion is successful
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao apagar. Tente novamente." }); // Return failure message if deletion fails
                }
            }

            return Json(new { success = false, message = "Erro ao apagar." }); // Return failure if validation fails
        }

        // POST action to register a new organizer
        [HttpPost]
        public JsonResult RegisterOrg([FromBody] OrganizadorModel model)
        {
            if (ModelState.IsValid)
            {
                OrganizadorClass org = new()
                {
                    Nome = model.Nome,
                    Localidade = model.Localidade,
                    Nif = model.Nif,
                    IdTipo = model.Tipo, // Assign the type of the organizer
                };

                bool sucesso = OrganizadorClass.InsertOrganizador(org); // Try to insert the organizer

                if (sucesso)
                {
                    return Json(new { success = true }); // Return success if the organizer is registered
                }
                else
                {
                    return Json(new { success = false }); // Return failure if registration fails
                }
            }

            return Json(new { success = false, message = "Erro ao registrar." }); // Return failure if validation fails
        }

        // POST action to register a new event
        [HttpPost]
        public JsonResult Register([FromBody] EventosModel model)
        {
            if (ModelState.IsValid)
            {
                EventosClass evento = new()
                {
                    Localidade = model.Localidade,
                    Descricao = model.Descricao,
                    Titulo = model.Titulo,
                    DataInicio = DateTime.Parse(model.DataInicio),
                    DataFim = DateTime.Parse(model.DataFim),
                    Facebook = model.Facebook,
                    Instagram = model.Instagram,
                    TikTok = model.TikTok,
                    Organizador = model.Organizador,
                };

                bool sucesso = EventosClass.InsertEvento(evento); // Try to insert the event

                if (sucesso)
                {
                    return Json(new { success = true }); // Return success if the event is registered
                }
                else
                {
                    return Json(new { success = false }); // Return failure if registration fails
                }
            }

            return Json(new { success = false, message = "Erro ao registrar." }); // Return failure if validation fails
        }

        // POST action to edit an existing event
        [HttpPost]
        public JsonResult Edit([FromBody] EventosModel model)
        {
            if (ModelState.IsValid)
            {
                EventosClass evento = new()
                {
                    Id = model.Id,
                    Localidade = model.Localidade,
                    Descricao = model.Descricao,
                    Titulo = model.Titulo,
                    DataInicio = DateTime.Parse(model.DataInicio),
                    DataFim = DateTime.Parse(model.DataFim),
                    Facebook = model.Facebook,
                    Instagram = model.Instagram,
                    TikTok = model.TikTok,
                    Organizador = model.Organizador,
                };

                bool sucesso = EventosClass.UpdateEvento(evento); // Try to update the event

                if (sucesso)
                {
                    return Json(new { success = true }); // Return success if the event is updated
                }
                else
                {
                    return Json(new { success = false, message = "Erro ao atualizar. Tente outro." }); // Return failure if update fails
                }
            }

            return Json(new { success = false, message = "Erro ao atualizar." }); // Return failure if validation fails
        }
    }
}
