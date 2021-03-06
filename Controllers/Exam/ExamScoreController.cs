using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ExamScoreController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public ExamScoreController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ExamScore exscamscore)
        {
            try
            {

                if (db.ExamScores.Any(c => c.StudentId == exscamscore.StudentId && c.ExamId == exscamscore.ExamId))
                {
                    return this.UnSuccessFunction("نمره این دانش آموز برای این آزمون قبلا ثبت شده است");
                }

                await db.ExamScores.AddAsync(exscamscore);

                var exam = await db.Exams.FirstOrDefaultAsync(c => c.Id == exscamscore.ExamId);

                exam.ResultDate = DateTime.Now;
                exam.Result = true;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdate([FromBody] ExamScore exscamscore)
        {
            try
            {
                if (db.ExamScores.Any(c => c.StudentId == exscamscore.StudentId && c.ExamId == exscamscore.ExamId))
                {
                    var nowYeareducationId = await this.getActiveYeareducationId();

                    var exscs = await db.ExamScores.Include(c => c.Exam).SingleAsync(c => c.StudentId == exscamscore.StudentId && c.ExamId == exscamscore.ExamId);

                    if (exscs.Exam.YeareducationId != nowYeareducationId)
                    {
                        return this.UnSuccessFunction("این نمره نمی تواند ویرایش شود");
                    }

                    exscs.ExamId = exscamscore.ExamId;
                    exscs.StudentId = exscamscore.StudentId;
                    exscs.Score = exscamscore.Score;
                    exscs.TopScore = exscamscore.TopScore;
                    exscs.NumberQ = exscamscore.NumberQ;
                    exscs.TrueAnswer = exscamscore.TrueAnswer;
                    exscs.FalseAnswer = exscamscore.FalseAnswer;
                    exscs.BlankAnswer = exscamscore.BlankAnswer;
                    exscs.State = exscamscore.State;

                    if (await db.ClassBooks.AnyAsync(c => c.ExamId == exscamscore.ExamId && c.StudentId == exscamscore.StudentId))
                    {
                        var classBook = await db.ClassBooks.FirstOrDefaultAsync(c => c.ExamId == exscamscore.ExamId && c.StudentId == exscamscore.StudentId);

                        classBook.Value = exscamscore.Score.ToString();
                    }

                    await db.SaveChangesAsync();

                    return this.SuccessFunction();
                }

                await db.ExamScores.AddAsync(exscamscore);

                var exam = await db.Exams.FirstOrDefaultAsync(c => c.Id == exscamscore.ExamId);

                exam.ResultDate = DateTime.Now;
                exam.Result = true;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] ExamScore exscamscore)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var exscs = await db.ExamScores.Include(c => c.Exam).SingleAsync(c => c.Id == exscamscore.Id);

                if (exscs.Exam.YeareducationId != nowYeareducationId)
                {
                    return this.UnSuccessFunction("این نمره نمی تواند ویرایش شود");
                }

                exscs.ExamId = exscamscore.ExamId;
                exscs.StudentId = exscamscore.StudentId;
                exscs.Score = exscamscore.Score;
                exscs.TopScore = exscamscore.TopScore;
                exscs.NumberQ = exscamscore.NumberQ;
                exscs.TrueAnswer = exscamscore.TrueAnswer;
                exscs.FalseAnswer = exscamscore.FalseAnswer;
                exscs.BlankAnswer = exscamscore.BlankAnswer;
                exscs.State = exscamscore.State;

                if (await db.ClassBooks.AnyAsync(c => c.ExamId == exscamscore.ExamId && c.StudentId == exscamscore.StudentId))
                {
                    var classBook = await db.ClassBooks.FirstOrDefaultAsync(c => c.ExamId == exscamscore.ExamId && c.StudentId == exscamscore.StudentId);

                    classBook.Value = exscamscore.Score.ToString();
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
        public async Task<IActionResult> Get([FromBody] getExamscoreparams getparams)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                getparams.getparam.pageIndex += 1;

                int count;


                var query = getparams.getparam.q;

                var exsc = db.ExamScores
                    .Include(c => c.Student)
                    .Include(c => c.Exam)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    exsc = exsc.Where(c => c.Student.Name.Contains(query) || c.Exam.Name.Contains(query) ||
                                        c.Student.LastName.Contains(query) || c.Score.ToString().Contains(query));

                }

                if (getparams.examId.HasValue && getparams.examId.Value != 0)
                {
                    exsc = exsc.Where(c => c.ExamId == getparams.examId.Value);
                }

                if (getparams.courseId.HasValue && getparams.courseId.Value != 0)
                {
                    exsc = exsc.Where(c => c.Exam.CourseId == getparams.courseId.Value);
                }

                if (!string.IsNullOrWhiteSpace(getparams.name))
                {
                    exsc = exsc.Where(c => c.Student.Name.Contains(getparams.name) || c.Student.LastName.Contains(getparams.name));
                }

                count = exsc.Count();


                if (getparams.getparam.direction.Equals("asc"))
                {
                    if (getparams.getparam.sort.Equals("id"))
                    {
                        exsc = exsc.OrderBy(c => c.Id);
                    }
                    if (getparams.getparam.sort.Equals("studentName"))
                    {
                        exsc = exsc.OrderBy(c => c.Student.LastName).ThenBy(c => c.Student.Name);
                    }
                    if (getparams.getparam.sort.Equals("examName"))
                    {
                        exsc = exsc.OrderBy(c => c.Exam.Name);
                    }
                    if (getparams.getparam.sort.Equals("scoreString"))
                    {
                        exsc = exsc.OrderBy(c => c.Score);
                    }
                    if (getparams.getparam.sort.Equals("topScore"))
                    {
                        exsc = exsc.OrderBy(c => c.Exam.TopScore);
                    }
                    if (getparams.getparam.sort.Equals("examDate"))
                    {
                        exsc = exsc.OrderBy(c => c.Exam.Date);
                    }
                }
                else if (getparams.getparam.direction.Equals("desc"))
                {
                    if (getparams.getparam.sort.Equals("id"))
                    {
                        exsc = exsc.OrderByDescending(c => c.Id);
                    }
                    if (getparams.getparam.sort.Equals("studentName"))
                    {
                        exsc = exsc.OrderByDescending(c => c.Student.LastName).ThenByDescending(c => c.Student.Name);
                    }
                    if (getparams.getparam.sort.Equals("examName"))
                    {
                        exsc = exsc.OrderByDescending(c => c.Exam.Name);
                    }
                    if (getparams.getparam.sort.Equals("scoreString"))
                    {
                        exsc = exsc.OrderByDescending(c => c.Score);
                    }
                    if (getparams.getparam.sort.Equals("topScore"))
                    {
                        exsc = exsc.OrderByDescending(c => c.Exam.TopScore);
                    }
                    if (getparams.getparam.sort.Equals("examDate"))
                    {
                        exsc = exsc.OrderByDescending(c => c.Exam.Date);
                    }
                }
                else
                {
                    exsc = exsc.OrderBy(c => c.Id);
                }

                exsc = exsc.Skip((getparams.getparam.pageIndex - 1) * getparams.getparam.pageSize);
                exsc = exsc.Take(getparams.getparam.pageSize);

                var q = await exsc
                    .Select(c => new
                    {
                        Id = c.Id,
                        studentName = c.Student.Name + " " + c.Student.LastName,
                        examName = c.Exam.Name,
                        State = c.State,
                        scoreString = c.scoreString,
                        YeareducationId = c.Exam.YeareducationId,
                        topScore = c.Exam.TopScore,
                        examDate = c.Exam.Date.ToPersianDate(),
                        examId = c.ExamId
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
        public async Task<IActionResult> getExamScore([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var exsc = await db.ExamScores.Include(c => c.Student).SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, exsc);
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

                var ex = await db.ExamScores.Include(c => c.Student)
                    .Select(c => new { id = c.Id, name = c.studentName, YeareducationId = c.Exam.YeareducationId })
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
        public async Task<IActionResult> getAllByExamId([FromBody] int examId)
        {
            try
            {
                var ex = await db.ExamScores.Where(c => c.ExamId == examId).ToListAsync();

                return this.DataFunction(true, ex);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByExamYGC([FromBody] getAllByExamYGC getexamscore)
        {
            try
            {

                var exsc = await db.ExamScores
                    .Include(c => c.Exam)
                        .Where(c => c.Exam.YeareducationId == getexamscore.YGC.yeareducationId && c.Exam.GradeId == getexamscore.YGC.gradeId && c.Exam.ClassId == getexamscore.YGC.classId)
                        .Where(c => c.StudentId == getexamscore.studentId)
                .ToListAsync();

                return this.DataFunction(true, exsc);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getAllByGrade_Student([FromBody] getExamScoreByGrade_Student getexamscore)
        {
            try
            {
                var examscore = await db.ExamScores
                    .Select(c => new
                    {
                        Id = c.Id,
                        Rating = c.getRating(c.Exam.ExamScores.ToList(), c.Score),
                        Score = c.Score,
                        StudentId = c.StudentId,
                        ExamId = c.ExamId,
                        State = c.State,
                        examName = c.Exam.Name,
                        studentName = c.Student.Name + " " + c.Student.LastName,
                        gradeId = c.Exam.GradeId,
                        TopScore = c.Exam.TopScore
                    })
                    .Where(c => c.StudentId == getexamscore.studentId)
                        .Where(c => c.gradeId == getexamscore.gradeId)
                .ToListAsync();

                return this.DataFunction(true, examscore);
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

                        var ex = await db.ExamScores
                            .SingleAsync(c => c.Id == id);

                        if (ex == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.ExamScores.Remove(ex);
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
        public async Task<IActionResult> getWorkbookDetail([FromBody] getScoreDetailByWorkbookParam param)
        {
            try
            {
                IEnumerable<StudentCourseRatingParams> students;

                var courses = await db.Courses.Where(c => c.GradeId == param.gradeId)
                    .Select(c => new
                    {
                        Id = c.Id,
                        CourseMix = c.CourseMix,
                        Name = c.Name
                    })
                .ToListAsync();

                if (param.classId.HasValue)
                {
                    students = await db.StdClassMngs
                        .Where(c => c.ClassId == param.classId.Value && c.IsActive == true)
                            .Select(c => c.Student)
                                .Distinct()
                            .Select(c => new StudentCourseRatingParams
                            {
                                Id = c.Id,
                                Name = c.Name + ' ' + c.LastName,
                                studentScores = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId),
                                exams = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId).Select(l => l.Exam).Where(l => l.WorkbookId == param.workbookId)
                            })
                    .ToListAsync();
                }
                else
                {
                    students = await db.StdClassMngs
                        .Where(c => c.GradeId == param.gradeId && c.IsActive == true)
                            .Select(c => c.Student)
                                .Distinct()
                            .Select(c => new StudentCourseRatingParams
                            {
                                Id = c.Id,
                                Name = c.Name + ' ' + c.LastName,
                                studentScores = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId),
                                exams = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId).Select(l => l.Exam).Where(l => l.WorkbookId == param.workbookId)
                            })
                    .ToListAsync();
                }

                var results = new List<WorkbookDetailResult>();

                foreach (var std in students)
                {
                    var avgForCourses = new List<double>();

                    foreach (var course in courses)
                    {
                        var count = 0;
                        var sumScore = 0.0;

                        var exams = std.exams.Where(c => c.CourseId == course.Id);

                        foreach (var exam in exams)
                        {
                            if (std.studentScores.Any(c => c.ExamId == exam.Id))
                            {
                                var examScore = std.studentScores.FirstOrDefault(c => c.ExamId == exam.Id);

                                count += course.CourseMix;
                                sumScore += getJustifiedScore(examScore.Score, exam.TopScore) * course.CourseMix;
                            }
                        }

                        if (count != 0)
                        {
                            avgForCourses.Add(sumScore / count);
                        }
                        else
                        {
                            avgForCourses.Add(0.0);
                        }
                    }

                    var totalAvg = avgForCourses.Where(c => c != 0.0).Average();

                    results.Add(new WorkbookDetailResult
                    {
                        courseAvgs = avgForCourses,
                        totalAvg = totalAvg,
                        name = std.Name
                    });
                }

                var uniqTotalAvgs = results.Select(c => c.totalAvg).Distinct().OrderByDescending(c => c).ToList();

                foreach (var result in results)
                {
                    result.rate = uniqTotalAvgs.FindIndex(c => c == result.totalAvg) + 1;
                }

                return this.DataFunction(true, new
                {
                    courses = courses,
                    results = results.OrderByDescending(c => c.totalAvg)
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getTotalAverageByStudentGrade([FromBody] getTotalAverageByStudentGradeParam param)
        {
            try
            {
                var headers = new List<string>();
                var averagesForCourses = new List<double>();

                var courseRatingsInClass = new List<int>();
                var courseRatingsInGrade = new List<int>();

                var totalAvg = 0.0;
                var ratingOfTotalAvgClass = 0.0;
                var ratingOfTotalAvgGrade = 0.0;
                var avgOfTotalAvrageGrade = 0.0;
                var topTotalAvrageGrade = 0.0;


                var studentsIdInGrade = await db.StdClassMngs
                    .Where(c => c.GradeId == param.gradeId && c.IsActive == true)
                        .Select(c => c.Student.Id)
                            .Distinct()
                .ToListAsync();

                var stds = db.Students.Where(c => studentsIdInGrade.Contains(c.Id))
                .Select(c => new
                {
                    Id = c.Id,
                    examScores = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId && l.Exam.WorkbookId == param.workbookId)
                                .Select(f => new ExamScoreForCourseAvg { Score = f.Score, TopScore = f.Exam.TopScore, CourseId = f.Exam.CourseId }).ToList(),
                    courses = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId && l.Exam.WorkbookId == param.workbookId).Select(l => l.Exam.Course).Distinct().ToList()
                })
                .Select(c => new
                {
                    Id = c.Id,
                    courseAvgs = c.courses.Select(l => l.getCourseAverageForStudent(c.examScores.Where(f => f.CourseId == l.Id).ToList(), l)).OrderBy(l => l.CourseName).ToList()
                })
                .Select(c => new
                {
                    Id = c.Id,
                    courseAvgs = c.courseAvgs,
                    totalAvg = getAverageOfCourseAvg(c.courseAvgs)
                }).ToList();

                var student = stds.Find(c => c.Id == param.studentId);


                var studentsInGrade = stds.Where(c => studentsIdInGrade.Contains(c.Id)).ToList();



                var studentsIdInClass = await db.StdClassMngs
                    .Where(c => c.ClassId == param.classId && c.IsActive == true)
                        .Select(c => c.Student.Id)
                            .Distinct()
                .ToListAsync();
                var studentsInClass = stds.Where(c => studentsIdInClass.Contains(c.Id)).ToList();



                headers = student.courseAvgs.Select(c => c.CourseName).ToList();
                averagesForCourses = student.courseAvgs.Select(c => c.Average).ToList();
                totalAvg = student.totalAvg;
                topTotalAvrageGrade = studentsInGrade.Select(c => c.totalAvg).OrderByDescending(c => c).First();
                avgOfTotalAvrageGrade = studentsInGrade.Average(c => c.totalAvg);
                ratingOfTotalAvgGrade = studentsInGrade.Select(c => c.totalAvg).OrderByDescending(c => c).Distinct().ToList().FindIndex(c => c == student.totalAvg) + 1;
                ratingOfTotalAvgClass = studentsInClass.Select(c => c.totalAvg).OrderByDescending(c => c).Distinct().ToList().FindIndex(c => c == student.totalAvg) + 1;


                headers.ForEach(courseName =>
                {
                    var stdCourseAvg = student.courseAvgs.FirstOrDefault(l => l.CourseName == courseName).Average;


                    var rateInClass = 0;

                    if (studentsInClass.SelectMany(c => c.courseAvgs).Any(c => c.CourseName.Contains(courseName)))
                    {

                        rateInClass = studentsInClass.SelectMany(c => c.courseAvgs).Where(c => c.CourseName.Contains(courseName))
                             .Select(c => c.Average)
                                 .OrderByDescending(c => c).Distinct().ToList().FindIndex(c => c == stdCourseAvg) + 1;
                    }

                    courseRatingsInClass.Add(rateInClass);


                    var rateInGrade = 0;

                    if (studentsInGrade.SelectMany(c => c.courseAvgs).Any(c => c.CourseName.Contains(courseName)))
                    {

                        rateInGrade = studentsInGrade.SelectMany(c => c.courseAvgs).Where(c => c.CourseName.Contains(courseName))
                             .Select(c => c.Average)
                                 .OrderByDescending(c => c).Distinct().ToList().FindIndex(c => c == stdCourseAvg) + 1;
                    }

                    courseRatingsInGrade.Add(rateInGrade);

                    // courseRatingsInClass.Add(studentsInClass
                    //             .Select(c => c.courseAvgs.FirstOrDefault(l => l.CourseName == courseName))
                    //             .Select(c => c.Average)
                    //                 .OrderByDescending(c => c).Distinct().ToList().FindIndex(c => c == stdCourseAvg) + 1);

                    // courseRatingsInGrade.Add(studentsInGrade
                    //             .Select(c => c.courseAvgs.FirstOrDefault(l => l.CourseName == courseName))
                    //             .Select(c => c.Average)
                    //                 .OrderByDescending(c => c).Distinct().ToList().FindIndex(c => c == stdCourseAvg) + 1);
                });

                return this.DataFunction(true, new
                {
                    headers = headers,
                    courseAvgs = averagesForCourses,
                    ratingsInClass = courseRatingsInClass,
                    ratingsInGrade = courseRatingsInGrade,
                    totalAvg = totalAvg,
                    ratingOfTotalAvgClass = ratingOfTotalAvgClass,
                    ratingOfTotalAvgGrade = ratingOfTotalAvgGrade,
                    avgOfTotalAvrageGrade = avgOfTotalAvrageGrade,
                    topTotalAvrageGrade = topTotalAvrageGrade
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        private double getJustifiedScore(double score, int topScore)
        {
            double mix = (double)20 / topScore;

            return (score * mix);
        }

        private double getAverageOfCourseAvg(IEnumerable<CourseAvgReturn> courseAvgs)
        {
            if (courseAvgs.Any())
            {
                return courseAvgs.Select(c => c.Average).Average();
            }

            return 0.0;
        }


        [HttpPost]
        public async Task<IActionResult> getEnteringScoreSheet([FromBody] int examId)
        {
            try
            {
                var exam = await db.Exams.Include(c => c.ExamScores).FirstOrDefaultAsync(c => c.Id == examId);

                var students = await db.StdClassMngs.Where(c => c.GradeId == exam.GradeId && c.ClassId == exam.ClassId && c.YeareducationId == exam.YeareducationId)
                        .Select(c => c.Student)
                    .OrderBy(c => c.LastName).ThenBy(c => c.Name)
                .ToListAsync();

                var user = await db.Users.Include(c => c.Role).FirstOrDefaultAsync(c => c.Username == User.Identity.Name);
                var canUserEditExamScore = user.Role.Edit_ExamScore;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.DefaultColWidth = 20;
                    worksheet.DefaultRowHeight = 20;
                    worksheet.View.RightToLeft = true;
                    worksheet.Cells.Style.Font.Name = "B Yekan";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;




                    worksheet.Cells[1, 1].Value = exam.Id;
                    worksheet.Cells[1, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                    worksheet.Cells[1, 1].AddComment("توجه \n به هیچ عنوان مقدار این فیلد را عوض نکنید", "Mabna");

                    worksheet.Cells[1, 2].Value = "ملاک آزمون: " + exam.TopScore;
                    worksheet.Cells[1, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                    worksheet.Cells[2, 1].Value = "نام و نام خانوادگی";
                    worksheet.Cells[2, 2].Value = "کد دانش آموز";

                    worksheet.Cells[2, 3].Value = "نمره";
                    worksheet.Cells[2, 3].AddComment("وضعیت های مختلف حضور دانش آموز \n"
                    + "غائب و موجه: 'T' \n"
                    + "غائب و غیر موجه: 'F' \n"
                    + "حاضر: با وارد کردن عدد، درصورتی که کمتر از ملاک آزمون باشد، به عنوان نمره دانش آموز با وضعیت حاضر ثبت می شود", "Mabna");

                    using (var range = worksheet.Cells[2, 1, 2, 3])
                    {
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.OrangeRed);
                    }


                    foreach (var (student, index) in students.WithIndex())
                    {
                        worksheet.Cells[2 + (index + 1), 1].Value = student.fullName;
                        worksheet.Cells[2 + (index + 1), 2].Value = student.Code;
                        // if (exam.ExamScores.Any(c => c.StudentId == student.Id))
                        // {
                        //     if (canUserEditExamScore)
                        //     {

                        //     }
                        //     else
                        //     {
                        //         worksheet.Cells[2 + (index + 1), 3].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        //     }
                        // }
                        worksheet.Cells[2 + (index + 1), 3].Value = "";
                    }

                    var fileContents = package.GetAsByteArray();

                    var guid = System.Guid.NewGuid().ToString();

                    var name = exam.Id + "--" + User.Identity.Name + ".xlsx";

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, name);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    System.IO.File.WriteAllBytes(path, fileContents);

                    var filepath = Path.Combine("/UploadFiles", guid, name);

                    await db.SaveChangesAsync();

                    return this.SuccessFunction(redirect: filepath);
                }
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> setScoreFromSheet([FromBody] ImportDataFromExcelParam importDataFromExcelParam)
        {
            try
            {
                var guid = System.Guid.NewGuid().ToString();

                var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, importDataFromExcelParam.fileName);
                Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                byte[] bytes = Convert.FromBase64String(importDataFromExcelParam.fileValue);
                System.IO.File.WriteAllBytes(path, bytes);

                var filepath = Path.Combine("/UploadFiles", guid, importDataFromExcelParam.fileName);

                FileInfo file = new FileInfo(path);

                var messages = new List<string>();

                var user = await db.Users.Include(c => c.Role).FirstOrDefaultAsync(c => c.Username == User.Identity.Name);
                var canUserEditExamScore = user.Role.Edit_ExamScore;

                var examScoresToAdd = new List<ExamScore>();

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                    int totalRows = workSheet.Dimension.Rows;

                    int examId;

                    if (!int.TryParse(workSheet.Cells[1, 1].Value.ToString(), out examId))
                    {
                        return this.UnSuccessFunction("لطفا مقدار فیلد A1 را بررسی کنید");
                    }

                    var exam = await db.Exams.FirstOrDefaultAsync(c => c.Id == examId);

                    for (int i = 3; i <= totalRows; i++)
                    {
                        int stdCode;

                        if (!int.TryParse(workSheet.Cells[i, 2].Value.ToString(), out stdCode))
                        {
                            messages.Add("کد دانش آموزی ردیف " + i + " را بررسی کنید");
                            continue;
                        }

                        double score;
                        var scoreState = ExamScoreState.Hazer;

                        var scoreCellValue = workSheet.Cells[i, 3].Value.ToString();
                        if (!double.TryParse(scoreCellValue, out score))
                        {
                            if (scoreCellValue.Equals("T"))
                            {
                                scoreState = ExamScoreState.TrueGhaeb;
                                score = 0;
                            }
                            else if (scoreCellValue.Equals("F"))
                            {
                                scoreState = ExamScoreState.FalseGhaeb;
                                score = 0;
                            }
                            else
                            {
                                messages.Add("نمره ردیف " + i + " را بررسی کنید");
                                continue;
                            }
                        }

                        if (score > exam.TopScore)
                        {
                            messages.Add("نمره ردیف " + i + " بیشتر از ملاک آزمون است");
                            continue;
                        }

                        var student = await db.Students.FirstOrDefaultAsync(c => c.Code == stdCode);

                        if (await db.ExamScores.AnyAsync(c => c.ExamId == examId && c.StudentId == student.Id))
                        {
                            //Edit
                            if (canUserEditExamScore)
                            {
                                var examScore = await db.ExamScores.FirstOrDefaultAsync(c => c.ExamId == examId && c.StudentId == student.Id);

                                examScore.State = scoreState;
                                examScore.Score = score;
                                examScore.TopScore = exam.TopScore;
                            }
                            else
                            {
                                messages.Add("نمره ردیف " + i + " قبلا وارد شده است، شما مجوز ویرایش نمره را ندارید!");
                            }
                        }
                        else
                        {
                            //Add
                            var examScore = new ExamScore
                            {
                                ExamId = examId,
                                StudentId = student.Id,
                                Score = score,
                                TopScore = exam.TopScore,
                                NumberQ = exam.NumberQ,
                                State = scoreState,
                                TrueAnswer = 0,
                                FalseAnswer = 0,
                                BlankAnswer = 0
                            };

                            examScoresToAdd.Add(examScore);
                        }
                    }

                    if (examScoresToAdd.Any())
                    {
                        await db.ExamScores.AddRangeAsync(examScoresToAdd);
                    }

                    await db.SaveChangesAsync();

                    if (messages.Any())
                    {
                        var message = "";

                        foreach (var item in messages)
                        {
                            message += "\n " + item;
                        }

                        return this.UnSuccessFunction(message);
                    }

                    return this.SuccessFunction();
                }
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


    }



    public class WorkbookDetailResult
    {
        public int rate { get; set; }

        public string name { get; set; }

        public double totalAvg { get; set; }

        public List<double> courseAvgs { get; set; }
    }

    public class getScoreDetailByWorkbookParam
    {
        public int workbookId { get; set; }

        public int gradeId { get; set; }

        public int? classId { get; set; }
    }

    public class TotalAverageReturn
    {
        public int rating { get; set; }

        public double? average { get; set; }
        public double? topTotal { get; set; }
    }

    public class StudentCourseRatingParams
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ExamScore> studentScores { get; set; }
        public IEnumerable<Exam> exams { get; set; }
        public IEnumerable<Course> courses { get; set; }
    }

    public class getTotalAverageByStudentGradeParam
    {
        public int studentId { get; set; }

        public int gradeId { get; set; }
        public int classId { get; set; }

        public int workbookId { get; set; }
    }

    public class getExamScoreByGrade_Student
    {
        public int gradeId { get; set; }

        public int studentId { get; set; }
    }

    public class getAllByExamYGC
    {
        public getByYGC YGC { get; set; }

        public int studentId { get; set; }
    }

    public class getExamscoreparams
    {
        public getparams getparam { get; set; }

        public int? examId { get; set; }

        public int? courseId { get; set; }

        public string name { get; set; }
    }
}