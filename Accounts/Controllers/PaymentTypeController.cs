using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Accounts.Core.Repositories.Interfaces;
using Accounts.ModelBuilders;
using Accounts.Models;

namespace Accounts.Controllers
{
    public class PaymentTypeController : Controller
    {
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly IPaymentTypeModelBuilder _paymentTypeModelBuilder;

        public PaymentTypeController(IPaymentTypeRepository paymentTypeRepository, IPaymentTypeModelBuilder paymentTypeModelBuilder)
        {
            _paymentTypeRepository = paymentTypeRepository;
            _paymentTypeModelBuilder = paymentTypeModelBuilder;
        }

        public async Task<ActionResult> Index()
        {
            var selectListModel = new SelectListModel { ListItems = new SelectList(await _paymentTypeRepository.GetPaymentTypes(), "Value", "Text") };

            return View("Index", selectListModel);
        }

        public ActionResult Add()
        {
            var model = new PaymentTypes();
            return View("Add", model);
        }

        [HttpPost]
        public async Task<ActionResult> Add(PaymentTypes model)
        {
            if (ModelState.IsValid)
            {
                var repoModel = _paymentTypeModelBuilder.BuildCoreModel(model);

                Guid addPaymentType = await _paymentTypeRepository.AddPaymentType(repoModel);

                if (addPaymentType != Guid.Empty)
                {
                    return RedirectToAction("Index", "PaymentType");
                }
            }

            return View("Add", model);
        }

        // GET: User
        public async Task<ActionResult> Update(Guid id)
        {
            Core.Models.PaymentTypes paymentType = await _paymentTypeRepository.GetPaymentType(id);

            if (paymentType == null)
            {
                throw new NullReferenceException("Payment Type not found");
            }

            var paymentTypes = _paymentTypeModelBuilder.BuildViewModel(paymentType);

            return View("Update", paymentTypes);
        }

        // GET: User
        [HttpPost]
        public async Task<ActionResult> Update(PaymentTypes model)
        {
            if (ModelState.IsValid)
            {
                var repoModel = _paymentTypeModelBuilder.BuildCoreModel(model);

                int updatePaymentType = await _paymentTypeRepository.UpdatePaymentType(repoModel);

                if (updatePaymentType > 0)
                {
                    return RedirectToAction("Index", "PaymentType");
                }
            }

            return View("Update", model);
        }
    }
}