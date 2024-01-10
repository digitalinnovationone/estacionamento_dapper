using System;

namespace estacionamento_dapper.Models;

public class Veiculo
{
    public int Id { get; set; } = default!;
    public string Placa { get; set; } = default!;
    public string Modelo { get; set; } = default!;
    public string Marca { get; set; } = default!;
    public int ClienteId { get; set; } = default!;
    public Cliente Cliente { get; set; } = default!;
}