using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiBlog.Models.Pages;

namespace OptiBlog.Controllers;

public class StartPageController : PageController<StartPage>
{
    public IActionResult Index(StartPage currentPage) => View(currentPage);
}
