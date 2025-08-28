using LibraryManagementSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; } // add db

    public ICollection<Book>? Books { get; set; }
}
