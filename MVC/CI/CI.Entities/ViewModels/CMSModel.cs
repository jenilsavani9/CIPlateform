using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class CMSModel
    {
        public long id { get; set; }
        public string? title { get; set; }
        public string? slug { get; set; }
        public string? status { get; set; }
        public string? description { get; set; }
    }
}
