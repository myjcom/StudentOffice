using System;
using System.Linq;
using System.Net.Mail;

namespace StudentOffice.Utils
{
    public static class Checker
    {
        public static bool IsValidDate(string date)
        {
            DateTime minDate = new(1900, 1, 1);
            return DateTime.TryParse(date, new System.Globalization.CultureInfo("ru-RU"),
                System.Globalization.DateTimeStyles.None, out DateTime tDate)
                 && minDate.CompareTo(tDate) < 0;
        }

        public static bool IsValidEmail(string email)
        {
            return MailAddress.TryCreate(email, out _);
        }

        public static bool IsValidName(string val)
        {
            return val.All(char.IsLetter);
        }

        public static bool IsValidAdress(string val)
        {
            return true;
        }

        public static bool IsValidNumber(string val)
        {
            return val.Length >= 4 && val.Length <= 6 && val.All(char.IsDigit);
        }

        public static bool IsValidPhone(string val)
        {
            return val.Count(char.IsDigit) == 11;
        }

        internal static bool IsValidCode(string val)
        {
            return val.Length == 7 && val.Count(c => char.IsDigit(c)) == 6;
        }
    }
}
