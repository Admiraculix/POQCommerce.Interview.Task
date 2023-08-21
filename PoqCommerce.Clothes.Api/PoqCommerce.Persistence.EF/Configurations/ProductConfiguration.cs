using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoqCommerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoqCommerce.Persistence.EF.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products"); // Set the table name

            builder.HasKey(p => p.Id); // Set the primary key

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(100); // Configure Title property

            builder.Property(p => p.Price)
                .HasColumnType("decimal(10, 2)"); // Configure Price property

            builder.Property(p => p.Sizes)
                .HasConversion(
                    v => string.Join(',', v), // Convert List<string> to comma-separated string
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() // Convert back to List<string>
                )
                .HasMaxLength(100); // Configure Sizes property with conversion

            builder.Property(p => p.Description)
                .HasMaxLength(500); // Configure Description property
        }
    }

}
