using System;
using System.Threading.Tasks;
using Accounts.Core.Repositories.Interfaces;
using Accounts.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Accounts.Core.Models;
using UserModel = Accounts.Web.Models.UserModel;

namespace Accounts.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            SelectListModel model = new SelectListModel { ListItems = new SelectList(await _userRepository.GetUsers(), "Value", "Text") };
            return View("Index", model);
        }

        public ActionResult Add()
        {
            var model = new UserModel();
            return View("Add", model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var buildCoreModel = _mapper.Map<Accounts.Core.Models.UserModel>(model);

                Guid addUser = await _userRepository.AddUser(buildCoreModel);

                if (addUser != Guid.Empty)
                {
                    return RedirectToAction("Index", "User");
                }
            }
            return View("Add", model);
        }

        public async Task<ActionResult> Update(Guid id)
        {
            var user = await _userRepository.GetUser(id);

            var buildViewModel = _mapper.Map<UserModel>(user);

            if (user == null)
            {
                throw new NullReferenceException("User not found");
            }

            return View("Update", buildViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Update(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var buildCoreModel = _mapper.Map<Accounts.Core.Models.UserModel>(model);

                int addUser = await _userRepository.UpdateUser(buildCoreModel);

                if (addUser > 0)
                {
                    return RedirectToAction("Index", "User");
                }
            }

            return View("Update", model);
        }
    }
}