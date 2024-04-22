using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmpleados.Models;
using GestionEmpleados.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Storage;
using Pomelo.EntityFrameworkCore.MySql.Metadata.Internal;

namespace GestionEmpleados.Controllers;

public class TurnosController : Controller
{
    public readonly EmployeesDataBaseContext _context;
    public TurnosController(EmployeesDataBaseContext context)
    {
        _context = context;
    }
    // ----------------- ERROR VIEW:
    public IActionResult Error()
    {
        return View();
    }
    // ----------------- HISTORIAL VIEW:
    public async Task<IActionResult> Index()
    {
        // Se inicializa la variable con la lista de horarios:
        // var turnos = await _context.Turnos.ToListAsync();

        // Se inicializa una variable confirmando la variable de sesión:
        var SessionId = Int32.Parse(HttpContext.Session.GetString("EmpleadoId"));
        // Se inicializan las variables en ViewBag que necesite utilizar en la vista:
        ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
        //Se inicializa una variable en ViewBag para confirmar si el empleado ya ha registrado una entrada:
        ViewBag.EmpleadoTurno = await _context.Turnos.OrderBy(e => e.EmpleadoId).FirstOrDefaultAsync(e => e.EmpleadoId == SessionId && e.HoraSalida == null);
        // Se confirma de que la variable de sesión esté inicializada:
        if(SessionId != null)
        {
            // Consulta LINQ para traer todos los registros del modelo:
            var registrosTurnos = from historial in _context.Turnos select historial;
            // Filtrar únicamente el historial que corresponda a la variable de sesión:
            registrosTurnos = registrosTurnos.Where(r => r.EmpleadoId == SessionId);
            // Retorno de la vista:
            return View(await registrosTurnos.ToListAsync());
        }
        else
        {
            // Si no hay variable de sesión se redirecciona al Login:
            return RedirectToAction("Index", "Empleados");
        }
    }
    // ----------------- FECHA ENTRADA ACTION:
    public async Task<IActionResult> RegistrarEntrada()
    {
        // Se inicializa la variable id del HttpContext.Session:
        var Id = Int32.Parse(HttpContext.Session.GetString("EmpleadoId"));
        // Se inicializa una instancia del modelo Turno:
        var turno = new Turno()
        {
            EmpleadoId = Id,
            HoraEntrada = DateTime.Now
        };
        // Se agrega el objeto turno al modelo:
        _context.Turnos.Add(turno);
        // Se guardan los cambios en la base de datos:
        await _context.SaveChangesAsync();
        // Se redirecciona a la vista indicada:
        return RedirectToAction("Index", "Turnos");
    }
     // ----------------- FECHA SALIDA VIEW:
    public async Task<IActionResult> RegistrarSalida()
    {
        // Se inicializa la variable id del HttpContext.Session:
        var Id = Int32.Parse(HttpContext.Session.GetString("EmpleadoId"));
        // Se inicializa una variable tipo objeto con el registro que coincida con las condiciones:
        var empleado = await _context.Turnos.OrderBy(e => e.Id).LastOrDefaultAsync(e => e.EmpleadoId == Id && e.HoraSalida == null);
        // Una vez encontrado el registro, se modifica/actualiza el campo HoraSalida:
        empleado.HoraSalida = DateTime.Now;
        // Se guardan los cambios en la base de datos:
        await _context.SaveChangesAsync();
        // Se redirecciona a la vista indicada:
        return RedirectToAction("Index", "Turnos");
    }
}