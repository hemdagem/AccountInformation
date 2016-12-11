using Accounts.Models;

namespace Accounts.ModelBuilders
{
    public interface IUserModelBuilder
    {
        Core.Models.UserModel BuildCoreModel(UserModel model);
        UserModel BuildViewModel(Core.Models.UserModel model);
    }
}