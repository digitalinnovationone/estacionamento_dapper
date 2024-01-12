using System;
using estacionamento_dapper.Models;

[TestClass]
public class TicketTest
{
    [TestMethod]
    public void TestandoMetodoValorTotal()
    {
        // Arrange (Uma forma de iniciarmos nossos objetos)
        var cliente = new Cliente { Id = 1, Nome = "Danilo" };
        var veiculo = new Veiculo { Id = 1, Marca = "Fiat", Modelo = "Uno", ClienteId = cliente.Id, Placa = "DASS134" };
        var ticket = new Ticket { DataEntrada = DateTime.Now.AddHours(-1), Id = 1, VagaId = 1, VeiculoId = veiculo.Id };
        var valorDoMinuto = new ValorDoMinuto { Minutos = 1, Valor = 1 };
        var totalDesejadoDeMinuto = 60.0;

        // Act (ação que gera o resultado a ser testado)
        var valorTotal = ticket.ValorTotal(valorDoMinuto);

        // Assert (o teste sobre o resultado)
        Assert.AreEqual(totalDesejadoDeMinuto, valorTotal);
    }

    public void TestandoValorPagoDoTicket()
    {
        // Arrange (Uma forma de iniciarmos nossos objetos)
        var cliente = new Cliente { Id = 1, Nome = "Danilo" };
        var veiculo = new Veiculo { Id = 1, Marca = "Fiat", Modelo = "Uno", ClienteId = cliente.Id, Placa = "DASS134" };
        var ticket = new Ticket { DataEntrada = DateTime.Now.AddHours(-1), Id = 1, VagaId = 1, VeiculoId = veiculo.Id };
        var valorDoMinuto = new ValorDoMinuto { Minutos = 1, Valor = 1 };
        var valorTotalDesejado = ticket.ValorTotal(valorDoMinuto);

        // Act (ação que gera o resultado a ser testado)
        ticket.Pago(valorDoMinuto);

        // Assert (o teste sobre o resultado)
        Assert.AreEqual(valorTotalDesejado, ticket.Valor);
        Assert.IsNotNull(ticket.DataSaida);
    }
}