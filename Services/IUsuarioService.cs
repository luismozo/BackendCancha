using BackendCancha.DTO;

namespace BackendCancha.Services
{
    public interface IUsuarioService
    {
        // Contrato para el método de registro de un nuevo usuario.
        // Recibe un DTO de registro y devuelve el usuario creado (como un DTO de respuesta).
        Task<UsuarioResponseDTO> RegisterAsync(UsuarioDTO dto);

        // Contrato para el método de login.
        // Recibe un DTO de login y, si es exitoso, devuelve un DTO con el token y datos del usuario.
        // Si las credenciales son incorrectas, devolverá null.
        Task<LoginResponseDTO?> LoginAsync(LoginDTO dto);
    }
}
