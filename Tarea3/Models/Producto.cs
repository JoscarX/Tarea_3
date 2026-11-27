using System.ComponentModel.DataAnnotations;

namespace Tarea3.Models;

public class Producto : IValidatableObject
{
    private string nombre = string.Empty;

    public int Id { get; init; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Nombre
    {
        get => nombre;
        set => nombre = value?.Trim() ?? string.Empty;
    }

    [StringLength(250, ErrorMessage = "La descripción admite hasta 250 caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(typeof(decimal), "0.01", "999999.99", ParseLimitsInInvariantCulture = true,
        ErrorMessage = "El precio debe estar entre 0,01 y 999.999,99.")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(0, 999999, ErrorMessage = "La cantidad debe estar entre 0 y 999.999.")]
    public int Cantidad { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (decimal.Round(Precio, 2) != Precio)
        {
            yield return new ValidationResult("El precio admite hasta dos decimales.", [nameof(Precio)]);
        }
    }
}
