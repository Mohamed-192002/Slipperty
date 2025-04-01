namespace Business.DTO
{
    public class RegionDTO
    {
        public int? Id { get; set; }
        [Display(Name = "الاسم")]
        [Required(ErrorMessage = "ادخل الاسم")]
        public string? Name { get; set; }

        [Required]
        public int GovernmentId { get; set; }

        [ValidateNever]
        [NotMapped]
        public Government? Government { get; set; }
    }
}
