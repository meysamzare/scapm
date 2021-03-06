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
    public class ExamController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public ExamController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ExamAddParam examAddParam)
        {
            try
            {

                examAddParam.exam.Date = examAddParam.exam.Date.AddDays(1);
                if (examAddParam.exam.ResultDate.HasValue)
                {
                    examAddParam.exam.ResultDate = examAddParam.exam.ResultDate.Value.AddDays(1);
                }

                TimeSpan resTime;
                if (!string.IsNullOrEmpty(examAddParam.resultTime))
                {


                    if (!TimeSpan.TryParse(examAddParam.resultTime.ToString(), out resTime))
                    {
                        return this.UnSuccessFunction("مقدار زمان اعلام نتایج را وارد کنید");
                    }

                }
                else
                {
                    resTime = TimeSpan.Parse("00:00");
                }

                if (examAddParam.exam.ResultDate.HasValue)
                {
                    examAddParam.exam.ResultDate = examAddParam.exam.ResultDate.Value.Date + resTime;
                }

                var exam = examAddParam.exam;


                await db.Exams.AddAsync(exam);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] ExamAddParam examAddParam)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                if (examAddParam.exam.Id == examAddParam.exam.ParentId)
                {
                    return this.UnSuccessFunction("این آزمون نمیتواند انتخاب شود C1");
                }

                examAddParam.exam.Date.AddDays(1);
                if (examAddParam.exam.ResultDate.HasValue)
                {
                    examAddParam.exam.ResultDate.Value.AddDays(1);
                }

                TimeSpan resTime;
                if (!TimeSpan.TryParse(examAddParam.resultTime.ToString(), out resTime))
                {
                    return this.UnSuccessFunction("مقدار زمان اعلام نتایج را وارد کنید");
                }

                if (examAddParam.exam.ResultDate.HasValue)
                {
                    examAddParam.exam.ResultDate = examAddParam.exam.ResultDate.Value.Date + resTime;
                }

                var exam = await db.Exams.Include(c => c.Children).Include(c => c.Parent).SingleAsync(c => c.Id == examAddParam.exam.Id);

                if (await isInChildExam(exam, examAddParam.exam.ParentId))
                {
                    return this.UnSuccessFunction("این آزمون نمیتواند انتخاب شود C2");
                }

                if (exam.YeareducationId != nowYeareducationId)
                {
                    return this.UnSuccessFunction("این آزمون نمی تواند ویرایش شود");
                }

                var dateBe = exam.Date;
                var RedateBe = exam.ResultDate;

                exam.Date = examAddParam.exam.Date;
                exam.ResultDate = examAddParam.exam.ResultDate;

                if (dateBe != examAddParam.exam.Date)
                {
                    exam.Date = exam.Date.AddDays(1);
                }

                if (examAddParam.exam.ResultDate.HasValue)
                {

                    if (RedateBe != examAddParam.exam.ResultDate.Value)
                    {
                        exam.ResultDate = exam.ResultDate.Value.AddDays(1);
                    }
                }


                exam.Name = examAddParam.exam.Name;
                exam.NumberQ = examAddParam.exam.NumberQ;
                exam.Source = examAddParam.exam.Source;
                exam.TopScore = examAddParam.exam.TopScore;
                exam.ExamTypeId = examAddParam.exam.ExamTypeId;
                exam.GradeId = examAddParam.exam.GradeId;
                exam.ClassId = examAddParam.exam.ClassId;
                exam.TeacherId = examAddParam.exam.TeacherId;
                exam.Order = examAddParam.exam.Order;
                exam.YeareducationId = examAddParam.exam.YeareducationId;
                exam.CourseId = examAddParam.exam.CourseId;
                exam.Time = examAddParam.exam.Time;
                exam.Result = examAddParam.exam.Result;
                exam.ParentId = examAddParam.exam.ParentId;
                exam.IsCancelled = examAddParam.exam.IsCancelled;
                exam.CancellReason = examAddParam.exam.CancellReason;
                exam.WorkbookId = examAddParam.exam.WorkbookId;

                if (await db.ClassBooks.AnyAsync(c => c.ExamId == exam.Id))
                {
                    var classBooks = db.ClassBooks.Where(c => c.ExamId == exam.Id);

                    await classBooks.ForEachAsync(c =>
                    {
                        c.ExamName = exam.Name;
                    });
                }

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }



        public bool isinchild { get; set; }

        public async Task<bool> isInChildExam(Exam exam, int? selectedId)
        {
            foreach (var i in exam.Children)
            {
                if (i.Id == selectedId)
                {
                    isinchild = true;
                }
                if (i.Children != null)
                {
                    if (i.Children.Any())
                    {
                        await isInChildExam(i, selectedId);
                    }
                }
            }

            return isinchild;
        }



        [HttpPost]
        public async Task<IActionResult> Get([FromBody] getExamParam getparam)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var getparams = getparam.getparam;

                getparams.pageIndex += 1;

                int count;

                var query = getparams.q;

                var ex = db.Exams.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    ex = ex.Where(c => c.Name.Contains(query) || c.Order.ToString().Contains(query) ||
                                    c.ExamType.Name.Contains(query) || c.Class.Name.Contains(query) || c.Grade.Name.Contains(query) ||
                                    c.Teacher.Name.Contains(query) || c.Yeareducation.Name.Contains(query) || c.Course.Name.Contains(query) ||
                                    c.parentName.Contains(query));

                }

                if (getparam.selectedGrade.HasValue)
                {
                    ex = ex.Where(c => c.GradeId == getparam.selectedGrade.Value);
                }

                if (getparam.selectedClass.HasValue)
                {
                    ex = ex.Where(c => c.ClassId == getparam.selectedClass.Value);
                }

                if (getparam.selectedTeacher.HasValue)
                {
                    ex = ex.Where(c => c.TeacherId == getparam.selectedTeacher.Value);
                }

                if (getparam.selectedWorkbook.HasValue)
                {
                    ex = ex.Where(c => c.WorkbookId == getparam.selectedWorkbook.Value);
                }

                if (getparam.selectedExamType.HasValue)
                {
                    ex = ex.Where(c => c.ExamTypeId == getparam.selectedExamType.Value);
                }

                

                if (getparam.type == "passed")
                {
                    ex = ex.Where(c => c.Date <= DateTime.Now);
                }

                if (getparam.type == "upcomming")
                {
                    ex = ex.Where(c => c.Date >= DateTime.Now);
                }

                if (getparam.type == "waitingForResult")
                {
                    ex = ex.Where(c => c.Result == false);
                }

                if (getparam.type == "cancelled")
                {
                    ex = ex.Where(c => c.IsCancelled == true);
                }

                count = ex.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        ex = ex.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("result"))
                    {
                        ex = ex.OrderBy(c => c.Result);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        ex = ex.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        ex = ex.OrderBy(c => c.Date);
                    }
                    if (getparams.sort.Equals("parentName"))
                    {
                        ex = ex.OrderBy(c => c.parentName);
                    }
                    if (getparams.sort.Equals("examTypeName"))
                    {
                        ex = ex.OrderBy(c => c.ExamType.Name);
                    }
                    if (getparams.sort.Equals("className"))
                    {
                        ex = ex.OrderBy(c => c.Class.Name);
                    }
                    if (getparams.sort.Equals("courseName"))
                    {
                        ex = ex.OrderBy(c => c.Course.Name);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        ex = ex.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("result"))
                    {
                        ex = ex.OrderByDescending(c => c.Result);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        ex = ex.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        ex = ex.OrderByDescending(c => c.Date);
                    }
                    if (getparams.sort.Equals("parentName"))
                    {
                        ex = ex.OrderByDescending(c => c.parentName);
                    }
                    if (getparams.sort.Equals("examTypeName"))
                    {
                        ex = ex.OrderByDescending(c => c.ExamType.Name);
                    }
                    if (getparams.sort.Equals("className"))
                    {
                        ex = ex.OrderByDescending(c => c.Class.Name);
                    }
                    if (getparams.sort.Equals("courseName"))
                    {
                        ex = ex.OrderByDescending(c => c.Course.Name);
                    }
                }
                else
                {
                    ex = ex.OrderByDescending(c => c.Date);
                }

                ex = ex.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                ex = ex.Take(getparams.pageSize);

                var q = await ex.Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    dateString = c.dateString,
                    examTypeName = c.ExamType.Name,
                    gradeName = c.Grade.Name,
                    className = c.Class.Name,
                    courseName = c.Course.Name,
                    haveChildren = c.Children.Any(),
                    Result = c.Result,
                    YeareducationId = c.YeareducationId
                })
                .Where(c => c.YeareducationId == nowYeareducationId)
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
        public async Task<IActionResult> getExam([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var ex = await db.Exams
                        .Include(c => c.ExamScores)
                        .Include(c => c.Course)
                        .Include(c => c.Teacher)
                        .Include(c => c.Grade)
                        .Include(c => c.Class)
                        .Include(c => c.ExamType)
                    .SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, ex);
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
        public async Task<IActionResult> getAll()
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var ex = await db.Exams
                .Where(c => c.IsCancelled == false)
                .Select(c => new
                {
                    id = c.Id,
                    name = c.Name,
                    CourseId = c.CourseId,
                    YeareducationId = c.YeareducationId,
                    ClassId = c.ClassId,
                    GradeId = c.GradeId,
                    WorkbookId = c.WorkbookId
                })
                .Where(c => c.YeareducationId == nowYeareducationId)
                .ToListAsync();

                return this.DataFunction(true, ex);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByYGC([FromBody] getByYGC getexam)
        {
            try
            {

                var ex = await db.Exams
                    .Where(c => c.YeareducationId == getexam.yeareducationId
                            && c.GradeId == getexam.gradeId
                            && c.ClassId == getexam.classId
                            && c.IsCancelled == false)
                .ToListAsync();

                return this.DataFunction(true, ex);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByGrade([FromBody] int gradeId)
        {
            try
            {
                var ex = await db.Exams
                    .Where(c => c.GradeId == gradeId && c.IsCancelled == false)
                        .Select(c => new
                        {
                            Id = c.Id,
                            Name = c.Name,
                            dateString = c.Date.ToPersianDate(),
                            courseName = c.Course.Name,
                            avgInExam = c.getMin_Max_Avg_InExam(c.ExamScores, ScoreType.Avg),
                            minInExam = c.getMin_Max_Avg_InExam(c.ExamScores, ScoreType.Min),
                            maxInExam = c.getMin_Max_Avg_InExam(c.ExamScores, ScoreType.Max),
                            CourseId = c.CourseId,
                            ExamTypeId = c.ExamTypeId,
                            Date = c.Date,
                            canShowByWorkBook = c.canShowByWorkBook(c.Workbook),
                            WorkbookId = c.WorkbookId,
                            TopScore = c.TopScore
                        })
                    .OrderByDescending(c => c.Date)
                .ToListAsync();

                return this.DataFunction(true, ex);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByClass([FromBody] int classId)
        {
            try
            {
                var exams = await db.Exams
                    .Where(c => c.ClassId == classId && c.IsCancelled == false)
                        .Select(c => new
                        {
                            Id = c.Id,
                            Name = c.Name,
                            dateString = c.Date.ToPersianDate(),
                            courseName = c.Course.Name,
                            avgInExam = c.getMin_Max_Avg_InExam(c.ExamScores, ScoreType.Avg),
                            minInExam = c.getMin_Max_Avg_InExam(c.ExamScores, ScoreType.Min),
                            maxInExam = c.getMin_Max_Avg_InExam(c.ExamScores, ScoreType.Max),
                            NumberQ = c.NumberQ,
                            Source = c.Source,
                            TopScore = c.TopScore,
                            ExamTypeId = c.ExamTypeId,
                            GradeId = c.GradeId,
                            ClassId = c.ClassId,
                            TeacherId = c.TeacherId,
                            Order = c.Order,
                            YeareducationId = c.YeareducationId,
                            CourseId = c.CourseId,
                            Time = c.Time,
                            Result = c.Result,
                            ResultDate = c.ResultDate,
                            ParentId = c.ParentId,
                            ExamType = c.ExamType,
                            Date = c.Date
                        })
                    .OrderByDescending(c => c.Date)
                .ToListAsync();

                return this.DataFunction(true, exams);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByTeacher([FromBody] int teacherId)
        {
            try
            {
                var exams = await db.Exams.Where(c => c.TeacherId == teacherId && c.IsCancelled == false).ToListAsync();

                return this.DataFunction(true, exams);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getExamAnalizeData([FromBody] int examId)
        {
            try
            {
                var exam = await db.Exams.Where(c => c.Id == examId)
                    .Include(c => c.Class)
                    .Include(c => c.Grade)
                    .Include(c => c.ExamScores)
                        .ThenInclude(c => c.Student)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        gradeName = c.Grade.Name,
                        className = c.Class.Name,
                        ExamScores = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer),
                        students = c.ExamScores.Select(l => l.Student).ToList(),
                        TopScore = c.TopScore,
                        Source = c.Source,
                        avgInExam = c.getMin_Max_Avg_InExam(c.ExamScores.ToList(), ScoreType.Avg),
                        minInExam = c.getMin_Max_Avg_InExam(c.ExamScores.ToList(), ScoreType.Min),
                        maxInExam = c.getMin_Max_Avg_InExam(c.ExamScores.ToList(), ScoreType.Max),
                        ClassId = c.ClassId,
                        haveResult = c.Result
                    })
                .FirstOrDefaultAsync();

                if (!exam.haveResult)
                {
                    return this.UnSuccessFunction("این آزمون اعلام نتایج نشده است");
                }

                var studentTypes = await db.StudentTypes
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        students = c.StdClassMngs.Where(h => h.ClassId == exam.ClassId).Select(l => l.Student)
                    })
                .ToListAsync();

                return this.DataFunction(true, new
                {
                    students = exam.students,
                    exam = exam,
                    studentTypes = studentTypes
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getExamBetweenDates([FromBody] exambetweenDatesParam exambetweendate)
        {
            try
            {
                exambetweendate.dateStart = exambetweendate.dateStart.AddDays(1);
                exambetweendate.dateEnd = exambetweendate.dateEnd.AddDays(1);


                var examByTypeAndDate = await db.Exams.Where(c => c.ExamTypeId == exambetweendate.examTypeId
                            && c.Date >= exambetweendate.dateStart && c.Date <= exambetweendate.dateEnd && c.IsCancelled == false)
                .Select(c => c.Id)
                .ToListAsync();

                return this.DataFunction(true, examByTypeAndDate);
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

                        var ex = await db.Exams
                            .SingleAsync(c => c.Id == id);

                        if (ex == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (ex.haveChildren)
                        {
                            return this.UnSuccessFunction("نمیتوان آزمون " + ex.Name + " را حذف کرد", "error");
                        }

                        db.Exams.Remove(ex);
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


        public IEnumerable<Exam> exams
        {
            get
            {
                return db.Exams.Where(c => c.YeareducationId == this.getActiveYeareducationIdNonAsync()).Include(c => c.Parent).Include(c => c.Children).ToList();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetTreeRoot()
        {
            List<JsTreeModel> items = GetParentTree();

            return Json(items);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public JsonResult GetTreeChildren(string id)
        {
            List<JsTreeModel> items = GetTree(id);

            return Json(items);
        }

        List<JsTreeModel> GetParentTree()
        {
            var items = new List<JsTreeModel>();

            foreach (var i in exams.Where(c => c.ParentId == null))
            {
                items.Add(new JsTreeModel
                {
                    id = i.Id.ToString(),
                    text = i.Name,
                    parent = "#",
                    children = i.Children.Any()
                });
            }

            return items;
        }

        List<JsTreeModel> GetTree(string id)
        {
            var isInt = int.TryParse(id, out int idd);
            var items = new List<JsTreeModel>();

            foreach (var i in exams.Where(c => c.ParentId == idd))
            {
                items.Add(new JsTreeModel
                {
                    id = i.Id.ToString(),
                    text = i.Name,
                    parent = i.Parent.Id.ToString(),
                    children = i.Children.Any()
                });
            }

            return items;
        }


        [HttpPost]
        public async Task<IActionResult> getUpComingExamInWeek([FromBody] int classId)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var exams = await db.Exams
                .Where(c => c.ClassId == classId && c.Date >= DateTime.Now && c.Date <= DateTime.Now.AddDays(7))
                    .OrderBy(c => c.Date)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Source = c.Source,
                        Time = c.Time,
                        dateString = c.Date.ToPersianDateWithDayString(),
                        courseName = c.Course.Name,
                        teacherName = c.Teacher.Name,
                        TopScore = c.TopScore,
                        examTypeName = c.ExamType.Name,
                        YeareducationId = c.YeareducationId
                    })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .ToListAsync();


                return this.DataFunction(true, exams);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getUpComingExamInWeekByTeacher([FromBody] int teacherId)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var exams = await db.Exams
                .Where(c => c.TeacherId == teacherId && c.Date >= DateTime.Now && c.Date <= DateTime.Now.AddDays(7))
                    .OrderBy(c => c.Date)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Source = c.Source,
                        Time = c.Time,
                        dateString = c.Date.ToPersianDateWithDayString(),
                        courseName = c.Course.Name,
                        teacherName = c.Teacher.Name,
                        TopScore = c.TopScore,
                        examTypeName = c.ExamType.Name,
                        YeareducationId = c.YeareducationId
                    })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .ToListAsync();


                return this.DataFunction(true, exams);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }
    }

    public class getExamParam
    {
        public getparams getparam { get; set; }

        public int? selectedGrade { get; set; }
        public int? selectedClass { get; set; }
        public int? selectedTeacher { get; set; }
        public int? selectedWorkbook { get; set; }
        public int? selectedExamType { get; set; }

        public string type { get; set; }
    }

    public class exambetweenDatesParam
    {
        public DateTime dateStart { get; set; }

        public DateTime dateEnd { get; set; }

        public int examTypeId { get; set; }
    }

    public class ExamAddParam
    {
        public Exam exam { get; set; }

        public string resultTime { get; set; }
    }
}