using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Model.Validations
{
    public class CpfAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var cpf = value.ToString();
            // Remove caracteres não numéricos do CPF
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifica se o CPF tem 11 dígitos
            if (cpf.Length != 11)
            {
                return false;
            }

            // Verifica se todos os dígitos são iguais (CPF inválido)
            if (cpf.All(digit => digit == cpf[0]))
            {
                return false;
            }

            int[] numbers = cpf.Select(digit => int.Parse(digit.ToString())).ToArray();

            // Calcula o primeiro dígito verificador
            int sum1 = 0;
            for (int i = 0; i < 9; i++)
            {
                sum1 += numbers[i] * (10 - i);
            }
            int digit1 = 11 - (sum1 % 11);

            // Calcula o segundo dígito verificador
            int sum2 = 0;
            for (int i = 0; i < 10; i++)
            {
                sum2 += numbers[i] * (11 - i);
            }
            int digit2 = 11 - (sum2 % 11);

            // Os dígitos verificadores devem ser iguais aos últimos dois dígitos do CPF
            if (digit1 == numbers[9] && digit2 == numbers[10])
            {
                return true;
            }

            return false;
        }
    }
}
