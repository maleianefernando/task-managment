using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class Worker
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Por favor, informe o nome")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Por favor, informe o cargo")]
        [StringLength(50, ErrorMessage = "Posição deve ter no máximo 50 caracteres")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Por favor, informe o gênero")]
        [RegularExpression(@"[MF]", ErrorMessage = "Por favor, informe o gênero")]
        public char Gender { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Relacionamentos
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}