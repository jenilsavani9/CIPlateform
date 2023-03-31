using CI.Entities.Models;

namespace CI.Entities.ViewModels
{
    public class MissionModel
    {
        public string? searchQuery { get; set; }

        public string? FCountries { get; set; }

        public string? FCities { get; set; }

        public string? FThemes { get; set; }

        public string? FSkills { get; set; }

        public int? TotalPages { get; set; }

        public int? CurrentPage { get; set; }

        public int? NoOfMissions { get; set; }

        public string? url { get; set; }

        public IEnumerable<CountryElementModel>? countryElements { get; set; }

        public IEnumerable<CityElementsModel>? cityElements { get; set; }

        public IEnumerable<ThemeElementModel>? themeElements { get; set; }

        public IEnumerable<Skill>? skillElements { get; set; }

        public IEnumerable<CityElementsModel>? cities { get; set; }

        public IEnumerable<MissionViewModel>? missionsVMList { get; set; }
    }
}
