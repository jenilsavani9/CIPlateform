using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        private readonly IWebHostEnvironment _webHostEnvironment;

        public StoryRepository(CiContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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
                var storyMedia = _db.StoryMedia.Where(s => s.StoryId == tempStory.StoryId).FirstOrDefault();

                storys.Add(new StoryModel
                {
                    story = tempStory,
                    user = tempUser,
                    mission = tempMission,
                    theme = tempTheme,
                    StoryMedia = storyMedia
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

        public void SaveStory(string? userEmail, long mission, string? title, string? date, string? details, string? url, string? status, string? desc, string[]? listOfImage)
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
                    story.Description = desc;
                    _db.Stories.Add(story);
                    _db.SaveChanges();

                    var id = story.StoryId;
                    for(var i=0; i<listOfImage?.Length; i++)
                    {
                        StoryMedium storyMedium = new StoryMedium();
                        storyMedium.StoryId = id;
                        storyMedium.StoryType = "png";
                        storyMedium.StoryPath = listOfImage[i];

                        _db.StoryMedia.Add(storyMedium);
                        _db.SaveChanges();
                    }
                }
                else
                {
                    var story = _db.Stories.Where(s => s.MissionId == mission && s.Title == title && s.Status == "draft").FirstOrDefault();
                    if (story != null)
                    {
                        story.Status = "pending";
                        _db.SaveChanges();
                    }

                }
                
            }
        }

        public StoryDetailsModel GetStoryDetails(long storyId)
        {
            StoryDetailsModel model = new StoryDetailsModel();
            var story = _db.Stories.Where(s => s.StoryId == storyId).FirstOrDefault();
            var mission = _db.Missions.Where(m => m.MissionId == story.MissionId).FirstOrDefault();
            var user = _db.Users.Where(u => u.UserId == story.UserId).FirstOrDefault();

            model.storyMedia = _db.StoryMedia.Where(s => s.StoryId == story.StoryId).ToList();
            model.whyIVolunteer = user.WhyIVolunteer;
            model.missionTitle = mission.Title;
            model.avatar = user.Avatar;
            model.storyDetails = story.Description;
            model.userName = user.FirstName + " " + user.LastName;
            model.missionId = mission.MissionId;

            if (story.Views == null)
            {
                story.Views = 1;
            }
            else
            {
                story.Views += 1;
            }
            _db.SaveChanges();
            model.views = story.Views;

            return model;
        }

        //save images
        //public bool OnPostMyUploader(IFormFile MyUploader)
        //{
        //    if (MyUploader != null)
        //    {
        //        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "mediaUpload");
        //        string filePath = Path.Combine(uploadsFolder, MyUploader.FileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            MyUploader.CopyTo(fileStream);
        //        }
        //        return true;
        //    }
        //    return false;
        //}
    }
}
