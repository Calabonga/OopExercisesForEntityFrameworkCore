using Calabonga.OopExercises.Domain;
using Calabonga.OopExercises.Domain.ValueObjects;
using Calabonga.OopExercises.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calabonga.OopExercises.Infrastructure.ModelConfigurations;

public class DocumentModelConfiguration : AuditableModelConfiguration<Document>
{
    protected override void AddBuilder(EntityTypeBuilder<Document> builder)
    {
        builder
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(TitleValue.MaxTitleLength)
            .HasConversion(x => x.Value, x => TitleValue.Create(x).Result);

        builder
            .HasMany(x => x.StateHistory)
            .WithOne()
            .HasForeignKey(x => x.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Participants)
            .WithOne()
            .HasForeignKey(x => x.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Description)
            .IsRequired(false)
            .HasMaxLength(1024);

        builder.HasIndex(x => x.Title);
    }

    protected override string TableName()
    {
        return "Documents";
    }
}
