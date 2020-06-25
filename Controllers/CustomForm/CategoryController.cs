using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model;
using SCMR_Api.Model.Index;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class CategoryController : Controller
    {
        public Data.DbContext db;

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
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            try
            {

                DateTime datepublish = category.DatePublish.Value.AddDays(1);
                TimeSpan timepublish;
                if (!TimeSpan.TryParse(category.timePublish, out timepublish))
                {
                    return this.UnSuccessFunction("مقدار زمان شروع را وارد کنید");
                }
                datepublish = datepublish.Date + timepublish;


                DateTime dateex = category.DateExpire.Value.AddDays(1);
                TimeSpan timeex;
                if (!TimeSpan.TryParse(category.timeExpire, out timeex))
                {
                    return this.UnSuccessFunction("مقدار زمان انقضا را وارد کنید");
                }
                dateex = dateex.Date + timeex;

                category.DateExpire = dateex;
                category.DatePublish = datepublish;

                if (!string.IsNullOrEmpty(category.RegisterFileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, category.RegisterFileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(category.RegisterFileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.RegisterPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + category.RegisterFileName);
                }

                if (!string.IsNullOrEmpty(category.ShowInfoFileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, category.ShowInfoFileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(category.ShowInfoFileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.ShowInfoPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + category.ShowInfoFileName);
                }

                if (!string.IsNullOrEmpty(category.HeaderPicData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, category.HeaderPicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(category.HeaderPicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.HeaderPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + category.HeaderPicName);
                }

                db.Categories.Add(category);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] AddCategoryParam addCategory)
        {
            try
            {
                var cat = await db.Categories.Include(c => c.Children).SingleAsync(c => c.Id == addCategory.id);

                if (addCategory.parentId == cat.Id)
                {
                    return this.UnSuccessFunction("این فرم نمی تواند انتخاب شود c1");
                }

                if (await isInChildCat(cat.Id, addCategory.parentId))
                {
                    return this.UnSuccessFunction("این فرم نمی تواند انتخاب شود c2");
                }

                cat.ParentId = addCategory.parentId;
                cat.Title = addCategory.title;
                cat.Type = addCategory.type;
                cat.RandomAttribute = addCategory.randomAttribute;
                cat.RandomAttributeOption = addCategory.randomAttributeOption;

                cat.TeachersIdAccess = addCategory.teachersIdAccess;

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
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + cat.RegisterPicUrl);
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
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + cat.ShowInfoPicUrl);
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
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + cat.HeaderPicUrl);
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
                var catList = await db.Categories.ToListAsync();

                return this.SuccessFunction(data: catList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPined()
        {
            try
            {
                var catList = await db.Categories
                    .Where(c => c.IsPined)
                .ToListAsync();

                return this.SuccessFunction(data: catList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetAllPinedByType([FromBody] int type)
        {
            try
            {
                var catList = await db.Categories
                    .Where(c => c.IsPined && c.Type == (CategoryTotalType)type)
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
                var cats = await db.Categories.Where(c => c.TeachersIdAccess == null ? false : c.TeachersIdAccess.Contains(param.teacherId) && c.Type == (CategoryTotalType)param.type).ToListAsync();

                return this.DataFunction(true, cats);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
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

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddRandomQuestionAttribute([FromBody] AddRandomQuestionAttributeParam param)
        {
            try
            {
                var questionIds = new List<int>();

                var questions = await db.Questions
                    .Where(c => c.CourseId == param.selectedCourseForQuestion)
                    .OrderBy(c => Guid.NewGuid())
                .ToListAsync();

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
                            EnTitle = "Section"
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
                                Score = 1,
                                QuestionId = question.Id
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
        public async Task<IActionResult> getAllByType([FromBody] int type)
        {
            try
            {
                var cat = await db.Categories.Where(c => c.Type == (CategoryTotalType)type).ToListAsync();

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
                    .Where(c => c.Type == CategoryTotalType.registerForm)
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
                    .Where(c => c.Type == CategoryTotalType.registerForm)
                    .Where(c => DateTime.Now >= c.DatePublish && DateTime.Now <= c.DateExpire && c.IsActive == true && c.PostType == type)
                .ToListAsync();

                return this.SuccessFunction(data: cats);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromBody] getCategoryParam param)
        {
            try
            {
                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;

                var query = getparams.q;

                var cls = db.Categories
                        .Include(c => c.ParentCategory)
                        .Include(c => c.Children)
                    .Where(c => c.Type == (CategoryTotalType)param.type)
                .AsQueryable();

                if (param.selectedGradeId.HasValue)
                {
                    cls = cls.Where(c => c.GradeId == param.selectedGradeId.Value);
                }

                if (param.selectedClassId.HasValue)
                {
                    cls = cls.Where(c => c.ClassId == param.selectedClassId.Value);
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
                    cls = cls.OrderByDescending(c => c.IsPined);
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
        [AllowAnonymous]
        public async Task<IActionResult> getOnlineExams()
        {
            try
            {
                var catList = await db.Categories
                    .Where(c => c.Type == CategoryTotalType.onlineExam)
                    .Where(c => DateTime.Now >= c.DatePublish && DateTime.Now <= c.DateExpire && c.IsActive == true)
                .ToListAsync();

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
                var attrs = await db.Attributes.Where(c => c.CategoryId == catId && c.IsInSearch == true).ToListAsync();

                return this.SuccessFunction(data: attrs);
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

                        var cat = await db.Categories.Include(c => c.Children).Include(c => c.Attributes).SingleAsync(c => c.Id == id);
                        if (cat == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (cat.Children.Any())
                        {
                            return this.UnSuccessFunction("این فرم دارای زیرمجموعه می باشد نمیتوان آن را حذف کرد", "warning");
                        }
                        if (cat.Attributes.Any())
                        {
                            return this.UnSuccessFunction("این فرم دارای فیلد هایی می باشد نمیتوان آن را حذف کرد", "warning");
                        }


                        db.Categories.Remove(cat);

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
        public int? hardQuestionNumber { get; set; }
        public int? mediumQuestionNumber { get; set; }
        public int? easyQuestionNumber { get; set; }
    }

    public class getCategoryParam
    {
        public getparams getparams { get; set; }

        public int type { get; set; }

        public int? selectedGradeId { get; set; }
        public int? selectedClassId { get; set; }
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

    }

    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; }
    }
}