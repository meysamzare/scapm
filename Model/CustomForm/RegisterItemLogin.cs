using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class RegisterItemLogin
    {
        public RegisterItemLogin() { }

        public long Id { get; set; }

        public int CategoryId { get; set; }

        public DateTime Date { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string CategoryAuthorizeState { get; set; }

        public string UserType { get; set; }

        public int GradeId { get; set; }

        public int ClassId { get; set; }

        public string IP { get; set; }



        public string DateString => Date.ToPersianDateWithTime();


        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }
}