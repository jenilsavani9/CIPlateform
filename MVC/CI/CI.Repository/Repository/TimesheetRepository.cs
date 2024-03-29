﻿using CI.Entities.Data;
using CI.Entities.Models;
using CI.Entities.ViewModels;
using CI.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.Repository.Repository
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly CiContext _db;

        public TimesheetRepository(CiContext db)
        {
            _db = db;
        }

        public User GetUser(string? userEmail)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            return user!;
        }

        public List<TimeSheetModel> GetGoalBasedTimeSheet(string? userEmail)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            var result = _db.Timesheets.Where(ts => ts.UserId == user!.UserId && ts.Action != null ).ToList();

            List<TimeSheetModel> timeSheets = new List<TimeSheetModel>();
            foreach (var t in result)
            {
                var tempMission = _db.Missions.FirstOrDefault(m => m.MissionId == t.MissionId);
                timeSheets.Add(new TimeSheetModel
                {
                    timesheetId = t.TimesheetId,
                    missionId = t.MissionId,
                    dateVolunteered = t.DateVolunteered,
                    notes = t.Notes,
                    mission = tempMission?.Title,
                    action = t.Action
                });
            }
            return timeSheets;
        }

        public List<TimeSheetModel> GetTimeBasedTimeSheet(string? userEmail)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);
            var result = _db.Timesheets.Where(ts => ts.UserId == user!.UserId && ts.Action == null ).ToList();

            List<TimeSheetModel> timeSheets = new List<TimeSheetModel>();
            foreach (var t in result)
            {
                var tempMission = _db.Missions.FirstOrDefault(m => m.MissionId == t.MissionId);
                timeSheets.Add(new TimeSheetModel
                {
                    timesheetId = t.TimesheetId,
                    missionId = t.MissionId,
                    dateVolunteered = t.DateVolunteered,
                    notes = t.Notes,
                    timesheetTime = t.TimesheetTime,
                    mission = tempMission?.Title
                });
            }
            return timeSheets;
        }

        public bool DeleteTimesheet(long? id)
        {
            var timeSheets = _db.Timesheets.Find(id);
            if (timeSheets == null)
            {
                return false;
            }
            else
            {
                _db.Remove(timeSheets);
                _db.SaveChanges();
                return true;
            }
        }

        public bool AddTimeSheets(TimeSheetModel model, long userId)
        {
            var DatabaseTimesheet = _db.Timesheets.FirstOrDefault(t => t.MissionId == model.missionId && t.DateVolunteered.Date == model.dateVolunteered.Date);
            if(DatabaseTimesheet != null)
            {
                return false;
            }
            Timesheet tempSheet = new Timesheet();
            tempSheet.UserId = userId;
            tempSheet.MissionId = (long)model.missionId!;
            tempSheet.DateVolunteered = model.dateVolunteered;
            tempSheet.Notes = model.notes;
            tempSheet.TimesheetTime = model.timesheetTime;
            _db.Timesheets.Add(tempSheet);
            _db.SaveChanges();
            return true;
        }

        public bool AddGoalSheets(TimeSheetModel model, long userId)
        {
            Timesheet tempSheet = new Timesheet();
            tempSheet.UserId = userId;
            tempSheet.MissionId = (long)model.missionId!;
            tempSheet.DateVolunteered = model.dateVolunteered;
            tempSheet.Notes = model.notes;
            tempSheet.Action = model.jenil;
            _db.Timesheets.Add(tempSheet);
            _db.SaveChanges();
            return true;
        }

        public Timesheet GetSingleTimeSheet(int id)
        {
            var timesheet = _db.Timesheets.Where(ts=> ts.TimesheetId == id).FirstOrDefault();
            return timesheet!;
        }

        public bool EditTimeSheets(TimeSheetModel model, long userId)
        {
            var tempSheet = _db.Timesheets.Where(ts => ts.TimesheetId == model.timesheetId).FirstOrDefault();
            tempSheet!.MissionId = model.missionId;
            tempSheet.DateVolunteered = model.dateVolunteered;
            tempSheet.Notes = model.notes;
            tempSheet.TimesheetTime = model.timesheetTime;
            tempSheet.UpdatedAt = DateTime.Now;
            _db.SaveChanges();
            return true;
        }

        public bool EditGoalSheets(TimeSheetModel model, long userId)
        {
            var tempSheet = _db.Timesheets.Where(ts => ts.TimesheetId == model.timesheetId).FirstOrDefault();
            tempSheet!.MissionId = model.missionId;
            tempSheet.DateVolunteered = model.dateVolunteered;
            tempSheet.Notes = model.notes;
            tempSheet.Action = model.jenil;
            tempSheet.UpdatedAt = DateTime.Now;
            _db.SaveChanges();
            return true;
        }

        public DatesModel CheckDate(long id)
        {
            var mission = _db.Missions.FirstOrDefault(m => m.MissionId == id);
            DatesModel dates = new DatesModel();
            if (mission != null)
            {
                dates.startdate = mission.StartDate;
                dates.enddate = mission.EndDate;
            }
            return dates;
        }
    }
}
