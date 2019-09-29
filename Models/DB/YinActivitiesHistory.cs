using System;
using System.Collections.Generic;

namespace TaskList.Models.DB
{
    public partial class YinActivitiesHistory
    {
        public int ActivitiesHistoryId { get; set; }
        public int ActivityId { get; set; }
        public string TaskName { get; set; }
        public DateTime ActivityCreatedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
