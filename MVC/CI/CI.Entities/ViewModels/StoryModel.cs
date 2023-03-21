using CI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class StoryModel
    {
        public IEnumerable<User>? user { get; set; }

        public IEnumerable<Story>? story { get; set; }

        public IEnumerable<Mission>? mission { get; set; }

        public IEnumerable<MissionTheme>? theme { get; set; }
    }
}
