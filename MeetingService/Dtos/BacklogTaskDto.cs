namespace MeetingService.Dtos;

using MeetingService.Enums;
using System.ComponentModel;
using Newtonsoft.Json;
public record BacklogTaskDto(
    Guid TaskId, 
    TimeSpan? EstimationTime, 
    int? EstimationPoint, 
    [JsonConverter(typeof(BacklogTypeConverter))] BacklogType BacklogType);
