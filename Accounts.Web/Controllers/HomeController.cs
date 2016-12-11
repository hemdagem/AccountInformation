using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Accounts.Core.Repositories.Interfaces;
using Accounts.ModelBuilders;
using Accounts.Models;
using PaymentModel = Accounts.Models.PaymentModel;
using PaymentViewModel = Accounts.Models.PaymentViewModel;


namespace Accounts.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentModelFactory _paymentModelFactory;
        private readonly IPaymentModelBuilder _paymentTypeModelBuilder;
        private readonly IUserModelBuilder _userModelBuilder;

        public HomeController(IPaymentRepository paymentRepository, IUserRepository userRepository,
            IPaymentModelFactory paymentModelFactory, IPaymentModelBuilder paymentTypeModelBuilder, IUserModelBuilder userModelBuilder)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _paymentModelFactory = paymentModelFactory;
            _paymentTypeModelBuilder = paymentTypeModelBuilder;
            _userModelBuilder = userModelBuilder;
        }

        public async Task<ActionResult> Index(Guid id)
        {
            var homeControllerModel = new HomeControllerModel
            {
                Users = id
            };

            var userModel = await  _userRepository.GetUser(id);

            if (userModel == null)
            {
                return RedirectToAction("Index", "Error");
            }
            
            return View("Index", homeControllerModel);
        }

        public async Task<JsonResult> GetPaymentSummaryById(Guid userGuid)
        {
            var userModel = _userRepository.GetUser(userGuid);
            var paymentsById = _paymentRepository.GetPaymentsById(userGuid);

            await Task.WhenAll(userModel, paymentsById);

            var paymentModels = _paymentModelFactory.CreatePayments(paymentsById.Result);

            var buildViewModel = _userModelBuilder.BuildViewModel(userModel.Result);

            PaymentViewModel paymentViewModel = _paymentModelFactory.CreatePaymentSummary(paymentModels, buildViewModel);

            return Json(paymentViewModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AddPayment(PaymentModel model)
        {
            var buildCoreModel = _paymentTypeModelBuilder.BuildCoreModel(model);
            Guid addPayment = await _paymentRepository.AddPayment(buildCoreModel);

            return Json(addPayment, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> DeletePayment(Guid paymentId)
        {
            int addPayment = await _paymentRepository.DeletePayment(paymentId);

            return Json(addPayment, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> UpdatePayment(PaymentModel model)
        {
            var buildCoreModel = _paymentTypeModelBuilder.BuildCoreModel(model);
            int addPayment = await _paymentRepository.UpdatePayment(buildCoreModel);

            return Json(addPayment, JsonRequestBehavior.AllowGet);
        }
    }
}