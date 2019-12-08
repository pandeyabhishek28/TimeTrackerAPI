using System.Collections.Generic;

namespace TimeTrackerAPI.DTO
{
    /// <summary>
    /// A type that represent a Project
    /// </summary>
    public class DTOProject
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectManager { get; set; }
        public string TechnProjectLeader { get; set; }
        public string ADArchitect { get; set; }
        public string TeamLeader { get; set; }
        public virtual IEnumerable<DTOMilestone> Milestones { get; set; }
    }
}
