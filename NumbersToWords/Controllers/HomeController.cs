using Microsoft.AspNetCore.Mvc;
using NumbersToWords.Models;
using System.Diagnostics;

namespace NumbersToWords.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string? number)
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                try
                {
                    // Convert the number to words using NumberConverter
                    string words = NumberConverter.ParseNumberInput(number);
                    ViewData["Words"] = words;
                    ViewData["Number"] = number;
                }
                catch (FormatException)
                {
                    ViewData["Error"] = "Invalid input. Please enter a valid number.";
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = $"An error occurred: {ex.Message}";
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
