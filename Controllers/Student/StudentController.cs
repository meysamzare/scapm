using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class StudentController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        private IConfiguration _config;

        public StudentController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IConfiguration config)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            _config = config;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StudentAddParams studentadd)
        {
            try
            {
                studentadd.student.BirthDate.AddDays(1);

                if (await db.Students.AnyAsync(c => c.IdNumber2 == studentadd.student.IdNumber2))
                {
                    return this.UnSuccessFunction("این کد ملی قبلا ثبت شده است");
                }

                if (await db.Students.AnyAsync(c => c.Code == studentadd.student.Code))
                {
                    return this.UnSuccessFunction("این کد دانش آموز قبلا ثبت شده است");
                }

                if (!string.IsNullOrEmpty(studentadd.student.PicData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, studentadd.student.PicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(studentadd.student.PicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    studentadd.student.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + studentadd.student.PicName);
                }

                var student = studentadd.student;

                student.StudentInfo = studentadd.studentInfo;

                await db.Students.AddAsync(student);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] StudentAddParams studentadd)
        {
            try
            {
                studentadd.student.BirthDate.AddDays(1);

                var student = await db.Students.Include(c => c.StudentInfo).SingleAsync(c => c.Id == studentadd.student.Id);

                var BirthDateBefore = student.BirthDate;

                student.BirthDate = studentadd.student.BirthDate;

                if (BirthDateBefore != student.BirthDate)
                {
                    student.BirthDate = student.BirthDate.AddDays(1);
                }


                if (await db.Students.Except(db.Students.Where(c => c.Id == studentadd.student.Id)).AnyAsync(c => c.IdNumber2 == studentadd.student.IdNumber2))
                {
                    return this.UnSuccessFunction("این کد ملی قبلا ثبت شده است");
                }


                if (await db.Students.Except(db.Students.Where(c => c.Id == studentadd.student.Id)).AnyAsync(c => c.Code == studentadd.student.Code))
                {
                    return this.UnSuccessFunction("این کد دانش آموز قبلا ثبت شده است");
                }


                if (!string.IsNullOrEmpty(studentadd.student.PicData))
                {
                    if (!string.IsNullOrEmpty(student.PicUrl))
                    {
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + student.PicUrl);
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, studentadd.student.PicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(studentadd.student.PicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    student.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + studentadd.student.PicName);
                }

                student.Code = studentadd.student.Code;
                student.OrgCode = studentadd.student.OrgCode;
                student.Name = studentadd.student.Name;
                student.LastName = studentadd.student.LastName;
                student.FatherName = studentadd.student.FatherName;
                student.IdNumber = studentadd.student.IdNumber;
                student.IdNumber2 = studentadd.student.IdNumber2;
                student.IdCardSerial = studentadd.student.IdCardSerial;
                student.BirthLocation = studentadd.student.BirthLocation;

                //---------------------------------------------------------------------------------------

                student.StudentInfo.FatherName = studentadd.studentInfo.FatherName;
                student.StudentInfo.FatherEdu = studentadd.studentInfo.FatherEdu;
                student.StudentInfo.FatherJob = studentadd.studentInfo.FatherJob;
                student.StudentInfo.FatherJobPhone = studentadd.studentInfo.FatherJobPhone;
                student.StudentInfo.FatherPhone = studentadd.studentInfo.FatherPhone;
                student.StudentInfo.FatherJobAddress = studentadd.studentInfo.FatherJobAddress;
                student.StudentInfo.MomName = studentadd.studentInfo.MomName;
                student.StudentInfo.MomEdu = studentadd.studentInfo.MomEdu;
                student.StudentInfo.MomJob = studentadd.studentInfo.MomJob;
                student.StudentInfo.MomJobPhone = studentadd.studentInfo.MomJobPhone;
                student.StudentInfo.MomPhone = studentadd.studentInfo.MomPhone;
                student.StudentInfo.MomJobAddress = studentadd.studentInfo.MomJobAddress;
                student.StudentInfo.HomeAddress = studentadd.studentInfo.HomeAddress;
                student.StudentInfo.HomePhone = studentadd.studentInfo.HomePhone;
                student.StudentInfo.FamilyState = studentadd.studentInfo.FamilyState;
                student.StudentInfo.Religion = studentadd.studentInfo.Religion;
                student.StudentInfo.SocialNet = studentadd.studentInfo.SocialNet;
                student.StudentInfo.Email = studentadd.studentInfo.Email;
                student.StudentInfo.Desc = studentadd.studentInfo.Desc;

                //---------------------------------------------------------------------------------------


                var agentId = student.Id;
                var agentType = TicketType.Student;
                var agentType2 = TicketType.StudentParent;
                var agentName = student.Name + " " + student.LastName;

                var tickets = db.Tickets.Where(c =>
                ((c.ReciverType == agentType) || (c.ReciverType == agentType2) && c.ReciverId == agentId) ||
                ((c.SenderType == agentType) || (c.SenderType == agentType2) && c.SenderId == agentId));

                await tickets.ForEachAsync(c =>
                {
                    if ((c.ReciverType == agentType) || (c.ReciverType == agentType2) && c.ReciverId == agentId)
                    {
                        c.ReciverName = agentName;
                    }
                    else if ((c.SenderType == agentType) || (c.SenderType == agentType2) && c.SenderId == agentId)
                    {
                        c.SenderName = agentName;
                    }
                });


                var mobileChats = db.MobileChats.Where(c =>
                (c.ReciverType == MobileChatType.StudentParent && c.ReciverId == agentId) ||
                (c.SenderType == MobileChatType.StudentParent && c.SenderId == agentId));

                await mobileChats.ForEachAsync(c =>
                {
                    if (c.ReciverType == MobileChatType.StudentParent && c.ReciverId == agentId)
                    {
                        c.ReciverName = agentName;
                    }
                    else if (c.SenderType == MobileChatType.StudentParent && c.SenderId == agentId)
                    {
                        c.SenderName = agentName;
                    }
                });

                var notificationSeens = db.NotificationSeens.Where(c => c.AgentId == agentId && (c.AgentType == 0 || c.AgentType == 1));

                await notificationSeens.ForEachAsync(c =>
                {
                    c.AgentName = agentName;
                });

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        // Fast Edit
        [HttpPost]
        public async Task<IActionResult> EditF([FromBody] Student studentp)
        {
            try
            {
                studentp.BirthDate.AddDays(1);

                var student = await db.Students.Include(c => c.StudentInfo).SingleAsync(c => c.Id == studentp.Id);

                var BirthDateBefore = student.BirthDate;

                student.BirthDate = studentp.BirthDate;

                if (BirthDateBefore != student.BirthDate)
                {
                    student.BirthDate = student.BirthDate.AddDays(1);
                }


                if (await db.Students.Except(db.Students.Where(c => c.Id == studentp.Id)).AnyAsync(c => c.IdNumber2 == studentp.IdNumber2))
                {
                    return this.UnSuccessFunction("این کد ملی قبلا ثبت شده است");
                }

                if (await db.Students.Except(db.Students.Where(c => c.Id == studentp.Id)).AnyAsync(c => c.Code == studentp.Code))
                {
                    return this.UnSuccessFunction("این کد دانش آموز قبلا ثبت شده است");
                }

                student.Code = studentp.Code;
                student.Name = studentp.Name;
                student.LastName = studentp.LastName;
                student.FatherName = studentp.FatherName;
                student.IdNumber = studentp.IdNumber;
                student.IdNumber2 = studentp.IdNumber2;

                var agentId = student.Id;
                var agentType = TicketType.Student;
                var agentType2 = TicketType.StudentParent;
                var agentName = student.Name + " " + student.LastName;

                var tickets = db.Tickets.Where(c =>
                ((c.ReciverType == agentType) || (c.ReciverType == agentType2) && c.ReciverId == agentId) ||
                ((c.SenderType == agentType) || (c.SenderType == agentType2) && c.SenderId == agentId));

                await tickets.ForEachAsync(c =>
                {
                    if ((c.ReciverType == agentType) || (c.ReciverType == agentType2) && c.ReciverId == agentId)
                    {
                        c.ReciverName = agentName;
                    }
                    else if ((c.SenderType == agentType) || (c.SenderType == agentType2) && c.SenderId == agentId)
                    {
                        c.SenderName = agentName;
                    }
                });


                var mobileChats = db.MobileChats.Where(c =>
                (c.ReciverType == MobileChatType.StudentParent && c.ReciverId == agentId) ||
                (c.SenderType == MobileChatType.StudentParent && c.SenderId == agentId));

                await mobileChats.ForEachAsync(c =>
                {
                    if (c.ReciverType == MobileChatType.StudentParent && c.ReciverId == agentId)
                    {
                        c.ReciverName = agentName;
                    }
                    else if (c.SenderType == MobileChatType.StudentParent && c.SenderId == agentId)
                    {
                        c.SenderName = agentName;
                    }
                });


                var notificationSeens = db.NotificationSeens.Where(c => c.AgentId == agentId && (c.AgentType == 0 || c.AgentType == 1));

                await notificationSeens.ForEachAsync(c =>
                {
                    c.AgentName = agentName;
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
        public IActionResult Get([FromBody] getparamsCustom getparams)
        {
            try
            {
                getparams.getparams.pageIndex += 1;

                int count;


                var query = getparams.getparams.q;

                var st = db.Students
                    .Include(c => c.StdClassMngs)
                .AsQueryable();



                if (!string.IsNullOrWhiteSpace(query))
                {

                    st = st.Where(c => c.Name.Contains(query) || c.Code.ToString().Contains(query) ||
                                    c.OrgCode.Contains(query) || c.LastName.Contains(query) || c.FatherName.Contains(query) ||
                                    c.IdNumber.Contains(query) || c.IdNumber2.Contains(query) || c.IdCardSerial.Contains(query) ||
                                    c.BirthLocation.Contains(query) || c.StudentInfo.FatherName.Contains(query) || c.StudentInfo.FatherEdu.Contains(query) ||
                                    c.StudentInfo.FatherJob.Contains(query) || c.StudentInfo.FatherJob.Contains(query) || c.StudentInfo.FatherPhone.Contains(query) ||
                                    c.StudentInfo.MomName.Contains(query) || c.StudentInfo.MomPhone.Contains(query) || c.StudentInfo.HomeAddress.Contains(query) ||
                                    c.StudentInfo.Religion.Contains(query) || c.StudentInfo.SocialNet.Contains(query) || c.StudentInfo.Email.Contains(query) || c.StudentInfo.Desc.Contains(query));
                }


                if (getparams.selectedStudentState.HasValue)
                {
                    st = st.Where(c => c.StudentState == getparams.selectedStudentState);
                }

                if (getparams.selectedStdStudyState.HasValue)
                {
                    st = st.Where(c => c.haveState(1, c.StdClassMngs.ToList(), getparams.selectedStdStudyState.Value));
                }

                if (getparams.selectedStdBehaveState.HasValue)
                {
                    st = st.Where(c => c.haveState(2, c.StdClassMngs.ToList(), getparams.selectedStdBehaveState.Value));
                }

                if (getparams.selectedStdPayrollState.HasValue)
                {
                    st = st.Where(c => c.haveState(3, c.StdClassMngs.ToList(), getparams.selectedStdPayrollState.Value));
                }


                if (getparams.gradeId.HasValue)
                {
                    st = st.Where(c => c.haveGrade(getparams.gradeId.Value, c.StdClassMngs.ToList()));
                }


                if (getparams.classId.HasValue)
                {
                    st = st.Where(c => c.haveClass(getparams.classId.Value, c.StdClassMngs.ToList()));
                }

                if (getparams.parentAccess == "access")
                {
                    st = st.Where(c => c.ParentAccess == true);
                }

                if (getparams.parentAccess == "block")
                {
                    st = st.Where(c => c.ParentAccess == false);
                }


                count = st.Count();


                if (getparams.getparams.direction.Equals("asc"))
                {
                    if (getparams.getparams.sort.Equals("id"))
                    {
                        st = st.OrderBy(c => c.Id);
                    }
                    if (getparams.getparams.sort.Equals("namelast"))
                    {
                        st = st.OrderBy(c => c.LastName).ThenBy(c => c.Name);
                    }
                    if (getparams.getparams.sort.Equals("code"))
                    {
                        st = st.OrderBy(c => c.Code);
                    }
                    if (getparams.getparams.sort.Equals("fatherName"))
                    {
                        st = st.OrderBy(c => c.FatherName);
                    }
                    if (getparams.getparams.sort.Equals("idNumber"))
                    {
                        st = st.OrderBy(c => c.IdNumber);
                    }
                }
                else if (getparams.getparams.direction.Equals("desc"))
                {
                    if (getparams.getparams.sort.Equals("id"))
                    {
                        st = st.OrderByDescending(c => c.Id);
                    }
                    if (getparams.getparams.sort.Equals("namelast"))
                    {
                        st = st.OrderByDescending(c => c.LastName).ThenByDescending(c => c.Name);
                    }
                    if (getparams.getparams.sort.Equals("code"))
                    {
                        st = st.OrderByDescending(c => c.Code);
                    }
                    if (getparams.getparams.sort.Equals("fatherName"))
                    {
                        st = st.OrderByDescending(c => c.FatherName);
                    }
                    if (getparams.getparams.sort.Equals("idNumber"))
                    {
                        st = st.OrderByDescending(c => c.IdNumber);
                    }
                }
                else
                {
                    st = st.OrderBy(c => c.Id);
                }

                st = st.Skip((getparams.getparams.pageIndex - 1) * getparams.getparams.pageSize);
                st = st.Take(getparams.getparams.pageSize);

                var q = st
                    .Include(c => c.StudentInfo)
                .ToList();

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
        public async Task<IActionResult> getStudent([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var st = await db.Students.Include(c => c.StudentInfo).SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, new { student = st, studentinfo = st.StudentInfo });
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
        public async Task<IActionResult> getAllInClass([FromBody] int classId)
        {
            try
            {
                var students = await db.StdClassMngs
                    .Where(c => c.ClassId == classId && c.IsActive == true)
                        .Select(c => c.Student)
                            .Distinct()
                .ToListAsync();

                return this.DataFunction(true, students.OrderBy(c => c.LastName).ThenBy(c => c.Name));
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllHaveStdClassMng()
        {
            try
            {
                var students = await db.Students
                        .Include(c => c.StdClassMngs)
                    .Where(c => c.StdClassMngs.Count != 0)
                .ToListAsync();

                return this.DataFunction(true, students);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> loginStudent([FromBody] loginStudetParam loginstd)
        {
            try
            {
                var falsemsg = "نام کاربری و کلمه عبور اشتباه است";

                if (await db.Students.AnyAsync(c => c.IdNumber2 == loginstd.username))
                {
                    if (loginstd.username == loginstd.password)
                    {
                        var std = await db.Students.SingleAsync(c => c.IdNumber2 == loginstd.username);
                        return this.SuccessFunction(new
                        {
                            id = std.Id,
                            tk = BuildToken(loginstd.username, 15)
                        });
                    }

                    return this.UnSuccessFunction(falsemsg);
                }
                return this.UnSuccessFunction(falsemsg);

            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }



        [HttpPost]
        public async Task<IActionResult> stdWB([FromBody] int stdId)
        {
            try
            {
                var std = await db.Students.FirstOrDefaultAsync(c => c.Id == stdId);
                var stdHeader = await db.WorkBookHeaders.Where(c => c.StdId == std.Code).FirstOrDefaultAsync();
                var stdDt = await db.WorkBookDetails.Where(c => c.StdId == std.Code).ToListAsync();

                return this.SuccessFunction(new
                {
                    stdHeader = stdHeader,
                    stdDt = stdDt
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        //setReviweRequest
        [HttpPost]
        public async Task<IActionResult> setReviweRequest([FromBody] int[] detailIds)
        {
            try
            {
                foreach (var id in detailIds)
                {
                    var detail = await db.WorkBookDetails.FirstOrDefaultAsync(c => c.Id == id);

                    if (detail != null)
                    {
                        detail.haveRequestToReview = true;
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
        public async Task<IActionResult> setState([FromBody] setstateparam setstate)
        {
            try
            {
                foreach (var id in setstate.ids)
                {
                    var std = await db.Students.FirstOrDefaultAsync(c => c.Id == id);

                    std.StudentState = setstate.state;
                }

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        public class setstateparam
        {
            public int[] ids { get; set; }

            public int state { get; set; }
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
        public async Task<IActionResult> getAll()
        {
            try
            {
                var st = await db.Students
                        .OrderBy(c => c.LastName)
                        .ThenBy(c => c.Name)
                    .Select(c => new { id = c.Id, name = c.Name + " " + c.LastName })
                .ToListAsync();

                return this.DataFunction(true, st);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public IActionResult GetByGradeClass([FromBody] GetByGradeClassParam param)
        {
            try
            {
                var studentts = db.Students
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name + ' ' + c.LastName,
                        Code = c.Code,
                        OrgCode = c.OrgCode,
                        LastName = c.LastName,
                        FatherName = c.FatherName,
                        IdNumber = c.IdNumber,
                        IdNumber2 = c.IdNumber2,
                        IdCardSerial = c.IdCardSerial,
                        haveClass = c.haveClass(param.classId, c.StdClassMngs.ToList()),
                        haveGrade = c.haveGrade(param.gradeId, c.StdClassMngs.ToList())
                    })
                .AsQueryable();

                var query = param.search;

                if (!string.IsNullOrEmpty(query))
                {
                    studentts = studentts.Where(c => c.Name.Contains(query) || c.Code.ToString().Contains(query) ||
                                    c.OrgCode.Contains(query) || c.LastName.Contains(query) || c.FatherName.Contains(query) ||
                                    c.IdNumber.Contains(query) || c.IdNumber2.Contains(query) || c.IdCardSerial.Contains(query));
                }

                if (param.classId != 0)
                {
                    studentts = studentts.Where(c => c.haveClass == true);
                }

                if (param.gradeId != 0)
                {
                    studentts = studentts.Where(c => c.haveGrade == true);
                }

                return this.DataFunction(true, studentts.ToList());
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

                        var st = await db.Students.Include(c => c.StdClassMngs).SingleAsync(c => c.Id == id);

                        if (st == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        if (st.StdClassMngs.Any())
                        {
                            return this.UnSuccessFunction("این دانش آموز دارای لیست ثبت نامی است، نمیتوان آن را حذف کرد");
                        }

                        db.Students.Remove(st);
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
        public IActionResult getExcelExample()
        {
            try
            {
                var filePath = "/UploadFiles/stdEX.xlsx";

                return this.SuccessFunction(redirect: filePath);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        //ImportDataFromExcel


        [HttpPost]
        public async Task<IActionResult> ImportDataFromExcel([FromBody] ImportDataFromExcelParam importDataFromExcelParam)
        {
            try
            {

                var guid = System.Guid.NewGuid().ToString();

                var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, importDataFromExcelParam.fileName);
                Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                byte[] bytes = Convert.FromBase64String(importDataFromExcelParam.fileValue);
                System.IO.File.WriteAllBytes(path, bytes);

                var filepath = Path.Combine("/UploadFiles/" + guid + "/" + importDataFromExcelParam.fileName);

                FileInfo file = new FileInfo(path);

                var message = new List<string>();

                List<Student> stdList = new List<Student>();

                var DateRegex = @"^[1-4]\d{3}\/((0[1-6]\/((3[0-1])|([1-2][0-9])|(0[1-9])))|((1[0-2]|(0[7-9]))\/(30|([1-2][0-9])|(0[1-9]))))$";

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                    int totalRows = workSheet.Dimension.Rows;


                    for (int i = 2; i <= totalRows; i++)
                    {
                        var date = workSheet.Cells[i, 7].Value.ToString();
                        var DateRegexMatch = Regex.Match(date, DateRegex).Success;

                        if (string.IsNullOrEmpty(workSheet.Cells[i, 1].Value.ToString()) ||
                            string.IsNullOrEmpty(workSheet.Cells[i, 2].Value.ToString()) ||
                            string.IsNullOrEmpty(workSheet.Cells[i, 3].Value.ToString()) ||
                            string.IsNullOrEmpty(workSheet.Cells[i, 4].Value.ToString()) ||
                            string.IsNullOrEmpty(workSheet.Cells[i, 5].Value.ToString()) ||
                            string.IsNullOrEmpty(workSheet.Cells[i, 6].Value.ToString()) ||
                            string.IsNullOrEmpty(workSheet.Cells[i, 7].Value.ToString()) ||
                            !DateRegexMatch
                            )
                        {
                            message.Add("مقادیر ردیف " + i + " را بررسی مجدد نمایید، مقادیر ستون یک تا هفت نمیتوانند خالی باشند و یا قالب تاریخ تولد اشتباه است");
                            continue;
                        }


                        if (await db.Students.AnyAsync(c => c.IdNumber == workSheet.Cells[i, 5].Value.ToString() ||
                                c.Code == int.Parse(workSheet.Cells[i, 1].Value.ToString())))
                        {
                            message.Add("مقدار ردیف " + i + " قبلا ثبت شده است");
                            continue;
                        }

                        stdList.Add(new Student
                        {
                            Code = int.Parse(workSheet.Cells[i, 1].Value.ToString()),
                            Name = workSheet.Cells[i, 2].Value.ToString().Trim(),
                            LastName = workSheet.Cells[i, 3].Value.ToString().Trim(),
                            FatherName = workSheet.Cells[i, 4].Value.ToString().Trim(),
                            IdNumber = workSheet.Cells[i, 5].Value.ToString().Trim(),
                            IdNumber2 = workSheet.Cells[i, 6].Value.ToString().Trim(),
                            BirthDate = workSheet.Cells[i, 7].Value.ToString().PersianStringDateToDateTime(),
                            OrgCode = workSheet.Cells[i, 8].Value == null ? string.Empty : workSheet.Cells[i, 8].Value.ToString(),
                            IdCardSerial = workSheet.Cells[i, 9].Value == null ? string.Empty : workSheet.Cells[i, 9].Value.ToString(),
                            BirthLocation = workSheet.Cells[i, 10].Value == null ? string.Empty : workSheet.Cells[i, 10].Value.ToString(),
                            StudentInfo = new StudentInfo
                            {
                                FatherName = workSheet.Cells[i, 11].Value == null ? string.Empty : workSheet.Cells[i, 11].Value.ToString(),
                                FatherEdu = workSheet.Cells[i, 12].Value == null ? string.Empty : workSheet.Cells[i, 12].Value.ToString(),
                                FatherJob = workSheet.Cells[i, 13].Value == null ? string.Empty : workSheet.Cells[i, 13].Value.ToString(),
                                FatherPhone = workSheet.Cells[i, 14].Value == null ? string.Empty : workSheet.Cells[i, 14].Value.ToString(),
                                FatherJobPhone = workSheet.Cells[i, 15].Value == null ? string.Empty : workSheet.Cells[i, 15].Value.ToString(),
                                FatherJobAddress = workSheet.Cells[i, 16].Value == null ? string.Empty : workSheet.Cells[i, 16].Value.ToString(),
                                MomName = workSheet.Cells[i, 17].Value == null ? string.Empty : workSheet.Cells[i, 17].Value.ToString(),
                                MomEdu = workSheet.Cells[i, 18].Value == null ? string.Empty : workSheet.Cells[i, 18].Value.ToString(),
                                MomJob = workSheet.Cells[i, 19].Value == null ? string.Empty : workSheet.Cells[i, 19].Value.ToString(),
                                MomPhone = workSheet.Cells[i, 20].Value == null ? string.Empty : workSheet.Cells[i, 20].Value.ToString(),
                                MomJobPhone = workSheet.Cells[i, 21].Value == null ? string.Empty : workSheet.Cells[i, 21].Value.ToString(),
                                MomJobAddress = workSheet.Cells[i, 22].Value == null ? string.Empty : workSheet.Cells[i, 22].Value.ToString(),
                                HomeAddress = workSheet.Cells[i, 23].Value == null ? string.Empty : workSheet.Cells[i, 23].Value.ToString(),
                                HomePhone = workSheet.Cells[i, 24].Value == null ? string.Empty : workSheet.Cells[i, 24].Value.ToString(),
                                FamilyState = workSheet.Cells[i, 25].Value == null ? string.Empty : workSheet.Cells[i, 25].Value.ToString(),
                                Religion = workSheet.Cells[i, 26].Value == null ? string.Empty : workSheet.Cells[i, 26].Value.ToString(),
                                SocialNet = workSheet.Cells[i, 27].Value == null ? string.Empty : workSheet.Cells[i, 27].Value.ToString(),
                                Email = workSheet.Cells[i, 28].Value == null ? string.Empty : workSheet.Cells[i, 28].Value.ToString(),
                                Desc = workSheet.Cells[i, 29].Value == null ? string.Empty : workSheet.Cells[i, 29].Value.ToString()
                            }
                        });
                    }
                }

                await db.Students.AddRangeAsync(stdList);

                await db.SaveChangesAsync();

                if (message.Any())
                {
                    var messages = "";

                    foreach (var item in message)
                    {
                        messages += "\n " + item;
                    }

                    return this.UnSuccessFunction(messages);
                }

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getDataToExcel()
        {
            try
            {
                var students = await db.Students.Include(c => c.StudentInfo).ToListAsync();

                byte[] fileContents;

                var sheatTitle = "لیست دانش آموزان";

                PersianCalendar pc = new PersianCalendar();
                var date = DateTime.Now;
                var persianDateString = pc.GetYear(date).ToString() + pc.GetMonth(date).ToString("0#") + pc.GetDayOfMonth(date).ToString("0#") + pc.GetHour(date).ToString("0#") + pc.GetMinute(date).ToString("0#") + pc.GetSecond(date).ToString("0#");

                var fileName = $"Students_Mabna_{persianDateString}";

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(sheatTitle);
                    worksheet.DefaultColWidth = 20;
                    worksheet.DefaultRowHeight = 20;
                    worksheet.View.RightToLeft = true;
                    worksheet.Cells.Style.Font.Name = "B Yekan";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    worksheet.Cells[1, 1].Value = "ردیف";
                    worksheet.Cells[1, 2].Value = "نام";
                    worksheet.Cells[1, 3].Value = "نام خانوادگی";
                    worksheet.Cells[1, 4].Value = "نام پدر";
                    worksheet.Cells[1, 5].Value = "تاریخ تولد";
                    worksheet.Cells[1, 6].Value = "شماره شناسنامه";
                    worksheet.Cells[1, 7].Value = "شماره ملی";
                    worksheet.Cells[1, 8].Value = "کد دانش آموز";
                    worksheet.Cells[1, 9].Value = "سریال شناسنامه";
                    worksheet.Cells[1, 10].Value = "کد شناسه ای آموزش و پرورش دانش آموز";
                    worksheet.Cells[1, 11].Value = "محل تولد";


                    worksheet.Cells[1, 12].Value = "نام و نام خانوادگی پدر";
                    worksheet.Cells[1, 13].Value = "تحصیلات پدر";
                    worksheet.Cells[1, 14].Value = "شغل پدر";
                    worksheet.Cells[1, 15].Value = "تلفن همراه پدر";
                    worksheet.Cells[1, 16].Value = "تلفن محل کار پدر";
                    worksheet.Cells[1, 17].Value = "آدرس محل کار پدر";


                    worksheet.Cells[1, 18].Value = "نام و نام خانوادگی مادر";
                    worksheet.Cells[1, 19].Value = "تحصیلات مادر";
                    worksheet.Cells[1, 20].Value = "شغل مادر";
                    worksheet.Cells[1, 21].Value = "تلفن همراه مادر";
                    worksheet.Cells[1, 22].Value = "تلفن محل کار مادر";
                    worksheet.Cells[1, 23].Value = "آدرس محل کار مادر";


                    worksheet.Cells[1, 24].Value = "نشانی منزل";
                    worksheet.Cells[1, 25].Value = "تلفن ثابت منزل";
                    worksheet.Cells[1, 26].Value = "وضعیت خانوادگی";
                    worksheet.Cells[1, 27].Value = "دین، مذهب";


                    worksheet.Cells[1, 28].Value = "شماره شبکه اجتماعی";
                    worksheet.Cells[1, 29].Value = "آدرس پست الکترونیک";
                    worksheet.Cells[1, 30].Value = "توضیحات";

                    foreach (var (student, index) in students.WithIndex())
                    {
                        worksheet.Cells[1 + (index + 1), 1].Value = student.Id;
                        worksheet.Cells[1 + (index + 1), 2].Value = student.Name;
                        worksheet.Cells[1 + (index + 1), 3].Value = student.LastName;
                        worksheet.Cells[1 + (index + 1), 4].Value = student.FatherName;
                        worksheet.Cells[1 + (index + 1), 5].Value = student.BirthDate.ToPersianDate();
                        worksheet.Cells[1 + (index + 1), 6].Value = student.IdNumber;
                        worksheet.Cells[1 + (index + 1), 7].Value = student.IdNumber2;
                        worksheet.Cells[1 + (index + 1), 8].Value = student.Code;
                        worksheet.Cells[1 + (index + 1), 9].Value = student.IdCardSerial;
                        worksheet.Cells[1 + (index + 1), 10].Value = student.OrgCode;
                        worksheet.Cells[1 + (index + 1), 11].Value = student.BirthLocation;

                        if (student.StudentInfo != null)
                        {
                            worksheet.Cells[1 + (index + 1), 12].Value = student.StudentInfo.FatherName;
                            worksheet.Cells[1 + (index + 1), 13].Value = student.StudentInfo.FatherEdu;
                            worksheet.Cells[1 + (index + 1), 14].Value = student.StudentInfo.FatherJob;
                            worksheet.Cells[1 + (index + 1), 15].Value = student.StudentInfo.FatherPhone;
                            worksheet.Cells[1 + (index + 1), 16].Value = student.StudentInfo.FatherJobPhone;
                            worksheet.Cells[1 + (index + 1), 17].Value = student.StudentInfo.FatherJobAddress;


                            worksheet.Cells[1 + (index + 1), 18].Value = student.StudentInfo.MomName;
                            worksheet.Cells[1 + (index + 1), 19].Value = student.StudentInfo.MomEdu;
                            worksheet.Cells[1 + (index + 1), 20].Value = student.StudentInfo.MomJob;
                            worksheet.Cells[1 + (index + 1), 21].Value = student.StudentInfo.MomPhone;
                            worksheet.Cells[1 + (index + 1), 22].Value = student.StudentInfo.MomJobPhone;
                            worksheet.Cells[1 + (index + 1), 23].Value = student.StudentInfo.MomJobAddress;


                            worksheet.Cells[1 + (index + 1), 24].Value = student.StudentInfo.HomeAddress;
                            worksheet.Cells[1 + (index + 1), 25].Value = student.StudentInfo.HomePhone;
                            worksheet.Cells[1 + (index + 1), 26].Value = student.StudentInfo.FamilyState;
                            worksheet.Cells[1 + (index + 1), 27].Value = student.StudentInfo.Religion;


                            worksheet.Cells[1 + (index + 1), 28].Value = student.StudentInfo.SocialNet;
                            worksheet.Cells[1 + (index + 1), 29].Value = student.StudentInfo.Email;
                            worksheet.Cells[1 + (index + 1), 30].Value = student.StudentInfo.Desc;
                        }
                    }

                    fileContents = package.GetAsByteArray();

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, fileName + ".xlsx");
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    System.IO.File.WriteAllBytes(path, fileContents);

                    var filepath = Path.Combine("/UploadFiles", guid, fileName + ".xlsx");

                    return this.SuccessFunction(redirect: filepath);
                }
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getStudentForExamScore([FromBody] int examId)
        {
            try
            {
                var exam = await db.Exams.SingleAsync(c => c.Id == examId);

                var students = await db.StdClassMngs.Where(c => c.GradeId == exam.GradeId && c.ClassId == exam.ClassId && c.YeareducationId == exam.YeareducationId)
                        .Select(c => c.Student)
                    .OrderBy(c => c.LastName).ThenBy(c => c.Name)
                .ToListAsync();

                return this.SuccessFunction(data: students);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getPrintData()
        {
            try
            {
                var headers = new List<string>();
                var datas = new List<List<string>>();

                var title = "لیست دانش آموزان";


                headers.Add("ردیف");
                headers.Add("کد دانش آموز");
                headers.Add("نام و نام خانوادگی");
                headers.Add("نام پدر");
                headers.Add("کد ملی");
                headers.Add("تاریخ تولد");

                var students = await db.Students.ToListAsync();

                students.ForEach(c =>
                {
                    var lstData = new List<string>();

                    lstData.Add(c.Id.ToString());
                    lstData.Add(c.Code.ToString());
                    lstData.Add(c.Name + " " + c.LastName);
                    lstData.Add(c.FatherName);
                    lstData.Add(c.IdNumber);
                    lstData.Add(c.BirthDate.ToPersianDate());

                    datas.Add(lstData);
                });

                return this.DataFunction(true, new PrintData
                {
                    headers = headers,
                    datas = datas,
                    title = title
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordByType([FromBody] ChangePasswordByTypeParam changepass)
        {
            try
            {
                var student = await db.Students.FirstOrDefaultAsync(c => c.Id == changepass.stdId);

                if (changepass.type == 1)
                {
                    student.StudentPassword = changepass.password;
                }

                if (changepass.type == 2)
                {
                    student.ParentsPassword = changepass.password;
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
        [AllowAnonymous]
        public async Task<IActionResult> LoginParent([FromBody] LoginParams loginParent)
        {
            try
            {
                var std = await db.Students
                    .Where(c => c.IdNumber2 == loginParent.username)
                    .Include(c => c.StudentInfo)
                .FirstOrDefaultAsync();


                if (std != null)
                {

                    var stdClassMngs = await db.StdClassMngs.Where(c => c.StudentId == std.Id)
                        .Select(c => new
                        {
                            yeareducationName = c.Yeareducation.Name,
                            gradeName = c.Grade.Name,
                            IsActive = c.IsActive,
                            className = c.Class.Name,
                            Id = c.Id,
                            ClassId = c.ClassId,
                            GradeId = c.GradeId,
                            YeareducationId = c.YeareducationId
                        })
                    .ToListAsync();

                    if (string.IsNullOrEmpty(std.ParentsPassword))
                    {
                        std.ParentsPassword = "1";
                    }

                    if (std.ParentsPassword == loginParent.password)
                    {
                        if (!std.ParentAccess)
                        {
                            return this.UnSuccessFunction("دسترسی شما مسدود شده است!");
                        }

                        return this.DataFunction(true, new
                        {
                            token = BuildToken(loginParent.username, 1440 * 7),
                            std = std,
                            stdClassMngs = stdClassMngs,
                            changePass = !IsPasswordSafe(std.ParentsPassword)
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

        [HttpPost]
        public async Task<IActionResult> ChangeParentPassword([FromBody] ChangePasswordParam changepass)
        {
            try
            {
                var student = await db.Students.FirstOrDefaultAsync(c => c.Id == changepass.id);

                if (student.ParentsPassword != changepass.nowPass)
                {
                    return this.UnSuccessFunction("کلمه عبور فعلی اشتباه است");
                }

                student.ParentsPassword = changepass.newPass;

                await db.SaveChangesAsync();

                return this.DataFunction(true, new
                {
                    isUserHaveToChangePass = !IsPasswordSafe(student.ParentsPassword)
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        private bool IsPasswordSafe(string pass)
        {
            if (pass == "1" || pass == "12345678")
                return false;

            return true;
        }


        [HttpPost]
        public async Task<IActionResult> ChangeAccessByType([FromBody] ChangeAccessByTypeParam changeAccess)
        {
            try
            {
                var std = await db.Students.FirstOrDefaultAsync(c => c.Id == changeAccess.stdId);

                if (changeAccess.type == 1)
                {
                    std.StudentAccess = changeAccess.access;
                }

                if (changeAccess.type == 2)
                {
                    std.ParentAccess = changeAccess.access;
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
        public async Task<IActionResult> IsUserAccessByType([FromBody] IsUserAccessByTypeParam accessparam)
        {
            try
            {
                var std = await db.Students.FirstOrDefaultAsync(c => c.Id == accessparam.stdId);

                var access = false;

                if (accessparam.type == 1)
                {
                    access = std.StudentAccess;
                }

                if (accessparam.type == 2)
                {
                    access = std.ParentAccess;
                }

                return this.DataFunction(true, access);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordGroup([FromBody] ResetPasswordParam param)
        {
            try
            {
                foreach (var id in param.ids)
                {
                    var student = await db.Students.FirstOrDefaultAsync(c => c.Id == id);

                    if (param.type == 1)
                    {
                        student.ParentsPassword = "1";
                    }
                    else if (param.type == 2)
                    {
                        student.StudentPassword = "1";
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
        public async Task<IActionResult> ChangeAccessGroup([FromBody] ChangeAccessGroupParam param)
        {
            try
            {
                foreach (var id in param.ids)
                {
                    var student = await db.Students.FirstOrDefaultAsync(c => c.Id == id);

                    if (param.type == 1)
                    {
                        student.ParentAccess = param.access;
                    }
                    else if (param.type == 2)
                    {
                        student.StudentAccess = param.access;
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



    }

    public class GetByGradeClassParam
    {
        public string search { get; set; }

        public int gradeId { get; set; }

        public int classId { get; set; }
    }

    public class ChangeAccessGroupParam
    {
        public int[] ids { get; set; }

        public int type { get; set; }

        public bool access { get; set; }
    }

    public class ResetPasswordParam
    {
        public int[] ids { get; set; }

        public int type { get; set; }
    }

    public class IsUserAccessByTypeParam
    {
        public int stdId { get; set; }

        public int type { get; set; }
    }

    public class ChangeAccessByTypeParam
    {
        public int stdId { get; set; }

        public int type { get; set; }

        public bool access { get; set; }
    }

    public class ChangePasswordParam
    {
        public string nowPass { get; set; }

        public string newPass { get; set; }

        public int id { get; set; }
    }

    public class ChangePasswordByTypeParam
    {
        public int stdId { get; set; }

        public int type { get; set; }

        public string password { get; set; }
    }

    public class loginStudetParam
    {
        public string username { get; set; }

        public string password { get; set; }
    }

    public class getparamsCustom
    {
        public getparams getparams { get; set; }

        public int? gradeId { get; set; }

        public int? classId { get; set; }

        public int? selectedStudentState { get; set; }
        public int? selectedStdStudyState { get; set; }
        public int? selectedStdBehaveState { get; set; }
        public int? selectedStdPayrollState { get; set; }

        public string parentAccess { get; set; }
    }

    public class ImportDataFromExcelParam
    {
        public string fileValue { get; set; }

        public string fileName { get; set; }

        public string fileType { get; set; }
    }

    public class StudentAddParams
    {
        public Student student { get; set; }

        public StudentInfo studentInfo { get; set; }
    }
}