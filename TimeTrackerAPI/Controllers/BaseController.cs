using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlannerRepository.Interfaces;
using Utilities;

namespace TimeTrackerAPI.Controllers
{
    /// <summary>
    /// Base type for Controller 
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        protected IProjectRepository projectRepository;
        protected IMilestoneRepository milestoneRepository;

        protected IMapper mapper;
        protected readonly EventLogger eventLogger = new EventLogger(typeof(BaseController));

        public BaseController(IProjectRepository projectRepository,
            IMilestoneRepository milestoneRepository, IMapper mapper)
        {
            this.projectRepository = projectRepository;
            this.milestoneRepository = milestoneRepository;
            this.mapper = mapper;
        }
    }
}
