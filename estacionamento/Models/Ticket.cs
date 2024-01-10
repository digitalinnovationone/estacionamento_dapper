using System;
using System.ComponentModel.DataAnnotations.Schema;
using estacionamento_dapper.Repositorios;

namespace estacionamento_dapper.Models;

[Table("tickets")]
public class Ticket
{
    [IgnoreInDapper]
    public int Id { get; set; } = default!;
    public DateTime DataEntrada { get; set; } = default!;
    public DateTime? DataSaida { get; set; } // Nullable para permitir valores nulos
    public float Valor { get; set; } = default!;
    public int VeiculoId { get; set; } = default!;
    [IgnoreInDapper]
    public Veiculo Veiculo { get; set; } = default!;
    public int VagaId { get; set; } = default!;
}