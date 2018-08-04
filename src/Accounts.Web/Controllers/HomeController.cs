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
        private readonly IWebApi _webApi;

        public HomeController(IUserRepository userRepository, IWebApi webApi)
        {
            _userRepository = userRepository;
            _webApi = webApi;
        }

        public async Task<ActionResult> Index()
        {
            //var list = await _webApi.Get();
            List<ListItem> userModel = await _userRepository.GetUsers();

            if (userModel == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View("Index", userModel);
        }
    }
}