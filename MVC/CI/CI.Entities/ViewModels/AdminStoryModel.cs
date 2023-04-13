using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class AdminStoryModel
    {
        public string? storyTitle { get; set; }

        public string? missionTitle { get; set; }

        public string? username { get; set; }

        public long storyId { get; set; }
    }
}
