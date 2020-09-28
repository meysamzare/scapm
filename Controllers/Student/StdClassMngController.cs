using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class StdClassMngController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public StdClassMngController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StdClassMng stdClassMng)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                if (stdClassMng.YeareducationId != nowYeareducationId)
                {
                    return this.UnSuccessFunction("تنها سال تحصیلی فعال می تواند انتخاب شود");
                }

                if (await db.StdClassMngs.AnyAsync(c =>
                    // c.InsTituteId == stdClassMng.InsTituteId &&
                    c.YeareducationId == stdClassMng.YeareducationId && c.StudentId == stdClassMng.StudentId))
                {
                    return this.UnSuccessFunction("این دانش آموز در این سال تحصیلی ثبت نام شده است");
                }

                var cls = await db.Classes.Include(c => c.StdClassMngs).FirstOrDefaultAsync(c => c.Id == stdClassMng.ClassId);

                lock (cls)
                {
                    if (cls.Capasity - cls.StdClassMngs.Count <= 0)
                    {
                        return this.UnSuccessFunction("ظرفیت کلاس تکمیل است");
                    }
                }

                await db.StdClassMngs.AddAsync(stdClassMng);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] AddGroupParam addgroup)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                if (addgroup.stdClassMng.YeareducationId != nowYeareducationId)
                {
                    return this.UnSuccessFunction("تنها سال تحصیلی فعال می تواند انتخاب شود");
                }

                var stdClassMngList = new List<StdClassMng>();


                var cls = await db.Classes.Include(c => c.StdClassMngs).FirstOrDefaultAsync(c => c.Id == addgroup.stdClassMng.ClassId);

                lock (cls)
                {
                    if (cls.Capasity - cls.StdClassMngs.Count + addgroup.ids.Count() <= 0)
                    {
                        return this.UnSuccessFunction("تعداد نفرات انتخابی برای ثبت نام بیش از ظرفیت کلاس است، ظرفیت کلاس: " +
                                    (cls.Capasity - cls.StdClassMngs.Count).ToString());
                    }
                }

                foreach (var id in addgroup.ids)
                {
                    var stdClassMng = new StdClassMng
                    {
                        StudentId = id,
                        ClassId = addgroup.stdClassMng.ClassId,
                        GradeId = addgroup.stdClassMng.GradeId,
                        InsTituteId = addgroup.stdClassMng.InsTituteId,
                        YeareducationId = addgroup.stdClassMng.YeareducationId
                    };

                    if (await db.StdClassMngs.AnyAsync(c =>
                        // c.InsTituteId == stdClassMng.InsTituteId &&
                        c.YeareducationId == stdClassMng.YeareducationId && c.StudentId == stdClassMng.StudentId))
                    {
                        continue;
                    }



                    stdClassMngList.Add(stdClassMng);
                }

                await db.StdClassMngs.AddRangeAsync(stdClassMngList);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllbyStd([FromBody] int StudentId)
        {
            try
            {
                var stds = await db.StdClassMngs
                    .Where(c => c.StudentId == StudentId)
                .Select(c => new
                {
                    Id = c.Id,
                    gradeName = c.Grade.Name,
                    tituteName = c.InsTitute.Name,
                    yeareducationName = c.Yeareducation.Name,
                    className = c.Class.Name,

                    canRemove = !c.Student.ExamScores.Any(b => b.Exam.YeareducationId == c.YeareducationId &&
                                    b.Exam.GradeId == c.GradeId && b.Exam.ClassId == c.ClassId),

                    StudyState = c.StudyState,
                    BehaveState = c.BehaveState,
                    PayrollState = c.PayrollState,

                    studentTypeName = c.StudentType == null ? "تعیین نشده" : c.StudentType.Name,
                    StudentId = c.StudentId,
                    StudentTypeId = c.StudentTypeId,
                    isActive = c.IsActive,
                    ClassId = c.ClassId,
                    GradeId = c.GradeId,
                    YeareducationId = c.YeareducationId
                })
                .ToListAsync();

                return this.SuccessFunction(data: stds);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllRegisteredByStudent([FromBody] int studentId)
        {
            try
            {
                var stdClassMngs = await db.StdClassMngs
                    .Where(c => c.StudentId == studentId)
                .Select(c => new
                {
                    Id = c.Id,
                    gradeName = c.Grade.Name,
                    yeareducationName = c.Yeareducation.Name,
                    className = c.Class.Name,
                    StudentId = c.StudentId,
                    ClassId = c.ClassId,
                    GradeId = c.GradeId,
                    YeareducationId = c.YeareducationId
                })
                .ToListAsync();

                return this.DataFunction(true, stdClassMngs);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllRegistredStudent()
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var students = await db.StdClassMngs
                    .Where(c => c.YeareducationId == nowYeareducationId)
                    .Select(c => c.Student).Distinct().ToListAsync();

                return this.DataFunction(true, students);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getRegistredGradeByStudent([FromBody] int studentId)
        {
            try
            {
                var grades = await db.StdClassMngs
                    .Where(c => c.StudentId == studentId)
                        .Include(c => c.Grade)
                    .Select(c => c.Grade)
                .ToListAsync();

                return this.DataFunction(true, grades);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                var stdcls = await db.StdClassMngs
                    .Include(c => c.Student)
                        .ThenInclude(c => c.ExamScores)
                            .ThenInclude(c => c.Exam)
                .FirstOrDefaultAsync(c => c.Id == id);

                if (!stdcls.canRemove)
                {
                    return this.UnSuccessFunction("این ثبت نام دانش آموز به دلیل ثبت نمره برای دانش آموز نمی تواند حذف شود");
                }

                db.StdClassMngs.Remove(stdcls);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStateByType([FromBody] ChangeStateByTypeParam changestateparam)
        {
            try
            {
                var stdClassMng = await db.StdClassMngs.SingleAsync(c => c.Id == changestateparam.id);


                var nowYeareducationId = await this.getActiveYeareducationId();
                if (stdClassMng.YeareducationId != nowYeareducationId)
                {
                    return this.UnSuccessFunction("این ردیف نمی تواند ویرایش شود");
                }

                if (changestateparam.type == 1)
                {
                    stdClassMng.StudyState = (StdStudyState)changestateparam.state;
                }

                if (changestateparam.type == 2)
                {
                    stdClassMng.BehaveState = (StdBehaveState)changestateparam.state;
                }

                if (changestateparam.type == 3)
                {
                    stdClassMng.PayrollState = (StdPayrollState)changestateparam.state;
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
        public async Task<IActionResult> getStudentsByYGC([FromBody] getByYGC getstudent)
        {
            try
            {
                var students = new List<Student>();

                var StdClassMngs = db.StdClassMngs
                    .Where(c => c.YeareducationId == getstudent.yeareducationId && c.GradeId == getstudent.gradeId && c.ClassId == getstudent.classId)
                        .Include(c => c.Student);

                await StdClassMngs.ForEachAsync(c =>
                {
                    students.Add(c.Student);
                });

                return this.DataFunction(true, students);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStudentType([FromBody] changeStudentTypeParam changestdparam)
        {
            try
            {
                var stdClassMng = await db.StdClassMngs.FirstOrDefaultAsync(c => c.Id == changestdparam.id);

                var nowYeareducationId = await this.getActiveYeareducationId();
                if (stdClassMng.YeareducationId != nowYeareducationId)
                {
                    return this.UnSuccessFunction("این ردیف نمی تواند ویرایش شود");
                }

                stdClassMng.StudentTypeId = changestdparam.stdType;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> setActiveStdClassMng([FromBody] ActiveStdClassMngParam activeStdClassMng)
        {
            try
            {
                var stdClassMngForStudent = db.StdClassMngs.Where(c => c.StudentId == activeStdClassMng.StudentId);

                await stdClassMngForStudent.ForEachAsync(scm =>
                {
                    scm.IsActive = false;
                });

                var stdClassMng = await db.StdClassMngs.FirstOrDefaultAsync(c => c.Id == activeStdClassMng.StdClassMngId);


                var nowYeareducationId = await this.getActiveYeareducationId();
                if (stdClassMng.YeareducationId != nowYeareducationId)
                {
                    return this.UnSuccessFunction("تنها ردیفی که دارای سال تحصیلی فعال است می تواند انتخاب شود");
                }

                stdClassMng.IsActive = true;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }
    }

    public class ActiveStdClassMngParam
    {
        public int StudentId { get; set; }

        public int StdClassMngId { get; set; }
    }

    public class changeStudentTypeParam
    {
        public int id { get; set; }

        public int? stdType { get; set; }
    }

    public class getByYGC
    {
        public int yeareducationId { get; set; }

        public int gradeId { get; set; }

        public int classId { get; set; }

    }

    public class ChangeStateByTypeParam
    {
        public int id { get; set; }

        public int type { get; set; }

        public int state { get; set; }

    }

    public class AddGroupParam
    {
        public StdClassMng stdClassMng { get; set; }

        public int[] ids { get; set; }

    }
}