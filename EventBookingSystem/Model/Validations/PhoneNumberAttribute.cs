using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class PhoneNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return true; // Aceita valores nulos

        string phoneNumber = value.ToString();
        
        // Expressão regular para validar um número de telefone comum
        string phoneNumberPattern = @"^\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$";
        Regex regex = new Regex(phoneNumberPattern);

        if (regex.IsMatch(phoneNumber))
            return true;

        return false;
    }
}