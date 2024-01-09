using System.Data;
using System.Diagnostics;
using Dapper;
using estacionamento_dapper.Models;
using Microsoft.AspNetCore.Mvc;

namespace estacionamento_dapper.Controllers;

[Route("/valores")]
public class ValorDoMinutoController : Controller
{
    private readonly IDbConnection _connection;

    public ValorDoMinutoController(IDbConnection connection)
    {
        _connection = connection;
    }

    public IActionResult Index()
    {
        var valores = _connection.Query<ValorDoMinuto>("SELECT * FROM valores");
        return View(valores);
    }

    [HttpGet("novo")]    
    public IActionResult Novo()
    {
        return View();
    }

    [HttpPost("Criar")]    
    public IActionResult Criar([FromForm] ValorDoMinuto valorDoMinuto)
    {
        var sql = "INSERT INTO valores (Minutos, Valor) VALUES (@Minutos, @Valor)";
        _connection.Execute(sql, valorDoMinuto);

        return Redirect("/valores");
    }

    [HttpPost("{id}/apagar")]    
    public IActionResult Apagar([FromRoute] int id)
    {
        var sql = "delete from valores where id=@id";
        _connection.Execute(sql, new ValorDoMinuto{ Id = id });

        return Redirect("/valores");
    }

    [HttpGet("{id}/editar")]    
    public IActionResult Editar([FromRoute] int id)
    {
        var valor = _connection.Query<ValorDoMinuto>("SELECT * FROM valores where id = @id", new ValorDoMinuto{Id = id}).FirstOrDefault();
        return View(valor);
    }

    [HttpPost("{id}/alterar")]    
    public IActionResult Alterar([FromRoute] int id, [FromForm] ValorDoMinuto valorDoMinuto)
    {
        valorDoMinuto.Id = id;

        var sql = "UPDATE valores SET Minutos = @Minutos, valor = @Valor where id = @id";
        _connection.Execute(sql, valorDoMinuto);

        return Redirect("/valores");
    }
}
