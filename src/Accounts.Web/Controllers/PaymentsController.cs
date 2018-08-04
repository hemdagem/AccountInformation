using System;
using System.Threading.Tasks;
using Accounts.Core.Models;
using Accounts.Core.Repositories.Interfaces;
using Accounts.Web.ModelBuilders;
using Accounts.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentModel = Accounts.Web.Models.PaymentModel;
using PaymentViewModel = Accounts.Web.Models.PaymentViewModel;
using UserModel = Accounts.Web.Models.UserModel;

namespace Accounts.Web.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPaymentModelFactory _paymentModelFactory;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentRepository paymentRepository, IUserRepository userRepository, IPaymentModelFactory paymentModelFactory, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _paymentModelFactory = paymentModelFactory;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index(Guid id)
        {
            var homeControllerModel = new HomeControllerModel { User = id };
            var userModel = await _userRepository.GetUser(id);

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

            var buildViewModel = _mapper.Map<UserModel>(userModel.Result);

            PaymentViewModel paymentViewModel = _paymentModelFactory.CreatePaymentSummary(paymentModels, buildViewModel);

            return Json(paymentViewModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddPayment([FromBody]PaymentModel paymentModel)
        {
            var buildCoreModel = _mapper.Map<Accounts.Core.Models.PaymentModel>(paymentModel);
            Guid addPayment = await _paymentRepository.AddPayment(buildCoreModel);
            return Json(addPayment);
        }

        [HttpPost]
        public async Task<JsonResult> DeletePayment([FromBody]PaymentModel paymentModel)
        {
            int addPayment = await _paymentRepository.DeletePayment(paymentModel.Id);
            return Json(addPayment);
        }

        [HttpPost]
        public async Task<JsonResult> UpdatePayment([FromBody]PaymentModel model)
        {
            var buildCoreModel = _mapper.Map<Accounts.Core.Models.PaymentModel>(model);
            int addPayment = await _paymentRepository.UpdatePayment(buildCoreModel);
            return Json(addPayment);
        }
    }
}