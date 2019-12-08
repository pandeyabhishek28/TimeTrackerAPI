using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerRepository.Interfaces;
using PlannerRepository.Models;
using TimeTrackerAPI.DTO;

namespace TimeTrackerAPI.Controllers
{

    [ApiController, Route("api/[controller]/[action]"), Produces("application/json")]
    public class MilestoneController : BaseController
    {
        public MilestoneController(IProjectRepository projectRepository,
            IMilestoneRepository milestoneRepository, IMapper mapper)
            : base(projectRepository, milestoneRepository, mapper)
        {

        }

        /// <summary>
        /// Get All Tasks of current project
        /// </summary>
        /// <returns>A Collection of Tasks</returns>
        [HttpGet]
        public async Task<ActionResult<DTOMilestone>> GetAllAsync()
        {
            try
            {
                var milestones = await milestoneRepository.GetAllAsync();
                var dtoMilestones = milestones.Select(x => mapper.Map<DTOMilestone>(x));
                return Ok(dtoMilestones);
            }
            catch (Exception ex)
            {
                eventLogger.LogError("An exception while processing Milestone's GetAll. ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, this.Request.ToString());
            }
        }

        /// <summary>
        /// It will add dTOMilestone into the Milestones collection  
        /// </summary>
        /// <param name="value">DTO Milestone object</param>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]DTOMilestone dTOMilestone)
        {
            try
            {
                if (dTOMilestone == null || !dTOMilestone.IsValid())
                {
                    eventLogger.LogWarning("Invalid milestone!, Does not have required details to add into Milestones, " +
                         "Request details are :" + this.Request.ToString());
                    return BadRequest("Invalid Milestone :" + dTOMilestone);
                }

                var mappedMilestone = mapper.Map<Milestone>(dTOMilestone);
                var created = await milestoneRepository.AddAsync(mappedMilestone);

                if (created)
                {
                    await milestoneRepository.SaveAsync();
                    return CreatedAtAction("Post", dTOMilestone);
                }
                else
                {
                    return BadRequest("Invalid Milestone :" + dTOMilestone);
                }
            }
            catch (Exception ex)
            {
                eventLogger.LogError("An exception while processing Milestone's Post request. ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, this.Request.ToString());
            }
        }

        /// <summary>
        /// Have used "DTOMilestone" object for updating so In future we can easily change the behaviour 
        /// of this method to update all required properties. 
        /// </summary>
        /// <param name="id"> Milestone ID </param>
        /// <param name="value">DTO Milestone object </param>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromQuery]int id, [FromBody]DTOMilestone dTOMilestone)
        {
            try
            {
                if (id <= 0)
                {
                    eventLogger.LogWarning("Invalid Id!, Does not have required details to add into Milestones");
                    return BadRequest("Invalid Id :" + dTOMilestone);
                }
                if (dTOMilestone == null || !dTOMilestone.IsValid())
                {
                    eventLogger.LogWarning("Invalid milestone!, Does not have required details to add into Milestones, " +
                         "Request details are :" + this.Request.ToString());
                    return BadRequest("Invalid Milestone :" + dTOMilestone);
                }

                var milestone = await milestoneRepository.GetAsync(id);
                milestone.TargetDate = dTOMilestone.EndDateTime;

                await milestoneRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                eventLogger.LogError("An exception while processing Milestone's Post request. ", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, this.Request.ToString());
            }
        }
    }
}
