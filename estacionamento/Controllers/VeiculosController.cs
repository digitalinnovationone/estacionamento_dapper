using System.Data;
using System.Diagnostics;
using Dapper;
using estacionamento_dapper.Models;
using estacionamento_dapper.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace estacionamento_dapper.Controllers;

[Route("/veiculos")]
public class VeiculosController : Controller
{
    private readonly IDbConnection _cnn;
    private readonly IRepositorio<Veiculo> _repo;

    public VeiculosController(IDbConnection cnn)
    {
        _cnn = cnn;
        _repo = new RepositorioDapper<Veiculo>(_cnn);
    }

    public IActionResult Index()
    {
        var sql = """
            SELECT v.*, c.* FROM veiculos v
            INNER JOIN clientes c ON c.id = v.clienteId
        """;

        var veiculos = _cnn.Query<Veiculo, Cliente, Veiculo>(sql, (veiculo, cliente) => {
            veiculo.Cliente = cliente;
            return veiculo;
        }, splitOn: "Id");


        return View(veiculos);
    }

    [HttpGet("novo")]    
    public IActionResult Novo()
    {
        return View();
    }

    [HttpPost("criar")]    
    public IActionResult Criar([FromForm] Veiculo veiculo)
    {
        _repo.Inserir(veiculo);
        return Redirect("/veiculos");
    }

    [HttpPost("{id}/apagar")]    
    public IActionResult Apagar([FromRoute] int id)
    {
        _repo.Excluir(id);

        return Redirect("/veiculos");
    }

    [HttpGet("{id}/editar")]    
    public IActionResult Editar([FromRoute] int id)
    {
        var valor = _repo.ObterPorId(id);
        return View(valor);
    }

    [HttpPost("{id}/alterar")]    
    public IActionResult Alterar([FromRoute] int id, [FromForm] Veiculo veiculo)
    {
        veiculo.Id = id;

        _repo.Atualizar(veiculo);

        return Redirect("/veiculos");
    }
}
