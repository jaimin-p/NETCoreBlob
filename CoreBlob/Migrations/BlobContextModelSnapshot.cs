﻿// <auto-generated />
using System;
using CoreBlob.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreBlob.Migrations
{
    [DbContext(typeof(BlobContext))]
    partial class BlobContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreBlob.Models.BlobSummary", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Blob");

                    b.Property<string>("ContentType");

                    b.Property<string>("Extension");

                    b.Property<int?>("FileDetailId");

                    b.Property<string>("FileName");

                    b.HasKey("Id");

                    b.HasIndex("FileDetailId");

                    b.ToTable("BlobSummary");
                });

            modelBuilder.Entity("CoreBlob.Models.FileDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.HasKey("Id");

                    b.ToTable("FileDetail");
                });

            modelBuilder.Entity("CoreBlob.Models.BlobSummary", b =>
                {
                    b.HasOne("CoreBlob.Models.FileDetail", "FileDetail")
                        .WithMany("Blobs")
                        .HasForeignKey("FileDetailId");
                });
#pragma warning restore 612, 618
        }
    }
}
