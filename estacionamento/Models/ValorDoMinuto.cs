using System;

namespace estacionamento_dapper.Models;

public class ValorDoMinuto
{
    public int Id { get; set; } = default!;
    public int Minutos { get; set; } = default!;
    public float ValorUnitario { get; set; } = default!;
}