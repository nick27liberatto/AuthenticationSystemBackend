namespace Infrastructure.Mapping
{
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("USER");

            builder.HasKey(x => x.Id)
                .HasName("ID_USER");

            builder.Property(x => x.Id)
                .HasColumnName("ID_USER")
                .HasColumnType("NUMBER(10)");

            builder.Property(x => x.Username)
                .HasColumnName("USERNAME")
                .HasColumnType("VARCHAR(50)")
                .IsRequired(true);

            builder.Property(x => x.Password)
                .HasColumnName("PASSWORD")
                .HasColumnType("VARCHAR(100)")
                .IsRequired(true);
        }
    }
}
