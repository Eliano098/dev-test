using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class ClientImportEntityTypeConfiguration : BaseEntityTypeConfiguration<ClientImport>
    {
        public override void Configure(EntityTypeBuilder<ClientImport> builder)
        {
            builder.ToTable(nameof(ClientImport));
            builder.Property(i => i.FileName).IsRequired().HasColumnType("varchar(255)");
            builder.Property(i => i.FilePath).IsRequired().HasColumnType("varchar(500)");
            builder.Property(i => i.Status).IsRequired().HasConversion<int>().HasColumnType("int");
            builder.Property(i => i.ProcessedRows).IsRequired().HasColumnType("int");
            builder.Property(i => i.SuccessfulRows).IsRequired().HasColumnType("int");
            builder.Property(i => i.FailedRows).IsRequired().HasColumnType("int");
            builder.Property(i => i.ErrorMessage).HasColumnType("varchar(2000)");
        }
    }
}
