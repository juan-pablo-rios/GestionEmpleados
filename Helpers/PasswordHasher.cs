using BCryptNet = BCrypt.Net.BCrypt;

namespace GestionEmpleados.Helpers;

// CLASE:
public class PasswordHasher
{
    // 1.MÉTODO: Encripta la contraseña:
    public string EncryptPassword(string password)
    {
        // Se inicializa una variable de la password encriptada con el método (HashPassword):
        string EncryptedPassword = BCryptNet.HashPassword(password, BCryptNet.GenerateSalt());
        // Se retorna la password encriptada:
        return EncryptedPassword;
    }

    // 2.MÉTODO: Compara la contraseña enviada por el usuario en el Login con la contraseña encriptada guardada en la base de datos:
    public bool VerifyPassword(string dataBasePassword, string passwordProvided)
    {
        // Se iniciaiza una variable para confirmar si la contraseña proporcionada coincide con la guardada en la base de datos con el método (Verify):
        bool passwordMatch = BCryptNet.Verify(passwordProvided, dataBasePassword);
        // Se retorna la variable confirmando si la contraseña coincidió:
        return passwordMatch;
    }
}