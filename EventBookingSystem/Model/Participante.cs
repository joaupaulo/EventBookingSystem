using MongoDB.Bson;

namespace EventBookingSystem.Model;

    public class Participante
    {
        public ObjectId ParticipanteId { get;  set; }
        public string Nome { get;  set; }
        public string Email { get;  set; }
        public string Telefone { get;  set; }
        public DateTime DataInscricao { get;  set; }

        public Participante(string nome, string email, string telefone)
        {
            if (string.IsNullOrEmpty(nome))
                throw new ArgumentException("O nome do participante não pode ser vazio.", nameof(nome));

            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("O email do participante não pode ser vazio.", nameof(email));

            if (!IsValidEmail(email))
                throw new ArgumentException("O email do participante não é válido.", nameof(email));


            ParticipanteId = ObjectId.GenerateNewId();
            Nome = nome;
            Email = email;
            Telefone = telefone;
            DataInscricao = DateTime.Now; 
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
