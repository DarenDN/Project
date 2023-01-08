using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementService.Controllers
{
    public class ProjectController : ControllerBase
    {
        public async Task<JsonResult> GetProjects()
        {
            // TODO implementation
            return new JsonResult(1);
        }
    }
}
