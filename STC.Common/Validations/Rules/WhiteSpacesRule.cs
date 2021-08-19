using System;
namespace STC.Common.Validations.Rules
{
    public class WhiteSpacesRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value == null)
            {

                return false;
            }

            string str = value as string;
            if (str.Replace(" ", "").Length == 0)
                return false;

            return true;

        }
    }
}
  
