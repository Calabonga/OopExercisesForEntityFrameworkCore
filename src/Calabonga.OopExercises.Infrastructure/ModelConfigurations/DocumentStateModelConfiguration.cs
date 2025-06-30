using Calabonga.OopExercises.Domain;
using Calabonga.OopExercises.Domain.ValueObjects;
using Calabonga.OopExercises.Infrastructure.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calabonga.OopExercises.Infrastructure.ModelConfigurations;

public class DocumentStateModelConfiguration : AuditableModelConfiguration<DocumentState>
{
    protected override void AddBuilder(EntityTypeBuilder<DocumentState> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(TitleValue.MaxTitleLength)
            .HasConversion(x => x.Value, x => TitleValue.Create(x).Result);
    }

    protected override string TableName()
    {
        return "DocumentStates";
    }
}
