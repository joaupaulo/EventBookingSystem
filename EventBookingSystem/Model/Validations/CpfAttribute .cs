using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EventBookingSystem.Model.Validations
{
    public class CpfAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string cpf)
            {
                cpf = Regex.Replace(cpf, @"[^\d]", ""); 
                if (cpf.Length != 11)
                {
                    return false;
                }

                if (new string(cpf[0], 11) == cpf)
                {
                    return false;
                }

                int[] numbers = cpf.Select(c => int.Parse(c.ToString())).ToArray();
                int sum1 = 0;
                int sum2 = 0;

                for (int i = 0; i < 9; i++)
                {
                    sum1 += numbers[i] * (10 - i);
                    sum2 += numbers[i] * (11 - i);
                }

                int mod1 = (sum1 % 11) % 10;
                int mod2 = (sum2 + 2 * mod1) % 11 % 10;

                if (numbers[9] != mod1 || numbers[10] != mod2)
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
