using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tournament.Core.Entities;
using Tournament.Data.Extensions;
using E = Tournament.Core.Entities;

namespace Tournament.Data.Data;

public static partial class SeedData
{
    private static readonly Random random = new();
    private static readonly List<Range<DateTime>> generatedDates = new();
    public static async Task SeedTournament(IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<TournamentContext>();
        await context.Database.MigrateAsync();

        if (await context.Tournaments.AnyAsync())
            return; // Database already seeded

        var tournaments = GenerateTournaments(10);
        context.Tournaments.AddRange(tournaments);

        //context.Tournaments.AddRange(
        //    new Tournament.Core.Entities.Tournament
        //    {
        //        Title = "Summer Tournament",
        //        StartTime = new DateTime(2023, 6, 1),
        //        Games =
        //        [
        //            new Tournament.Core.Entities.Game
        //            {
        //                Title = "Game 1",
        //                Time = new DateTime(2023, 6, 1, 10, 0, 0)
        //            },
        //            new Tournament.Core.Entities.Game
        //            {
        //                Title = "Game 2",
        //                Time = new DateTime(2023, 6, 1, 12, 0, 0)
        //            }
        //        ]
        //    },
        //    new Tournament.Core.Entities.Tournament
        //    {
        //        Title = "Winter Tournament",
        //        StartTime = new DateTime(2023, 12, 1),
        //        Games =
        //        [
        //            new Tournament.Core.Entities.Game
        //            {
        //                Title = "Game 3",
        //                Time = new DateTime(2023, 12, 1, 10, 0, 0)
        //            },
        //            new Tournament.Core.Entities.Game
        //            {
        //                Title = "Game 4",
        //                Time = new DateTime(2023, 12, 1, 12, 0, 0)
        //            }
        //        ]
        //    }
        //);
        await context.SaveChangesAsync();

    }

    private static List<E.Tournament> GenerateTournaments(int noOfTournaments)
    {
        var faker = new Faker<E.Tournament>("sv")
            .Rules((f, t) =>
                {
                    generatedDates.Clear();
                    t.Title = f.Lorem.Slug(f.Random.Int(1, 5)).Capitalize();
                    t.StartDate = f.Date.Future(1);
                    t.EndDate = t.StartDate.AddMonths(3);
                    t.Games = GenerateGames(f.Random.Int(1, 10), t.StartDate, t.EndDate);
                });
        return faker.Generate(noOfTournaments);
    }

    private static DateTime RandomizeDate(DateTime startDate, DateTime endDate)
    {
        TimeSpan timeSpan = endDate - startDate;
        TimeSpan newSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);
        return startDate + newSpan;
    }


    private static List<Game> GenerateGames(int noOfGames, DateTime startDate, DateTime endDate)
    {
        var faker = new Faker<Game>("sv")
            .Rules((f, g) =>
            {
                g.Title = f.Lorem.Slug(f.Random.Int(1, 5)).Capitalize();
                do
                {
                    DateTime starts = RandomizeDate(startDate, endDate);
                    DateTime ends = RandomizeDate(starts, endDate);
                    Range<DateTime> range = new(starts, ends);
                    if (!generatedDates.Any(r => !r.Intersects(range)))
                    {
                        generatedDates.Add(range);
                        g.StartTime = starts;
                        g.EndTime = ends;
                        break;
                    }
                }
                while (true);
            });
        return faker.Generate(noOfGames);
    }
}
