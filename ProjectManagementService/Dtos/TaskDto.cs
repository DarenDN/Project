namespace ProjectManagementService.Dtos;
using Models;
public sealed class TaskDto
{
    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Status { get; private set; }

    public string Type { get; private set; }

    public TaskDto(TaskModel taskModel)
    {
        Title = taskModel.Title;
        Description = taskModel.Description;
        Status = taskModel.Status.Name;
        Type = taskModel.Type.Name;
    }

    public void FillData()
    {

    }

    // TODO Creator and Performer users
    // TODO time spent on the task. prob List of dtos
    // TODO timing 
}
