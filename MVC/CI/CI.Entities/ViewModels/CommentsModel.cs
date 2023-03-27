using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class CommentsModel
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? commentText { get; set; }
        public DateTime? createdAt { get; set; }
    }
}
