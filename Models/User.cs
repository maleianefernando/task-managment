using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Worker é obrigatório")]
        public int WorkerId { get; set; }
        
        [Required(ErrorMessage = "Função é obrigatória")]
        [StringLength(30, ErrorMessage = "Função deve ter no máximo 30 caracteres")]
        public string Role { get; set; }
        
        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
        public string Password { get; set; }
        
        // Relacionamento
        [ForeignKey("WorkerId")]
        public virtual Worker Worker { get; set; }
    }
}