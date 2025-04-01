namespace Infrastructure.Models
{
    public class ProductVariable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public int? ProductId { get; set; }



        //public virtual IEnumerable<VariableValue>? ProductVariableValues { get; set; }
        public virtual IEnumerable<VariableValue>? VariableValues { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }


    }
}
