using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BookId { get; set; }

    public string Title { get; set; }
    public string Author { get; set; }
    public string? ISBN { get; set; }
    public int PublishYear { get; set; }
    public int? AvailableCopies { get; set; }
    public string? Image { get; set; }
    public string? Description { get; set; } // add db
    public int CategoryId { get; set; }

    [ValidateNever]
    public Category Category { get; set; } = null!; // Navigation Properties

    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
