using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TeacherController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        private IConfiguration _config;

        public TeacherController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IConfiguration config)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] TeacherLoginParam param)
        {
            try
            {
                var teacher = await db.Teachers
                    .Include(c => c.OrgPerson)
                .FirstOrDefaultAsync(c => c.OrgPerson.IdNum == param.username);

                if (teacher != null)
                {
                    if (string.IsNullOrEmpty(teacher.Password))
                    {
                        teacher.Password = "1";
                    }

                    if (teacher.Password == param.password)
                    {
                        return this.DataFunction(true, new
                        {
                            teacher = teacher,
                            tk = BuildToken(param.username, 1440)
                        });
                    }

                    return this.UnSuccessFunction("کلمه عبور اشتباه است");
                }

                return this.UnSuccessFunction("نام کاربری اشتباه است");
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        private string BuildToken(string username, int timeInMins)
        {

            var claims = new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(timeInMins),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeTeacherPassword([FromBody] ChangeTeacherPasswordParam param)
        {
            try
            {
                var teacher = await db.Teachers.FirstOrDefaultAsync(c => c.Id == param.id);

                teacher.Password = param.newPass;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordParam param)
        {
            try
            {
                var teacher = await db.Teachers.FirstOrDefaultAsync(c => c.Id == param.id);

                if (string.IsNullOrEmpty(teacher.Password))
                {
                    teacher.Password = "1";
                }

                if (teacher.Password == param.nowPass)
                {
                    teacher.Password = param.newPass;

                    await db.SaveChangesAsync();

                    return this.SuccessFunction();
                }

                return this.UnSuccessFunction("کلمه عبور فعلی اشتباه است");
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Teacher teacher)
        {
            try
            {

                if (await db.Teachers.AnyAsync(c => c.OrgPersonId == teacher.OrgPersonId))
                {
                    return this.UnSuccessFunction("این پرسنل قبلا برای دبیری ثبت شده است");
                }

                await db.Teachers.AddAsync(teacher);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Teacher teacher)
        {
            try
            {
                var tc = await db.Teachers.SingleAsync(c => c.Id == teacher.Id);

                tc.Name = teacher.Name;
                tc.Code = teacher.Code;
                tc.OrgPersonId = teacher.OrgPersonId;


                var agentId = teacher.Id;
                var agentType = TicketType.Teacher;
                var agentName = teacher.Name;

                var tickets = db.Tickets.Where(c =>
                (c.ReciverType == agentType && c.ReciverId == agentId) ||
                (c.SenderType == agentType && c.SenderId == agentId));

                await tickets.ForEachAsync(c =>
                {
                    if (c.ReciverType == agentType && c.ReciverId == agentId)
                    {
                        c.ReciverName = agentName;
                    }
                    else if (c.SenderType == agentType && c.SenderId == agentId)
                    {
                        c.SenderName = agentName;
                    }
                });

                var mobileChats = db.MobileChats.Where(c =>
                (c.ReciverType == MobileChatType.Teacher && c.ReciverId == agentId) ||
                (c.SenderType == MobileChatType.Teacher && c.SenderId == agentId));

                await mobileChats.ForEachAsync(c =>
                {
                    if (c.ReciverType == MobileChatType.Teacher && c.ReciverId == agentId)
                    {
                        c.ReciverName = agentName;
                    }
                    else if (c.SenderType == MobileChatType.Teacher && c.SenderId == agentId)
                    {
                        c.SenderName = agentName;
                    }
                });

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromBody] TeacherGetParam param)
        {
            try
            {
                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var tc = db.Teachers.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    tc = tc.Where(c => c.Name.Contains(query) || c.OrgPerson.Name.Contains(query) || c.OrgPerson.Code.ToString().Contains(query) ||
                                    c.Code.ToString().Contains(query) || c.OrgPerson.IdNum.Contains(query) ||
                                    c.OrgPerson.IdNumber.Contains(query) || c.OrgPerson.FatherName.Contains(query));
                }

                if (param.access == "allcourse")
                {
                    tc = tc.Where(c => c.AllCourseAccess == true);
                }

                if (param.access == "owncourse")
                {
                    tc = tc.Where(c => c.AllCourseAccess == false);
                }


                count = tc.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        tc = tc.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        tc = tc.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        tc = tc.OrderBy(c => c.Code);
                    }
                    if (getparams.sort.Equals("getPersonelCode"))
                    {
                        tc = tc.OrderBy(c => c.OrgPerson.Code);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        tc = tc.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        tc = tc.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        tc = tc.OrderByDescending(c => c.Code);
                    }
                    if (getparams.sort.Equals("getPersonelCode"))
                    {
                        tc = tc.OrderByDescending(c => c.OrgPerson.Code);
                    }
                }
                else
                {
                    tc = tc.OrderBy(c => c.Id);
                }

                tc = tc.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                tc = tc.Take(getparams.pageSize);

                var q = await tc
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Code = c.Code,
                        getPersonelCode = c.OrgPerson.Code,
                        haveTimeSchedules = c.TimeSchedules.Any(),
                        haveCourses = c.Courses.Any(),
                        AllCourseAccess = c.AllCourseAccess,
                        OrgPersonId = c.OrgPersonId
                    })
                .ToListAsync();

                return Json(new jsondata
                {
                    success = true,
                    data = q,
                    type = "" + count
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getAll()
        {
            try
            {

                var tc = await db.Teachers.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, tc);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getTeacher([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var tc = await db.Teachers.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, tc);
                }
                else
                {
                    return this.UnSuccessFunction("Undefined Value", "error");
                }
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    if (id != 0)
                    {

                        var sl = await db.Teachers.Include(c => c.Courses).Include(c => c.TimeSchedules)
                            .SingleAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (sl.haveCourses)
                        {
                            return this.UnSuccessFunction(" دبیر " + sl.Name + " دارای دروسی است", "error");
                        }
                        if (sl.haveTimeSchedules)
                        {
                            return this.UnSuccessFunction(" دبیر " + sl.Name + " دارای برنامه زمان بندی است", "error");
                        }

                        db.Teachers.Remove(sl);
                    }
                    else
                    {
                        return this.UnSuccessFunction("Undefined Value", "error");
                    }
                }

                await db.SaveChangesAsync();


                return this.SuccessFunction();

            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllRegistredCourseByTeacher([FromBody] int teacherId)
        {
            try
            {
                var courses = await db.Courses
                    .Where(c => c.TeacherId == teacherId)
                        .Include(c => c.Grade)
                            .ThenInclude(c => c.Yeareducation)
                .ToListAsync();

                return this.DataFunction(true, courses);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAllCourseAccess([FromBody] ChangeAllCourseAccessParam param)
        {
            try
            {
                var teacher = await db.Teachers.FirstOrDefaultAsync(c => c.Id == param.id);

                teacher.AllCourseAccess = param.access;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }




    }

    public class TeacherGetParam
    {
        public getparams getparams { get; set; }

        public string access { get; set; }
    }

    public class ChangeAllCourseAccessParam
    {
        public int id { get; set; }

        public bool access { get; set; }
    }

    public class ChangeTeacherPasswordParam
    {
        public int id { get; set; }

        public string newPass { get; set; }
    }

    public class TeacherLoginParam
    {
        public string username { get; set; }

        public string password { get; set; }
    }
}