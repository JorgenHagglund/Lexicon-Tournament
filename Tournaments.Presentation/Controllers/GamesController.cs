using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Tournament.Core.DTOs;
using Tournament.Core.Entities;

namespace Tournaments.Presentation.Controllers;

[Route("api/Game")]
[ApiController]
public class GamesController(IServiceManager serviceManager) : ControllerBase
{
    // GET: api/Games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGameAsync([FromQuery] GameQueryOptionsDto options, [FromQuery] PaginationDto pagination)
    {
        //if (string.IsNullOrEmpty(options.Title))
        //    return Ok(await serviceManager.GameService.GetGamesAsync(options, pagination, false));

        try
        {
            var games = await serviceManager.GameService.GetGamesAsync(options, pagination, false);
            if (games == null || !games.Any())
                return NotFound($"No games with title '{options.Title}' were found");
            return Ok(games);
        }
        //    var games = await serviceManager.GameService.GetByTitleAsync(options, pagination, false);
        //    if (games == null || !games.Any())
        //        return NotFound($"No games with title '{options.Title}' were found");
        //    return Ok(games);
        //}
        catch
        {
            return NotFound($"No games with title '{options.Title}' were found");
        }
    }

    // GET: api/Games/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameDto>> GetGameAsync(int id)
    {
        try
        {
            var game = await serviceManager.GameService.GetGameAsync(id);

            if (game == null)
                return NotFound();

            return Ok(game);
        }
        catch
        {
            return NotFound($"No game with id {id} was found");
        }
    }

    // PUT: api/Games/5
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGameAsync(int id, GameDto gameDto)
    {
        Game game;
        try
        {
            //var existing = await serviceManager.GameService.GetGameAsync(id);
            //if (existing == null)
            //    return NotFound();

            //game = mapper.Map<GameDto, Game>(gameDto, existing, opts =>
            //{
            //    opts.BeforeMap((src, dest) =>
            //    {
            //        dest.Id = existing.Id;
            //        dest.Title = existing.Title;
            //        dest.StartTime = existing.StartTime;
            //        dest.EndTime = existing.EndTime;
            //        dest.TournamentId = existing.TournamentId;
            //    });
            //});

            game = await serviceManager.GameService.UpdateAsync(id, gameDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GameExistsAsync(id))
                return NotFound();
            else
                throw;
        }

        return AcceptedAtAction(nameof(GetGameAsync), new { id }, game);
    }

    // POST: api/Games
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<GameDto>> PostGameAsync(GameToCreateDto gameDto)
    {
        var game = await serviceManager.GameService.CreateAsync(gameDto);
        //var game = mapper.Map<Game>(gameDto);
        //uow.GameRepository.Create(game);
        //await uow.CompleteAsync();

        return CreatedAtAction("GetGame", new { game.Id }, game);
    }

    // DELETE: api/Games/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGameAsync(int id)
    {
        try
        {
            //var game = await uow.GameRepository.GetGameAsync(id);
            //if (game == null)
            //    return NotFound();

            //uow.GameRepository.Delete(game);
            //await uow.CompleteAsync();
            await serviceManager.GameService.DeleteAsync(id);

            return NoContent();
        }
        catch
        {
            return NotFound($"No game with id {id} found");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchGameAsync(int id, JsonPatchDocument<GameUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("Patch document cannot be null");

        //var existingTournament = await uow.TournamentRepository.GetTournamentAsync(tournamentId);
        //if (existingTournament is null)
        //    return NotFound($"No Tournament with id {tournamentId} found");

        //var gameToPatch = await uow.GameRepository.GetGameAsync(id, true);
        //if (gameToPatch is null)
        //    return NotFound($"No game with id {id} found");

        //var dto = mapper.Map<GameUpdateDto>(gameToPatch);
        //patchDoc.ApplyTo(dto, ModelState);
        //if (!TryValidateModel(dto))
        //    return BadRequest(ModelState);

        //mapper.Map(dto, gameToPatch);
        //await uow.CompleteAsync();

        try
        {
            await serviceManager.GameService.PatchAsync(id, patchDoc, this);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No game with id {id} found");
        }
        return NoContent();
    }

    private async Task<bool> GameExistsAsync(int id) =>
        await serviceManager.GameService.ExistsAsync(id);
}
