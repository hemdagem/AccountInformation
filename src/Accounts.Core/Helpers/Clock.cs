using System;

namespace Accounts.Core.Helpers
{
    public class Clock : IClock
    {
        public DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}