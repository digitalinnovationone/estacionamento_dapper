using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace estacionamento_dapper.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
