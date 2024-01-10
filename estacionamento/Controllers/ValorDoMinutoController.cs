using System.Data;
using System.Diagnostics;
using Dapper;
using estacionamento_dapper.Models;
using estacionamento_dapper.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace estacionamento_dapper.Controllers;

[Route("/valores")]
public class ValorDoMinutoController : Controller
{
    private readonly IRepositorio<ValorDoMinuto> _repo;
    private readonly IRepositorio<Veiculo> _repoVeic;

    public ValorDoMinutoController(IRepositorio<ValorDoMinuto> repo, IRepositorio<Veiculo> repoVeic)
    {
        _repo = repo;
        _repoVeic = repoVeic;
    }

    public IActionResult Index()
    {
        var veiculos = _repoVeic.ObterTodos();
        var valores = _repo.ObterTodos();
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
        _repo.Inserir(valorDoMinuto);
        return Redirect("/valores");
    }

    [HttpPost("{id}/apagar")]    
    public IActionResult Apagar([FromRoute] int id)
    {
        _repo.Excluir(id);

        return Redirect("/valores");
    }

    [HttpGet("{id}/editar")]    
    public IActionResult Editar([FromRoute] int id)
    {
        var valor = _repo.ObterPorId(id);
        return View(valor);
    }

    [HttpPost("{id}/alterar")]    
    public IActionResult Alterar([FromRoute] int id, [FromForm] ValorDoMinuto valorDoMinuto)
    {
        valorDoMinuto.Id = id;

        _repo.Atualizar(valorDoMinuto);

        return Redirect("/valores");
    }
}
