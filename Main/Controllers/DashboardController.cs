using Main.Class;
using Main.Models;
using Main.Models.Aluguer;
using Main.Models.Dashboard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI;
using System.Diagnostics;

namespace Main.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        // Constructor that takes an ILogger instance for logging purposes
        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        // Action for the Dashboard index view
        public IActionResult Index()
        {
            // Check if the user is authenticated
            if (HttpContext.Session.GetString("Authenticated") != "true")
            {
                // Redirect to the home page if not authenticated
                return RedirectToAction("Index", "Home");
            }
            var al = AluguerClass.GetAlugueresActiveCount();
            var totalal = TrajesClass.GetTrajesAlugadosCount();
            var totalregs = TrajesClass.GetTotalTrajesRegistrados();
            var valorTotal = AluguerClass.GetTotalValorAnoAtual();
            var cli = ClientesClass.GetLastFiveClient().OrderBy(c => c.Id);
            var clienteN = ClientesClass.GetClientN().OrderBy(c => c.Id);
            var tra = TrajesClass.GetLastTenTrajes().OrderByDescending(t => t.DataAlugou);
            var viewModel = new DashboardModel
            {
                Aluguer = al,
				TrajesAlugados = totalal,
				TrajesRegistados = totalregs,
				ValorTotal = valorTotal,
				Cliente = cli,
				ClienteN = clienteN,
				Trajes = tra,
            };

            return View(viewModel); // Return the dashboard view if authenticated
        }

        // Action to handle user logout
        [HttpPost]
        public IActionResult Logout()
        {
            // Log user exit and remove session data
            LogsUserClass.UpdateExit(HttpContext.Session.GetString("email")!);
            HttpContext.Session.Remove("Authenticated");
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("nome");
            HttpContext.Session.Remove("tipo");

            // Redirect to the home page after logout
            return RedirectToAction("Index", "Home");
        }

        // Action for handling error responses (No caching)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Return error view with request ID
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Action for a custom 404 error page
        public IActionResult Error404()
        {
            return View(); // Return the custom 404 error view
        }

        // Error handling action that maps to specific status codes
        [Route("Home/Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            // Check for 404 status code
            if (statusCode == 404)
            {
                return View("Error404"); // Return custom 404 view
            }

            // For other status codes, return a generic error view
            return View("Error");
        }
    }
}
