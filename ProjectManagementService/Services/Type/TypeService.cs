namespace ProjectManagementService.Services.Type;
using Dtos.Type;
using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Data;

public class TypeService : ITypeService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public TypeService(ApplicationDbContext applicationDbContext)
    {
        this._applicationDbContext = applicationDbContext;
    }

    public async Task<IEnumerable<TypeDto>> GetTypesAsync()
    {
        return await _applicationDbContext.TaskTypes.Select(t=>new TypeDto(t.Id, t.Name)).ToListAsync();
    }
}
