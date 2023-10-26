using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace EventBookingSystem.Model.Validations;

public class EmailAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        try
        {
            var addr = new MailAddress(value.ToString());
            return true;
        }
        catch 
        {
            return false;
        }
    } 
}