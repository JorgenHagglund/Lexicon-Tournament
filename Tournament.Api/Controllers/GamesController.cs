using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.DTOs;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Api.Controllers;

[Route("api/Game")]
[ApiController]
public class GamesController(IUnitOfWork uow, IMapper mapper) : ControllerBase
{
    // GET: api/Games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDto>>> GetGame([FromQuery] GameQueryOptionsDto options)
    {
        if (string.IsNullOrEmpty(options.Title))
            return Ok((await uow.GameRepository.GetGamesAsync())
                .Select(game => mapper.Map<GameDto>(game)));

        try
        {
            var games = await uow.GameRepository.GetByTitleAsync(options.Title!, options.ExactMatch);
            if (games == null || !games.Any())
                return NotFound($"No games with title '{options.Title}' were found");
            return Ok(games.Select(game => mapper.Map<GameDto>(game)));
        }
        catch
        {
            return NotFound($"No games with title '{options.Title}' were found");
        }
    }

    // GET: api/Games/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GameDto>> GetGame(int id)
    {
        try
        {
            var game = await uow.GameRepository.GetGameAsync(id);

            if (game == null)
                return NotFound();

            return Ok(mapper.Map<GameDto>(game));
        }
        catch
        {
            return NotFound($"No game with id {id} was found");
        }
    }

    // PUT: api/Games/5
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(int id, GameDto gameDto)
    {
        Game game;
        try
        {
            var existing = await uow.GameRepository.GetGameAsync(id);
            if (existing == null)
                return NotFound();

            game = mapper.Map<GameDto, Game>(gameDto, existing, opts =>
            {
                opts.BeforeMap((src, dest) =>
                {
                    dest.Id = existing.Id;
                    dest.Title = existing.Title;
                    dest.StartTime = existing.StartTime;
                    dest.EndTime = existing.EndTime;
                    dest.TournamentId = existing.TournamentId;
                });
            });

            uow.GameRepository.Update(game);
            await uow.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await GameExistsAsync(id))
                return NotFound();
            else
                throw;
        }

        return AcceptedAtAction(nameof(GetGame), new { id }, game);
    }

    // POST: api/Games
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
    {
        var game = mapper.Map<Game>(gameDto);
        uow.GameRepository.Create(game);
        await uow.CompleteAsync();

        return CreatedAtAction("GetGame", new { game.Id }, game);
    }

    // DELETE: api/Games/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(int id)
    {
        try
        {
            var game = await uow.GameRepository.GetGameAsync(id);
            if (game == null)
                return NotFound();

            uow.GameRepository.Delete(game);
            await uow.CompleteAsync();

            return NoContent();
        }
        catch
        {
            return NotFound($"No game with id {id} found");
        }
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PatchGame(int id, JsonPatchDocument<GameUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("Patch document cannot be null"); 

        //var existingTournament = await uow.TournamentRepository.GetTournamentAsync(tournamentId);
        //if (existingTournament is null)
        //    return NotFound($"No Tournament with id {tournamentId} found");

        var gameToPatch = await uow.GameRepository.GetGameAsync(id, true);
        if (gameToPatch is null)
            return NotFound($"No game with id {id} found");

        var dto = mapper.Map<GameUpdateDto>(gameToPatch);
        patchDoc.ApplyTo(dto, ModelState);
        if (!TryValidateModel(dto))
            return BadRequest(ModelState);

        mapper.Map(dto, gameToPatch);
        await uow.CompleteAsync();

        return NoContent();

    }

    private async Task<bool> GameExistsAsync(int id) =>
        await uow.GameRepository.ExistsAsync(id);
}
