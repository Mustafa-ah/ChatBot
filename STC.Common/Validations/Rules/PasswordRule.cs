using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace STC.Common.Validations.Rules
{
   public class PasswordRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value == null)
            {
                
                return false;
            }
           
            var str = value as string;
            if (!HasMinimumLength(str, 8)) return false;
            if (!HasDigit(str)) return false;
            if (!HasSpecialChar(str)) return false;
            if (!HasUpperCaseLetter(str)) return false;
            if (!HasLowerCaseLetter(str)) return false;
    
            return true;
       
        }
        public static bool HasMinimumLength(string password, int minLength)
        {
            return password.Length >= minLength;
        }

        public static bool HasDigit(string password)
        {
            return password.Any(c => char.IsDigit(c));
        }

        public static bool HasSpecialChar(string password)
        {
            return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
        }

        public static bool HasUpperCaseLetter(string password)
        {
            return password.Any(c => char.IsUpper(c));
        }
    
        public static bool HasLowerCaseLetter(string password)
        {
            return password.Any(c => char.IsLower(c));
        }
    }
}
