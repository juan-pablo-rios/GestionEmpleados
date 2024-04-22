using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmpleados.Models;
using GestionEmpleados.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore.Storage;
using GestionEmpleados.Helpers;


namespace GestionEmpleados.Controllers;

public class EmpleadosController : Controller
{
    public readonly EmployeesDataBaseContext _context;
    public EmpleadosController(EmployeesDataBaseContext context)
    {
        _context = context;
    }
    // ----------------- LOGIN VIEW:
    public async Task<IActionResult> Index(string message = "")
    {
        ViewBag.Message = message;
        return View();
    }
    // ----------------- CREATE:
    // VIEW:
    public IActionResult Create()
    {
        return View();
    }
    // ACTION:
    [HttpPost]
    public async Task<IActionResult> Create(Empleado empleado)
    {
        // Se crea una instancia de la clase (PasswordHasher):
        var passHasher = new PasswordHasher();
        // Se inicializa una variable con la password encriptada utilizando el método de la clase (EncryptPassword):
        var EncriptedPassword = passHasher.EncryptPassword(empleado.Password);
        // Se actualiza la contraseña en el modelo:
        empleado.Password = EncriptedPassword;
        // Agrega el empleado al modelo:
        _context.Empleados.Add(empleado);
        // Guardar los cambios de DbSet en la db:
        await _context.SaveChangesAsync();
        // Se inicializa una variable de sesión:
        var auth = HttpContext.Session.GetString("EmpleadoId");
        // Se confirma si hay una variable de sesión:
        if (auth != null){
            // Redirección al panel:
            return RedirectToAction("PanelEmployees");
        }
        else
        {
            // Redirección al login:
            return RedirectToAction("Index");
        }
    }
    // ----------------- LOGIN ACTION:
    [HttpPost]
    public async Task<ActionResult> SignIn(string email, string password)
    {
        // Se confirma que los campos no estén vacíos:
        if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        {
            // Se busca el empleado en la base de datos:
            var user = await _context.Empleados.FirstOrDefaultAsync(e => e.Email == email);
            // Se confirma que se haya encontrado un usuario y se confirma la password:
            if(user != null)
            {
                // Se crea una instancia de la clase (PasswordHasher):
                var PasswordCompare = new PasswordHasher();
                // Se inicializa una variable para confirmar si la contraseña proporcionada coincide con la encriptada en la base de datos:
                var authPassword = PasswordCompare.VerifyPassword(user.Password, password);
                // Se confirma si coincidió:
                if(authPassword)
                {
                    // Se inicializa las variables de sesión necesarias:
                    HttpContext.Session.SetString("EmpleadoId", user.Id.ToString());
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("Nombre", user.Nombre);
                    // Se redirecciona a la vista 'Panel':
                    return RedirectToAction("PanelEmployees", "Empleados");
                }
                else{
                    return RedirectToAction("Index", new {message = "¡Los datos ingresados no coinciden!" });
                }
            }
            else
            {
                return RedirectToAction("Index", new {message = "¡Usuario no registrado!" });
            }
        }
        else
        {
            return RedirectToAction("Index", new {message = "¡Llena los campos!"});
        }
    }
    // ----------------- PANEL VIEW:
    public async Task<IActionResult> PanelEmployees()
    {
        // Se inicializa la variable con la lista de empleados:
        var empleados = await _context.Empleados.ToListAsync();
        // Se inicializa una variable confirmando la variable de sesión:
        var empleadoId = Int32.Parse(HttpContext.Session.GetString("EmpleadoId"));
        // Se inicializan las variables en ViewBag que necesite utilizar en la vista:
        ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
        // Se confirma de que la variable de sesión esté inicializada:
        if(empleadoId != null){
            return View(empleados.ToList());
        }
        else
        {
            // Si no está inicializada, no deja acceder a la vista 'Panel':
            return RedirectToAction("Index", "Empleados");
        }
    }
    // ----------------- LOGOUT ACTION:
    public IActionResult Logout()
    {
        // Se remueven las variables de sesión:
        HttpContext.Session.Remove("Email");
        HttpContext.Session.Remove("Nombre");
        HttpContext.Session.Remove("EmpleadoId");
        return RedirectToAction("Index", "Empleados");
    }
    // ----------------- DELETE ACTION:
    // 4. ELIMINAR:
    public async Task<IActionResult> Eliminar(int id)
    {
        // Se inicializa una variable con la variable de sesión:
        var SessionId = Int32.Parse(HttpContext.Session.GetString("EmpleadoId"));
        // Se busca el registro que coincida con el 'id' en el modelo 'empleado':
        var empleado = await _context.Empleados.FindAsync(id);
        // Una vez encontrado, se elimina ese registro del modelo:
        _context.Empleados.Remove(empleado);
        // Se actualiza la base de datos:
        await _context.SaveChangesAsync();
        // Se confirma si el usuario eliminado es el mismo que está en la variable de sesión:
        if(id == SessionId)
        {
            // Si es así, se eliminan las variables de sesión (Logout action):
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Nombre");
            HttpContext.Session.Remove("EmpleadoId");
            return RedirectToAction("Index", "Empleados");
        }
        else
        {
            return RedirectToAction("PanelEmployees", "Empleados");
        }
    }

}