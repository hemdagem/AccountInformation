using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public class UserModelBuilder : IUserModelBuilder
    {
        public Core.Models.UserModel BuildCoreModel(UserModel model)
        {
            return new Core.Models.UserModel
            {
                PayDay = model.PayDay,
                Amount = model.Amount,
                Id=model.Id,
                Name = model.Name
            };
        }

        public UserModel BuildViewModel(Core.Models.UserModel model)
        {
            return new UserModel
            {
                PayDay = model.PayDay,
                Amount = model.Amount,
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}