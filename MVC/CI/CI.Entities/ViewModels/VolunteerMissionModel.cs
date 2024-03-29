﻿using CI.Entities.Models;

namespace CI.Entities.ViewModels
{
    public class VolunteerMissionModel
    {
        public User? user { get; set; }

        public IEnumerable<Mission>? Mission { get; set; }

        public IEnumerable<MissionViewModel>? relatedMission { get; set; }

        public IEnumerable<City>? City { get; set; }

        public IEnumerable<MissionTheme>? Themes { get; set; }

        public IEnumerable<Country>? Country { get; set; }

        public MissionViewModel? MissionViewModel { get; set; }
    }
}
