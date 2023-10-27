using EventBookingSystem.Model.Validations;
using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Model;

public class Participante
{
    [Required(ErrorMessage = "Fill in the participant's name")]
    public string Nome { get; set; }
    [Email(ErrorMessage = "Send a valid email")]
    public string Email { get; set; }
    [PhoneNumber(ErrorMessage = "Send a valid number")]
    public string Telefone { get; set; }
    [FutureDate]
    public DateTime DataInscricao { get; set; }
    [Cpf(ErrorMessage = "Invalid CPF")]
    public string CPF { get; set; }

}
