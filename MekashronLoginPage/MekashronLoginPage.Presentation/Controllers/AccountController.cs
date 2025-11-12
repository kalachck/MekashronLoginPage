using MekashronLoginPage.Application;
using MekashronLoginPage.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace MekashronLoginPage.Presentation.Controllers;

public class AccountController : Controller
{
    private readonly IAuthorizationProvider _authProvider;

    public AccountController(IAuthorizationProvider authProvider)
    {
        _authProvider = authProvider;
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

        var loginSuccess = await _authProvider.LoginAsync(model.Username, model.Password);

        ViewBag.ShowToast = true;
        if (loginSuccess)
        {
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
