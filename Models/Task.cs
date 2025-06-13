using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    public class Task
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Worker é obrigatório")]
        public int WorkerId { get; set; }
        
        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Prazo é obrigatório")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        
        [Required(ErrorMessage = "Status é obrigatório")]
        [StringLength(20, ErrorMessage = "Status deve ter no máximo 20 caracteres")]
        public string Status { get; set; }
        
        [Required(ErrorMessage = "Tipo é obrigatório")]
        [StringLength(30, ErrorMessage = "Tipo deve ter no máximo 30 caracteres")]
        public string Type { get; set; }
        
        // Relacionamento
        // [ForeignKey("WorkerId")]
        public virtual Worker? Worker { get; set; }
    }
}