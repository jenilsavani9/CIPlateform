using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using System.Net;
using System.Net.Mail;

namespace CI.Repository.Repository
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly CiContext _db;

        public VolunteerRepository(CiContext db)
        {
            _db = db;
        }

        public List<City>? GetCitys(long? id)
        {
            var city = _db.Cities.Where(c => c.CityId == id).ToList();
            return city;
        }

        public List<Country>? GetCountrys(long? id)
        {
            var country = _db.Countries.Where(c => c.CountryId == id).ToList();
            return country;
        }

        public List<Mission>? GetMissions(int? id)
        {
            var mission = _db.Missions.Where(m => m.MissionId == id).ToList();
            return mission;
        }

        public List<MissionTheme>? GetThemes(long? id)
        {
            var theme = _db.MissionThemes.Where(c => c.MissionThemeId == id).ToList();
            return theme;
        }

        public User? GetUser(string? userMail)
        {
            var user = _db.Users.Where(e => e.Email == userMail).SingleOrDefault();
            return user;
        }

        public List<MissionViewModel>? RelatedMissions(long? CityId, long? CountryId, long? ThemeId)
        {
            var relatedMission = _db.Missions.Where(m => m.CityId == CityId).Take(3).ToList();
            if (relatedMission.Count < 3)
            {
                relatedMission = _db.Missions.Where(m => m.CountryId == CountryId).Take(3).ToList();
            }
            if (relatedMission.Count < 3)
            {
                relatedMission = _db.Missions.Where(m => m.ThemeId == ThemeId).Take(3).ToList();
            }
            List<MissionViewModel> missionsVMList = new List<MissionViewModel>();
            foreach (var mission in relatedMission)
            {
                var city = _db.Cities.Where(e => e.CityId == mission.CityId).FirstOrDefault();
                var theme = _db.MissionThemes.Where(e => e.MissionThemeId == mission.ThemeId).FirstOrDefault();
                string[] startDateNtime = mission.StartDate.ToString().Split(' ');
                string[] endDateNtime = mission.EndDate.ToString().Split(' ');
                var missionURL = _db.MissionMedia.Where(e => e.MissionId == mission.MissionId).FirstOrDefault();
                missionsVMList.Add(new MissionViewModel
                {
                    MissionId = mission.MissionId,
                    Title = (mission.Title != null) ? mission.Title : "",
                    Description = (mission.Description != null) ? mission.Description : "",
                    City = (city?.Name != null) ? city.Name : "",
                    Organization = (mission?.OrganizationName != null)? mission.OrganizationName : "",
                    Theme = (theme?.Title != null) ? theme.Title : "",
                    //Rating = rating,
                    StartDate = (mission?.StartDate != null) ? (DateTime)mission.StartDate : DateTime.Now,
                    EndDate = (mission?.EndDate != null) ? (DateTime)mission.EndDate : DateTime.Now,
                    missionType = (mission?.MissionType != null) ? mission.MissionType : "",
                    //isFavrouite = (user != null) ? _db.FavoriteMissions.Any(e => e.MissionId == mission.MissionId && e.UserId == user.UserId) : false,
                    //userApplied = (user != null) ? _db.MissionApplications.Any(e => e.MissionId == mission.MissionId && e.UserId == user.UserId && e.ApprovalStatus != "pending") : false,
                    ImgUrl = (missionURL?.MediaPath != null) ? missionURL.MediaPath : "404-Page-image.png",
                    StartDateEndDate = "From " + startDateNtime[0] + " until " + endDateNtime[0],
                    NoOfSeatsLeft = (mission?.SeatLeft != null) ? (int)mission.SeatLeft : 0,
                    Deadline = endDateNtime[0],
                    //createdAt = (DateTime)mission.CreatedAt
                });
            }
            return missionsVMList;
        }

        public bool AddToFavorite(long? userId, int? missionId)
        {
            var tuser = _db.FavoriteMissions.Where(m => m.UserId == userId && m.MissionId == missionId).ToList();
            if (tuser.Any())
            {
                var temp = _db.FavoriteMissions.Where(m => m.UserId == userId && m.MissionId == missionId).First();
                _db.FavoriteMissions.Remove(temp);
                _db.SaveChanges();
                return false;
            }
            else
            {
                _db.FavoriteMissions.Add(new FavoriteMission { UserId = userId, MissionId = (long)missionId });
                _db.SaveChanges();
                return true;
            }
        }

        public List<FavoriteMission>? CheckAddToFavorite(long userId, int id)
        {
            var tuser = _db.FavoriteMissions.Where(m => m.UserId == userId && m.MissionId == id).ToList();
            return tuser;
        }

        public string? Rating(long userId, int rate, int missionId)
        {
            var tuser = _db.MissionRatings.Where(m => m.UserId == userId && m.MissionId == missionId).ToList();
            if (tuser.Any())
            {
                var temp = _db.MissionRatings.Where(m => m.UserId == userId && m.MissionId == missionId).First();
                _db.MissionRatings.Remove(temp);
                _db.MissionRatings.Add(new MissionRating { UserId = userId, MissionId = (long)missionId, Rating = rate.ToString() });
                _db.SaveChanges();
                return rate.ToString();
            }
            else
            {
                _db.MissionRatings.Add(new MissionRating { UserId = userId, MissionId = missionId, Rating = rate.ToString() });
                _db.SaveChanges();
                return rate.ToString();
            }
        }

        public List<MissionRating> CheckRating(long userId, int missionId)
        {
            var tmission = _db.MissionRatings.Where(m => m.UserId == userId && m.MissionId == missionId).ToList();
            return tmission;
        }

        public List<MissionRating> RatingGroup(int missionId)
        {
            var tmission = _db.MissionRatings.Where(m => m.MissionId == missionId).ToList();
            return tmission;
        }

        public bool Recommand(int id, int missionId)
        {
            var user = _db.Users.Where(u => u.UserId == id).FirstOrDefault();

            if (user != null)
            {
                var checkRecommend = _db.MissionInvites.Where(u => u.FromUserId == user.UserId && u.MissionId == missionId && u.ToUserId == id).FirstOrDefault();
                if (checkRecommend == null)
                {
                    _db.MissionInvites.Add(new MissionInvite { ToUserId = user.UserId, MissionId = missionId, FromUserId = user.UserId });
                    _db.SaveChanges();
                    // Send an email with the password reset link to the user's email address
                    var resetLink = "https://localhost:44398/missions/" + missionId.ToString();
                    // Send email to user with reset password link
                    // ...
                    var fromAddress = new MailAddress("jenilsavani8@gmail.com", "CI Platform");
                    var toAddress = new MailAddress(user.Email);
                    var subject = "Recommendation for Joining In Mission";
                    var body = $"Hi,<br /><br />Please click on the following link to Joining In Mission:<br /><br /><a href='{resetLink}'>{resetLink}</a>";
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
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public Mission Organization(int missionId)
        {
            var mission = _db.Missions.Where(m => m.MissionId == missionId).FirstOrDefault();
            return mission;
        }

        public List<CommentsModel> GetComments(int missionId)
        {

            List<CommentsModel> CommentsModel = new List<CommentsModel>();
            var comments = _db.Comments.Where(c => c.MissionId == missionId && c.ApprovalStatus != "pending").ToList();
            foreach (var comment in comments)
            {
                var u = _db.Users.Where(u => u.UserId == comment.UserId).SingleOrDefault();
                CommentsModel.Add(new CommentsModel
                {
                    firstName = u?.FirstName,
                    lastName = u?.LastName,
                    commentText = comment.CommentText,
                    createdAt = comment.CreatedAt,
                    avatar = u?.Avatar
                   
                });
            }
            return CommentsModel;
        }

        public bool AddComments(long userId, int missionId, string? getTextarea)
        {
            _db.Comments.Add(new Comment { UserId = userId, MissionId = missionId, CommentText = getTextarea });
            _db.SaveChanges();
            return true;
        }

        public MissionApplication GetApplyMission(long userId, int missionId)
        {
            var mission = _db.MissionApplications.Where(m => m.UserId == userId && m.MissionId == missionId).FirstOrDefault();
            return mission;
        }

        public bool ApplyMission(long userId, int missionId)
        {
            var mission = _db.MissionApplications.Where(m => m.UserId == userId && m.MissionId == missionId);
            if (!mission.Any())
            {
                _db.MissionApplications.Add(new MissionApplication { UserId = userId, MissionId = missionId, AppliedAt = DateTime.Now });
                _db.SaveChanges();
            }
            return true;
        }

        public List<RecentVolunteerModel> GetVolunteers(int missionId, string page)
        {

            int pageSize = 9;
            var recentVoluntters = _db.MissionApplications.Where(m => m.MissionId == missionId && m.ApprovalStatus != "pending").Skip(int.Parse(page) * pageSize).Take(pageSize);

            List<RecentVolunteerModel> RecentVolunteerModel = new List<RecentVolunteerModel>();

            foreach (var item in recentVoluntters)
            {
                RecentVolunteerModel user = new RecentVolunteerModel();
                user.user = _db.Users.Where(u => u.UserId == item.UserId);
                RecentVolunteerModel.Add(user);
            }

            return RecentVolunteerModel;
        }

        public List<MissionDocument> GetMissionDocument(int missionId)
        {
            var document = _db.MissionDocuments.Where(m => m.MissionId == missionId).ToList();
            return document;
        }

        public List<MissionSkillModel> GetMissionSkills(int missionId)
        {
            List<MissionSkillModel> missionSkills = new List<MissionSkillModel>();
            var skills = _db.MissionSkills.Where(s => s.MissionId == missionId);
            foreach (var skill in skills)
            {
                var sk = _db.Skills.Where(s => s.SkillId == skill.SkillId).FirstOrDefault();
                missionSkills.Add(new MissionSkillModel
                {
                    skillName = sk?.SkillName
                });
            }
            return missionSkills;
        }

        public List<MissionAvailableModel> GetMissionDays(int missionId)
        {
            List<MissionAvailableModel> missionDays = new List<MissionAvailableModel>();
            var missions = _db.Missions.Where(m => m.MissionId == missionId).FirstOrDefault();

            missionDays.Add(new MissionAvailableModel
            {
                missionDays = missions?.Availability?.ToString()
            });

            return missionDays;
        }

        public List<MissionMedium> GetMissionMedia(int missionId)
        {
            var result = _db.MissionMedia.Where(mm => mm.MissionId == missionId).ToList();
            return result;
        }
    }
}
