using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.DL.Models;

namespace Movies.DL.Data.Config
{
    public class GenreConfigurations : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {

            builder.Property(G => G.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
