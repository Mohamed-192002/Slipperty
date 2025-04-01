namespace Infrastructure.Models
{
    public class LinkType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
    }
}
