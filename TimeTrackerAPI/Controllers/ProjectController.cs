using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerRepository.Interfaces;
using TimeTrackerAPI.DTO;

namespace TimeTrackerAPI.Controllers
{
    // [Authorize]

    [ApiController, Route("api/[controller]/[action]"), Produces("application/json")]
    public class ProjectController : BaseController
    {
        public ProjectController(IProjectRepository projectRepository, IMilestoneRepository milestoneRepository,
            IMapper mapper)
            : base(projectRepository, milestoneRepository, mapper)
        {
        }


        /// <summary>
        /// Get All Projects
        /// </summary>
        /// <returns>A Collection Projects</returns>
        [HttpGet]
        public async Task<ActionResult<DTOProject>> GetAllAsync()
        {
            try
            {
                var projects = await projectRepository.GetAllAsync();
                var dtoProjects = projects.Select(x => mapper.Map<DTOProject>(x));
                return Ok(dtoProjects);
            }
            catch (Exception ex)
            {
                eventLogger.LogError("An exception occured while processing Project's  GetAll().", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, this.Request.ToString());
            }
        }

        /// <summary>
        /// Get All Projects with Milestones
        /// </summary>
        /// <returns>A Collection Projects</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllWithMilestonesAsync()
        {
            try
            {
                var projects = await projectRepository.GetAllWithMilestonesAsync();
                var dtoProjects = projects.Select(x => mapper.Map<DTOProject>(x));
                return Ok(dtoProjects);
            }
            catch (Exception ex)
            {
                eventLogger.LogError("An exception occured while processing Project's GetAllWithMilestones().", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, this.Request.ToString());
            }
        }


        /// <summary>
        /// Get Project by using ID
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Project </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync([FromRoute]int id)
        {
            try
            {
                var project = await projectRepository.GetAsync(id);
                if (project == null)
                {
                    eventLogger.LogWarning("Invalid Id have been provided to fetch Project," +
                        " Request details are :" + this.Request.ToString());
                    return NotFound("Invalid Id, Not able to find a Entity with given ID.");
                }

                return Ok(mapper.Map<DTOProject>(project));
            }
            catch (Exception ex)
            {
                eventLogger.LogError("An exception occured while processing Project's Get(id).", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, this.Request.ToString());
            }
        }
    }
}
