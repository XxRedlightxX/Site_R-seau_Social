﻿using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (username == "root" && password == "root")
        {
            HttpContext.Session.SetString("UserName", username);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.ErrorMessage = "Identifiants incorrects";
            return View();
        }
    }
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}