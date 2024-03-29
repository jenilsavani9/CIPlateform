﻿using CI.Entities.Models;
using CI.Entities.ViewModels;

namespace CI.Repository.Interface
{
    public interface IStoryRepository
    {
        public List<StoryModel> GetStory(string page);

        public User GetUser(string userEmail);

        public int StoryCount();

        public List<ShareMissionApplyMissionModel> GetAppliedMission(string userEmail);

        public void SaveStory(string? userEmail, long mission, string? title, string? date, string? details, string? url, string? status, string? desc, string[]? listOfImage);

        public StoryDetailsModel GetStoryDetails(long storyId);

        public bool InviteUser(long userId, long storyId, string userEmail);

        public Story DraftStory(long missionId, string userEmail);

        public List<StoryMedium> DraftStoryMedia(long storyId);
    }
}
