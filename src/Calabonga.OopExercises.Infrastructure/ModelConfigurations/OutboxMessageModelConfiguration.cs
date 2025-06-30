using Calabonga.OopExercises.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calabonga.OopExercises.Infrastructure.ModelConfigurations;

public class OutboxMessageModelConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable("OutboxMessages");

        builder.Property(x => x.CreatedAtUtc).IsRequired().HasConversion
        (
            src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
            dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
        );

        builder
            .Property(x => x.Content)
            .HasMaxLength(4096)
            .IsRequired();

        builder.Property(x => x.ProcessedAtUtc).HasConversion
        (
            src => src != null && src.Value.Kind == DateTimeKind.Utc
                ? src
                : DateTime.SpecifyKind(src!.Value, DateTimeKind.Utc),

            dst => dst != null && dst.Value.Kind == DateTimeKind.Utc
                ? dst
                : DateTime.SpecifyKind(dst!.Value, DateTimeKind.Utc)
        );

        builder.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(x => x.Error)
            .HasMaxLength(8192);
    }
}
