using System;

namespace Accounts.Core.Helpers
{
    public interface IClock
    {
        DateTime GetDateTime();
    }
}