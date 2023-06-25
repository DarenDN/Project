namespace MeetingService.Dtos;

using MeetingService.Enums;
using System.ComponentModel;
using Newtonsoft.Json;
public record BacklogTaskDto(
    Guid TaskId, 
    string Name,
    TimeSpan? EstimationTime, 
    int? EstimationPoint, 
    [JsonConverter(typeof(BacklogTypeConverter))] BacklogType BacklogType);
