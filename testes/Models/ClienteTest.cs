using System;
using estacionamento_dapper.Models;

[TestClass]
public class ClienteTest
{
    [TestMethod]
    public void TestandoPropiedadesDoModelCliente()
    {
        // Arrange (Uma forma de iniciarmos nossos objetos)
        var cliente = new Cliente();

        // Act (ação que gera o resultado a ser testado)
        cliente.Id = 1;
        cliente.Nome = "Danilo";
        cliente.Cpf = "653.027.290-91";

        // Assert (o teste sobre o resultado)
        Assert.AreEqual(1, cliente.Id);
        Assert.AreEqual("Danilo", cliente.Nome);
        Assert.AreEqual("653.027.290-91", cliente.Cpf);
    }
}