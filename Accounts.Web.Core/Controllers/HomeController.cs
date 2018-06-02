using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Core.Models;
using Accounts.Core.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;

        public HomeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ActionResult> Index()
        {
            List<ListItem> userModel = await _userRepository.GetUsers();

            if (userModel == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View("Index", userModel);
        }
    }
}