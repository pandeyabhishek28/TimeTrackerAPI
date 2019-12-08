using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerRepository.Interfaces;

namespace TimeTrackerAPI.Controllers
{
    [ApiController, Route("api/[controller]/[action]"), Produces("application/json")]
    public class TimeLineController : BaseController
    {
        public TimeLineController(IProjectRepository projectRepository,
        IMilestoneRepository milestoneRepository, IMapper mapper) :
        base(projectRepository, milestoneRepository, mapper)
        {
        }

        /// <summary>
        /// A method for getting schedule.
        /// </summary>
        /// <param name="numberOfDay">Number of Days.</param>
        /// <param name="startDate">Start Date.</param>
        /// <returns>Schedule of a range(start date + number of days).</returns>
        [HttpGet]
        public async Task<IActionResult> GetScheduleAsync([FromQuery]int numberOfDay, [FromQuery]DateTime startDate)
        {
            try
            {
                if (startDate == default || numberOfDay <= 0)
                {
                    return BadRequest("Invalid request, Please verify request parameter.");
                }

                var startfrom = startDate.Date;
                DateTime endAt = startfrom.AddDays(numberOfDay);

                var filteredMilestones = await milestoneRepository.FindAsync(milestone =>
                 (milestone.StartDate >= startfrom && milestone.StartDate <= endAt) ||
                 (milestone.TargetDate <= endAt && milestone.TargetDate >= startDate));

                var mappedDTOMilestones = filteredMilestones.Select(milestone => mapper.Map<DTO.DTOMilestone>(milestone)).ToList();

                return Ok(mappedDTOMilestones);
            }
            catch (Exception ex)
            {
                eventLogger.LogError("An exception occured while looking for events at TimeLine's GetSchedule.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, this.Request.ToString());
            }
        }
    }
}
