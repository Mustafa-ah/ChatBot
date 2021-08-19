using System;
using System.Collections.Generic;
using System.Text;

namespace STC.Common.Validations.Rules
{
   public class NameSpecialCharsRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }
            var str = value as string;
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (str.Contains(item)) return false;
            }

            return true;


        }
    }
}
