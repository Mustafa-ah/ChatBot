using System;
using System.Collections.Generic;
using System.Text;

namespace STC.Common.Validations.Rules
{
  public  class MobileMaxLengthRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            return str.Length == 10;
        }
    }
}
