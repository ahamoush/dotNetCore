using AutoMapper;
using ProjectManagementSystem.Application.DTOs.Projects;
using ProjectManagementSystem.Application.DTOs.Tasks;
using ProjectManagementSystem.Domain.Entities;

namespace ProjectManagementSystem.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectResponseDto>();
        CreateMap<CreateProjectDto, Project>(MemberList.Source);
        CreateMap<UpdateProjectDto, Project>(MemberList.Source);

        CreateMap<ProjectTask, TaskResponseDto>();
        CreateMap<CreateTaskDto, ProjectTask>(MemberList.Source);
        CreateMap<UpdateTaskDto, ProjectTask>(MemberList.Source);
    }
}
