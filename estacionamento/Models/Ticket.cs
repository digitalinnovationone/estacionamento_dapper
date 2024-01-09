using System;

namespace estacionamento_dapper.Models;

public class Ticket
{
    public int Id { get; set; } = default!;
    public DateTime DataEntrada { get; set; } = default!;
    public DateTime? DataSaida { get; set; } // Nullable para permitir valores nulos
    public float Valor { get; set; } = default!;
    public int VeiculoId { get; set; } = default!;
    public int VagaId { get; set; } = default!;
}