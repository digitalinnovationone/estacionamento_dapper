namespace estacionamento_dapper.Repositorios;

public interface IRepositorio<T>
{
    IEnumerable<T> ObterTodos();
    T ObterPorId(int id);
    void Inserir(T entidade);
    void Atualizar(T entidade);
    void Excluir(int id);
}
