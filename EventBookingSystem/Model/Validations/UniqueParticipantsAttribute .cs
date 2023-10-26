using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Model.Validations
{
    public class UniqueParticipantsAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is List<Participante> participantes)
            {
                return participantes.GroupBy(p => p.Email).All(g => g.Count() == 1);
            }

            return true;
        }
    }
}
