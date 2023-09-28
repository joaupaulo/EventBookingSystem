using MongoDB.Bson;

namespace EventBookingSystem.Model;

public class Usuario
{
   public ObjectId UsuarioId { get; set; }
   public string Nome { get; set; }
   public string Email { get; set; }
   public string Senha { get; set; }
   public string InformacoesContato { get; set; }
}