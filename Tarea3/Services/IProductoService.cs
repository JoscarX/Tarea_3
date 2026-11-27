using Tarea3.Models;

namespace Tarea3.Services;

public interface IProductoService
{
    IReadOnlyList<Producto> Listar(string? nombre = null);
    string? Crear(Producto producto);
}
