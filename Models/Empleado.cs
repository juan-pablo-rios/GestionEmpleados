

using System.ComponentModel.DataAnnotations;

namespace GestionEmpleados.Models;

/* REPRESENTACIÓN DE LA BASE DE DATOS*/
// TABLA
public class Empleado
{
    // CAMPOS:
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellidos { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? Estado { get; set; }
    public int? Documento { get; set; }
}