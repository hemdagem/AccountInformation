using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Accounts.Core.Repositories.Interfaces;
using Accounts.ModelBuilders;
using Accounts.Models;

namespace Accounts.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly IUserModelBuilder _userModelBuilder;

        public UserController(IUserRepository userRepository, IUserModelBuilder userModelBuilder)
        {
            _userRepository = userRepository;
            _userModelBuilder = userModelBuilder;
        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            SelectListModel model = new SelectListModel { ListItems = new SelectList(await _userRepository.GetUsers(), "Value", "Text") };

            return View("Index", model);
        }


        // GET: User
        public ActionResult Add()
        {
            var model = new UserModel();
            return View("Add", model);
        }

        // GET: User
        [HttpPost]
        public async Task<ActionResult> Add(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var buildCoreModel = _userModelBuilder.BuildCoreModel(model);

                Guid addUser = await _userRepository.AddUser(buildCoreModel);

                if (addUser != Guid.Empty)
                {
                    return RedirectToAction("Index", "User");
                }

            }

            return View("Add", model);

        }

        // GET: User
        public async Task<ActionResult> Update(Guid Id)
        {
            var user = await _userRepository.GetUser(Id);

            var buildViewModel = _userModelBuilder.BuildViewModel(user);

            if (user == null)
            {
                throw new NullReferenceException("User not found");
            }

            return View("Update", buildViewModel);
        }

        // GET: User
        [HttpPost]
        public async Task<ActionResult> Update(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var buildCoreModel = _userModelBuilder.BuildCoreModel(model);

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