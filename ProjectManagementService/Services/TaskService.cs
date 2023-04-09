namespace ProjectManagementService.Services;

using Data;
using Dtos;
using Microsoft.EntityFrameworkCore;
using Models;

public sealed class TaskService
{
    private readonly ApplicationDbContext _appDbContext;

    public TaskService(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<TaskDto> CreateTaskAsync(TaskDto newTaskDto)
    {
        var newTask = new Task
        {

        };

        var createdTask = await _appDbContext.AddAsync(newTask);
        await _appDbContext.SaveChangesAsync();

        return newTaskDto;
    }

    public async Task<TaskDto> UpdateTaskAsync(TaskDto taskDto)
    {
        var neededTask = await _appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskDto.Id);
        if(neededTask is null)
        {

        }
        
        // TODO fill in new data

        return taskDto;
    }

    public async Task<bool> DeleteTaskAsync(Guid taskId)
    {
        var neededTask = await _appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if(neededTask is null)
        {
            // TODO 
            return false;
        }

        var deletedTask = _appDbContext.Tasks.Remove(neededTask);
        
        // TODO check if success

        return true;
    }
}

