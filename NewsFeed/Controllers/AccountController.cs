using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsFeed.Models;

namespace NewsFeed.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "News");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            //Tjekker om modelstaten overholdes
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            //Finder ud af om der en user med det username
            var user = await _userManager.FindByNameAsync(model.UserName);

            //Hvis den finder en user prøver den at logge ind
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "News");
                }
            }

            ModelState.AddModelError("", "bruger ikke fundet");

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = model.UserName };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "News");
                }
            }

            ModelState.AddModelError("", "noget gik galt");

            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "News");
        }
    }
}