using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Mail;

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
                var storyMedia = _db.StoryMedia.Where(s => s.StoryId == tempStory.StoryId && s.StoryType == "png").FirstOrDefault();

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
                    var tempStory = _db.Stories.Where(s => s.MissionId == mission && s.UserId == user.UserId).FirstOrDefault();
                    var id = (long)0;
                    if (tempStory != null)
                    {
                        tempStory.MissionId = mission;
                        tempStory.Title = title;
                        tempStory.UserId = user.UserId;
                        tempStory.PublishedAt = DateTime.Now;
                        tempStory.Description = desc;
                        id = tempStory.StoryId;
                    }
                    else
                    {

                        story.MissionId = mission;
                        story.Title = title;
                        story.UserId = user.UserId;
                        story.PublishedAt = DateTime.Now;
                        story.Description = desc;
                        _db.Stories.Add(story);

                    }

                    _db.SaveChanges();


                    if (id <= 0)
                    {
                        id = story.StoryId;
                    }

                    var deleteMedia = _db.StoryMedia.Where(s => s.StoryId == id).ToList();
                    if (deleteMedia.Any())
                    {
                        foreach (var media in deleteMedia)
                        {
                            _db.StoryMedia.Remove(media);
                            _db.SaveChanges();
                        }

                    }

                    //for saving video urls
                    if (url != null)
                    {
                        string[] videoUrl = url.Split(" ");
                        if (videoUrl.Length > 0)
                        {
                            foreach (var video in videoUrl)
                            {
                                StoryMedium storyMedium = new StoryMedium();
                                storyMedium.StoryId = id;
                                storyMedium.StoryType = "video";
                                storyMedium.StoryPath = video;

                                _db.StoryMedia.Add(storyMedium);
                                _db.SaveChanges();
                            }
                        }

                    }

                    for (var i = 0; i < listOfImage?.Length; i++)
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

            model.storyMedia = _db.StoryMedia.Where(s => s.StoryId == story.StoryId && s.StoryType == "png").ToList();
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

        public bool InviteUser(long userId, long storyId, string userEmail)
        {
            var fromUser = _db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            var user = _db.Users.Where(u => u.UserId == userId).FirstOrDefault();

            if (user != null && fromUser != null)
            {
                var checkRecommend = _db.StoryInvites.Where(u => u.FromUserId == fromUser.UserId && u.StoryId == storyId && u.ToUserId == user.UserId).FirstOrDefault();
                if (checkRecommend == null)
                {
                    _db.StoryInvites.Add(new StoryInvite { ToUserId = user.UserId, FromUserId = user.UserId, StoryId = storyId });
                    _db.SaveChanges();

                    var resetLink = "https://localhost:44398/story/" + storyId.ToString();
                    var fromAddress = new MailAddress("jenilsavani8@gmail.com", "CI Platform");
                    var toAddress = new MailAddress(user.Email);
                    var subject = "Recommendation for Story";
                    var body = $"Hi,<br /><br />Please click on the following link to See Mission Story from {fromUser.FirstName} {fromUser.LastName}:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
                    var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("jenilsavani8@gmail.com", "bwgnmdxyggqrylsu"),
                        EnableSsl = true
                    };
                    smtpClient.Send(message);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Story DraftStory(long missionId, string userEmail)
        {
            var user = _db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            var story = _db.Stories.Where(s => s.MissionId == missionId && s.UserId == user.UserId).OrderByDescending(s => s.PublishedAt).FirstOrDefault();

            return story;
        }

        public List<StoryMedium> DraftStoryMedia(long storyId)
        {
            var media = _db.StoryMedia.Where(s => s.StoryId == storyId && s.StoryType == "png").ToList();
            return media;
        }
    }
}
