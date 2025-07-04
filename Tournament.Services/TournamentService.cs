using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Tournament.Core.Contracts;
using Tournament.Core.DTOs;
using E = Tournament.Core.Entities;

namespace Tournament.Services;

public class TournamentService(IUnitOfWork uow, IMapper mapper) : ITournamentService
{
    public async Task<IEnumerable<TournamentDto>> GetTournamentsAsync(TournamentDisplayOptionsDto options, PaginationDto pagination, bool trackChanges = false) =>
        mapper.Map<IEnumerable<TournamentDto>>(await uow.TournamentRepository.GetTournamentsAsync(options, pagination, trackChanges));

    public async Task<TournamentDto?> GetTournamentAsync(int id, bool includeGames = false, bool trackChanges = false) =>
        mapper.Map<TournamentDto>(await uow.TournamentRepository.GetTournamentAsync(id, includeGames, trackChanges));

    public async Task<E.Tournament> CreateAsync(TournamentDto dto)
    {
        var tournament = mapper.Map<E.Tournament>(dto);
        uow.TournamentRepository.Create(tournament);
        await uow.CompleteAsync();
        return tournament;
    }

    public async Task DeleteAsync(int id)
    {
        var tournament = await uow.TournamentRepository.GetTournamentAsync(id, false, true) ?? throw new KeyNotFoundException($"Tournament with ID {id} not found.");
        uow.TournamentRepository.Delete(tournament);
        await uow.CompleteAsync();
    }

    public async Task<E.Tournament> UpdateAsync(int id, TournamentDto dto)
    {
        var existingTournament = await uow.TournamentRepository.GetTournamentAsync(id, false, true) ?? throw new KeyNotFoundException($"Tournament with ID {id} not found.");
        mapper.Map(dto, existingTournament);
        uow.TournamentRepository.Update(existingTournament);
        await uow.CompleteAsync();
        return existingTournament;
    }

    public async Task PatchAsync(int id, JsonPatchDocument<TournamentUpdateDto> patchDoc, ControllerBase controller)
    {
        var tournamentToPatch = await uow.TournamentRepository.GetTournamentAsync(id, false, true) ?? throw new KeyNotFoundException($"Tournament with ID {id} not found.");
        var dto = mapper.Map<TournamentUpdateDto>(tournamentToPatch);
        patchDoc.ApplyTo(dto, error =>
            controller.ModelState.AddModelError(error.AffectedObject.ToString() ?? string.Empty, error.ErrorMessage));
        if (!controller.TryValidateModel(dto))
            throw new InvalidOperationException("Invalid patch document.");
        mapper.Map(dto, tournamentToPatch);
        uow.TournamentRepository.Update(tournamentToPatch);
        await uow.CompleteAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await uow.TournamentRepository.ExistsAsync(id);
}
