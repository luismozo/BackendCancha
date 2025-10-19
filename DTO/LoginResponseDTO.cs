namespace BackendCancha.DTO
{
    // DTO para la respuesta del login, coincidiendo con lo que espera el frontend.
    public class LoginResponseDTO
    {
        public string Token { get; set; } = default!;
        public int Id { get; set; }
        public string Nombre { get; set; } = default!;
        public string Rol { get; set; } = default!;
    }
}
