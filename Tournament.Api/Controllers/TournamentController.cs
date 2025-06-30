using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.DTOs;
using Tournament.Core.Repositories;

namespace Tournament.Api.Controllers;

[Route("api/Tournament")]
[ApiController]
public class TournamentController(IUnitOfWork uow, IMapper mapper) : ControllerBase
{
    // GET: api/Tournament
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails([FromQuery] TournamentDisplayOptionsDto options) =>
        Ok(mapper.Map<IEnumerable<TournamentDto>>(await uow.TournamentRepository.GetTournamentsAsync(options)));

    // GET: api/Tournament/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id, bool includeGames)
    {
        try
        {
            var tournamentDetails = await uow.TournamentRepository.GetTournamentAsync(id, includeGames);
            if (tournamentDetails == null)
                return NotFound($"No tournament with id {id} exists in the database");

            return Ok(mapper.Map<TournamentDto>(tournamentDetails));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    // PUT: api/Tournament/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTournamentDetails(int id, TournamentUpdateDto tournamentDetailsDto)
    {
        var tournamentDetails = mapper.Map<Core.Entities.Tournament>(tournamentDetailsDto);
        // Commented out as id shouldn't be changable, hence it's not part of the DTO
        //if (id != tournamentDetails.Id)
        //    return BadRequest($"Tournament id's mismatch {id} <> {tournamentDetails.Id}");

        try
        {
            uow.TournamentRepository.Update(tournamentDetails);

            await uow.CompleteAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TournamentDetailsExists(id))
                return NotFound();
            else
                throw;
        }

        return AcceptedAtAction(nameof(GetTournamentDetails), new { id }, tournamentDetails);
    }

    // POST: api/Tournament
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TournamentUpdateDto>> PostTournamentDetails(TournamentUpdateDto tournamentDto)
    {
        var tournament = mapper.Map<Core.Entities.Tournament>(tournamentDto);
        uow.TournamentRepository.Create(tournament);
        await uow.CompleteAsync();

        return CreatedAtAction("GetTournamentDetails", new { tournament.Id }, tournament);
    }

    // DELETE: api/Tournament/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTournamentDetails(int id)
    {
        var tournamentDetails = await uow.TournamentRepository.GetTournamentAsync(id, false, true);
        if (tournamentDetails == null)
            return NotFound();

        uow.TournamentRepository.Delete(tournamentDetails);
        await uow.CompleteAsync();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTournamentDetails(int id, JsonPatchDocument<TournamentUpdateDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("Patch document cannot be null.");

        var existingTournament = await uow.TournamentRepository.GetTournamentAsync(id, false, true);
        if (existingTournament == null)
            return NotFound();

        var dto = mapper.Map<TournamentUpdateDto>(existingTournament);
        patchDoc.ApplyTo(dto, ModelState);
        if (!TryValidateModel(dto))
            return BadRequest(ModelState);

        mapper.Map(dto, existingTournament);
        uow.TournamentRepository.Update(existingTournament);
        await uow.CompleteAsync();

        return AcceptedAtAction(nameof(GetTournamentDetails), new { id }, existingTournament);
    }

    private async Task<bool> TournamentDetailsExists(int id) =>
        await uow.TournamentRepository.ExistsAsync(id);
}
