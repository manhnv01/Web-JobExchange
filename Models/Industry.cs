using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobExchange.Models
{
    public partial class Industry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IndustryId { get; set; }

        [Required(ErrorMessage = "Không được để trống!")]
        [Display(Name = "Tên ngành nghề")]
        public string IndustryName { get; set; }

		[DataType(DataType.Upload)]
		[FileExtensions(Extensions = "png, jpg", ErrorMessage = "Bạn chỉ có thể thêm file *.png hoặc *.jpg")]
		public string? IndustryImage { get; set; }
    }
}
