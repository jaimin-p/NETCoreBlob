using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBlob.Models
{
    public class BlobSummary
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public int FileDetail_Id { get; set; }
        public byte[] Blob { get; set; }
        public virtual FileDetail FileDetail { get; set; }
    }
}
