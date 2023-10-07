using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoAPI.Models;

public class TodoItem
{
    [Key]
    public int Id { set; get; }
    
    [Required]
    [Column]
    public string Title { set; get; } = "";
    
    [Column]
    public string Description { set; get; } = "";
    
    public bool IsCompleted { set; get; } = false;

}