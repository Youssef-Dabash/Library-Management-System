using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Borrowing
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BorrowId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } // Navigation Properties

    public int BookId { get; set; }
    public Book Book { get; set; } // Navigation Properties

    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public decimal FineAmount { get; set; }
    public string Status { get; set; } // Pending, Returned, Overdue...
}
