using System.ComponentModel.DataAnnotations;
using EventBookingSystem.Model.Validations;
using MongoDB.Bson;

namespace EventBookingSystem.Model;

    public class Participante
    {
        [Required(ErrorMessage = "Fill in the participant's name")]
        public string Nome { get;  set; }
        [Email(ErrorMessage = "Send a valid email")]
        public string Email { get;  set; }
        [PhoneNumber(ErrorMessage = "Send a valid number")]
        public string Telefone { get;  set; }
        [FutureDate]
        public DateTime DataInscricao { get;  set; }
        
    }
