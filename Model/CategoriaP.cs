using BackendCancha.Data;
using System.ComponentModel.DataAnnotations;

namespace BackendCancha.Model
{
    public class CategoriaP
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public string Nombre { get; set; } = default!;

        // Inicializamos la colección para evitar advertencias de nulabilidad.
        public ICollection<Productos> Productos { get; set; } = new List<Productos>();
    }
}
