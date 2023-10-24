using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Model.Validations;

public class EmailAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(value.ToString());
            return addr.Address == value;
        }
        catch 
        {
            return false;
        }
    } 
}