using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.Validators
{
    public class CollectionAttribute : ValidationAttribute
    {
        public int MinElementLength { get; set; }
        public int MinLength { get; set; }

        public CollectionAttribute(int minElementLength = 0, int minLength = 0)
        {
            MinElementLength = minElementLength;
            MinLength = minLength;
        }

        public override string FormatErrorMessage(string name) =>
            $"The field {name} must be an array type with a minimum length of '{MinLength}' and with minimum element size of '{MinElementLength}'.";

        public override bool IsValid(object value)
        {
            if (value is null) return false;

            return value is ICollection<string> list &&
                   list.Count >= MinLength &&
                   list.All(item => item.Length >= MinElementLength);
        }
    }
}