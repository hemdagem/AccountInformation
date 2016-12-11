using System;

namespace Accounts.Core.Helpers
{
    // This class needs to be reworked.
    public class PaymentHelper : IPaymentHelper
    {
        private readonly DateTime _date;

        public PaymentHelper(IClock clock)
        {
            _date = clock.GetDateTime();
        }

        public DateTime GetPaymentRecurringDate(int payDay, DateTime paymentDate)
        {
            if (payDay < 1 || payDay > 31)
                throw new ArgumentOutOfRangeException();

            if (payDay > _date.Day)
            {
                if (paymentDate.Day >= payDay)
                {
                    return new DateTime(GetPreviousYear(), GetPreviousMonth(), paymentDate.Day);
                }

                return new DateTime(_date.Year, _date.Month, paymentDate.Day);
            }
            var month = _date.Month;
            int year = _date.Year;

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

            return new DateTime(_date.Year, _date.Month, payDay);
        }

        public DateTime GetNextPayDay(int payDay)
        {
            if (payDay < 1 || payDay > 31)
                throw new ArgumentOutOfRangeException();

            return new DateTime(GetYear(), GetMonth(), payDay);
        }

        public bool HasPaid(DateTime paymentDate)
        {
            return paymentDate <= _date;
        }

        private int GetYear()
        {
            return _date.Month == 12 ? _date.Year + 1 : _date.Year;
        }

        private int GetMonth()
        {
            return _date.Month == 12 ? 1 : _date.Month + 1;
        }

        private int GetPreviousMonth()
        {
            return _date.Month == 1 ? 12 : _date.Month - 1;
        }

        private int GetPreviousYear()
        {
            return _date.Month == 1 ? _date.Year - 1 : _date.Year;
        }

    }
}