using Calabonga.OopExercises.Domain;
using Calabonga.OopExercises.Domain.ValueObjects;
using Calabonga.OopExercises.Infrastructure.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Calabonga.OopExercises.Infrastructure.ModelConfigurations;

public class ParticipantStateModelConfiguration : AuditableModelConfiguration<Participant>
{
    protected override void AddBuilder(EntityTypeBuilder<Participant> builder)
    {
        builder
            .Property(x => x.Position)
            .IsRequired()
            .HasMaxLength(PositionValue.MaxLength)
            .HasConversion(x => x.Value, x => PositionValue.Create(x).Result);

        builder
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(256);

        builder
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(256);
    }

    protected override string TableName()
    {
        return "Participants";
    }
}
