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
        public async Task<IActionResult> Add([FromBody] AddCategoryParam addCategory)
        {
            try
            {

                var category = new Category
                {
                    Title = addCategory.title,
                    ParentId = addCategory.parentId,
                    IsActive = addCategory.isActive,
                    Desc = addCategory.desc,
                    EndMessage = addCategory.endMessage,
                    HaveInfo = addCategory.haveInfo,
                    IsInfoShow = addCategory.isInfoShow,
                    ActiveMessage = addCategory.activeMessage,
                    DeActiveMessage = addCategory.deActiveMessage,
                    RoleAccess = addCategory.roleAccess,
                    HaveEntringCard = addCategory.haveEntringCard,
                    BtnTitle = addCategory.btnTitle,
                    ShowRow = (ShowRow)addCategory.showRow,
                    PostType = addCategory.postType,
                    AuthorizeState = addCategory.authorizeState,
                    License = addCategory.license
                };

                DateTime datepublish = addCategory.datePublish.AddDays(1);
                TimeSpan timepublish;
                if (!TimeSpan.TryParse(addCategory.timePublish, out timepublish))
                {
                    return this.UnSuccessFunction("مقدار زمان شروع را وارد کنید");
                }
                datepublish = datepublish.Date + timepublish;


                DateTime dateex = addCategory.dateExpire.AddDays(1);
                TimeSpan timeex;
                if (!TimeSpan.TryParse(addCategory.timeExpire, out timeex))
                {
                    return this.UnSuccessFunction("مقدار زمان انقضا را وارد کنید");
                }
                dateex = dateex.Date + timeex;

                category.DateExpire = dateex;
                category.DatePublish = datepublish;

                if (!string.IsNullOrEmpty(addCategory.registerFileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, addCategory.registerFileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(addCategory.registerFileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.RegisterPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + addCategory.registerFileName);
                }

                if (!string.IsNullOrEmpty(addCategory.showInfoFileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, addCategory.showInfoFileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(addCategory.showInfoFileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.ShowInfoPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + addCategory.showInfoFileName);
                }

                if (!string.IsNullOrEmpty(addCategory.headerPicData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, addCategory.headerPicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(addCategory.headerPicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    category.HeaderPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + addCategory.headerPicName);
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
        [AllowAnonymous]
        public async Task<IActionResult> getAllIndex()
        {
            try
            {
                var catList = await db.Categories
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
        public async Task<IActionResult> Get([FromBody] getparams getparams)
        {
            try
            {
                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var cls = db.Categories
                    .Include(c => c.ParentCategory)
                    .Include(c => c.Children)
                .AsQueryable();


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
                    cls = cls.OrderBy(c => c.Id);
                }

                cls = cls.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                cls = cls.Take(getparams.pageSize);

                var q = await cls.ToListAsync();

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
    }

    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; }
    }
}