using Calabonga.OopExercises.ConsoleApp;
using Calabonga.OopExercises.ConsoleApp.Helpers;
using Calabonga.OopExercises.Domain;
using Calabonga.OopExercises.Domain.ValueObjects;
using Calabonga.OopExercises.Infrastructure;
using Calabonga.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Data;

#region prepare container and factories
var container = ConsoleApp.CreateContainer();
var logger = container.GetRequiredService<ILogger<Program>>();
#endregion

#region Check Database Exists and create repository

var unitOfWork = container.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();

unitOfWork.DbContext.Database.EnsureCreated();
if (unitOfWork.DbContext.ChangeTracker.HasChanges())
{
    unitOfWork.DbContext.Database.Migrate();
}

var repository = unitOfWork.GetRepository<Document>();
#endregion

Document document;

if (repository.Count() == 0)
{
    #region document creation
    document = Document.Create(Guid.Empty, "Закупка нового оборудования");
    var position = PositionValue.Create("DevLead").Result;
    var state = DocumentState.Create(Guid.Empty, "Draft", "dev@calabonga.net").Result;
    var participant = Participant.Create("Sergei", "Calabonga", position);
    document.AddState(state, participant);

    #endregion

    #region save document

    repository.Insert(document);
    try
    {
        unitOfWork.SaveChanges();
    }
    catch (Exception exception)
    {
        AnsiConsole.WriteException(exception);
        logger.LogError(exception, exception.Message);
        AnsiConsole.Console.Input.ReadKey(false);
    }
    #endregion
}
else
{
    #region getting from database
    var firstItem = repository
        .GetFirstOrDefault(
            include: i => i
                .Include(x => x.Participants)
                .Include(x => x.StateHistory),
            trackingType: TrackingType.Tracking);

    if (firstItem is null)
    {
        return;
    }

    document = firstItem;
    #endregion

    #region add new state

    var participantRepository = unitOfWork.GetRepository<Participant>();
    var participant = participantRepository.GetFirstOrDefault(predicate: x => x.LastName == "Calabonga");
    if (participant is null)
    {
        var error = new InvalidOperationException();
        logger.LogError(error, error.Message);
        return;
    }
    var state = DocumentState.Create(Guid.Empty, "New State " + DateTime.UtcNow.ToShortTimeString(), "dev@calabonga.net").Result;
    document.AddState(state, participant);
    document.AddDescription("Welcome to Domain Driven Design");

    #endregion

    #region save added data

    if (unitOfWork.DbContext.ChangeTracker.HasChanges())
    {
        repository.Update(document);

        unitOfWork.SaveChanges();

        if (!unitOfWork.Result.Ok)
        {
            var error = unitOfWork.Result.Exception ?? new DataException();
            logger.LogError(error, error.Message);
        }
    }

    #endregion
}

#region dump document

document.Dump();
AnsiConsole.Console.Input.ReadKey(false);

#endregion
