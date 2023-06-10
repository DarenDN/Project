namespace ProjectManagementService.Services.Type;
using Dtos.Type;
using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Data;

public class TypeService : ITypeService
{
    private readonly Data.AppDbContext _applicationDbContext;

    public TypeService(Data.AppDbContext applicationDbContext)
    {
        this._applicationDbContext = applicationDbContext;
    }

    public async Task<IEnumerable<TypeDto>> GetTypesAsync()
    {
        return await _applicationDbContext.TaskTypes.Select(t=>new TypeDto(t.Id, t.Name)).ToListAsync();
    }
}
