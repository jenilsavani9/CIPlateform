using CI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class RecentVolunteerModel
    {
        public IEnumerable<User>? user { get; set; }
    }
}
