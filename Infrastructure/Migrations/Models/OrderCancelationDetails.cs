namespace Infrastructure.Migrations.Models
{
    public class OrderCancelationDetails : IModelBase
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public int OrderID { get; set; }
        public string Notes { get; set; }
        public int? RejectionReasoneId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        [ForeignKey("OrderID")]
        public virtual OrderHead OrderHead { get; set; } = null!;
    }
}
