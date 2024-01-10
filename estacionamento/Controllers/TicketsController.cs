using System.Data;
using System.Diagnostics;
using Dapper;
using estacionamento_dapper.Models;
using estacionamento_dapper.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace estacionamento_dapper.Controllers;

[Route("/tickets")]
public class TicketsController : Controller
{
    private readonly IRepositorio<Ticket> _repo;

    public TicketsController(IRepositorio<Ticket> repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var tickets = _repo.ObterTodos();
        return View(tickets);
    }

    [HttpGet("novo")]    
    public IActionResult Novo()
    {
        return View();
    }

    [HttpPost("criar")]    
    public IActionResult Criar([FromForm] Ticket ticket)
    {
        _repo.Inserir(ticket);
        return Redirect("/tickets");
    }

    [HttpPost("{id}/apagar")]    
    public IActionResult Apagar([FromRoute] int id)
    {
        _repo.Excluir(id);

        return Redirect("/tickets");
    }

    [HttpGet("{id}/editar")]    
    public IActionResult Editar([FromRoute] int id)
    {
        var valor = _repo.ObterPorId(id);
        return View(valor);
    }

    [HttpPost("{id}/alterar")]    
    public IActionResult Alterar([FromRoute] int id, [FromForm] Ticket ticket)
    {
        ticket.Id = id;

        _repo.Atualizar(ticket);

        return Redirect("/tickets");
    }
}
