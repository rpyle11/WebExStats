using AutoMapper;
using Stats.Api.Entities;
using Stats.Models;

namespace Stats.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.WebExMeeting.Item, MeetingItem>().ReverseMap();
            CreateMap<Meetings, MeetingItem>().ReverseMap();
            CreateMap<MeetingParticipants, ParticipantItem>().ReverseMap();
            CreateMap<Models.WebExParticipant.Item, ParticipantItem>().ReverseMap();
            CreateMap<LastDataPullResult, LastDataPullDto>().ReverseMap();
            CreateMap<StartEndDatesResult, StartEndDatesDto>().ReverseMap();
            CreateMap<AllEmployeesInAllMeetingsResult, EmployeesAllMeetingsDto>().ReverseMap();
            CreateMap<TotalEmployeeTimeInAllMeetingsResult, TotalEmployeeTimeInAllMeetingsDto>().ReverseMap();
            CreateMap<ByEmployeeTotalTimeInMeetingsResult, ByEmployeeTotalTimeInMeetingsDto>().ReverseMap();
            CreateMap<TopUsersResult, TopUsersDto>().ReverseMap();
            CreateMap<MeetingParticipantsListResult, MeetingParticipantDto>().ReverseMap();
            CreateMap<EmployeeMeetingTotalTimeResult, EmployeeMeetingTotalTimeDto>().ReverseMap();

        }
    }
}
