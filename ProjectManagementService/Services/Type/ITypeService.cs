namespace ProjectManagementService.Services.Type;
using Dtos.Type;
public interface ITypeService
{
    System.Threading.Tasks.Task<IEnumerable<TypeDto>> GetTypesAsync();
}
