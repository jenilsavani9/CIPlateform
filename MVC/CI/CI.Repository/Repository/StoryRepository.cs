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

        public JsonResult GetStory(string page)
        {

            throw new NotImplementedException();
        }
    }
}
