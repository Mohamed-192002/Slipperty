namespace Infrastructure.Models
{
    public class VariableValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Value { get; set; } 

        [Required]
        public int ProductVariableId { get; set; }


        public string? ImageUrl { get; set; }

        [ForeignKey(nameof(ProductVariableId))]
        public virtual ProductVariable ProductVariable { get; set; }
    }
}
