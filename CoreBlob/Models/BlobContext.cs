using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBlob.Models
{
    public class BlobContext :DbContext
    {
        public BlobContext(DbContextOptions<BlobContext> options): base(options)
        {
        }

        public DbSet<FileDetail> FileDetail { get; set; }
        public DbSet<BlobSummary> BlobSummary { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
        }

    }
}
