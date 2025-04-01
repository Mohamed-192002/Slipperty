namespace Infrastructure.Models
{
    public class BlockedNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
    }
}
