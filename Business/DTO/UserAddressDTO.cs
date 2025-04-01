namespace Business.DTO
{
    public class UserAddressDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "ادخل الاسم")]
        [Display(Name = "الاسم")]
        public string? Name { get; set; }
        [Required]
        public string? UserId { get; set; }


        [Display(Name = "المحافظة")]
        [Required(ErrorMessage = "ختر المحافظة")]
        public int? GovernmentId { get; set; }

        [Display(Name = "المنطقة")]
        [Required(ErrorMessage = "اختر المنطقة")]
        public int? RegionId { get; set; }


        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "ادخل العنوان")]
        public string? Address { get; set; }


        [Display(Name = "النوع")]
        [Required(ErrorMessage = "اختر النوع")]
        public int AddressTypeId { get; set; }

        public string? GovernmentName { get; set; }
        public string? RegionName { get; set; }

        [ValidateNever]
        [NotMapped]
        public GovernmentDTO? Government { get; set; }
        [ValidateNever]
        [NotMapped]
        public RegionDTO? Region { get; set; }
        [ValidateNever]
        [NotMapped]
        public AddressTypeDTO? AddressType { get; set; }


        [ValidateNever]
        [NotMapped]
        public string? oldAddress { get; set; }


    }
}
