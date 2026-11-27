using System.ComponentModel.DataAnnotations;
using Tarea3.Models;

namespace Tarea3.Services;

public class ProductoService : IProductoService
{
    private readonly object bloqueo = new();
    private readonly List<Producto> productos =
    [
        new() { Id = 1, Nombre = "Cuaderno cuadriculado", Descripcion = "Tapa dura, 100 hojas.", Precio = 2490m, Cantidad = 24 },
        new() { Id = 2, Nombre = "Botella térmica", Descripcion = "Acero inoxidable, 500 ml.", Precio = 12990m, Cantidad = 12 },
        new() { Id = 3, Nombre = "Mochila urbana", Descripcion = "Compartimento para portátil.", Precio = 28990m, Cantidad = 8 },
        new() { Id = 4, Nombre = "Lámpara de escritorio", Descripcion = "LED con brazo ajustable.", Precio = 17990m, Cantidad = 6 },
        new() { Id = 5, Nombre = "Organizador de escritorio", Descripcion = "Tres compartimentos.", Precio = 6990m, Cantidad = 15 }
    ];
    private int siguienteId;

    public ProductoService()
    {
        siguienteId = productos.Max(producto => producto.Id) + 1;
    }

    public IReadOnlyList<Producto> Listar(string? nombre = null)
    {
        string filtro = nombre?.Trim() ?? string.Empty;

        lock (bloqueo)
        {
            return productos
                .Where(producto => producto.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                .Select(producto => Copiar(producto, producto.Id))
                .ToList();
        }
    }

    public string? Crear(Producto producto)
    {
        lock (bloqueo)
        {
            string? error = Validar(producto);
            if (error is not null)
            {
                return error;
            }

            productos.Add(Copiar(producto, GenerarId()));
            return null;
        }
    }

    private string? Validar(Producto producto, int? idExcluido = null)
    {
        var errores = new List<ValidationResult>();
        if (!Validator.TryValidateObject(producto, new ValidationContext(producto), errores, true))
        {
            return errores[0].ErrorMessage ?? "Revisa los datos del producto.";
        }

        bool duplicado = productos.Any(existente =>
            existente.Id != idExcluido &&
            existente.Nombre.Equals(producto.Nombre, StringComparison.OrdinalIgnoreCase));

        return duplicado ? "Ya existe un producto con ese nombre." : null;
    }

    private int GenerarId()
    {
        return siguienteId++;
    }

    private static Producto Copiar(Producto producto, int id)
    {
        return new Producto
        {
            Id = id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Cantidad = producto.Cantidad
        };
    }
}
