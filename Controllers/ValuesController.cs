using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskList.Models.DB;

namespace TaskList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private const string NEW = "New";
        private const string REMOVED = "Removed";
        private const string NOCHANGE = "NoChange";

        private readonly TaskListContext _db;

        public ValuesController(TaskListContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<YinActivity>> Get()
        {
            return _db.YinActivities.ToList();
        }

        [HttpPost]
        public IActionResult Post([FromBody] IEnumerable<YinActivity> activities)
        {
            DateTime dt = DateTime.Now;

            foreach (var act in activities)
            {
                act.AssignmentCd = NEW;
                act.CreatedDateTime = dt;
            }
            _db.YinActivities.AddRange(activities);
            _db.SaveChanges();

            return CreatedAtAction(nameof(Post), activities);
        }

        [HttpPut]
        public IActionResult Put([FromBody] List<YinActivity> activities)
        {
            var prevActivities = _db.YinActivities.Where(x => !x.AssignmentCd.Equals(REMOVED)).ToList();
            SaveToHistory(prevActivities);

            DateTime dt = DateTime.Now;

            var newActivities = GetNewActivities(activities, prevActivities, dt);
            _db.YinActivities.AddRange(newActivities);

            var removedActivities = GetRemovedActivities(activities, prevActivities, dt);
            _db.YinActivities.UpdateRange(removedActivities);

            var existingActivities = GetExistingActivities(activities, prevActivities, dt);
            _db.YinActivities.UpdateRange(existingActivities);

            var activitiesToRemove = _db.YinActivities.Where(x => x.AssignmentCd.Equals(REMOVED));
            _db.YinActivities.RemoveRange(activitiesToRemove);

            _db.SaveChanges();

            var currentActivities = _db.YinActivities.Where(x => x.CreatedDateTime == dt);
            return Ok(currentActivities);
        }

        private void SaveToHistory(List<YinActivity> activities)
        {
            var histories = activities
                .Where(x => !x.AssignmentCd.Equals(REMOVED))
                .Select(x => new YinActivitiesHistory()
                {
                    ActivityId = x.ActivityId,
                    TaskName = x.TaskName,
                    ActivityCreatedDateTime = x.CreatedDateTime
                });

            _db.YinActivitiesHistory.AddRange(histories);
        }

        private static IEnumerable<YinActivity> GetExistingActivities(
            List<YinActivity> activities,
            List<YinActivity> prevActivities,
            DateTime dt)
        {
            var existingActivities = prevActivities
                .Where(a => activities.Any(p => p.TaskName.Equals(a.TaskName, StringComparison.InvariantCultureIgnoreCase)));
            existingActivities.ToList().ForEach(x =>
            {
                x.AssignmentCd = NOCHANGE;
                x.CreatedDateTime = dt;
            });
            return existingActivities;
        }

        private static IEnumerable<YinActivity> GetRemovedActivities(
            List<YinActivity> activities,
            List<YinActivity> prevActivities,
            DateTime dt)
        {
            var removedActivities = prevActivities
                .Where(a => !activities.Any(p => p.TaskName.Equals(a.TaskName, StringComparison.InvariantCultureIgnoreCase)));
            removedActivities.ToList().ForEach(x =>
            {
                x.AssignmentCd = REMOVED;
                x.CreatedDateTime = dt;
            });
            return removedActivities;
        }

        private static IEnumerable<YinActivity> GetNewActivities(
            List<YinActivity> activities,
            List<YinActivity> prevActivities,
            DateTime dt)
        {
            var newActivities = activities
                .Where(a => !prevActivities.Any(p => p.TaskName.Equals(a.TaskName, StringComparison.InvariantCultureIgnoreCase)));
            newActivities.ToList().ForEach(x =>
            {
                x.AssignmentCd = NEW;
                x.CreatedDateTime = dt;
            });
            return newActivities;
        }

        [HttpDelete("deleteAll")]
        public IActionResult DeleteAll()
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                _db.Database.ExecuteSqlCommand("truncate table yin_Activities");
                _db.Database.ExecuteSqlCommand("truncate table yin_ActivitiesHistory");
                transaction.Commit();
            }

            return NoContent();
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}