using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum MembershipType
{
    Normal = 1,
    Premium = 2,
    VIP = 3
}

public class MembershipTier
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TierId { get; set; }

    public MembershipType TierName { get; set; } 

    public int ExtraBooks { get; set; }
    public int ExtraDays { get; set; }
    public decimal ExtraPenalty { get; set; }

    public ICollection<User>? Users { get; set; }
}
