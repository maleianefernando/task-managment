using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Models
{
    public class Task
    {
        public int Id { get; set; }
        
        // [Required(ErrorMessage = "Por favor, indique um funcionário")]
        public int? WorkerId { get; set; }
        
        [Required(ErrorMessage = "Por favor, informe título")]
        [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Por favor, informe o prazo")]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }
        
        [Required(ErrorMessage = "Por favor, informe o estado")]
        [StringLength(20, ErrorMessage = "Status deve ter no máximo 20 caracteres")]
        public string Status { get; set; }
        
        [Required(ErrorMessage = "Por favor, informe o tipo")]
        [StringLength(30, ErrorMessage = "Tipo deve ter no máximo 30 caracteres")]
        public string Type { get; set; }
        
        // Relacionamento
        // [ForeignKey("WorkerId")]
        public virtual Worker? Worker { get; set; }
    }
}