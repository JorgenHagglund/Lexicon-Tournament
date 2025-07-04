using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.AspNetCore.Mvc;

namespace Services.Contracts;

public interface IServiceBase<DTO, CreateDTO, UpdateDTO, Return>
    where DTO : class
    where CreateDTO : class
    where UpdateDTO : class
    where Return : class
{
    Task<Return> CreateAsync(CreateDTO dto);
    Task DeleteAsync(int id);
    Task<Return> UpdateAsync(int id, DTO dto);
    Task PatchAsync(int id, JsonPatchDocument<UpdateDTO> patchDoc, ControllerBase controller);
    Task<bool> ExistsAsync(int id);
}