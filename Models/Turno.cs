
namespace GestionEmpleados.Models;

/* REPRESENTACIÓN DE LA BASE DE DATOS*/
// TABLA
public class Turno
{
    // CAMPOS:
    public int? Id { get; set; }
    public DateTime HoraEntrada { get; set; }
    public DateTime? HoraSalida { get; set; }
    public int EmpleadoId { get; set; }
}