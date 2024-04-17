using Microsoft.AspNetCore.Identity;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Inmuebles;

public class InmuebleRepository : IInmuebleRepository
{
    private readonly AppDbContext _context;
    private readonly IUsuarioSesion _usuariosesion;
    private readonly UserManager<Usuario> _userManager;

    public InmuebleRepository(AppDbContext context, IUsuarioSesion usuariosesion, UserManager<Usuario> userManager)
    {
        _context = context;
        _usuariosesion = usuariosesion;
        _userManager = userManager;
    }

    public async Task CreateInmueble(Inmueble inmueble)
    {
        var usuario = await _userManager.FindByNameAsync(_usuariosesion.ObtenerUsuarioSesion());

        inmueble.FechaCreacion = DateTime.Now;
        inmueble.UsuarioId = Guid.Parse(usuario!.Id);

        await _context.Inmuebles!.AddAsync(inmueble);
    }

    public void DeleteInmueble(int id)
    {
        var inmueble = _context.Inmuebles!
                        .FirstOrDefault(x => x.Id == id);

        _context.Inmuebles!.Remove(inmueble!);
    }

    public IEnumerable<Inmueble> GetAllInmuebles()
    {
        return [.. _context.Inmuebles!]; // or you can use: _context.Inmueble.ToList();
    }

    public Inmueble GetInmuebleById(int id)
    {
        return _context.Inmuebles!.FirstOrDefault(x => x.Id == id)!;
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}