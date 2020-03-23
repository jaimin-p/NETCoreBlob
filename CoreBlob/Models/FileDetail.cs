using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBlob.Models
{
    public class FileDetail
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        [Display(Name = "Name")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Summary")]
        [Display(Name = "Summary")]
        [MaxLength(500)]
        public string Summary { get; set; }

        public DateTime Modified { get; set; }

        public virtual ICollection<BlobSummary> Blobs { get; set; }

    }
}
