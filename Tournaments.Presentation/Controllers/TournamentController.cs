using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using Tournament.Core.DTOs;
using E = Tournament.Core.Entities;

namespace Tournaments.Presentation.Controllers;

[Route("api/Tournament")]
[ApiController]
public class TournamentController(IServiceManager serviceManager) : ControllerBase
{
    // GET: api/Tournament
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetailsAsync([FromQuery] TournamentDisplayOptionsDto options, [FromQuery] PaginationDto pagination) =>
        Ok(await serviceManager.TournamentService.GetTournamentsAsync(options, pagination, false));

    // GET: api/Tournament/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentDetailsAsync(int id, bool includeGames)
    {
        try
        {
            var tournamentDetails = await serviceManager.TournamentService.GetTournamentAsync(id, includeGames, false);
            if (tournamentDetails == null)
                return NotFound($"No tournament with id {id} exists in the database");

            return Ok(tournamentDetails);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("/throw")]
    public static ActionResult<TournamentDto> ThrowException() =>
        // This is just for testing purposes, to see how the error handling works
        throw new KeyNotFoundException("This is a test exception");

    // PUT: api/Tournament/5
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetailsAsync(int id, TournamentDto tournamentDetailsDto)
    {
        //var dto = serviceManager.TournamentService.GetTournamentAsync(id, false, true);
        // Commented out as id shouldn't be changeable, hence it's not part of the DTO
        //if (id != tournamentDetails.Id)
        //    return BadRequest($"Tournament id's mismatch {id} <> {tournamentDetails.Id}");

        try
        {
            await serviceManager.TournamentService.UpdateAsync(id, tournamentDetailsDto);
            //uow.TournamentRepository.Update(tournamentDetails);
            //await uow.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TournamentDetailsExistsAsync(id))
                return NotFound();
            else
                throw;
        }

        return AcceptedAtAction(nameof(GetTournamentDetailsAsync), new { id }, tournamentDetailsDto);
    }

    // POST: api/Tournament
    // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TournamentUpdateDto>> PostTournamentDetailsAsync(TournamentDto tournamentDto)
    {
        E.Tournament result;
        try
        {
            if (tournamentDto is null)
                return BadRequest("Tournament details cannot be null.");
            result = await serviceManager.TournamentService.CreateAsync(tournamentDto);
        }
        catch (DbUpdateException)
        {
            return Conflict("A tournament with the same name already exists.");
        }

        return CreatedAtAction("GetTournamentDetails", new { result.Id }, result);
    }

    // DELETE: api/Tournament/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentDetailsAsync(int id)
    {
        try
        {
            await serviceManager.TournamentService.DeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No tournament with id {id} exists in the database");
        }

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTournamentDetailsAsync(int id, JsonPatchDocument<TournamentUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("Patch document cannot be null.");

        TournamentDto? existingTournament = await serviceManager.TournamentService.GetTournamentAsync(id, false, true);
        try
        {
            await serviceManager.TournamentService.PatchAsync(id, patchDoc, this);
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"No tournament with id {id} found");
        }

        //var existingTournament = await uow.TournamentRepository.GetTournamentAsync(id, false, true);
        //if (existingTournament == null)
        //    return NotFound();

        //var dto = mapper.Map<TournamentUpdateDto>(existingTournament);
        //patchDoc.ApplyTo(dto, ModelState);
        //if (!TryValidateModel(dto))
        //    return BadRequest(ModelState);

        //mapper.Map(dto, existingTournament);
        //uow.TournamentRepository.Update(existingTournament);
        //await uow.CompleteAsync();

        return AcceptedAtAction(nameof(GetTournamentDetailsAsync), new { id }, existingTournament);
    }

    private async Task<bool> TournamentDetailsExistsAsync(int id) =>
        await serviceManager.TournamentService.ExistsAsync(id);
}
