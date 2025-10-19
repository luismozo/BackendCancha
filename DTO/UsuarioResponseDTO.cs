namespace BackendCancha.DTO
{
    // DTO para devolver información segura de un usuario (sin contraseña).
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = default!;
        public string Apellido { get; set; } = default!;
        public string Correo { get; set; } = default!;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = default!;
    }
}
