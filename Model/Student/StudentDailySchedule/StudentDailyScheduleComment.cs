using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class StudentDailyScheduleComment
    {
        public StudentDailyScheduleComment() { }

        public int Id { get; set; }

        public string Text { get; set; }

        public int UserId { get; set; }

        public string UserFullName { get; set; }

        public DateTime Date { get; set; }

        public StudentDailyScheduleCommentType Type { get; set; }

        public string FileUrl { get; set; }

        public bool HasSeen { get; set; }


        public int StudentDailyScheduleId { get; set; }


        [ForeignKey("StudentDailyScheduleId")]
        public virtual StudentDailySchedule StudentDailySchedule { get; set; }

    }
}

public enum StudentDailyScheduleCommentType
{
    StudentParent = 1,
    Consultant
}