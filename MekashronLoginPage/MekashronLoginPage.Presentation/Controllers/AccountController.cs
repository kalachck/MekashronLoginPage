using MekashronLoginPage.Application.Providers;
using MekashronLoginPage.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace MekashronLoginPage.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly ILoginProvider _loginProvider;

    public AccountController(ILoginProvider loginProvider)
    {
        _loginProvider = loginProvider;
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ShowToast = true;
            ViewBag.ToastMessage = "Invalid input!";
            ViewBag.ToastClass = "bg-danger";
            return View(model);
        }

        var loginResponse = await _loginProvider.LoginAsync(model.Username, model.Password);

        ViewBag.ShowToast = true;
        if (loginResponse is not null)
        {
            ViewBag.LoginResponse = loginResponse;
            ViewBag.ToastMessage = "Login successful!";
            ViewBag.ToastClass = "bg-success";
        }
        else
        {
            ViewBag.ToastMessage = "Invalid username or password!";
            ViewBag.ToastClass = "bg-danger";
        }

        return View(model);
    }
}
