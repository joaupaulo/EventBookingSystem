using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Model.Validations
{
    public class UniqueCPFAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is List<Participante> participantes)
            {
                return participantes.GroupBy(p => p.CPF).All(g => g.Count() == 1);
            }

            return true;
        }
    }

}
