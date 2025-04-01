namespace Infrastructure.Models
{
    public class UserAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public int GovernmentId { get; set; }
        [Required] 
        public int RegionId { get; set; }

        [Required]
        public string? Address { get; set; }


        [Required]
        public int AddressTypeId { get; set; }

        //[ForeignKey(nameof(UserId))]
        //public ApplicationUser? User { get; set; }
        [ForeignKey(nameof(GovernmentId))]
        public Government? Government { get; set; }
        [ForeignKey(nameof(RegionId))]
        public Region? Region { get; set; }
        [ForeignKey(nameof(AddressTypeId))]
        public AddressType? AddressType { get; set; }

    }
}
