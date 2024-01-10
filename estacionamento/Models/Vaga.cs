using System;
using System.ComponentModel.DataAnnotations.Schema;
using estacionamento_dapper.Repositorios;

namespace estacionamento_dapper.Models;

[Table("vagas")]
public class Vaga
{
    [IgnoreInDapper]
    public int Id { get; set; } = default!;
    public string CodigoLocalizacao { get; set; } = default!;
    public bool Ocupada { get; set; } = default!;
}