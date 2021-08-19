using System;
using System.Collections.Generic;
using System.Text;

namespace STC.Common.Validations.Rules
{
   public class MobileRule<T> : IValidationRule<T>
    {
         public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
              
                return false;
            }

            var str = value as string;
            if(str.Length != 10)
            {
              
                return false;
            }
            else
            {
          
                if (str[0] == '0' && str[1] == '5') return true;
                return false;
            }
        }
    }
}
