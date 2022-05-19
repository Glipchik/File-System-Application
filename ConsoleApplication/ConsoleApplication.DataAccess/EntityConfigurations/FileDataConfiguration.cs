using ConsoleApplication.Domain.MetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApplication.DataAccess.EntityConfigurations
{
    public class FileDataConfiguration : IEntityTypeConfiguration<FileData>
    {
        public void Configure(EntityTypeBuilder<FileData> builder)
        {
            builder.Property(f => f.Name).HasMaxLength(50);
            builder.Property(f => f.Extension).HasMaxLength(10);
            builder.Property(f => f.Path).HasMaxLength(150);
            builder.Property(f => f.Size).HasColumnType("bigint");
            builder.Property(f => f.CreationDate);
            builder.Property(f => f.DownloadsNumber).HasColumnType("smallint");
            builder.Property(f => f.LastWriteTime);
        }
    }
}