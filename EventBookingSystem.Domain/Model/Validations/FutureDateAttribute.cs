using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Model.Validations;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime data)
        {
            return data >= DateTime.Now;
        }

        return false;
    }
}