using Microsoft.AspNetCore.Identity;
using NetKubernetes.Dtos.UsuarioDtos;
using NetKubernetes.Models;
using NetKubernetes.Token;

namespace NetKubernetes.Data.Usuarios;


public class UsuarioRepository : IUsuarioRepository
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IJwtGenerator _jwtGenerador;
    private readonly AppDbContext _contexto;
    private readonly IUsuarioSesion _usuarioSesion;

    public UsuarioRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerator jwtGenerador, AppDbContext contexto, IUsuarioSesion usuarioSesion)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerador = jwtGenerador;
        _contexto = contexto;
        _usuarioSesion = usuarioSesion;
    }

    public async Task<UsuarioResponseDto> GetUsuario()
    {
        var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
        return TransforUserTOUserDto(usuario!);
    }

    private UsuarioResponseDto TransforUserTOUserDto(Usuario usuario)
    {
        return new UsuarioResponseDto {
            Id = usuario?.Id,
            Nombre = usuario?.Nombre,
            Apellido = usuario?.Apellido,
            Telefono = usuario?.Telefono,
            Email = usuario?.Email,
            Username = usuario?.UserName,
            Token = _jwtGenerador.CreatToken(usuario!)
        };
    }

    public async Task<UsuarioResponseDto> Login(UsuarioLoginRequestDto loginRequest)
    {
        var usuario = await _userManager.FindByEmailAsync(loginRequest.Email!);

        await _signInManager.CheckPasswordSignInAsync(usuario!, loginRequest.Password!, false);

        return TransforUserTOUserDto(usuario!);
    }

    public async Task<UsuarioResponseDto> RegistroUsuario(UsuarioRegistroRequestDto userrequest)
    {
        var usuario = new Usuario {
            Nombre = userrequest.Nombre,
            Apellido = userrequest.Apellido,
            Telefono = userrequest.Telefono,
            Email = userrequest.Email,
            UserName = userrequest.UserName
        };

        await _userManager.CreateAsync(usuario!, userrequest.Password!);

        return TransforUserTOUserDto(usuario!);
    }
}