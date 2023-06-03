namespace MeetingService.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

public sealed class Evaluations
{
    public string Code { get; set; }
    public Dictionary<Guid, TaskEvaluation> TaskFinalEvaluations { get; set; }
    public Dictionary<Guid, List<TaskEvaluation>> EvaluationsByParticipant { get; set; }
}
