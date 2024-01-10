using System.Data;
using System.Diagnostics;
using Dapper;
using estacionamento_dapper.Models;
using estacionamento_dapper.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace estacionamento_dapper.Controllers;

[Route("/vagas")]
public class VagasController : Controller
{
    private readonly IRepositorio<Vaga> _repo;

    public VagasController(IRepositorio<Vaga> repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var vagas = _repo.ObterTodos();
        return View(vagas);
    }

    [HttpGet("novo")]    
    public IActionResult Novo()
    {
        return View();
    }

    [HttpPost("criar")]    
    public IActionResult Criar([FromForm] Vaga vaga)
    {
        _repo.Inserir(vaga);
        return Redirect("/vagas");
    }

    [HttpPost("{id}/apagar")]    
    public IActionResult Apagar([FromRoute] int id)
    {
        _repo.Excluir(id);

        return Redirect("/vagas");
    }

    [HttpGet("{id}/editar")]    
    public IActionResult Editar([FromRoute] int id)
    {
        var valor = _repo.ObterPorId(id);
        return View(valor);
    }

    [HttpPost("{id}/alterar")]    
    public IActionResult Alterar([FromRoute] int id, [FromForm] Vaga vaga)
    {
        vaga.Id = id;

        _repo.Atualizar(vaga);

        return Redirect("/vagas");
    }
}
