using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Entities.ViewModels
{
    public class UserProfileModel
    {
        public long Id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public long phoneNumber { get; set; }
        public string? avatar { get; set; }
        public string? whyIVolunteer { get; set; }
        public string? employeeId { get; set; }
        public string? department { get; set; }
        public string? manager { get; set; }
        public string? city { get; set; }
        public string? country { get; set; }
        public string? profileText { get; set; }
        public string? linkedInUrl { get; set; }
        public string? title { get; set; }
        public string? available { get; set; }
        public string? email { get; set; }
        public long? cityId { get; set; }
        public long? countryId { get; set; }
    }
}
