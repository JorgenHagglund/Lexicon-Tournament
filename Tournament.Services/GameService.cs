using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Contracts;
using Tournament.Core.Contracts;
using Tournament.Core.DTOs;
using Tournament.Core.Entities;

namespace Tournament.Services;

public class GameService(IUnitOfWork uow, IMapper mapper) : IGameService
{
    public async Task<GameDto?> GetGameAsync(int id, bool trackChanges = false) =>
        mapper.Map<GameDto?>(await uow.GameRepository.GetGameAsync(id, trackChanges));

    public async Task<IEnumerable<GameDto>> GetGamesAsync(GameQueryOptionsDto options, PaginationDto pagination, bool trackChanges = false) =>
        mapper.Map<IEnumerable<GameDto>>(await uow.GameRepository.GetGamesAsync(options, pagination, trackChanges));

    public async Task<Game> CreateAsync(GameToCreateDto dto)
    {
        if (uow.GameRepository.CountGames(dto.TournamentId) < 10)
        {
            var game = mapper.Map<Game>(dto);
            uow.GameRepository.Create(game);
            await uow.CompleteAsync();
            return game;
        }
        else
            throw new InvalidOperationException("Cannot create more than 10 games for a tournament.");
    }

    public async Task<Game> UpdateAsync(int id, GameDto dto)
    {
        var existingGame = await uow.GameRepository.GetGameAsync(id, true) ?? throw new KeyNotFoundException($"Game with ID {id} not found.");
        mapper.Map(dto, existingGame);
        uow.GameRepository.Update(existingGame);
        await uow.CompleteAsync();
        return existingGame;
    }

    public async Task DeleteAsync(int id)
    {
        var game = await uow.GameRepository.GetGameAsync(id, true) ?? throw new KeyNotFoundException($"Game with ID {id} not found.");
        uow.GameRepository.Delete(game);
        await uow.CompleteAsync();
    }

    public async Task PatchAsync(int id, JsonPatchDocument<GameUpdateDto> patchDoc, ControllerBase controller)
    {
        var gameToPatch = await uow.GameRepository.GetGameAsync(id, true) ?? throw new KeyNotFoundException($"Game with ID {id} not found.");
        var dto = mapper.Map<GameUpdateDto>(gameToPatch);
        patchDoc.ApplyTo(dto, error =>
            controller.ModelState.AddModelError(error.AffectedObject.ToString() ?? string.Empty, error.ErrorMessage));
        if (!controller.TryValidateModel(dto))
            throw new InvalidOperationException("Model validation failed");

        mapper.Map(dto, gameToPatch);
        uow.GameRepository.Update(gameToPatch);
        await uow.CompleteAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await uow.GameRepository.ExistsAsync(id);
}
