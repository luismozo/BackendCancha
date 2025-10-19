using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BackendCancha.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _config;

        // El servicio ahora depende del DbContext y la Configuración.
        public UsuarioService(AppDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginDTO dto)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Correo == dto.Correo);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            {
                // Si el usuario no existe o la contraseña es incorrecta, devolvemos null.
                return null;
            }

            var token = GenerarToken(usuario);

            // Devolvemos el DTO de respuesta que el frontend espera.
            return new LoginResponseDTO
            {
                Token = token,
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol
            };
        }

        public async Task<UsuarioResponseDTO> RegisterAsync(UsuarioDTO dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Telefono = dto.Telefono,
                PasswordHash = passwordHash,
                Rol = dto.Rol // Asumimos "Cliente" desde el frontend.
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            // Mapeamos la entidad a un DTO de respuesta para no exponer el hash.
            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                Rol = usuario.Rol
            };
        }

        // La lógica para generar el token ahora vive dentro del servicio.
        private string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2), // Extendemos la expiración
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
