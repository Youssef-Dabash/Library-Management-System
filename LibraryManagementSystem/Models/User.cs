using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 100 characters.")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "User Name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 50 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "User Name can contain only letters and numbers.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone must be a valid Egyptian mobile number.")]
    public string Phone { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Birth Date")]
    public DateTime? BirthOfDate { get; set; }

    public bool Status { get; set; } // add db

    [Required(ErrorMessage = "Tier is required.")]
    public int TierId { get; set; } 

    [ValidateNever]
    public MembershipTier MembershipTier { get; set; }

    [ValidateNever]
    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();


}
