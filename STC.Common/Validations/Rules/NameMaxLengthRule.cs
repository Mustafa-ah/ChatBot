using System;
using System.Collections.Generic;
using System.Text;

namespace STC.Common.Validations.Rules
{
   public class NameMaxLengthRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }
            var str = value as string;
            if (str.Length > 32) return false;
            return true;
        }
    }
}
