using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class OnlineClass
    {
        public OnlineClass() { }

        public long Id { get; set; }
        

        public string meetingId { get; set; }

        public string name { get; set; }


        public int GradeId { get; set; }

        public int? ClassId { get; set; }

        public int CourseId { get; set; }

        public int? OnlineClassServerId { get; set; }

        // user password
        public string attendeePW { get; set; }
        // teacher password
        public string moderatorPW { get; set; }

        // welcome message
        public string welcome { get; set; }

        // Maximum users that allows to join class
        public string maxParticipants { get; set; }

        // The maximum length (in minutes) for the class.
        public int duration { get; set; }

        public string logoutURL { get; set; }

        public string meta { get; set; }

        public string copyright { get; set; }


        public string parentMeetingID { get; set; }

        public int sequence { get; set; }




        public bool record { get; set; }

        public bool isBreakout { get; set; } = false;

        public bool freeJoin { get; set; }

        public bool autoStartRecording { get; set; }
        
        public bool allowStartStopRecording { get; set; }
        
        public bool webcamsOnlyForModerator { get; set; }
        
        public bool muteOnStart { get; set; }
        
        public bool allowModsToUnmuteUsers { get; set; }
        
        public bool lockSettingsDisableCam { get; set; }
        
        public bool lockSettingsDisableMic { get; set; }
        
        public bool lockSettingsDisablePrivateChat { get; set; }
        
        public bool lockSettingsDisablePublicChat { get; set; }
        
        public bool lockSettingsDisableNote { get; set; }
        
        public bool lockSettingsLockedLayout { get; set; }
        
        public bool lockSettingsLockOnJoin { get; set; }


        #region Relations
            
            [ForeignKey("GradeId")]
            public virtual Grade Grade { get; set; }
            
            [ForeignKey("ClassId")]
            public virtual Class Class { get; set; }
            
            [ForeignKey("CourseId")]
            public virtual Course Course { get; set; }
            
            [ForeignKey("OnlineClassServerId")]
            public virtual OnlineClassServer OnlineClassServer { get; set; }

        #endregion
        

        #region CalCalculationsps
            public string gradeName => Grade != null ? Grade.Name : "";
            public string className => Class != null ? Class.Name : "";
            public string courseName => Course != null ? Course.Name : "";
        #endregion
    }
}