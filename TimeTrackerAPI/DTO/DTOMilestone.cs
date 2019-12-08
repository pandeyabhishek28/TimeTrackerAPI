using System;

namespace TimeTrackerAPI.DTO
{
    /// <summary>
    /// A type that that represent a Milestone
    /// </summary>
    public class DTOMilestone
    {
        public int ItemId { get; set; }
        public int ProjectId { get; set; }
        public int MilestoneId { get; set; }
        public string Title { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Validate Milestone DTO, to insure everything is correct. 
        /// </summary>
        /// <returns>True, if valid else false</returns>
        public bool IsValid()
        {
            if (ProjectId > 0 &&
                MilestoneId > 0 &&
                !string.IsNullOrEmpty(Title) &&
                ItemId > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
