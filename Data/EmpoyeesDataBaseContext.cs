using Microsoft.EntityFrameworkCore;
using GestionEmpleados.Models;

namespace GestionEmpleados.Data;

public class EmployeesDataBaseContext : DbContext
{
    public EmployeesDataBaseContext(DbContextOptions<EmployeesDataBaseContext> options) : base(options)
    {

    }
    // SE REGISTRAN LOS MODELOS EN Context:
    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Turno> Turnos { get; set; }
    public DbSet<Documento> Documentos { get; set; }
}