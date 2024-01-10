using System.Data;
using System.Diagnostics;
using Dapper;
using estacionamento_dapper.Models;
using estacionamento_dapper.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace estacionamento_dapper.Controllers;

[Route("/clientes")]
public class ClientesController : Controller
{
    private readonly IRepositorio<Cliente> _repo;

    public ClientesController(IRepositorio<Cliente> repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var clientes = _repo.ObterTodos();
        return View(clientes);
    }

    [HttpGet("novo")]    
    public IActionResult Novo()
    {
        return View();
    }

    [HttpPost("criar")]    
    public IActionResult Criar([FromForm] Cliente cliente)
    {
        _repo.Inserir(cliente);
        return Redirect("/clientes");
    }

    [HttpPost("{id}/apagar")]    
    public IActionResult Apagar([FromRoute] int id)
    {
        _repo.Excluir(id);

        return Redirect("/clientes");
    }

    [HttpGet("{id}/editar")]    
    public IActionResult Editar([FromRoute] int id)
    {
        var valor = _repo.ObterPorId(id);
        return View(valor);
    }

    [HttpPost("{id}/alterar")]    
    public IActionResult Alterar([FromRoute] int id, [FromForm] Cliente cliente)
    {
        cliente.Id = id;

        _repo.Atualizar(cliente);

        return Redirect("/clientes");
    }
}
