using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;

namespace NetKubernetes.Data;

public class LoadDatabase {

    public static async Task InsertarData(AppDbContext context, UserManager<Usuario> usuarioManager)
    {
        if (!usuarioManager.Users.Any()){

            var usuario = new Usuario{
              Nombre = "Carlos",
                Apellido = "Noel",
                Email = "carlos.noel@gmail.com",
                UserName = "cnoel",
                Telefono = "936172648"  
            };

            await usuarioManager.CreateAsync(usuario, "PasswordCnoel123@");
        }

        if (!context.Inmuebles!.Any()){

            context.Inmuebles!.AddRange(
                new Inmueble{
                    Nombre = "Casa Playa",
                    Direccion = "Av. el Sol",
                    Precio = 4500M,
                    FechaCreacion = DateTime.Now
                },
                new Inmueble{
                    Nombre = "Casa Invierno",
                    Direccion = "Av. el Hielo",
                    Precio = 5100M,
                    FechaCreacion = DateTime.Now
                }
            );
        }

        context.SaveChanges();
    }
}