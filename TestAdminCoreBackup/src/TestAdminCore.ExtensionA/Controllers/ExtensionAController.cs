// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using ExtCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestAdminCore.Data;
using TestAdminCore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TestAdminCore.ExtensionA.Controllers
{
  [Authorize]
    public class ExtensionAController : Controller
  {
        private readonly ApplicationDbContext _Context;
        private readonly UserManager<Models.ApplicationUser> _userManager;

        public ExtensionAController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _Context = context;
            _userManager = userManager;

        }

        //kan søge på en brugers rolle samt giver en liste over mulige roller
        public ActionResult Index()
    {
            var list = _Context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
        new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            // prepopulat roles for the view dropdown
             var listen = _Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
             ViewBag.Roles = listen;

            var roles = _Context.Roles.ToList();
            _Context.SaveChangesAsync();
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Username)
        {
            
            if (!string.IsNullOrWhiteSpace(Username))
            {
                ApplicationUser user = _Context.Users.Where(u => u.UserName.Equals(Username, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                Task<IList<string>> test = _userManager.GetRolesAsync(user);
                var listen = test.ToString().ToList().Distinct();

                ViewBag.RolesForThisUser = test.Result;
                _Context.SaveChangesAsync();
            }
        
            return View();
        }

        //tilføj en rolle til en bruger.
        [Authorize(Roles = "Editor")]
        public ActionResult Addrole()
        {
            var list = _Context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
        new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            _Context.SaveChangesAsync();
            return this.View();
        }
          [Authorize(Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addrole(string UserName, string RoleName)
        {
            try
            {
                ApplicationUser user = _Context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                _userManager.AddToRoleAsync(user, RoleName);

                ViewBag.ResultMessage = UserName + " blev tilføjet til rollen" + RoleName;
            }
            catch
            {
                ViewBag.ResultMessage = "der skete en fejl";
            }
            
            

            // prepopulat roles for the view dropdown
            var list = _Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            _Context.SaveChangesAsync();

            return View();
        }
        //fjern en rolle fra en bruger
       [Authorize(Roles = "Editor")]
        public ActionResult Removerole()
        {
            var list = _Context.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
           new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            _Context.SaveChangesAsync();
            return this.View();
        }

        [Authorize( Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Removerole(string UserName, string RoleName)
        {
            try
            {
                ApplicationUser user = _Context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                _userManager.RemoveFromRoleAsync(user, RoleName);
                ViewBag.ResultMessage = UserName + " er fjernet fra rollen " + RoleName;
            }
            catch
            {
                ViewBag.ResultMessage = "Der skete en fejl med at fjerne " + UserName + " fra rollen  " + RoleName;
            }
            // prepopulat roles for the view dropdown
            var list = _Context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            _Context.SaveChangesAsync();

            return View();
        }
    }
}