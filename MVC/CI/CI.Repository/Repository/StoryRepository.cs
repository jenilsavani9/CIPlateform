using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly CiContext _db;

        public StoryRepository(CiContext db)
        {
            _db = db;
        }

        public List<StoryModel> GetStory(string page)
        {
            List<StoryModel> storys = new List<StoryModel>();

            int pageSize = 3;
            var tempStorys = _db.Stories.Skip(int.Parse(page) * pageSize).Take(pageSize).ToList();
            foreach (var storyItem in tempStorys)
            {
                var tempStory = _db.Stories.Where(s => s.StoryId == storyItem.StoryId).SingleOrDefault();
                var tempUser = _db.Users.Where(u => u.UserId == storyItem.UserId).SingleOrDefault();
                var tempMission = _db.Missions.Where(u => u.MissionId == storyItem.MissionId).SingleOrDefault();
                var tempTheme = _db.MissionThemes.Where(u => u.MissionThemeId == storyItem.MissionId).SingleOrDefault();
                storys.Add(new StoryModel
                {
                    story = tempStory,
                    user = tempUser,
                    mission = tempMission,
                    theme = tempTheme,
                });
            }
            return storys;
        }

        public int StoryCount()
        {
            var story = _db.Stories.Count();
            return story;
        }

        public List<ShareMissionApplyMissionModel> GetAppliedMission(string userEmail)
        {
            var user = _db.Users.Where(e => e.Email == userEmail).SingleOrDefault();
            var mission = new List<ShareMissionApplyMissionModel>();
            if (user != null)
            {
                var missionApplication = _db.MissionApplications.Where(a => a.UserId == user.UserId).ToList();
                
                foreach (var item in missionApplication)
                {
                    var tempMission = _db.Missions.Where(m => m.MissionId == item.MissionId).SingleOrDefault();
                    mission.Add(new ShareMissionApplyMissionModel
                    {
                        id = item.MissionId,
                        title = tempMission?.Title
                    });
                }

                return mission;
            }
            else
            {
                return mission;
            }
        }

        public User GetUser(string userEmail)
        {
            var user = _db.Users.Where(e => e.Email == userEmail).SingleOrDefault();
            return user;
        }

        public void SaveStory(string? userEmail, long mission, string? title, string? date, string? details, string? url, string? status)
        {
            var user = _db.Users.Where(e => e.Email == userEmail).SingleOrDefault();
            if (user != null)
            {


                if (status == "save")
                {
                    var story = new Story();
                    story.MissionId = mission;
                    story.Title = title;
                    story.UserId = user.UserId;
                    story.PublishedAt = DateTime.Now;
                    _db.Stories.Add(story);
                }
                else
                {
                    var story = _db.Stories.Where(s => s.MissionId == mission && s.Title == title && s.Status == "draft").FirstOrDefault();
                    if (story != null)
                    {
                        story.Status = "pending";
                    }

                }
                _db.SaveChanges();
            }
        }
    }
}
