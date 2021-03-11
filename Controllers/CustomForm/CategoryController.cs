﻿using System;
using System.Collections.Generic;
using System.IO;
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
    [Authorize]
    [ApiController]
    public class CategoryController : Controller
    {
        public Data.DbContext db;
        const string roleTitle = "Category";

        private IHostingEnvironment hostingEnvironment;

        public CategoryController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        public IEnumerable<Category> Categories
        {
            get
            {
                return db.Categories.Include(c => c.ParentCategory).Include(c => c.Children).ToList();
            }
        }


        [HttpPost]
        [Role(RolePrefix.Add, roleTitle)]
        public async Task<IActionResult> Add([FromBody] AddCategoryParam category)
        {
            try
            {
                DateTime datepublish = category.datePublish.AddDays(1);
                TimeSpan timepublish;
                if (!TimeSpan.TryParse(category.timePublish, out timepublish))
                {
                    return this.UnSuccessFunction("مقدار زمان شروع را وارد کنید");
                }
                datepublish = datepublish.Date + timepublish;


                DateTime dateex = category.dateExpire.AddDays(1);
                TimeSpan timeex;
                if (!TimeSpan.TryParse(category.timeExpire, out timeex))
                {
                    return this.UnSuccessFunction("مقدار زمان انقضا را وارد کنید");
                }
                dateex = dateex.Date + timeex;

                category.dateExpire = dateex;
                category.datePublish = datepublish;

                if (!string.IsNullOrEmpty(category.registerFileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, category.registerFileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(category.registerFileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.registerPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + category.registerFileName);
                }

                if (!string.IsNullOrEmpty(category.showInfoFileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, category.showInfoFileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(category.showInfoFileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.showInfoPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + category.showInfoFileName);
                }

                if (!string.IsNullOrEmpty(category.headerPicData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, category.headerPicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(category.headerPicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.headerPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + category.headerPicName);
                }

                var cat = (Category)category;

                db.Categories.Add(cat);

                await db.SaveChangesAsync();

                return this.DataFunction(true, cat.Id);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle)]
        public async Task<IActionResult> Edit([FromBody] AddCategoryParam addCategory)
        {
            try
            {
                var cat = await db.Categories
                    .Include(c => c.Children)
                    .Include(c => c.Attributes)
                .SingleAsync(c => c.Id == addCategory.id);

                if (addCategory.parentId == cat.Id)
                {
                    return this.UnSuccessFunction("این فرم نمی تواند انتخاب شود c1");
                }

                if (await isInChildCat(cat.Id, addCategory.parentId))
                {
                    return this.UnSuccessFunction("این فرم نمی تواند انتخاب شود c2");
                }

                if (addCategory.useLimitedRandomQuestionNumber && cat.Type == CategoryTotalType.onlineExam)
                {
                    if (
                        addCategory.veryHardQuestionNumber.EqualsAnyOf(null, "", 0) &&
                        addCategory.hardQuestionNumber.EqualsAnyOf(null, "", 0) &&
                        addCategory.moderateQuestionNumber.EqualsAnyOf(null, "", 0) &&
                        addCategory.easyQuestionNumber.EqualsAnyOf(null, "", 0)
                    )
                    {
                        return this.UnSuccessFunction("شما می بایست حداقل یکی از میزان سختی سوالات را وارد نمایید");
                    }

                    var veryHardQuestionNumber = addCategory.veryHardQuestionNumber.HasValue ? addCategory.veryHardQuestionNumber.Value : 0;
                    var hardQuestionNumber = addCategory.hardQuestionNumber.HasValue ? addCategory.hardQuestionNumber.Value : 0;
                    var moderateQuestionNumber = addCategory.moderateQuestionNumber.HasValue ? addCategory.moderateQuestionNumber.Value : 0;
                    var easyQuestionNumber = addCategory.easyQuestionNumber.HasValue ? addCategory.easyQuestionNumber.Value : 0;

                    var sumOfQuestionNumbers = veryHardQuestionNumber + hardQuestionNumber + moderateQuestionNumber + easyQuestionNumber;

                    if (sumOfQuestionNumbers > cat.Attributes.Where(c => c.AttrType == AttrType.Question).Count())
                    {
                        return this.UnSuccessFunction("جمع سوالات تصادفی نمی تواند از تعداد سوال های آزمون بیشتر باشد");
                    }
                }



                cat.UseLimitedRandomQuestionNumber = addCategory.useLimitedRandomQuestionNumber;

                cat.VeryHardQuestionNumber = addCategory.veryHardQuestionNumber;
                cat.HardQuestionNumber = addCategory.hardQuestionNumber;
                cat.ModerateQuestionNumber = addCategory.moderateQuestionNumber;
                cat.EasyQuestionNumber = addCategory.easyQuestionNumber;
                cat.RegisterItemStepType = (CategoryRegisterItemStepType)addCategory.registerItemStepType;



                cat.ParentId = addCategory.parentId;
                cat.Title = addCategory.title;
                cat.Type = addCategory.type;
                cat.RandomAttribute = addCategory.randomAttribute;
                cat.RandomAttributeOption = addCategory.randomAttributeOption;

                cat.TeachersIdAccess = addCategory.teachersIdAccess;

                cat.WorkbookId = addCategory.workbookId;
                cat.CourseId = addCategory.courseId;
                cat.ExamTypeId = addCategory.examTypeId;
                cat.ClassId = addCategory.classId;
                cat.GradeId = addCategory.gradeId;

                cat.ShowScoreAfterDone = addCategory.showScoreAfterDone;
                cat.CalculateNegativeScore = addCategory.calculateNegativeScore;
                cat.IsBackStepAllowed = addCategory.isBackStepAllowed;

                DateTime datepublish = addCategory.datePublish;
                if (cat.DatePublish != datepublish)
                {
                    datepublish = datepublish.AddDays(1);
                }
                TimeSpan timepublish;
                if (!TimeSpan.TryParse(addCategory.timePublish, out timepublish))
                {
                    return this.UnSuccessFunction("مقدار زمان شروع را وارد کنید");
                }
                datepublish = datepublish.Date + timepublish;


                DateTime dateex = addCategory.dateExpire;
                if (cat.DateExpire != dateex)
                {
                    dateex = dateex.AddDays(1);
                }
                TimeSpan timeex;
                if (!TimeSpan.TryParse(addCategory.timeExpire, out timeex))
                {
                    return this.UnSuccessFunction("مقدار زمان انقضا را وارد کنید");
                }
                dateex = dateex.Date + timeex;

                cat.DateExpire = dateex;
                cat.DatePublish = datepublish;

                cat.Desc = addCategory.desc;
                cat.IsActive = addCategory.isActive;
                cat.EndMessage = addCategory.endMessage;


                cat.HaveInfo = addCategory.haveInfo;
                cat.IsInfoShow = addCategory.isInfoShow;
                cat.ActiveMessage = addCategory.activeMessage;
                cat.DeActiveMessage = addCategory.deActiveMessage;


                cat.RoleAccess = addCategory.roleAccess;


                cat.HaveEntringCard = addCategory.haveEntringCard;
                cat.BtnTitle = addCategory.btnTitle;

                cat.ShowRow = (ShowRow)addCategory.showRow;

                cat.PostType = addCategory.postType;

                cat.AuthorizeState = addCategory.authorizeState;
                cat.License = addCategory.license;


                if (!string.IsNullOrEmpty(addCategory.registerFileData))
                {

                    if (!string.IsNullOrEmpty(cat.RegisterPicUrl))
                    {
                        try
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + cat.RegisterPicUrl);
                        }
                        catch { }
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, addCategory.registerFileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(addCategory.registerFileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    cat.RegisterPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + addCategory.registerFileName);
                }

                if (!string.IsNullOrEmpty(addCategory.showInfoFileData))
                {

                    if (!string.IsNullOrEmpty(cat.ShowInfoPicUrl))
                    {
                        try
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + cat.ShowInfoPicUrl);
                        }
                        catch { }
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, addCategory.showInfoFileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(addCategory.showInfoFileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    cat.ShowInfoPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + addCategory.showInfoFileName);
                }

                if (!string.IsNullOrEmpty(addCategory.headerPicData))
                {

                    if (!string.IsNullOrEmpty(cat.HeaderPicUrl))
                    {
                        try
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + cat.HeaderPicUrl);
                        }
                        catch { }
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, addCategory.headerPicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(addCategory.headerPicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    cat.HeaderPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + addCategory.headerPicName);
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

        public async Task<bool> isInChildCat(int catId, int? selectedId)
        {
            var cat = await db.Categories.Include(c => c.Children).SingleAsync(c => c.Id == catId);
            foreach (var i in cat.Children)
            {
                if (i.Id == selectedId)
                {
                    isinchild = true;
                }
                if (i.Children != null)
                {
                    if (i.Children.Any())
                    {
                        await isInChildCat(i.Id, selectedId);
                    }
                }
            }

            return isinchild;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAll()
        {
            try
            {
                var catList = await db.Categories.Where(c => !c.IsArchived).ToListAsync();

                return this.SuccessFunction(data: catList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.View, roleTitle)]
        public async Task<IActionResult> GetAllPined()
        {
            try
            {
                var catList = await db.Categories
                    .Where(c => c.IsPined && !c.IsArchived)
                .ToListAsync();

                return this.SuccessFunction(data: catList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.View, roleTitle)]
        public async Task<IActionResult> GetAllPinedByType([FromBody] int type)
        {
            try
            {
                var catList = await db.Categories
                    .Where(c => c.IsPined && c.Type == (CategoryTotalType)type && !c.IsArchived)
                .ToListAsync();

                return this.SuccessFunction(data: catList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByTeacher([FromBody] getAllByTeacherParam param)
        {
            try
            {
                var catList = new List<Category>();

                if ((CategoryTotalType)param.type == CategoryTotalType.registerForm)
                {
                    catList = await db.Categories
                        .Where(c => c.TeachersIdAccess == null ? false : c.TeachersIdAccess.Contains(param.teacherId) && c.Type == (CategoryTotalType)param.type)
                    .ToListAsync();
                }
                else
                {
                    var teacher = await db.Teachers
                        .Include(c => c.Courses)
                    .FirstOrDefaultAsync(c => c.Id == param.teacherId);

                    var teacherCourseIds = teacher.Courses != null ? teacher.Courses.Select(c => c.Id).ToList() : new List<int>();

                    catList = await db.Categories
                        .Where(c => c.Type == (CategoryTotalType)param.type)
                        .Where(c => teacherCourseIds.Contains(c.CourseId.HasValue ? c.CourseId.Value : 0))
                    .ToListAsync();
                }


                return this.DataFunction(true, catList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle)]
        public async Task<IActionResult> togglePin([FromBody] int id)
        {
            try
            {
                var cat = await db.Categories.FirstOrDefaultAsync(c => c.Id == id);

                cat.IsPined = !cat.IsPined;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle)]
        public async Task<IActionResult> ChangeCheckableProperty([FromBody] ChangeCheckablePropertyParam param)
        {
            try
            {
                var cat = await db.Categories.FirstOrDefaultAsync(c => c.Id == param.catId);

                if (param.type == "isActive")
                {
                    cat.IsActive = param.check;
                }

                if (param.type == "haveInfo")
                {
                    cat.HaveInfo = param.check;
                }

                if (param.type == "isInfoShow")
                {
                    cat.IsInfoShow = param.check;
                }

                if (param.type == "isArchived")
                {
                    cat.IsArchived = param.check;
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
        [Role(RolePrefix.Edit, roleTitle)]
        public async Task<IActionResult> AddRandomQuestionAttribute([FromBody] AddRandomQuestionAttributeParam param)
        {
            try
            {
                var questionIds = new List<int>();

                var questions = await db.Questions
                    .Where(c => c.CourseId == param.selectedCourseForQuestion)
                    .OrderBy(c => Guid.NewGuid())
                .ToListAsync();

                if (param.veryHardQuestionNumber.HasValue && param.veryHardQuestionNumber.Value > 0)
                {
                    var qus = questions.Where(c => c.Defact == QueDefact.VeryHard).Take(param.veryHardQuestionNumber.Value).ToList();

                    qus.ForEach(q => questionIds.Add(q.Id));
                }

                if (param.hardQuestionNumber.HasValue && param.hardQuestionNumber.Value > 0)
                {
                    var qus = questions.Where(c => c.Defact == QueDefact.Hard).Take(param.hardQuestionNumber.Value).ToList();

                    qus.ForEach(q => questionIds.Add(q.Id));
                }

                if (param.mediumQuestionNumber.HasValue && param.mediumQuestionNumber.Value > 0)
                {
                    var qus = questions.Where(c => c.Defact == QueDefact.Modrate).Take(param.mediumQuestionNumber.Value).ToList();

                    qus.ForEach(q => questionIds.Add(q.Id));
                }

                if (param.easyQuestionNumber.HasValue && param.easyQuestionNumber.Value > 0)
                {
                    var qus = questions.Where(c => c.Defact == QueDefact.Easy).Take(param.easyQuestionNumber.Value).ToList();

                    qus.ForEach(q => questionIds.Add(q.Id));
                }

                if (questionIds.Count != 0)
                {
                    var cat = await db.Categories
                        .Include(c => c.Attributes)
                    .FirstOrDefaultAsync(c => c.Id == param.catId);

                    var attributes = new List<Model.Attribute>();

                    var unitId = 1;
                    var course = await db.Courses.FirstOrDefaultAsync(c => c.Id == param.selectedCourseForQuestion);

                    if (await db.Units.AnyAsync(c => c.Title == course.Name))
                    {
                        unitId = db.Units.FirstOrDefault(c => c.Title == course.Name).Id;
                    }
                    else
                    {
                        var unit = new Unit
                        {
                            Title = course.Name,
                            EnTitle = "Section (Auto Create)"
                        };

                        db.Units.Add(unit);

                        await db.SaveChangesAsync();

                        unitId = unit.Id;
                    }

                    questionIds.ForEach(qId =>
                    {
                        if (!cat.Attributes.Any(c => c.QuestionId == qId))
                        {
                            var question = db.Questions.FirstOrDefault(c => c.Id == qId);

                            var attr = new Model.Attribute
                            {
                                Title = $"{cat.Title} - {question.Name}",
                                Values = "",
                                UnitId = unitId,
                                AttrType = AttrType.Question,
                                IsInClient = true,
                                IsRequired = true,
                                Score = question.Mark,
                                QuestionId = question.Id,
                                IsTemplate = false
                            };

                            attributes.Add(attr);
                        }
                    });

                    if (attributes.Any())
                    {
                        attributes.ForEach(attr => cat.Attributes.Add(attr));

                        await db.SaveChangesAsync();

                        return this.SuccessFunction();
                    }

                    return this.UnSuccessFunction("سوالی با فیلتر مورد نظر یافت نشد و یا قبلا ثبت شده است");
                }
                else
                {
                    return this.UnSuccessFunction("سوالی با فیلتر مورد نظر یافت نشد");
                }
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AbsenceForOnlineExam([FromBody] AbsenceForOnlineExamParam param)
        {
            try
            {
                var catId = param.catId;

                var cat = await db.Categories
                    .Include(c => c.Course)
                    .Include(c => c.Attributes)
                    .Include(c => c.Items)
                        .ThenInclude(c => c.ItemAttribute)
                            .ThenInclude(c => c.Attribute)
                                .ThenInclude(m => m.Question)
                                    .ThenInclude(m => m.QuestionOptions)
                .FirstOrDefaultAsync(c => c.Id == catId);

                if (cat.Type != CategoryTotalType.onlineExam)
                {
                    return this.UnSuccessFunction("نمون برگ انتخابی قابلیت حضور غیاب را ندارد");
                }

                var studentsList = new List<Student>();

                if (cat.ClassId.HasValue)
                {
                    studentsList = await db.StdClassMngs.Where(c => c.ClassId == cat.ClassId.Value).Select(c => c.Student).Distinct().ToListAsync();
                }
                if (cat.GradeId.HasValue)
                {
                    studentsList = await db.StdClassMngs.Where(c => c.GradeId == cat.GradeId.Value).Select(c => c.Student).Distinct().ToListAsync();
                }


                if (studentsList.Count == 0)
                {
                    return this.UnSuccessFunction("دانش آموزی برای این آزمون مشخص نشد!");
                }

                var peresentStuedntsMeliCode = new List<string>();

                var isCatHaveMeliCodeAttribute = cat.Attributes.Any(c => c.IsMeliCode);
                var isCatHaveAuthrization = cat.AuthorizeState != CategoryAuthorizeState.none;

                if (isCatHaveMeliCodeAttribute)
                {
                    peresentStuedntsMeliCode = cat.Items.SelectMany(c => c.ItemAttribute).Where(c => c.Attribute.IsMeliCode).Select(c => c.AttrubuteValue.Trim().PersianToEnglishDigit()).ToList();
                }
                else if (isCatHaveAuthrization)
                {
                    peresentStuedntsMeliCode = cat.Items.Select(c => c.AuthorizedUsername.Trim().PersianToEnglishDigit()).ToList();
                }
                else
                {
                    return this.UnSuccessFunction("خطا در احراز هویت داده های ثبت شده");
                }

                var mustToBeAddedItems = new List<Item>();

                studentsList.ForEach(std =>
                {
                    if (!peresentStuedntsMeliCode.Contains(std.IdNumber2))
                    {
                        var itemAttrs = new List<ItemAttribute>();

                        cat.Attributes.ToList().ForEach(attr =>
                        {
                            itemAttrs.Add(new ItemAttribute
                            {
                                AttributeId = attr.Id,
                                AttrubuteValue = attr.IsMeliCode ? std.IdNumber2 : ""
                            });
                        });


                        var item = new Item()
                        {
                            IsActive = false,
                            DateAdd = DateTime.Now,
                            AuthorizedUsername = "---",
                            AuthorizedFullName = "",
                            AuthorizedType = 0,
                            CategoryId = cat.Id,
                            UnitId = db.Units.First().Id,
                            RahCode = new Random().Next(11111111, 99999999),
                            Title = std.LastName + " - " + std.Name + " (غائب)",
                            MeliCode = std.IdNumber2,
                            Score = 0,
                            ItemAttribute = itemAttrs
                        };

                        mustToBeAddedItems.Add(item);
                    }
                });

                cat.TopScore = cat.CalculateNegativeScore ? 20 : cat.getTotalScore(cat.Attributes.ToList(),
                                        cat.UseLimitedRandomQuestionNumber,
                                        cat.VeryHardQuestionNumber,
                                        cat.HardQuestionNumber,
                                        cat.ModerateQuestionNumber,
                                        cat.EasyQuestionNumber);

                foreach (var item in cat.Items)
                {
                    var dbItem = await db.Items.FirstOrDefaultAsync(c => c.Id == item.Id);

                    dbItem.Score = item.getTotalScoreFunction(item.ItemAttribute, cat.CalculateNegativeScore);

                    var itemMeliCode = "";

                    if (isCatHaveMeliCodeAttribute)
                    {
                        itemMeliCode = item.ItemAttribute.FirstOrDefault(c => c.Attribute.IsMeliCode).AttrubuteValue.Trim().PersianToEnglishDigit();
                    }
                    else
                    {
                        itemMeliCode = item.AuthorizedUsername.Trim().PersianToEnglishDigit();
                    }

                    dbItem.MeliCode = itemMeliCode;

                    dbItem.TrueCount = item.ItemAttribute.Where(c => c.getPaperState(c, c.Attribute) == ItemAttributePaperState.True).Count();
                    dbItem.FalseCount = item.ItemAttribute.Where(c => c.getPaperState(c, c.Attribute) == ItemAttributePaperState.False).Count();
                    dbItem.BlankCount = item.ItemAttribute.Where(c => c.getPaperState(c, c.Attribute) == ItemAttributePaperState.Blank).Count();
                }

                cat.IsStaticDataSaved = true;
                cat.LastSavedStaticData = DateTime.Now;

                await db.Items.AddRangeAsync(mustToBeAddedItems);

                if (param.deleteUnknownDatas)
                {
                    var meliCodes = studentsList.Select(c => c.IdNumber2.Trim());

                    var itemsToDelete = cat.Items.Where(c => !meliCodes.Contains(c.MeliCode));

                    db.Items.RemoveRange(itemsToDelete);
                    db.ItemAttributes.RemoveRange(itemsToDelete.SelectMany(c => c.ItemAttribute));
                }

                await db.SaveChangesAsync();


                var pureTopScore = cat.getTotalScore(cat.Attributes.ToList(),
                                    cat.UseLimitedRandomQuestionNumber,
                                    cat.VeryHardQuestionNumber,
                                    cat.HardQuestionNumber,
                                    cat.ModerateQuestionNumber,
                                    cat.EasyQuestionNumber);

                var topScore = pureTopScore;

                if (param.saveScoreType == 2)
                {
                    topScore = 100;
                }

                if (param.saveScoreType == 3)
                {
                    topScore = 20;
                }

                if (!await db.Exams.AnyAsync(c => c.OnlineExamId == cat.Id))
                {
                    var examScores = new List<ExamScore>();

                    foreach (var item in cat.Items)
                    {
                        var score = item.getTotalScoreFunction(item.ItemAttribute, param.saveScoreType == 2);

                        if (param.saveScoreType == 2)
                        {
                            score = score * 5;
                        }

                        if (param.saveScoreType == 3)
                        {
                            score = getJustifiedScore(score, pureTopScore);
                        }

                        if (studentsList.Any(c => c.IdNumber2 == item.MeliCode))
                        {
                            examScores.Add(new ExamScore
                            {
                                StudentId = studentsList.FirstOrDefault(c => c.IdNumber2 == item.MeliCode).Id,
                                TrueAnswer = item.TrueCount,
                                FalseAnswer = item.FalseCount,
                                BlankAnswer = item.BlankCount,
                                State = item.Title.EndsWith("(غائب)") ? ExamScoreState.TrueGhaeb : ExamScoreState.Hazer,
                                Score = score,
                                OnlineExamItemId = item.Id
                            });
                        }
                    }

                    var exam = new Exam
                    {
                        Name = cat.Title.Trim().Replace("/", "-"),
                        Date = cat.DatePublish.Value,
                        NumberQ = cat.Attributes.Where(c => c.AttrType == AttrType.Question).Count(),
                        ExamTypeId = cat.ExamTypeId,
                        GradeId = cat.GradeId.Value,
                        ClassId = cat.ClassId,
                        CourseId = cat.CourseId.Value,
                        WorkbookId = cat.WorkbookId,
                        Time = (cat.DateExpire - cat.DatePublish).Value.Minutes,
                        Result = true,
                        ResultDate = DateTime.Now,
                        OnlineExamId = cat.Id,
                        ExamScores = examScores,
                        TopScore = topScore,
                        TeacherId = cat.Course.TeacherId,
                        Source = "---",
                        ShowAvgOfExam = true
                    };

                    await db.Exams.AddAsync(exam);
                }
                else
                {
                    var exam = await db.Exams
                        .Include(c => c.ExamScores)
                    .FirstOrDefaultAsync(c => c.OnlineExamId == cat.Id);

                    exam.Name = cat.Title.Trim().Replace("/", "-");
                    exam.Date = cat.DatePublish.Value;
                    exam.NumberQ = cat.Attributes.Where(c => c.AttrType == AttrType.Question).Count();
                    exam.ExamTypeId = cat.ExamTypeId;
                    exam.GradeId = cat.GradeId.Value;
                    exam.ClassId = cat.ClassId;
                    exam.CourseId = cat.CourseId.Value;
                    exam.WorkbookId = cat.WorkbookId;
                    exam.Time = (cat.DateExpire - cat.DatePublish).Value.Minutes;
                    exam.Result = true;
                    exam.ResultDate = DateTime.Now;
                    exam.TopScore = topScore;
                    exam.TeacherId = cat.Course.TeacherId;
                    exam.Source = "---";

                    var mustToAddExamScores = new List<ExamScore>();

                    foreach (var item in cat.Items)
                    {
                        var score = item.getTotalScoreFunction(item.ItemAttribute, param.saveScoreType == 2);

                        if (param.saveScoreType == 2)
                        {
                            score = score * 5;
                        }

                        if (param.saveScoreType == 3)
                        {
                            score = getJustifiedScore(score, pureTopScore);
                        }

                        if (studentsList.Any(c => c.IdNumber2 == item.MeliCode))
                        {
                            var stdId = studentsList.FirstOrDefault(c => c.IdNumber2 == item.MeliCode).Id;

                            if (exam.ExamScores.Any(c => c.StudentId == stdId))
                            {
                                var examScore = exam.ExamScores.FirstOrDefault(c => c.StudentId == stdId);
                                var dbExamScore = db.ExamScores.FirstOrDefault(c => c.Id == examScore.Id);

                                dbExamScore.TrueAnswer = item.TrueCount;
                                dbExamScore.FalseAnswer = item.FalseCount;
                                dbExamScore.BlankAnswer = item.BlankCount;
                                dbExamScore.State = item.Title.EndsWith("(غائب)") ? ExamScoreState.TrueGhaeb : ExamScoreState.Hazer;
                                dbExamScore.Score = score;
                                dbExamScore.OnlineExamItemId = item.Id;
                            }
                            else
                            {
                                mustToAddExamScores.Add(new ExamScore
                                {
                                    ExamId = exam.Id,
                                    StudentId = studentsList.FirstOrDefault(c => c.IdNumber2 == item.MeliCode).Id,
                                    TrueAnswer = item.TrueCount,
                                    FalseAnswer = item.FalseCount,
                                    BlankAnswer = item.BlankCount,
                                    State = item.Title.EndsWith("(غائب)") ? ExamScoreState.TrueGhaeb : ExamScoreState.Hazer,
                                    Score = score,
                                    OnlineExamItemId = item.Id
                                });
                            }
                        }
                    }

                    if (mustToAddExamScores.Any())
                    {
                        await db.ExamScores.AddRangeAsync(mustToAddExamScores);
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


        private double getJustifiedScore(double score, double topScore)
        {
            double mix = (double)20 / topScore;

            return (score * mix);
        }

        [HttpPost]
        [Role(RolePrefix.View, roleTitle)]
        public async Task<IActionResult> getAllByType([FromBody] int type)
        {
            try
            {
                var cat = await db.Categories.Where(c => c.Type == (CategoryTotalType)type)
                .OrderByDescending(c => c.Id)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title
                    })
                .ToListAsync();

                return this.DataFunction(true, cat);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAllIndex()
        {
            try
            {
                var catList = await db.Categories
                    .Where(c => c.Type == CategoryTotalType.registerForm && !c.IsArchived)
                    .Where(c => DateTime.Now >= c.DatePublish && DateTime.Now <= c.DateExpire && c.IsActive == true && c.PostType == 0)
                .ToListAsync();

                return this.SuccessFunction(data: catList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getByType([FromBody] int type)
        {
            try
            {
                var cats = await db.Categories
                    .Where(c => c.Type == CategoryTotalType.registerForm && !c.IsArchived)
                    .Where(c => DateTime.Now >= c.DatePublish && DateTime.Now <= c.DateExpire && c.IsActive == true && c.PostType == type)
                .Select(c => new
                {
                    Id = c.Id,
                    Title = c.Title,
                    isInfoShow = c.IsInfoShow,
                    registerPicUrl = string.IsNullOrEmpty(c.RegisterPicUrl) ? c.HeaderPicUrl : c.RegisterPicUrl,
                    showInfoPicUrl = string.IsNullOrEmpty(c.ShowInfoPicUrl) ? c.HeaderPicUrl : c.ShowInfoPicUrl,
                })
                .ToListAsync();

                return this.SuccessFunction(data: cats);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getRegisterItemLogins([FromBody] int catId)
        {
            try
            {
                var cat = await db.Categories.FirstOrDefaultAsync(c => c.Id == catId);

                var logins = await db.RegisterItemLogins.Where(c => c.CategoryId == catId)
                .OrderByDescending(c => c.Date)
                    .Select(c => new
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        Username = c.Username,
                        CategoryAuthorizeState = c.CategoryAuthorizeState,
                        UserType = c.UserType,
                        DateString = c.Date.ToPersianDateWithTime(),
                        GradeId = c.GradeId,
                        ClassId = c.ClassId,
                        ip = c.IP,
                        absence = false
                    })
                .ToListAsync();

                if (cat.AuthorizeState == CategoryAuthorizeState.PMA)
                {
                    var stdClassMngs = db.StdClassMngs.Where(c => c.GradeId == cat.GradeId && c.IsActive);

                    if (cat.ClassId.HasValue)
                    {
                        stdClassMngs = stdClassMngs.Where(c => c.ClassId == cat.ClassId.Value);
                    }

                    var students = await stdClassMngs.Select(c => c.Student)
                        .Distinct()
                    .ToListAsync();

                    var studentsMeliCode = students.Select(c => c.IdNumber2.Trim().PersianToEnglishDigit());

                    studentsMeliCode.Where(c => !logins.Select(l => l.Username).Contains(c)).ToList().ForEach(absenceMeliCode =>
                    {
                        var std = students.FirstOrDefault(c => c.IdNumber2 == absenceMeliCode);

                        logins.Add(new
                        {
                            Id = (long)0,
                            FullName = std.LastName + " - " + std.Name,
                            Username = absenceMeliCode,
                            CategoryAuthorizeState = Enum.GetName(typeof(CategoryAuthorizeState), cat.AuthorizeState),
                            UserType = "PMA",
                            DateString = "غائب",
                            GradeId = 0,
                            ClassId = 0,
                            ip = "---",
                            absence = true
                        });
                    });
                }

                return this.DataFunction(true, logins);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        [Role(RolePrefix.View, roleTitle)]
        public async Task<IActionResult> Get([FromBody] getCategoryParam param)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var getparams = param.getparams;

                int count;

                var query = getparams.q;

                var cls = db.Categories
                        .Include(c => c.ParentCategory)
                        .Include(c => c.Children)
                    .Where(c => c.Type == (CategoryTotalType)param.type)
                .AsQueryable();

                if (param.archiveType.HasValue && param.archiveType.Value != 3)
                {
                    if (param.archiveType.Value == 1)
                    {
                        cls = cls.Where(c => !c.IsArchived);
                    }
                    if (param.archiveType.Value == 2)
                    {
                        cls = cls.Where(c => c.IsArchived);
                    }
                }

                if ((CategoryTotalType)param.type == CategoryTotalType.onlineExam)
                {
                    cls = cls.Where(c => c.GradeId.HasValue ? c.Grade.YeareducationId == nowYeareducationId : true);
                }

                if (param.selectedGradeId.HasValue)
                {
                    cls = cls.Where(c => c.GradeId == param.selectedGradeId.Value);
                }

                if (param.selectedCourseId.HasValue)
                {
                    cls = cls.Where(c => c.CourseId == param.selectedCourseId);
                }

                if (param.selectedExamTypeId.HasValue)
                {
                    cls = cls.Where(c => c.ExamTypeId == param.selectedExamTypeId);
                }

                if (!string.IsNullOrWhiteSpace(query))
                {

                    cls = cls.Where(c => c.Title.Contains(query) || c.ParentCategory.Title.Contains(query) ||
                                    c.Id.ToString().Contains(query));
                }

                count = cls.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        cls = cls.OrderBy(c => c.Title);
                    }
                    if (getparams.sort.Equals("parentTitle"))
                    {
                        cls = cls.OrderBy(c => c.ParentCategory.Title);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        cls = cls.OrderByDescending(c => c.Title);
                    }
                    if (getparams.sort.Equals("parentTitle"))
                    {
                        cls = cls.OrderByDescending(c => c.ParentCategory.Title);
                    }
                }
                else
                {
                    cls = cls.OrderByDescending(c => c.IsPined).ThenByDescending(c => c.Id);
                }

                cls = cls.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                cls = cls.Take(getparams.pageSize);

                var q = await cls
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        parentTitle = c.ParentCategory == null || c.ParentId == null ? "ریشه" : c.ParentCategory.Title,
                        gradeString = c.Grade == null ? "" : c.Grade.Name,
                        classString = c.Class == null ? "" : c.Class.Name,
                        isPined = c.IsPined,
                        isActive = c.IsActive,
                        haveInfo = c.HaveInfo,
                        isInfoShow = c.IsInfoShow,
                        courseString = c.Course != null ? c.Course.Name : "",
                        examTypeString = c.ExamType != null ? c.ExamType.Name : "",
                        workbookString = c.Workbook != null ? c.Workbook.Name : "",
                        datePublishString = c.DatePublish.Value.ToPersianDate(),
                        timePublish = c.DatePublish.Value.ToShortTimeString(),
                        dateExpireString = c.DateExpire.Value.ToPersianDate(),
                        timeExpire = c.DateExpire.Value.ToShortTimeString(),
                        headerPicUrl = string.IsNullOrEmpty(c.HeaderPicUrl) ? c.RegisterPicUrl : c.HeaderPicUrl,
                        isArchived = c.IsArchived,
                        CalculateNegativeScore = c.CalculateNegativeScore
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
        [AllowAnonymous]
        public async Task<IActionResult> getCategory([FromBody] int id)
        {
            try
            {
                if (id != 0)
                {

                    var cat = await db.Categories.SingleAsync(c => c.Id == id);
                    if (cat == null)
                    {
                        return this.UnSuccessFunction("Data Not Found", "error");
                    }

                    return this.SuccessFunction(data: cat);
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
        [Role(RolePrefix.View, roleTitle)]
        public async Task<IActionResult> getQuestionIds([FromBody] int catId)
        {
            try
            {
                var cat = await db.Categories
                        .Include(c => c.Attributes)
                    .FirstOrDefaultAsync(c => c.Id == catId);

                var questions = cat.Attributes.Where(c => c.QuestionId.HasValue).Select(c => c.QuestionId.Value).ToList();

                return this.DataFunction(true, questions);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getOnlineExams()
        {
            try
            {
                var catList = await db.Categories
                    .Where(c => c.Type == CategoryTotalType.onlineExam && !c.IsArchived && c.IsActive == true)
                    .Where(c => c.DateExpire > DateTime.Now)
                .OrderBy(c => c.DatePublish)
                .Select(c => new
                {
                    Id = c.Id,
                    Title = c.Title,
                    gradeString = c.Grade == null ? "" : c.Grade.Name,
                    classString = c.Class == null ? "" : c.Class.Name,
                    courseString = c.Course != null ? c.Course.Name : "",
                    examTypeString = c.ExamType != null ? c.ExamType.Name : "",
                    workbookString = c.Workbook != null ? c.Workbook.Name : "",
                    datePublishString = c.DatePublish.Value.ToPersianDate(),
                    timePublish = c.DatePublish.Value.ToShortTimeString(),
                    dateExpireString = c.DateExpire.Value.ToPersianDate(),
                    timeExpire = c.DateExpire.Value.ToShortTimeString(),
                    headerPicUrl = string.IsNullOrEmpty(c.HeaderPicUrl) ? c.RegisterPicUrl : c.HeaderPicUrl,
                    canStartExam = DateTime.Now >= c.DatePublish
                }).ToListAsync();

                return this.SuccessFunction(data: catList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getSearchedAttrs([FromBody] int catId)
        {
            try
            {
                var attrs = await db.Attributes.Where(c => c.CategoryId == catId && c.IsInSearch == true)
                    .Select(c => new
                    {
                        Id = c.Id,
                        AttrTypeInt = c.AttrTypeToInt(c.AttrType),
                        UnitId = c.UnitId,
                        UnitOrder = c.Unit.Order,
                        Title = c.AttrType == AttrType.Question ? c.Question.Title : c.Title,
                        Placeholder = c.Placeholder,
                        Desc = c.Desc,
                        Values = c.Values,
                        MaxFileSize = c.MaxFileSize,
                        CategoryId = c.CategoryId,
                        AttributeOptions = c.getAttributeOptions(true, c.AttrType, c.AttributeOptions, c.Question == null ? new List<QuestionOption>() : c.Question.QuestionOptions.ToList(), true),
                        Score = c.Score,
                        queId = c.QuestionId.HasValue ? c.QuestionId.Value : 0,
                        QuestionType = c.AttrType == AttrType.Question ? (int)c.Question.Type : 0,
                        ComplatabelContent = c.Question == null ? "" : c.Question.ComplatabelContent,
                        IsInClient = c.IsInClient,
                        IsRequired = false,
                        IsUniq = c.IsUniq,
                        IsInShowInfo = c.IsInShowInfo,
                        IsInSearch = c.IsInSearch,
                        Order = c.Order,
                        questiondefactString = c.Question != null ? c.Question.getDefctString(c.Question.Defact) : "",
                        questionperson = c.Question != null ? c.Question.Person : "",
                        questiontypeString = c.Question != null ? c.Question.getTypeString(c.Question.Type) : "",
                        questionname = c.Question != null ? c.Question.Name : "",
                        questiontype = c.Question != null ? (int)c.Question.Type : 0,
                        questionDefactInt = c.Question != null ? (int)c.Question.Defact : 0,
                        canDeleteAttr = c.CategoryId == catId
                    })
                    .OrderBy(c => c.UnitOrder).ThenBy(c => c.UnitId)
                        .ThenBy(c => c.Order).ThenBy(c => c.queId).ThenBy(c => c.Id)
                .ToListAsync();

                return this.SuccessFunction(data: attrs);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Remove, roleTitle)]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            try
            {
                var removingFilePaths = new List<string>();

                foreach (var id in ids)
                {
                    if (id != 0)
                    {

                        var cat = await db.Categories
                            .Include(c => c.Children)
                            .Include(c => c.Attributes)
                            .Include(c => c.Items)
                                .ThenInclude(c => c.ItemAttribute)
                        .SingleAsync(c => c.Id == id);


                        if (cat == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (cat.Children.Any())
                        {
                            return this.UnSuccessFunction("این فرم دارای زیرمجموعه می باشد نمیتوان آن را حذف کرد", "warning");
                        }
                        if (cat.Attributes.Any() && !cat.IsArchived)
                        {
                            return this.UnSuccessFunction("این فرم دارای فیلد هایی می باشد نمیتوان آن را حذف کرد", "warning");
                        }

                        if (cat.Attributes.Any())
                        {
                            db.RemoveRange(cat.Attributes);
                        }
                        if (cat.Items.Any())
                        {
                            var itemAttrs = cat.Items.SelectMany(c => c.ItemAttribute);
                            removingFilePaths = itemAttrs.Select(c => c.AttrubuteValue).ToList();
                            db.RemoveRange(itemAttrs);
                            db.RemoveRange(cat.Items);
                        }


                        db.Categories.Remove(cat);

                    }
                    else
                    {
                        return this.UnSuccessFunction("Undefined Value", "error");
                    }
                }

                await db.SaveChangesAsync();

                removingFilePaths.ForEach(path =>
                {
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch { }
                });


                return this.SuccessFunction();

            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
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

            foreach (var i in Categories.Where(c => c.ParentId == null))
            {
                items.Add(new JsTreeModel
                {
                    id = i.Id.ToString(),
                    text = i.Title,
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

            foreach (var i in Categories.Where(c => c.ParentId == idd))
            {
                items.Add(new JsTreeModel
                {
                    id = i.Id.ToString(),
                    text = i.Title,
                    parent = i.ParentCategory.Id.ToString(),
                    children = i.Children.Any()
                });
            }

            return items;
        }

    }

    public class AbsenceForOnlineExamParam
    {
        public int catId { get; set; }
        public int saveScoreType { get; set; }
        public bool deleteUnknownDatas { get; set; }
    }

    public class getAllByTeacherParam
    {
        public int type { get; set; } = 0;
        public int teacherId { get; set; }
    }

    public class ChangeCheckablePropertyParam
    {
        public int catId { get; set; }
        public string type { get; set; }
        public bool check { get; set; }
    }

    public class AddRandomQuestionAttributeParam
    {
        public int catId { get; set; }
        public int selectedCourseForQuestion { get; set; }
        public int? veryHardQuestionNumber { get; set; }
        public int? hardQuestionNumber { get; set; }
        public int? mediumQuestionNumber { get; set; }
        public int? easyQuestionNumber { get; set; }
    }

    public class getCategoryParam
    {
        public getparams getparams { get; set; }

        public int type { get; set; }

        public int? selectedGradeId { get; set; }
        public int? selectedCourseId { get; set; }
        public int? selectedExamTypeId { get; set; }

        // 1: noArchive, 2: onlyArchived, 3: All
        public int? archiveType { get; set; }
    }

    public class AddCategoryParam
    {
        public int id { get; set; }

        public string title { get; set; }

        public string desc { get; set; }

        public int? parentId { get; set; }

        public DateTime datePublish { get; set; }
        public string timePublish { get; set; }


        public DateTime dateExpire { get; set; }
        public string timeExpire { get; set; }


        public bool isActive { get; set; }

        public string endMessage { get; set; }


        public CategoryAuthorizeState authorizeState { get; set; }

        public string license { get; set; }

        public bool haveInfo { get; set; }

        public bool isInfoShow { get; set; }

        public string activeMessage { get; set; }

        public string deActiveMessage { get; set; }

        public int roleAccess { get; set; }


        public bool haveEntringCard { get; set; }

        public string btnTitle { get; set; }


        public int showRow { get; set; }

        public int postType { get; set; }


        public string registerPicUrl { get; set; }

        public string showInfoPicUrl { get; set; }

        public string registerFileData { get; set; }
        public string registerFileName { get; set; }
        public string showInfoFileData { get; set; }
        public string showInfoFileName { get; set; }

        public string headerPicUrl { get; set; }
        public string headerPicData { get; set; }
        public string headerPicName { get; set; }



        public CategoryTotalType type { get; set; }

        public int? gradeId { get; set; }

        public int? classId { get; set; }

        public bool randomAttribute { get; set; }

        public bool randomAttributeOption { get; set; }

        public bool isPined { get; set; }

        public int[] teachersIdAccess { get; set; }

        public int? courseId { get; set; }

        public int? examTypeId { get; set; }

        public int? workbookId { get; set; }

        public bool showScoreAfterDone { get; set; }

        public bool calculateNegativeScore { get; set; }
        public bool isBackStepAllowed { get; set; }


        public bool useLimitedRandomQuestionNumber { get; set; }

        public int? veryHardQuestionNumber { get; set; }
        public int? hardQuestionNumber { get; set; }
        public int? moderateQuestionNumber { get; set; }
        public int? easyQuestionNumber { get; set; }

        public int registerItemStepType { get; set; }
    }

    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; }
    }
}