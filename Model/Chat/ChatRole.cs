using System.ComponentModel.DataAnnotations;

namespace SCMR_Api.Model
{
    public class ChatRole
    {
        public ChatRole()
        {

        }

        [Key]
        public int Id { get; set; }

        public int RoleFromId { get; set; }
        public int RoleToId { get; set; }


        public bool Allow { get; set; }
    }
}
