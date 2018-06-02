using System;

namespace Accounts.Core.Helpers
{
    // This class needs to be reworked.
    public class PaymentHelper : IPaymentHelper
    {
        private readonly DateTime _currentDateTime;

        public PaymentHelper(IClock clock)
        {
            _currentDateTime = clock.GetDateTime();
        }

        public DateTime GetPaymentRecurringDate(int payDay, DateTime paymentDate)
        {
            if (payDay < 1 || payDay > 31)
                throw new ArgumentOutOfRangeException();

            if (payDay > _currentDateTime.Day)
            {
                if (paymentDate.Day >= payDay)
                {
                    return new DateTime(GetPreviousYear(), GetPreviousMonth(), paymentDate.Day);
                }

                return new DateTime(_currentDateTime.Year, _currentDateTime.Month, paymentDate.Day);
            }
            var month = _currentDateTime.Month;
            int year = _currentDateTime.Year;

            if (paymentDate.Day < payDay)
            {
                month = GetMonth();
                year = GetYear();
            }

            return new DateTime(year, month, paymentDate.Day);
        }

        public DateTime GetPreviousPayDay(int payDay)
        {
            if (payDay < 1 || payDay > 31)
                throw new ArgumentOutOfRangeException();

            return new DateTime(_currentDateTime.Year, _currentDateTime.Month, payDay);
        }

        public DateTime GetNextPayDay(int payDay)
        {
            if (payDay < 1 || payDay > 31)
                throw new ArgumentOutOfRangeException();

            return new DateTime(GetYear(), GetMonth(), payDay);
        }

        public bool HasPaid(DateTime paymentDate)
        {
            return paymentDate <= _currentDateTime;
        }

        private int GetYear()
        {
            return _currentDateTime.Month == 12 ? _currentDateTime.Year + 1 : _currentDateTime.Year;
        }

        private int GetMonth()
        {
            return _currentDateTime.Month == 12 ? 1 : _currentDateTime.Month + 1;
        }

        private int GetPreviousMonth()
        {
            return _currentDateTime.Month == 1 ? 12 : _currentDateTime.Month - 1;
        }

        private int GetPreviousYear()
        {
            return _currentDateTime.Month == 1 ? _currentDateTime.Year - 1 : _currentDateTime.Year;
        }

    }
}