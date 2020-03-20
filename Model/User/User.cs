using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SCMR_Api.Model
{
    public class User
    {
        public User()
        {
            GId = Guid.NewGuid();
        }

        [Key]
        public int Id { get; set; }

        public Guid GId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string MeliCode { get; set; }

        public UserState UserState { get; set; }

        public string UserStateDesc { get; set; }

        public DateTime DateAdd { get; set; }

        public DateTime DateEdit { get; set; }

        // public string ProfilePic { get; set; }

        public bool isLogedIn { get; set; }

        
        public string PicUrl { get; set; }
        [NotMapped]
        public string PicData { get; set; }
        [NotMapped]
        public string PicName { get; set; }


        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        public virtual IList<Chat> SendedChat { get; set; }
        public virtual IList<Chat> RecivedChat { get; set; }


        public string fullName
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
        public string roleName
        {
            get
            {
                if (Role != null)
                {
                    return Role.Name;
                }
                else
                {
                    return "";
                }
            }
        }
        public string userStateString
        {
            get
            {
                if (UserState == UserState.Active)
                {
                    return "فعال";
                }
                if (UserState == UserState.DeActive)
                {
                    return "غیر فعال";
                }
                if (UserState == UserState.Unknown)
                {
                    return "معلق";
                }
                if (UserState == UserState.Removed)
                {
                    return "حذف شده";
                }

                return "نا مشخص";
            }
        }
        public string userStateColor
        {
            get
            {
                if (UserState == UserState.Active)
                {
                    return "primary";
                }
                if (UserState == UserState.DeActive)
                {
                    return "warn";
                }
                if (UserState == UserState.Unknown)
                {
                    return "accent";
                }
                if (UserState == UserState.Removed)
                {
                    return "warn";
                }

                return "none";
            }
        }


    }

    public enum UserState
    {
        Active = 1,
        Unknown = 2,
        DeActive = 3,
        Removed = 4
    }
}

