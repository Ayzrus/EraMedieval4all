using Main.Class;
using Main.Models.Dashboard;
using Main.Models.Relatorios;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    public class RelatoriosController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to Home if the user is not authenticated.
                return RedirectToAction("Index", "Home");
            }


			var cli = ClientesClass.GetClientsFilter();
			var tra = TrajesClass.GetTop10MostRentedTrajes();
			var cliva = ClientesClass.GetClientsHigh();
			var viewModel = new RelatorioModel
			{
				Cliente = cli.OrderByDescending(c => c.Quantidade),
				Trajes = tra.OrderByDescending(t => t.TrajesAlugados),
				ClienteV = cliva.OrderByDescending(c => c.Valor),
			};

			return View(viewModel);
        }        
		
		public IActionResult Bloqueados()
        {
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to Home if the user is not authenticated.
                return RedirectToAction("Index", "Home");
            }


			var user = UsersClass.GetAllUsersBlocked();
			var viewModel = new UserModel
			{
				Users = user,
			};

			return View(viewModel);
        }
    }
}
