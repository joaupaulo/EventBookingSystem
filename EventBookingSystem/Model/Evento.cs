using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventBookingSystem.Model;

public class Evento
{
    [BsonId]
    public ObjectId _Id { get; private set; }
    public Guid EventKey { get; private set; }
    public string Nome { get; private set; }
    public DateTime Data { get; private set; }
    public string Local { get; private set; }
    public int CapacidadeMaxima { get; private set; }
    public decimal Preco { get; private set; }
    public string Descricao { get; private set; }

    private Evento(string nome, DateTime data, string local, int capacidadeMaxima, decimal preco, string descricao)
    {
        if (string.IsNullOrEmpty(nome))
            throw new ArgumentException("O nome do evento não pode ser vazio.", nameof(nome));

        if (data < DateTime.Now)
            throw new ArgumentException("A data do evento não pode estar no passado.", nameof(data));

        if (string.IsNullOrEmpty(local))
            throw new ArgumentException("O local do evento não pode ser vazio.", nameof(local));

        if (capacidadeMaxima <= 0)
            throw new ArgumentException("A capacidade máxima deve ser maior que zero.", nameof(capacidadeMaxima));

        if (preco < 0)
            throw new ArgumentException("O preço não pode ser negativo.", nameof(preco));

        _Id = ObjectId.GenerateNewId();
        EventKey = Guid.NewGuid();
        Nome = nome;
        Data = data;
        Local = local;
        CapacidadeMaxima = capacidadeMaxima;
        Preco = preco;
        Descricao = descricao;
    }

    public static Evento CriarNovoEvento(string nome, DateTime data, string local, int capacidadeMaxima, decimal preco, string descricao)
    {
        return new Evento(nome, data, local, capacidadeMaxima, preco, descricao);
    }

    public void DefinirCapacidadeMaxima(int novaCapacidade)
    {
        if (novaCapacidade <= 0)
            throw new ArgumentException(" A capacidade maxima deve ser maior que 0");
        CapacidadeMaxima = CapacidadeMaxima - novaCapacidade;
    }
}