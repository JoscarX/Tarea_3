using Tarea3.Models;

namespace Tarea3.Services;

public interface IProductoService
{
    IReadOnlyList<Producto> Listar(string? nombre = null);
    Producto? Obtener(int id);
    string? Crear(Producto producto);
    string? Actualizar(Producto producto);
    bool Eliminar(int id);
}
