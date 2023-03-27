using CI.Entities.Models;
using CI.Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Interface
{
    public interface IStoryRepository
    {
        public List<StoryModel> GetStory(string page);

        public User GetUser(string userEmail);

        public int StoryCount();

        public List<ShareMissionApplyMissionModel> GetAppliedMission(string userEmail);

        public void SaveStory(string? userEmail, long mission, string? title, string? date, string? details, string? url, string? status);
    }
}
