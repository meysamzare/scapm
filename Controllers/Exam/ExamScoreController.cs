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
                    .Where(c => c.Exam.YeareducationId == nowYeareducationId)
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
                var examscores = await db.ExamScores
                    .Select(c => new
                    {
                        Id = c.Id.ToString(),
                        Rating = c.getRating(c.Exam.ExamScores.ToList(), c.Score),
                        Score = c.Score.ToString(),
                        StudentId = c.StudentId,
                        ExamId = c.ExamId.ToString(),
                        State = c.State,
                        examName = c.Exam.Name,
                        studentName = c.Student.Name + " " + c.Student.LastName,
                        gradeId = c.Exam.GradeId,
                        TopScore = double.Parse(c.Exam.TopScore.ToString())
                    })
                    .Where(c => c.StudentId == getexamscore.studentId)
                        .Where(c => c.gradeId == getexamscore.gradeId)
                .ToListAsync();

                var student = await db.Students.FirstOrDefaultAsync(c => c.Id == getexamscore.studentId);
                var stdCodeMeli = student.IdNumber2.Trim();

                var items = db.Items
                    .Include(c => c.ItemAttribute)
                        .ThenInclude(c => c.Attribute)
                            .ThenInclude(c => c.Question)
                                .ThenInclude(c => c.QuestionOptions)
                    .Include(c => c.Category)
                        .ThenInclude(c => c.Attributes)
                .Where(c => c.Category.Type == CategoryTotalType.onlineExam && c.Category.GradeId.HasValue ? c.Category.GradeId.Value == getexamscore.gradeId : false)
                    .Where(c => c.Category.Attributes.Any(l => l.IsMeliCode))
                .Where(c => c.ItemAttribute.Where(f => f.Attribute.IsMeliCode).Select(g => g.AttrubuteValue.Trim()).Contains(stdCodeMeli))
                .ToList()
                    .GroupBy(c => c.Category.Id)
                        .Select(c => c.OrderByDescending(l => l.getTotalScoreDouble).FirstOrDefault());

                examscores.AddRange(items.Select(c => new
                {
                    Id = $"OEI{c.Id}",
                    Rating = c.getRating(c.Category.Items.ToList(), c.getTotalScoreDouble),
                    Score = c.getTotalScore,
                    StudentId = student.Id,
                    ExamId = $"OE{c.Category.Id}",
                    State = ExamScoreState.Hazer,
                    examName = c.Category.Title,
                    studentName = student.Name + " " + student.LastName,
                    gradeId = c.Category.GradeId.HasValue ? c.Category.GradeId.Value : 0,
                    TopScore = c.Category.getTotalScore(c.Category.Attributes.ToList(), 
                        c.Category.UseLimitedRandomQuestionNumber,
                        c.Category.VeryHardQuestionNumber,
                        c.Category.HardQuestionNumber,
                        c.Category.ModerateQuestionNumber,
                        c.Category.EasyQuestionNumber)
                }));

                return this.DataFunction(true, examscores);
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

                var cats = db.Categories.Where(c => c.Type == CategoryTotalType.onlineExam && c.GradeId.HasValue ? c.GradeId.Value == param.gradeId : false && c.Attributes.Any(l => l.IsMeliCode))
                                    .Where(c => c.WorkbookId == param.workbookId)
                                        .Include(c => c.Items);

                var items = cats.SelectMany(c => c.Items)
                    .Include(m => m.ItemAttribute)
                        .ThenInclude(m => m.Attribute)
                            .ThenInclude(m => m.Question)
                                .ThenInclude(m => m.QuestionOptions);

                var stds = new List<StudentCourseRatingParams>();


                var Students = await db.StdClassMngs
                    .Select(c => c.Student).Distinct()
                    .Include(c => c.ExamScores)
                        .ThenInclude(c => c.Exam)
                .ToListAsync();


                Students.ForEach(c =>
                {
                    var stdTemp = new StudentCourseRatingParams();

                    stdTemp.Id = c.Id;
                    stdTemp.Name = c.Name + ' ' + c.LastName;
                    stdTemp.studentScores = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId)
                            .Select(l => new ExamScoreCourseRatingParam
                            {
                                ExamId = l.ExamId.ToString(),
                                Score = l.Score
                            }).ToList();
                    stdTemp.exams = c.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId).Select(l => l.Exam).Where(l => l.WorkbookId == param.workbookId)
                            .Select(l => new ExamCourseRatingParam
                            {
                                Id = l.Id.ToString(),
                                CourseId = l.CourseId,
                                TopScore = l.TopScore
                            }).ToList();

                    stdTemp.onlineExams = cats.Select(l => new ExamCourseRatingParam
                    {
                        Id = $"OE{l.Id}",
                        CourseId = l.CourseId.HasValue ? l.CourseId.Value : 0,
                        TopScore = l.CalculateNegativeScore ? 20 : l.getTotalScore(l.Attributes.ToList(),
                                l.UseLimitedRandomQuestionNumber,
                                l.VeryHardQuestionNumber,
                                l.HardQuestionNumber,
                                l.ModerateQuestionNumber,
                                l.EasyQuestionNumber)
                    }).ToList();

                    stdTemp.onlineExamScores = items.Where(v => v.ItemAttribute.Where(f => f.Attribute.IsMeliCode).Select(g => g.AttrubuteValue.Trim()).Contains(c.IdNumber2.Trim()))
                    .Select(l => new ExamScoreCourseRatingParam
                    {
                        ExamId = $"OE{l.Category.Id}",
                        Score = l.getTotalScoreFunction(l.ItemAttribute, l.Category.CalculateNegativeScore)
                    }).ToList();


                    stdTemp.exams.AddRange(stdTemp.onlineExams);
                    stdTemp.studentScores.AddRange(stdTemp.onlineExamScores);

                    stds.Add(stdTemp);
                });


                if (param.classId.HasValue)
                {

                    var studentsIdInClass = await db.StdClassMngs
                        .Where(c => c.ClassId == param.classId.Value && c.IsActive == true)
                            .Select(c => c.Student.Id)
                                .Distinct()
                    .ToListAsync();

                    students = stds.Where(c => studentsIdInClass.Contains(c.Id));
                }
                else
                {
                    var studentsIdInGrade = await db.StdClassMngs
                        .Where(c => c.GradeId == param.gradeId && c.IsActive == true)
                            .Select(c => c.Student.Id)
                                .Distinct()
                    .ToListAsync();

                    students = stds.Where(c => studentsIdInGrade.Contains(c.Id));
                }

                var results = new List<WorkbookDetailResult>();

                var courseScores = new List<CourseScoreType>();

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
                            var avg = sumScore / count;
                            avgForCourses.Add(Math.Round(avg, 2));

                            courseScores.Add(new CourseScoreType
                            {
                                courseId = course.Id,
                                score = avg
                            });
                        }
                        else
                        {
                            if (!param.onlyShowCourseHaveScore || exams.Count() != 0)
                            {
                                avgForCourses.Add(0.0);
                            }

                            courseScores.Add(new CourseScoreType
                            {
                                courseId = course.Id,
                                score = 0.0
                            });
                        }
                    }

                    var totalAvg = 0.0;

                    if (avgForCourses.Any(c => c != 0.0))
                    {
                        totalAvg = avgForCourses.Where(c => c != 0.0).Average();
                    }

                    results.Add(new WorkbookDetailResult
                    {
                        courseAvgs = avgForCourses,
                        totalAvg = totalAvg,
                        name = std.Name
                    });
                }

                var uniqTotalAvgs = results.Select(c => c.totalAvg).Distinct().OrderByDescending(c => c).ToList();

                foreach (var (result, index) in results.WithIndex())
                {
                    results[index].rate = uniqTotalAvgs.FindIndex(c => c == result.totalAvg) + 1;
                }

                var courseHaveScore = courses;

                if (param.onlyShowCourseHaveScore)
                {
                    var haveScoreCourses = new List<int>();
                    courses.ForEach(course =>
                    {
                        var allZero = courseScores.Where(c => c.courseId == course.Id).Select(c => c.score).All(c => c == 0.0);

                        if (!allZero)
                        {
                            haveScoreCourses.Add(course.Id);
                        }
                    });

                    courseHaveScore = courseHaveScore.Where(c => haveScoreCourses.Contains(c.Id)).ToList();
                }

                return this.DataFunction(true, new
                {
                    courses = courseHaveScore,
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

                var cats = db.Categories.Where(c => c.Type == CategoryTotalType.onlineExam && c.GradeId.HasValue ? c.GradeId.Value == param.gradeId : false && c.Attributes.Any(l => l.IsMeliCode))
                                .Where(c => c.WorkbookId == param.workbookId)
                                    .Include(c => c.Items);

                var items = cats.SelectMany(c => c.Items)
                    .Include(m => m.ItemAttribute)
                    .ThenInclude(m => m.Attribute)
                    .ThenInclude(m => m.Question)
                    .ThenInclude(m => m.QuestionOptions);

                var stds = new List<STDWorkbookReturn>();

                var students = await db.Students.Where(c => studentsIdInGrade.Contains(c.Id))
                    .Include(c => c.ExamScores)
                        .ThenInclude(c => c.Exam)
                .ToListAsync();
                var courses = await db.Courses.Where(l => l.GradeId == param.gradeId).ToListAsync();

                students.ForEach(studen =>
                {
                    var stdTemp = new STDWorkbookReturn();

                    stdTemp.Id = studen.Id;
                    stdTemp.examScores = studen.ExamScores.Where(l => l.State == ExamScoreState.Hazer && l.Exam.GradeId == param.gradeId && l.Exam.WorkbookId == param.workbookId)
                                .Select(f => new ExamScoreForCourseAvg { Score = f.Score, TopScore = double.Parse(f.Exam.TopScore.ToString()), CourseId = f.Exam.CourseId }).ToList();

                    stdTemp.onlineExamScores = items
                    .Where(v => v.ItemAttribute.Where(f => f.Attribute.IsMeliCode).Select(g => g.AttrubuteValue.Trim()).Contains(studen.IdNumber2.Trim()))
                            .Select(l => new ExamScoreForCourseAvg
                            {
                                Score = l.getTotalScoreFunction(l.ItemAttribute, l.Category.CalculateNegativeScore),
                                TopScore = l.Category.CalculateNegativeScore ? 20 : l.Category.getTotalScore(l.Category.Attributes.ToList(),
                                        l.Category.UseLimitedRandomQuestionNumber,
                                        l.Category.VeryHardQuestionNumber,
                                        l.Category.HardQuestionNumber,
                                        l.Category.ModerateQuestionNumber,
                                        l.Category.EasyQuestionNumber),
                                CourseId = l.Category.CourseId.HasValue ? l.Category.CourseId.Value : 0
                            }).ToList();

                    stdTemp.examScores.AddRange(stdTemp.onlineExamScores);


                    stdTemp.courses = courses.Where(l => stdTemp.examScores.Select(f => f.CourseId).Contains(l.Id)).ToList();
                    stdTemp.courseAvgs = stdTemp.courses.Select(l => l.getCourseAverageForStudent(stdTemp.examScores.Where(f => f.CourseId == l.Id).ToList(), l)).OrderBy(l => l.CourseName).ToList();
                    stdTemp.totalAvg = getAverageOfCourseAvg(stdTemp.courseAvgs);

                    stds.Add(stdTemp);
                });

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


        private double getJustifiedScore(double score, double topScore)
        {
            double mix = 0.0;
            if (topScore != 0.0)
            {
                mix = (double)20 / topScore;
            }

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

    internal class CourseScoreType
    {
        public int courseId { get; set; }
        public double score { get; set; }
    }

    public class STDWorkbookReturn
    {
        public int Id { get; set; }
        public List<ExamScoreForCourseAvg> examScores { get; set; }
        public IEnumerable<ExamScoreForCourseAvg> onlineExamScores { get; set; }
        public List<Course> courses { get; set; }
        public List<CourseAvgReturn> courseAvgs { get; set; }
        public double totalAvg { get; set; }
        public int index { get; set; }
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
        public int? workbookId { get; set; }

        public int gradeId { get; set; }

        public int? classId { get; set; }

        public bool onlyShowCourseHaveScore { get; set; }
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
        public List<ExamScoreCourseRatingParam> studentScores { get; set; }
        public List<ExamCourseRatingParam> exams { get; set; }
        public List<Course> courses { get; set; }
        public List<ExamCourseRatingParam> onlineExams { get; set; }
        public List<ExamScoreCourseRatingParam> onlineExamScores { get; set; }
    }

    public class ExamCourseRatingParam
    {
        public string Id { get; set; }

        public double TopScore { get; set; }

        public int CourseId { get; set; }
    }


    public class ExamScoreCourseRatingParam
    {
        public double Score { get; set; }

        public string ExamId { get; set; }
    }

    public class getTotalAverageByStudentGradeParam
    {
        public int studentId { get; set; }

        public int gradeId { get; set; }
        public int classId { get; set; }

        public int? workbookId { get; set; }
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