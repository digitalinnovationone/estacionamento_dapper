using System.Data;
using System.Diagnostics;
using Dapper;
using estacionamento_dapper.DTO;
using estacionamento_dapper.Models;
using estacionamento_dapper.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace estacionamento_dapper.Controllers;

[Route("/tickets")]
public class TicketsController : Controller
{
    private readonly IDbConnection _cnn;
    private readonly IRepositorio<Ticket> _repo;

    public TicketsController(IDbConnection cnn)
    {
        _cnn = cnn;
        _repo = new RepositorioDapper<Ticket>(_cnn);
    }

    public IActionResult Index()
    {
         var sql = """
            SELECT t.*, v.*, c.*, vg.* FROM tickets t
            INNER JOIN veiculos v ON v.id = t.veiculoId
            INNER JOIN clientes c ON c.id = v.clienteId
            INNER JOIN vagas vg ON vg.id = t.vagaId
            order by t.id desc
        """;

        var tickets = _cnn.Query<Ticket, Veiculo, Cliente, Vaga, Ticket>(sql, (ticket, veiculo, cliente, vaga) => {
            veiculo.Cliente = cliente;
            ticket.Veiculo = veiculo;
            ticket.Vaga = vaga;
            return ticket;
        }, splitOn: "Id, Id, Id");

        
        ViewBag.ValorDoMinuto = _cnn.QueryFirstOrDefault<ValorDoMinuto>("select * from valores order by id desc limit 1")!;
        return View(tickets);
    }

    [HttpGet("novo")]    
    public IActionResult Novo()
    {
        preencheVagasViewBag();
        return View();
    }

    [HttpPost("criar")]    
    public IActionResult Criar([FromForm] TicketDTO ticketDTO)
    {
        Cliente cliente = buscarOuCadastrarClientePorDTO(ticketDTO);
        Veiculo veiculo = buscarOuCadastrarVeiculoPorDTO(ticketDTO, cliente);

        var ticket = new Ticket();
        ticket.VeiculoId = veiculo.Id;
        ticket.DataEntrada = DateTime.Now;
        ticket.VagaId = ticketDTO.VagaId;

        _repo.Inserir(ticket);

        alteraStatusVaga(ticket.VagaId, true);

        return Redirect("/tickets");
    }

    [HttpPost("{id}/pago")]    
    public IActionResult Pago([FromRoute] int id)
    {
       var sql = """
            SELECT t.*, v.*, c.*, vg.* FROM tickets t
            INNER JOIN veiculos v ON v.id = t.veiculoId
            INNER JOIN clientes c ON c.id = v.clienteId
            INNER JOIN vagas vg ON vg.id = t.vagaId
            where t.id = @id
        """;

        Ticket? ticket = _cnn.Query<Ticket, Veiculo, Cliente, Vaga, Ticket>(sql, (ticket, veiculo, cliente, vaga) => {
            veiculo.Cliente = cliente;
            ticket.Veiculo = veiculo;
            ticket.Vaga = vaga;
            return ticket;
        }, new { id = id }, splitOn: "Id, Id, Id").FirstOrDefault();

        if(ticket != null)
        {
            var valorDoMinuto = _cnn.QueryFirstOrDefault<ValorDoMinuto>("select * from valores order by id desc limit 1")!;
            
            ticket.Pago(valorDoMinuto);
            _repo.Atualizar(ticket);
            alteraStatusVaga(ticket.VagaId, false);
        }
        return Redirect("/tickets");
    }

    [HttpPost("{id}/apagar")]    
    public IActionResult Apagar([FromRoute] int id)
    {
        var ticket = _repo.ObterPorId(id);
        alteraStatusVaga(ticket.VagaId, false);
        _repo.Excluir(id);
        return Redirect("/tickets");
    }

    private void preencheVagasViewBag()
    {
        var sql = """
            SELECT * FROM vagas 
            where Ocupada = false
        """;

        var vagas = _cnn.Query<Vaga>(sql);

        ViewBag.Vagas = new SelectList(vagas, "Id", "CodigoLocalizacao");
    }

    private Cliente buscarOuCadastrarClientePorDTO(TicketDTO ticketDTO)
    {
        Cliente? cliente = null;

        if(!string.IsNullOrEmpty(ticketDTO.Cpf))
        {
            var query = "SELECT * FROM clientes where Cpf = @Cpf";
            cliente = _cnn.QueryFirstOrDefault<Cliente>(query, new Cliente { Cpf = ticketDTO.Cpf });
        } 

        if(cliente != null) return cliente;

        cliente = new Cliente();
        cliente.Nome = ticketDTO.Nome;
        cliente.Cpf = ticketDTO.Cpf;

        string sql = @"INSERT INTO clientes (Nome, CPF) VALUES (@Nome, @Cpf); SELECT LAST_INSERT_ID();";

        cliente.Id = _cnn.QuerySingle<int>(sql, cliente);

        return cliente;
    }
    
    private Veiculo buscarOuCadastrarVeiculoPorDTO(TicketDTO ticketDTO, Cliente cliente)
    {
        Veiculo? veiculo = null;

        if(!string.IsNullOrEmpty(ticketDTO.Placa))
        {
            var query = "SELECT * FROM veiculos where placa = @Placa and ClienteId = @ClienteId";
            veiculo = _cnn.QueryFirstOrDefault<Veiculo>(query, new Veiculo { Placa = ticketDTO.Placa, ClienteId = cliente.Id });
        } 

        if(veiculo != null) return veiculo;

        veiculo = new Veiculo();
        veiculo.Placa = ticketDTO.Placa;
        veiculo.Marca = ticketDTO.Marca;
        veiculo.Modelo = ticketDTO.Modelo;
        veiculo.ClienteId = cliente.Id;

        var sql = $"INSERT INTO veiculos (Placa, Marca, Modelo, ClienteId) VALUES (@Placa, @Marca, @Modelo, @ClienteId); SELECT LAST_INSERT_ID()";
        veiculo.Id = _cnn.QuerySingle<int>(sql, veiculo);

        return veiculo;
    }

    private void alteraStatusVaga(int VagaId, bool ocupada)
    {
        var sql = $"UPDATE vagas SET ocupada = @Ocupada where id = @Id";
        _cnn.Execute(sql, new Vaga { Id = VagaId, Ocupada = ocupada });
    }
}
