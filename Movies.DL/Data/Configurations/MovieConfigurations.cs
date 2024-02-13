using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.DL.Data.Configurations
{
    public class MovieConfigurations : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
             builder.Property(m => m.Title)
             .HasMaxLength(250)
              .IsRequired();

            builder.Property(m => m.StoryLine)
                   .HasMaxLength(2500)
                   .IsRequired();

       

        }
    }
}
