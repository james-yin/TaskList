using System;
using System.Collections.Generic;

namespace TaskList.Models.DB
{
    public partial class YinActivity
    {
        public int ActivityId { get; set; }
        public string TaskName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string AssignmentCd { get; set; }
    }
}