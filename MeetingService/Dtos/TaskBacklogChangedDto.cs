namespace MeetingService.Dtos;

using MeetingService.Enums;
using Newtonsoft.Json;

public record TaskBacklogChangedDto(Guid TaskId, [JsonConverter(typeof(BacklogTypeConverter))] BacklogType BacklogType);
