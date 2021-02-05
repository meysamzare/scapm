using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SCMR_Api.Model;
using Newtonsoft.Json;
using System.Drawing;
using System.Globalization;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ItemController : Controller
    {
        public Data.DbContext db;

        const string roleTitle = "Item";
        const string onlineExamRoleTitle = "OnlineExamResult";

        private IHostingEnvironment hostingEnvironment;
        private IHttpContextAccessor accessor;

        private IConfiguration _config;
        public ItemController(Data.DbContext _db, IHttpContextAccessor _accessor, IHostingEnvironment _hostingEnvironment, IConfiguration config)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            accessor = _accessor;
            _config = config;
        }


        [HttpPost]
        [Role(RolePrefix.Add, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> Add([FromBody] Item item)
        {
            try
            {
                Random random = new System.Random();
                int value = random.Next(1111111, 99999999);

                item.RahCode = value;

                item.DateAdd = DateTime.Now;

                db.Items.Add(item);

                await db.SaveChangesAsync();

                return this.SuccessFunction(data: item.Id);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> Edit([FromBody] EditItemParams item)
        {
            try
            {
                var i = await db.Items.SingleAsync(c => c.Id == item.id);

                i.Tags = item.tags;
                i.Title = item.title;
                i.CategoryId = item.categoryId;
                i.UnitId = item.unitId;
                i.IsActive = item.isActive;

                i.DateEdit = DateTime.Now;

                if (i.RahCode == 0)
                {
                    Random random = new System.Random();
                    int value = random.Next(1111111, 99999999);

                    i.RahCode = value;
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
        public async Task<IActionResult> Get([FromBody] getParamComplex getparamsComplex)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                getparamsComplex.param.pageIndex += 1;

                int count;


                var query = getparamsComplex.param.q;

                var items = db.Items
                        .Include(c => c.Category)
                    .Where(c => c.Category.Type == (CategoryTotalType)getparamsComplex.Type)
                    .Where(c => !c.Category.IsArchived)
                .AsQueryable();

                if ((CategoryTotalType)getparamsComplex.Type == CategoryTotalType.onlineExam)
                {
                    items = items.Where(c => c.Category.GradeId.HasValue ? c.Category.Grade.YeareducationId == nowYeareducationId : true);
                }


                if (getparamsComplex.catId.HasValue)
                {
                    items = items.Where(c => c.Category.Id == getparamsComplex.catId.Value);
                }

                var state = getparamsComplex.state;

                if (!string.IsNullOrEmpty(state))
                {
                    if (state == "both")
                    {
                        // Do Nothing
                    }
                    else if (state == "active")
                    {
                        items = items.Where(c => c.IsActive == true);
                    }
                    else if (state == "deactive")
                    {
                        items = items.Where(c => c.IsActive == false);
                    }
                }



                if (!string.IsNullOrWhiteSpace(query))
                {
                    items = items.Where(c => c.Title.Contains(query) || c.Id.ToString().Contains(query) ||
                            c.RahCode.ToString().Contains(query) ||
                            c.Unit.Title.Contains(query) || c.Tags.Contains(query) || c.Id.ToString().Contains(query));
                }



                if (getparamsComplex.attrvalsearch.Any())
                {
                    var itemAttrs = db.ItemAttributes.AsQueryable();

                    IList<IList<Item>> tempItem = new List<IList<Item>>();

                    for (int i = 0; i < getparamsComplex.attrvalsearch.Count; i++)
                    {
                        tempItem.Add(new List<Item>());
                    }

                    foreach (var attrvalsearch in getparamsComplex.attrvalsearch.WithIndex())
                    {
                        if (!string.IsNullOrWhiteSpace(attrvalsearch.item.val))
                        {
                            var itemAttrsForAttrId = itemAttrs
                                .Where(c => c.AttributeId == attrvalsearch.item.attrId && c.AttrubuteValue.Contains(attrvalsearch.item.val))
                                .Include(c => c.Item)
                                    .ThenInclude(c => c.Category);

                            foreach (var itemAttr in itemAttrsForAttrId.WithIndex())
                            {
                                if (attrvalsearch.index != 0)
                                {
                                    if (tempItem[attrvalsearch.index - 1].Contains(itemAttr.item.Item))
                                    {
                                        tempItem[attrvalsearch.index].Add(itemAttr.item.Item);
                                    }
                                }
                                else
                                {
                                    if (!tempItem[0].Contains(itemAttr.item.Item) && items.Contains(itemAttr.item.Item))
                                    {
                                        tempItem[0].Add(itemAttr.item.Item);
                                    }
                                }

                            }
                        }
                    }

                    items = tempItem[getparamsComplex.attrvalsearch.Count - 1].AsQueryable();

                }



                count = items.Count();


                if (getparamsComplex.param.direction.Equals("asc"))
                {
                    if (getparamsComplex.param.sort.Equals("id"))
                    {
                        items = items.OrderBy(c => c.Id);
                    }
                    if (getparamsComplex.param.sort.Equals("title"))
                    {
                        items = items.OrderBy(c => c.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("isActive"))
                    {
                        items = items.OrderBy(c => c.IsActive);
                    }
                    if (getparamsComplex.param.sort.Equals("rahCode"))
                    {
                        items = items.OrderBy(c => c.RahCode);
                    }
                    if (getparamsComplex.param.sort.Equals("categoryString"))
                    {
                        items = items.OrderBy(c => c.Category.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("unitString"))
                    {
                        items = items.OrderBy(c => c.Unit.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("date"))
                    {
                        items = items.OrderBy(c => c.DateAdd).ThenBy(c => c.DateEdit);
                    }
                }
                else if (getparamsComplex.param.direction.Equals("desc"))
                {
                    if (getparamsComplex.param.sort.Equals("id"))
                    {
                        items = items.OrderByDescending(c => c.Id);
                    }
                    if (getparamsComplex.param.sort.Equals("title"))
                    {
                        items = items.OrderByDescending(c => c.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("isActive"))
                    {
                        items = items.OrderByDescending(c => c.IsActive);
                    }
                    if (getparamsComplex.param.sort.Equals("rahCode"))
                    {
                        items = items.OrderByDescending(c => c.RahCode);
                    }
                    if (getparamsComplex.param.sort.Equals("categoryString"))
                    {
                        items = items.OrderByDescending(c => c.Category.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("unitString"))
                    {
                        items = items.OrderByDescending(c => c.Unit.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("date"))
                    {
                        items = items.OrderByDescending(c => c.DateAdd).ThenByDescending(c => c.DateEdit);
                    }
                }
                else
                {
                    items = items.OrderByDescending(c => c.Id);
                }

                items = items.Skip((getparamsComplex.param.pageIndex - 1) * getparamsComplex.param.pageSize);
                items = items.Take(getparamsComplex.param.pageSize);

                var q = items
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.AuthorizedUsername == "---" ? c.Title : $"{c.AuthorizedFullName} - {c.AuthorizedUsername}",
                        IsActive = c.IsActive,
                        RahCode = c.RahCode,
                        CategoryString = c.Category != null ? c.Category.Title : "",
                        dateAddPersian = c.DateAdd.HasValue ? c.DateAdd.Value.ToPersianDateWithTime() : "",
                        dateEditPersian = c.DateEdit.HasValue ? c.DateEdit.Value.ToPersianDateWithTime() : "",
                        CategoryId = c.CategoryId
                    })
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
        [AllowAnonymous]
        public async Task<IActionResult> IsRahCodeExist([FromBody] RahCodeParam rahcodeparam)
        {
            try
            {
                if (await db.Items.AnyAsync(c => c.RahCode == rahcodeparam.rahCode && c.CategoryId == rahcodeparam.catId))
                {
                    return this.SuccessFunction(data: DateTime.Now.AddMinutes(5));
                }

                return this.UnSuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getItemByRahCode([FromBody] int rahCode)
        {
            try
            {

                if (rahCode != 0)
                {
                    var item = await db.Items.SingleOrDefaultAsync(c => c.RahCode == rahCode);

                    return this.DataFunction(true, item);
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
        public async Task<IActionResult> getItemShowInfoAttr([FromBody] int itemId)
        {
            try
            {
                if (itemId != 0)
                {
                    var item = await db.Items.FirstOrDefaultAsync(c => c.Id == itemId);

                    Categories = await db.Categories
                        .Include(c => c.Attributes)
                            .ThenInclude(c => c.AttributeOptions)
                        .Include(c => c.Attributes)
                            .ThenInclude(c => c.Question)
                                .ThenInclude(c => c.QuestionOptions)
                        .Include(c => c.ParentCategory)
                    .ToListAsync();

                    var attrs = await getAttrForCat(item.CategoryId);

                    var attributes = attrs
                        .Where(c => c.IsInShowInfo).OrderBy(c => c.OrderInInfo).ThenBy(c => c.Id)
                    .Select(c => new RegisterItemAttrs
                    {
                        Id = c.Id,
                        AttrTypeInt = c.AttrTypeToInt(c.AttrType),
                        UnitId = c.UnitId,
                        IsRequired = c.IsRequired,
                        IsUniq = c.IsUniq,
                        Title = c.AttrType == AttrType.Question ? c.Question.Title : c.Title,
                        Placeholder = c.Placeholder,
                        Desc = c.Desc,
                        Values = c.Values,
                        MaxFileSize = c.MaxFileSize,
                        IsMeliCode = c.IsMeliCode,
                        AttributeOptions = c.getAttributeOptions(false, c.AttrType, c.AttributeOptions, c.Question == null || c.Question.QuestionOptions == null ? new List<QuestionOption>() : c.Question.QuestionOptions.ToList()),
                        QuestionType = c.AttrType == AttrType.Question ? (int)c.Question.Type : 0,
                        QuestionDefct = c.Question != null ? c.Question.Defact : 0,
                        ComplatabelContent = c.Question == null ? "" : c.Question.ComplatabelContent
                    });

                    return this.DataFunction(true, attributes);
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
        public async Task<IActionResult> getAll()
        {
            try
            {
                var items = await db.Items
                    .Include(c => c.Category)
                    .Include(c => c.Unit)
                .ToListAsync();

                return this.DataFunction(true, items);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getItem([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {

                    var it = await db.Items
                        .Include(c => c.Category)
                    .SingleAsync(c => c.Id == id);

                    if (it == null)
                    {
                        return this.UnSuccessFunction("Data Not Found", "error");
                    }

                    return this.DataFunction(true, new { item = it });
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

        List<Model.Attribute> attrList = new List<Model.Attribute>();

        public IEnumerable<Category> Categories
        {
            get; set;
        }

        public async Task<IEnumerable<Model.Attribute>> getAttrForCat(int catId)
        {
            var cat = Categories.Single(c => c.Id == catId);

            if (cat.Attributes.Count != 0)
            {
                foreach (var i in cat.Attributes)
                {
                    attrList.Add(i);
                }

                if (cat.ParentCategory != null)
                {
                    await getAttrForCat(cat.ParentCategory.Id);
                }
            }

            return attrList;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getItemAttrForItem([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {

                    var itemattrs = await db.ItemAttributes
                    .Where(c => c.ItemId == id)
                        .Include(c => c.Attribute)
                            .ThenInclude(c => c.Question)
                                .ThenInclude(c => c.QuestionOptions)
                    .Select(c => new
                    {
                        attributeId = c.AttributeId,
                        itemId = c.ItemId,
                        attrubuteValue = c.AttrubuteValue,
                        attributeFilePath = c.AttributeFilePath,
                        score = c.Score,
                        scoreString = c.getScore(c, c.Attribute, c.Score)
                    })
                    .ToListAsync();

                    return this.DataFunction(true, itemattrs);
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
        [Role(RolePrefix.Remove, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    if (id != 0)
                    {

                        var item = await db.Items
                            .Include(c => c.ItemAttribute)
                            .Include(c => c.Category)
                            .ThenInclude(c => c.Attributes)
                            .Include(c => c.Unit)
                        .SingleAsync(c => c.Id == id);

                        if (item == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        foreach (var itemAttr in item.ItemAttribute)
                        {
                            if (!string.IsNullOrEmpty(itemAttr.AttributeFilePath))
                            {
                                if (!string.IsNullOrEmpty(itemAttr.AttrubuteValue))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(itemAttr.AttrubuteValue);
                                    }
                                    catch { }
                                }
                            }
                        }

                        db.ItemAttributes.RemoveRange(item.ItemAttribute);

                        db.Items.Remove(item);
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
        [Role(RolePrefix.View, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> getItemAttr([FromBody] int itemid)
        {
            try
            {
                var itemAttrs = await db.ItemAttributes.Where(c => c.ItemId == itemid).ToListAsync();

                return this.DataFunction(true, itemAttrs);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> SetItemAttributeScore([FromBody] SetItemAttributeScoreParam param)
        {
            try
            {
                var itemAttr = await db.ItemAttributes.FirstOrDefaultAsync(c => c.ItemId == param.itemAttr.ItemId && c.AttributeId == param.itemAttr.AttributeId);

                itemAttr.Score = param.score;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> setTagsGroup([FromBody] setTagsGroupParam settags)
        {
            try
            {
                foreach (var id in settags.itemsIds)
                {
                    var item = await db.Items.SingleAsync(c => c.Id == id);

                    item.Tags = settags.tags;
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
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> setItemAttr([FromBody] SetItemAttrParams attrParams)
        {
            try
            {
                var itemid = attrParams.itemId;
                var attrid = attrParams.attrId;
                var value = attrParams.inputValue;

                Item i = await db.Items.SingleAsync(c => c.Id == itemid);
                var attr = await db.Attributes.SingleAsync(c => c.Id == attrid);


                if (await db.ItemAttributes.AnyAsync(c => c.AttributeId == attrid && c.ItemId == itemid))
                {
                    if (string.IsNullOrEmpty(value))
                    {

                        var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                        db.ItemAttributes.Remove(ia);

                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                        ia.AttrubuteValue = value;


                        await db.SaveChangesAsync();

                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        //Do Nothing
                    }
                    else
                    {
                        ItemAttribute ia = new ItemAttribute();

                        ia.Item = i;
                        ia.Attribute = attr;
                        ia.AttrubuteValue = value;
                        ia.AttributeFilePath = "";

                        db.ItemAttributes.Add(ia);

                        await db.SaveChangesAsync();
                    }
                }


                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> setItemAttrGroup([FromBody] SetItemAttrGroupParams attrParams)
        {
            try
            {

                foreach (var id in attrParams.itemsIds)
                {
                    var itemid = id;
                    var attrid = attrParams.attrId;
                    var value = attrParams.inputValue;

                    Item i = await db.Items.SingleAsync(c => c.Id == itemid);
                    var attr = await db.Attributes.SingleAsync(c => c.Id == attrid);


                    if (await db.ItemAttributes.AnyAsync(c => c.AttributeId == attrid && c.ItemId == itemid))
                    {
                        if (string.IsNullOrEmpty(value))
                        {

                            var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                            db.ItemAttributes.Remove(ia);

                            await db.SaveChangesAsync();

                        }
                        else
                        {
                            var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                            ia.AttrubuteValue = value;


                            await db.SaveChangesAsync();

                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            //Do Nothing
                        }
                        else
                        {
                            ItemAttribute ia = new ItemAttribute();

                            ia.Item = i;
                            ia.Attribute = attr;
                            ia.AttrubuteValue = value;
                            ia.AttributeFilePath = "";

                            db.ItemAttributes.Add(ia);

                            await db.SaveChangesAsync();
                        }
                    }

                }


                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> setItemAttrForFiles()
        {
            try
            {
                var itemAttr = JsonConvert.DeserializeObject<dynamic>(Request.Form.FirstOrDefault(c => c.Key == "object").Value);

                var files = Request.Form.Files;

                string filename = itemAttr.fileName;

                var file = files.FirstOrDefault(c => c.FileName == filename);

                int itemid = itemAttr.itemId;
                int attrid = itemAttr.attributeId;

                string guidd = "";

                var item = await db.Items.SingleAsync(c => c.Id == itemid);
                var attr = await db.Attributes.SingleAsync(c => c.Id == attrid);

                if (await db.ItemAttributes.AnyAsync(c => c.AttributeId == attrid && c.ItemId == itemid))
                {
                    var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                    //Remove Old File And Save New File
                    try
                    {
                        System.IO.File.Delete(ia.AttrubuteValue);
                    }
                    catch { }

                    var guid = System.Guid.NewGuid().ToString();
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));
                    string fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, fileName);

                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    var filepath = Path.Combine("/UploadFiles", guid, fileName);

                    ia.AttrubuteValue = path;
                    ia.AttributeFilePath = filepath;

                    guidd = filepath;

                    await db.SaveChangesAsync();
                }
                else
                {
                    ItemAttribute ia = new ItemAttribute();

                    var guid = System.Guid.NewGuid().ToString();
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));
                    string fileName = System.Net.Http.Headers.ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, fileName);

                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    var filepath = Path.Combine("/UploadFiles", guid, fileName);

                    guidd = filepath;


                    ia.ItemId = itemid;
                    ia.AttributeId = attrid;
                    ia.AttrubuteValue = path;
                    ia.AttributeFilePath = filepath;

                    db.ItemAttributes.Add(ia);

                    await db.SaveChangesAsync();

                }


                return this.SuccessFunction(data: guidd);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Remove, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> removeItemAttrFile([FromBody] removeItemAttrFileParam param)
        {
            try
            {
                var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == param.attrId && c.ItemId == param.itemId);
                try
                {
                    System.IO.File.Delete(ia.AttrubuteValue);
                }
                catch { }

                db.ItemAttributes.Remove(ia);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> setItemAttrForFilesGroup([FromBody] ItemAttrForFileGroupParams itemAttrForFileParams)
        {
            try
            {
                foreach (var id in itemAttrForFileParams.itemsIds)
                {

                    var itemid = id;
                    var attrid = itemAttrForFileParams.attrId;
                    string guidd = "";

                    var item = await db.Items.SingleAsync(c => c.Id == itemid);
                    var attr = await db.Attributes.SingleAsync(c => c.Id == attrid);

                    if (await db.ItemAttributes.AnyAsync(c => c.AttributeId == attrid && c.ItemId == itemid))
                    {
                        if (string.IsNullOrEmpty(itemAttrForFileParams.inputValue))
                        {

                            var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);
                            try
                            {
                                System.IO.File.Delete(ia.AttrubuteValue);
                            }
                            catch { }

                            db.ItemAttributes.Remove(ia);

                            await db.SaveChangesAsync();

                        }
                        else
                        {
                            var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                            //Remove Old File And Save New File
                            try
                            {
                                System.IO.File.Delete(ia.AttrubuteValue);
                            }
                            catch { }

                            var guid = System.Guid.NewGuid().ToString();

                            var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, itemAttrForFileParams.fileName);
                            Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                            byte[] bytes = Convert.FromBase64String(itemAttrForFileParams.inputValue);
                            System.IO.File.WriteAllBytes(path, bytes);

                            ia.AttrubuteValue = path;
                            ia.AttributeFilePath = Path.Combine("/UploadFiles/" + guid + "/" + itemAttrForFileParams.fileName);

                            guidd = Path.Combine("/UploadFiles/" + guid + "/" + itemAttrForFileParams.fileName);


                            await db.SaveChangesAsync();

                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(itemAttrForFileParams.inputValue))
                        {
                            //Do Nothing
                        }
                        else
                        {
                            ItemAttribute ia = new ItemAttribute();

                            var guid = System.Guid.NewGuid().ToString();

                            var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, itemAttrForFileParams.fileName);
                            Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                            byte[] bytes = Convert.FromBase64String(itemAttrForFileParams.inputValue);
                            System.IO.File.WriteAllBytes(path, bytes);

                            guidd = Path.Combine("/UploadFiles/" + guid + "/" + itemAttrForFileParams.fileName);


                            ia.Item = item;
                            ia.Attribute = attr;
                            ia.AttrubuteValue = path;
                            ia.AttributeFilePath = Path.Combine("/UploadFiles/" + guid + "/" + itemAttrForFileParams.fileName);

                            db.ItemAttributes.Add(ia);

                            await db.SaveChangesAsync();
                        }
                    }

                }

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginForRegisterCat([FromBody] LoginForRegisterCatParam param)
        {
            try
            {
                param.username = param.username.Trim().PersianToEnglishDigit();

                var cat = await db.Categories
                    .Include(c => c.Items)
                        .ThenInclude(c => c.ItemAttribute)
                            .ThenInclude(c => c.Attribute)
                .FirstOrDefaultAsync(c => c.Id == param.catId);

                var isValid = false;

                var anyTeacher = await db.Teachers
                    .Include(c => c.OrgPerson)
                            .AnyAsync(c => c.OrgPerson.IdNum == param.username && c.Password == param.password);

                var anyParent = await db.Students
                            .AnyAsync(c => c.IdNumber2 == param.username && c.ParentsPassword == param.password && c.ParentAccess);

                var anyStudent = await db.Students
                            .AnyAsync(c => c.IdNumber2 == param.username && c.StudentPassword == param.password && c.StudentAccess);

                var userType = CategoryAuthorizeState.none;
                var userFullname = "";

                switch (cat.AuthorizeState)
                {
                    case CategoryAuthorizeState.TMA:
                        isValid = anyTeacher;
                        if (anyTeacher)
                        {
                            userType = CategoryAuthorizeState.TMA;
                            var teacher = await db.Teachers.Include(c => c.OrgPerson).FirstOrDefaultAsync(c => c.OrgPerson.IdNum == param.username && c.Password == param.password);
                            userFullname = teacher.Name;
                        }
                        break;
                    case CategoryAuthorizeState.PMA:
                        isValid = anyParent;
                        if (anyParent)
                        {
                            userType = CategoryAuthorizeState.PMA;
                            var std = await db.Students.FirstOrDefaultAsync(c => c.IdNumber2 == param.username && c.ParentsPassword == param.password);
                            userFullname = std.LastName + " - " + std.Name;
                        }
                        break;
                    case CategoryAuthorizeState.SMA:
                        isValid = anyStudent;
                        if (anyStudent)
                        {
                            userType = CategoryAuthorizeState.SMA;
                            var std = await db.Students.FirstOrDefaultAsync(c => c.IdNumber2 == param.username && c.ParentsPassword == param.password);
                            userFullname = std.LastName + " - " + std.Name;
                        }
                        break;
                    case CategoryAuthorizeState.All:
                        isValid = (anyParent || anyStudent || anyStudent);
                        if (anyTeacher)
                        {
                            userType = CategoryAuthorizeState.TMA;
                            var teacher = await db.Teachers.Include(c => c.OrgPerson).FirstOrDefaultAsync(c => c.OrgPerson.IdNum == param.username && c.Password == param.password);
                            userFullname = teacher.Name;
                        }
                        else if (anyParent)
                        {
                            userType = CategoryAuthorizeState.PMA;
                            var std = await db.Students.FirstOrDefaultAsync(c => c.IdNumber2 == param.username && c.ParentsPassword == param.password);
                            userFullname = std.LastName + " - " + std.Name;
                        }
                        else if (anyStudent)
                        {
                            userType = CategoryAuthorizeState.SMA;
                            var std = await db.Students.FirstOrDefaultAsync(c => c.IdNumber2 == param.username && c.ParentsPassword == param.password);
                            userFullname = std.LastName + " - " + std.Name;
                        }
                        break;

                    default:
                        isValid = false;
                        break;
                }

                var catTypeTitle = cat.Type == CategoryTotalType.onlineExam ? "آزمون آنلاین" : "نمون برگ";

                if (!isValid)
                {
                    return this.UnSuccessFunction(" نام کاربری و یا کلمه عبور شما اشتباه است و یا شما مجوز مشاهده این " + catTypeTitle + " را ندارید!");
                }

                if (cat.Items.Any(c => c.AuthorizedUsername == param.username) || cat.Items.SelectMany(c => c.ItemAttribute.Where(l => l.Attribute.IsMeliCode).Select(l => l.AttrubuteValue)).Contains(param.username))
                {
                    return this.UnSuccessFunction("داده ای با این نام کاربری قبلا در سیستم ثبت شده است");
                }

                var gradeId = 0;
                var classId = 0;

                if (cat.Type == CategoryTotalType.onlineExam && cat.AuthorizeState == CategoryAuthorizeState.PMA)
                {
                    var studentStdClassMng = await db.Students.Where(c => c.IdNumber2 == param.username)
                        .SelectMany(c => c.StdClassMngs).Where(c => c.IsActive).FirstOrDefaultAsync();

                    gradeId = studentStdClassMng.GradeId;
                    classId = studentStdClassMng.ClassId;

                    if (cat.GradeId != studentStdClassMng.GradeId || (cat.ClassId.HasValue && cat.ClassId.Value != studentStdClassMng.ClassId))
                    {
                        return this.UnSuccessFunction("این آزمون آنلاین برای پایه و یا کلاس دیگری است! و شما مجاز به شرکت در آن نیستید.");
                    }
                }

                await db.RegisterItemLogins.AddAsync(new RegisterItemLogin
                {
                    CategoryId = cat.Id,
                    Date = DateTime.Now,
                    FullName = userFullname,
                    Username = param.username,
                    Password = param.password,
                    CategoryAuthorizeState = Enum.GetName(typeof(CategoryAuthorizeState), cat.AuthorizeState),
                    UserType = Enum.GetName(typeof(CategoryAuthorizeState), userType),
                    GradeId = gradeId,
                    ClassId = classId,
                    IP = accessor.HttpContext.Connection.RemoteIpAddress.ToString()
                });

                await db.SaveChangesAsync();

                return this.SuccessFunction(data: new
                {
                    userType = userType,
                    userFullname = userFullname,
                    userName = param.username
                });
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

        class UploadedFileType
        {
            public string fileName { get; set; }
            public string filePath { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        [DisableFormValueModelBinding]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> SetItemWithAttrs()
        {
            var uploadedFiles = new List<UploadedFileType>();

            try
            {
                ItemWithAttrsParams itemWithAttrs = null;

                var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), 10000);
                var reader = new MultipartReader(boundary, HttpContext.Request.Body);
                var section = await reader.ReadNextSectionAsync();


                var guidd = System.Guid.NewGuid().ToString();
                Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guidd));

                while (section != null)
                {
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                    if (hasContentDispositionHeader)
                    {
                        if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                        {
                            var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                                section, contentDisposition, ModelState,
                                new string[0], 10);

                            var str = System.Text.Encoding.Default.GetString(streamedFileContent);

                            itemWithAttrs = JsonConvert.DeserializeObject<ItemWithAttrsParams>(str);
                        }
                        else
                        {
                            var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                                section, contentDisposition, ModelState,
                                new string[0], 1000);

                            if (ModelState.IsValid)
                            {
                                string fileName = contentDisposition.FileName.Value;
                                var path = Path.Combine("/UploadFiles", guidd, fileName);
                                var directoryPath = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guidd, fileName);

                                using (var targetStream = System.IO.File.Create(directoryPath))
                                {
                                    await targetStream.WriteAsync(streamedFileContent);
                                }

                                uploadedFiles.Add(new UploadedFileType
                                {
                                    fileName = fileName,
                                    filePath = path
                                });
                            }
                        }
                    }

                    section = await reader.ReadNextSectionAsync();
                }

                if (itemWithAttrs == null)
                {
                    DeleteFiles(uploadedFiles);
                    return this.UnSuccessFunction("Incomplate Data");
                }

                var item = new Item
                {
                    CategoryId = itemWithAttrs.catId,
                    IsActive = false,
                    Tags = "",
                    Title = itemWithAttrs.itemAttrs.FirstOrDefault().AttrubuteValue,
                    UnitId = db.Units.FirstOrDefault().Id,
                    DateAdd = DateTime.Now,
                    AuthorizedUsername = itemWithAttrs.authorizedUsername,
                    AuthorizedFullName = itemWithAttrs.authorizedFullName
                };

                var cat = await db.Categories
                    .Include(c => c.Attributes)
                    .Include(c => c.Items)
                        .ThenInclude(c => c.ItemAttribute)
                .FirstOrDefaultAsync(c => c.Id == itemWithAttrs.catId);

                var catAttrIds = cat.Attributes.Select(c => c.Id);

                var itemAttrs = new List<ItemAttribute>();

                foreach (var itemAttr in itemWithAttrs.itemAttrs.Where(c => catAttrIds.Contains(c.AttributeId)))
                {
                    var filepath = "";
                    if (itemAttr.AttributeFilePath.Equals("1"))
                    {
                        var uploadedFile = uploadedFiles.SingleOrDefault(c => c.fileName == itemAttr.FileName);
                        if (uploadedFile != null)
                        {
                            filepath = uploadedFile.filePath;
                        }
                    }

                    var attrValue = itemAttr.AttrubuteValue.Trim().PersianToEnglishDigit();

                    itemAttrs.Add(new ItemAttribute
                    {
                        AttributeId = itemAttr.AttributeId,
                        AttrubuteValue = attrValue,
                        AttributeFilePath = filepath
                    });
                }

                var catUniqAttrIds = cat.Attributes.Where(c => c.IsUniq).Select(c => c.Id);

                var isAnySameValue = false;

                foreach (var shouldCheckItemAttr in itemAttrs.Where(c => catUniqAttrIds.Contains(c.AttributeId)))
                {
                    isAnySameValue = cat.Items.SelectMany(c => c.ItemAttribute)
                        .Where(c => c.AttributeId == shouldCheckItemAttr.AttributeId)
                    .Any(c => c.AttrubuteValue == shouldCheckItemAttr.AttrubuteValue);

                    if (isAnySameValue) { break; }
                }

                if (isAnySameValue)
                {
                    DeleteFiles(uploadedFiles);
                    return this.UnSuccessFunction("برخی از داده های شما به صورت تکراری وارد شده است!");
                }

                item.ItemAttribute = itemAttrs;

                var rahCode = new System.Random().Next(1111111, 99999999);

                item.RahCode = rahCode;

                await db.Items.AddAsync(item);

                await db.SaveChangesAsync();

                var itemId = item.Id;

                var scoreString = "";

                if (cat.ShowScoreAfterDone && cat.Type == CategoryTotalType.onlineExam)
                {
                    var itemIncluded = await db.Items
                        .Include(c => c.ItemAttribute)
                            .ThenInclude(c => c.Attribute)
                                .ThenInclude(c => c.Question)
                                    .ThenInclude(c => c.QuestionOptions)
                    .FirstOrDefaultAsync(c => c.Id == itemId);

                    var catTotalScore = cat.getTotalScore(
                        cat.Attributes.ToList(),
                        cat.UseLimitedRandomQuestionNumber,
                        cat.VeryHardQuestionNumber,
                        cat.HardQuestionNumber,
                        cat.ModerateQuestionNumber,
                        cat.EasyQuestionNumber);

                    var itemScore = itemIncluded.getTotalScore;

                    scoreString = cat.ShowScoreAfterDone && cat.Type == CategoryTotalType.onlineExam ? $"\n نمره فعلی شما: {itemScore} از {catTotalScore} \n" : "";
                }

                return this.CustomFunction(true, cat.EndMessage + scoreString, rahCode.ToString(), null);
            }
            catch (System.Exception e)
            {
                DeleteFiles(uploadedFiles);
                return this.CatchFunction(e);
            }
        }

        private void DeleteFiles(List<UploadedFileType> files)
        {
            files.ForEach(file =>
            {
                try
                {
                    System.IO.File.Delete(Path.Combine(hostingEnvironment.ContentRootPath, file.filePath));
                }
                catch { }
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CheckForUniqAttr([FromBody] CheckForUniqAttrParams uniqAttrParams)
        {
            try
            {
                var items = await db.Items.Include(c => c.ItemAttribute).Where(c => c.CategoryId == uniqAttrParams.catId).ToListAsync();

                foreach (var item in items)
                {
                    if (item.ItemAttribute.Any())
                    {
                        foreach (var itemAttr in item.ItemAttribute)
                        {
                            if (itemAttr.AttributeId == uniqAttrParams.attrId &&
                                itemAttr.AttrubuteValue.Trim().PersianToEnglishDigit().Equals(uniqAttrParams.val.Trim().PersianToEnglishDigit()))
                            {
                                return this.SuccessFunction();
                            }
                        }
                    }
                }

                return this.DataFunction(false, null);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.View, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> getItemsToExel([FromBody] int catId)
        {
            try
            {
                var items = await db.Items
                .Where(c => c.CategoryId == catId)
                    .Include(c => c.ItemAttribute)
                        .ThenInclude(c => c.Attribute)
                            .ThenInclude(m => m.Question)
                                .ThenInclude(m => m.QuestionOptions)
                .ToListAsync();

                var cat = await db.Categories.Include(c => c.Attributes).SingleAsync(c => c.Id == catId);

                Categories = await db.Categories
                        .Include(c => c.ParentCategory)
                        .Include(c => c.Attributes)
                            .ThenInclude(c => c.AttributeOptions)
                        .Include(c => c.Attributes)
                            .ThenInclude(c => c.Question)
                                .ThenInclude(c => c.QuestionOptions)
                    .ToListAsync();

                var attributes = await getAttrForCat(catId);

                var attrs = attributes.Select(c => new
                {
                    Id = c.Id,
                    Order = c.Order,
                    AttrType = c.AttrType,
                    UnitId = c.UnitId,
                    Title = c.AttrType == AttrType.Question ? c.Question.Name : c.Title,
                    Placeholder = c.Placeholder,
                    Desc = c.Desc,
                    Values = c.Values,
                    MaxFileSize = c.MaxFileSize,
                    CategoryId = c.CategoryId,
                    AttributeOptions = c.getAttributeOptions(true, c.AttrType, c.AttributeOptions, c.Question == null ? new List<QuestionOption>() : c.Question.QuestionOptions.ToList(), true),
                    Score = c.Score,
                    QuestionId = c.QuestionId,
                    QuestionType = c.AttrType == AttrType.Question ? (int)c.Question.Type : 0,
                    ComplatabelContent = c.Question == null ? "" : c.Question.ComplatabelContent,
                    IsInClient = c.IsInClient,
                    IsRequired = c.IsRequired,
                    IsUniq = c.IsUniq,
                    IsInShowInfo = c.IsInShowInfo,
                    IsInSearch = c.IsInSearch
                }).OrderBy(c => c.UnitId).ThenBy(c => c.QuestionId).ThenBy(c => c.Order);

                byte[] fileContents;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(cat.Title);
                    worksheet.DefaultColWidth = 20;
                    worksheet.DefaultRowHeight = 20;
                    worksheet.View.RightToLeft = true;
                    worksheet.Cells.Style.Font.Name = "B Yekan";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    worksheet.Cells[1, 1].Value = "ردیف";
                    worksheet.Cells[1, 2].Value = "عنوان";
                    worksheet.Cells[1, 3].Value = "کد رهگیری";
                    worksheet.Cells[1, 4].Value = "تاریخ";
                    worksheet.Cells[1, 5].Value = cat.Type == CategoryTotalType.registerForm ? "نمون برگ" : "آزمون آنلاین";
                    worksheet.Cells[1, 6].Value = cat.Type == CategoryTotalType.registerForm ? "امتیاز" : "نمره";

                    var staticColsCount = 6;

                    if (cat.Type == CategoryTotalType.onlineExam)
                    {
                        worksheet.Cells[1, 7].Value = "غلط";
                        worksheet.Cells[1, 8].Value = "صحیح";
                        worksheet.Cells[1, 9].Value = "سفید";

                        staticColsCount = 9;

                        if (cat.CalculateNegativeScore)
                        {
                            worksheet.Cells[1, 10].Value = "درصد نمره";
                            worksheet.Cells[1, 11].Value = "نمره از 20";

                            staticColsCount = 11;
                        }
                    }

                    var apiurl = _config["Sites:Self"];

                    foreach (var (attr, index) in attrs.WithIndex())
                    {
                        worksheet.Cells[1, staticColsCount + (index + 1)].Value = attr.Title;
                    }

                    foreach (var (item, index) in items.WithIndex())
                    {
                        var itemScore = item.getTotalScoreFunction(item.ItemAttribute, cat.CalculateNegativeScore);

                        var catTotalScore = cat.getTotalScore(
                            cat.Attributes.ToList(),
                            cat.UseLimitedRandomQuestionNumber,
                            cat.VeryHardQuestionNumber,
                            cat.HardQuestionNumber,
                            cat.ModerateQuestionNumber,
                            cat.EasyQuestionNumber);

                        worksheet.Cells[1 + (index + 1), 1].Value = item.Id;
                        worksheet.Cells[1 + (index + 1), 2].Value = item.AuthorizedUsername == "---" ? item.Title : $"{item.AuthorizedFullName} - {item.AuthorizedUsername}";
                        worksheet.Cells[1 + (index + 1), 3].Value = item.RahCode;
                        worksheet.Cells[1 + (index + 1), 4].Value = item.DateAdd.Value.ToPersianDateWithTime();
                        worksheet.Cells[1 + (index + 1), 5].Value = item.CategoryString;
                        worksheet.Cells[1 + (index + 1), 6].Value = $"{itemScore}/{catTotalScore}";


                        var trueCount = 0;
                        var falseCount = 0;
                        var blankCount = 0;

                        foreach (var (attr, i) in attrs.WithIndex())
                        {
                            ItemAttribute itemAttr = null;

                            if (item.ItemAttribute.Any(c => c.AttributeId == attr.Id))
                            {
                                itemAttr = item.ItemAttribute.Where(c => c.AttributeId == attr.Id).First();
                            }

                            if (itemAttr == null)
                            {
                                worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].Value = "";
                            }
                            else
                            {

                                if (!string.IsNullOrWhiteSpace(itemAttr.AttributeFilePath))
                                {
                                    worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].Value = apiurl + itemAttr.AttributeFilePath;
                                }
                                else if (itemAttr.Attribute.AttrType == AttrType.date)
                                {
                                    var value = "";

                                    try
                                    {
                                        value = DateTime.Parse(itemAttr.AttrubuteValue).ToPersianDate();
                                    }
                                    catch { }

                                    worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].Value = value;
                                }
                                else if (attr.AttrType == AttrType.radiobutton || attr.AttrType == AttrType.combobox || (attr.AttrType == AttrType.Question && attr.QuestionType == 2))
                                {
                                    var attrOptions = attr.AttributeOptions;

                                    var value = "";
                                    var isTrue = false;
                                    var isBlank = false;
                                    var haveTrue = false;

                                    if (attrOptions.Count != 0)
                                    {
                                        haveTrue = attrOptions.Any(c => c.IsTrue);

                                        if (attrOptions.Any(c => c.Id.ToString() == itemAttr.AttrubuteValue))
                                        {
                                            value = attrOptions.FirstOrDefault(c => c.Id.ToString() == itemAttr.AttrubuteValue).Title;
                                        }
                                        else
                                        {
                                            value = itemAttr.AttrubuteValue;
                                        }

                                        if (haveTrue)
                                        {
                                            isTrue = attrOptions.FirstOrDefault(c => c.IsTrue).Id.ToString() == itemAttr.AttrubuteValue;
                                        }
                                    }
                                    else
                                    {
                                        value = itemAttr.AttrubuteValue;
                                    }

                                    isBlank = haveTrue && !isTrue && string.IsNullOrEmpty(value);


                                    worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].Value = value;

                                    if (haveTrue)
                                    {
                                        worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].Style.Fill.BackgroundColor.SetColor(isTrue ? Color.LightGreen : Color.IndianRed);
                                        var catScoreTitle = cat.Type == CategoryTotalType.registerForm ? "امتیاز" : "نمره";
                                        var attrTypeTitle = cat.Type == CategoryTotalType.registerForm ? "فیلد" : "سوال";
                                        worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].AddComment($"{catScoreTitle} این {attrTypeTitle} {attr.Score}", "Mabna");


                                        if (isTrue && !isBlank)
                                        {
                                            trueCount += 1;
                                        }
                                        if (!isTrue && !isBlank)
                                        {
                                            falseCount += 1;
                                        }
                                        if (!isTrue && isBlank)
                                        {
                                            blankCount += 1;

                                            worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                        }
                                    }
                                }
                                else
                                {
                                    worksheet.Cells[1 + (index + 1), staticColsCount + (i + 1)].Value = itemAttr.AttrubuteValue.HtmlToPlaneText();
                                }

                            }
                        }


                        if (cat.Type == CategoryTotalType.onlineExam)
                        {
                            worksheet.Cells[1 + (index + 1), 7].Value = falseCount;
                            worksheet.Cells[1 + (index + 1), 8].Value = trueCount;
                            worksheet.Cells[1 + (index + 1), 9].Value = blankCount;

                            if (cat.CalculateNegativeScore)
                            {
                                var scorePrecent = (double)((trueCount * 3) - (falseCount)) / ((trueCount + falseCount + blankCount) * 3);

                                worksheet.Cells[1 + (index + 1), 10].Value = Math.Round(scorePrecent * 100, 2);
                                worksheet.Cells[1 + (index + 1), 11].Value = Math.Round((scorePrecent * 100) / 5, 2);
                            }
                        }
                    }

                    fileContents = package.GetAsByteArray();

                    var guid = System.Guid.NewGuid().ToString();

                    PersianCalendar pc = new PersianCalendar();
                    var date = DateTime.Now;
                    var persianDateString = pc.GetYear(date).ToString() + pc.GetMonth(date).ToString("0#") + pc.GetDayOfMonth(date).ToString("0#") + pc.GetHour(date).ToString("0#") + pc.GetMinute(date).ToString("0#") + pc.GetSecond(date).ToString("0#");

                    var name = $"{cat.Id}_{persianDateString}";

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, $"Items_" + name + ".xlsx");
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    System.IO.File.WriteAllBytes(path, fileContents);

                    var filepath = Path.Combine("/UploadFiles/" + guid + "/" + $"Items_" + name + ".xlsx");

                    await db.SaveChangesAsync();

                    return this.SuccessFunction(redirect: filepath);
                }

            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        private double getTotalScoreForItem(IEnumerable<dynamic> attributes, List<ItemAttribute> itemAttributes)
        {
            double score = 0;

            foreach (var attr in attributes)
            {
                var itemAttr = itemAttributes.FirstOrDefault(c => c.AttributeId == attr.Id);

                if (itemAttr != null)
                {
                    if (attr.AttrType == AttrType.radiobutton || attr.AttrType == AttrType.combobox || (attr.AttrType == AttrType.Question && attr.QuestionType == 2))
                    {
                        IEnumerable<dynamic> attrOptions = attr.AttributeOptions;

                        if (attrOptions.Count() != 0 && attrOptions.Any(c => c.IsTrue))
                        {
                            if (attrOptions.FirstOrDefault(c => c.IsTrue).Id.ToString() == itemAttr.AttrubuteValue)
                            {
                                score += attr.Score;
                            }
                        }
                    }
                }
            }

            return score;
        }


        [HttpPost]
        [Role(RolePrefix.View, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> getExcelFileOfList([FromBody] getExcelParamComplex getparamsComplex)
        {
            try
            {
                getparamsComplex.param.pageIndex += 1;

                var query = getparamsComplex.param.q;

                var items = db.Items.AsQueryable();


                if (!string.IsNullOrEmpty(getparamsComplex.catName))
                {
                    items = items.Where(c => c.Category.Title.Contains(getparamsComplex.catName));
                }

                var state = getparamsComplex.state;

                if (!string.IsNullOrEmpty(state))
                {
                    if (state == "both")
                    {
                        // Do Nothing
                    }
                    else if (state == "active")
                    {
                        items = items.Where(c => c.IsActive == true);
                    }
                    else if (state == "deactive")
                    {
                        items = items.Where(c => c.IsActive == false);
                    }
                }



                if (!string.IsNullOrWhiteSpace(query))
                {
                    items = items.Where(c => c.Title.Contains(query) || c.Id.ToString().Contains(query) ||
                            c.RahCode.ToString().Contains(query) ||
                            c.Unit.Title.Contains(query) || c.Tags.Contains(query) || c.Id.ToString().Contains(query));
                }



                if (getparamsComplex.attrvalsearch.Any())
                {
                    var itemAttrs = db.ItemAttributes.AsQueryable();

                    IList<IList<Item>> tempItem = new List<IList<Item>>();

                    for (int i = 0; i < getparamsComplex.attrvalsearch.Count; i++)
                    {
                        tempItem.Add(new List<Item>());
                    }

                    foreach (var attrvalsearch in getparamsComplex.attrvalsearch.WithIndex())
                    {
                        if (!string.IsNullOrWhiteSpace(attrvalsearch.item.val))
                        {
                            var itemAttrsForAttrId = itemAttrs.Where(c => c.AttributeId == attrvalsearch.item.attrId && c.AttrubuteValue.Contains(attrvalsearch.item.val)).Include(c => c.Item);

                            foreach (var itemAttr in itemAttrsForAttrId.WithIndex())
                            {
                                if (attrvalsearch.index != 0)
                                {
                                    if (tempItem[attrvalsearch.index - 1].Contains(itemAttr.item.Item))
                                    {
                                        tempItem[attrvalsearch.index].Add(itemAttr.item.Item);
                                    }
                                }
                                else
                                {
                                    if (!tempItem[0].Contains(itemAttr.item.Item) && items.Contains(itemAttr.item.Item))
                                    {
                                        tempItem[0].Add(itemAttr.item.Item);
                                    }
                                }

                            }
                        }
                    }

                    items = tempItem[getparamsComplex.attrvalsearch.Count - 1].AsQueryable();

                }


                if (getparamsComplex.param.direction.Equals("asc"))
                {
                    if (getparamsComplex.param.sort.Equals("id"))
                    {
                        items = items.OrderBy(c => c.Id);
                    }
                    if (getparamsComplex.param.sort.Equals("title"))
                    {
                        items = items.OrderBy(c => c.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("isActive"))
                    {
                        items = items.OrderBy(c => c.IsActive);
                    }
                    if (getparamsComplex.param.sort.Equals("rahCode"))
                    {
                        items = items.OrderBy(c => c.RahCode);
                    }
                    if (getparamsComplex.param.sort.Equals("categoryString"))
                    {
                        items = items.OrderBy(c => c.Category.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("unitString"))
                    {
                        items = items.OrderBy(c => c.Unit.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("date"))
                    {
                        items = items.OrderBy(c => c.DateAdd).ThenBy(c => c.DateEdit);
                    }
                }
                else if (getparamsComplex.param.direction.Equals("desc"))
                {
                    if (getparamsComplex.param.sort.Equals("id"))
                    {
                        items = items.OrderByDescending(c => c.Id);
                    }
                    if (getparamsComplex.param.sort.Equals("title"))
                    {
                        items = items.OrderByDescending(c => c.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("isActive"))
                    {
                        items = items.OrderByDescending(c => c.IsActive);
                    }
                    if (getparamsComplex.param.sort.Equals("rahCode"))
                    {
                        items = items.OrderByDescending(c => c.RahCode);
                    }
                    if (getparamsComplex.param.sort.Equals("categoryString"))
                    {
                        items = items.OrderByDescending(c => c.Category.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("unitString"))
                    {
                        items = items.OrderByDescending(c => c.Unit.Title);
                    }
                    if (getparamsComplex.param.sort.Equals("date"))
                    {
                        items = items.OrderByDescending(c => c.DateAdd).ThenByDescending(c => c.DateEdit);
                    }
                }
                else
                {
                    items = items.OrderBy(c => c.Id);
                }

                var cat = await db.Categories
                    .Include(c => c.Attributes)
                        .ThenInclude(c => c.AttributeOptions)
                .SingleAsync(c => c.Id == getparamsComplex.catId);

                var allitemAttrs = db.ItemAttributes
                    .Include(c => c.Attribute)
                        .ThenInclude(c => c.AttributeOptions)
                .AsQueryable();

                byte[] fileContents;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add(cat.Title);
                    worksheet.DefaultColWidth = 20;
                    worksheet.DefaultRowHeight = 20;
                    worksheet.View.RightToLeft = true;
                    worksheet.Cells.Style.Font.Name = "B Yekan";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    worksheet.Cells[1, 1].Value = "ردیف";
                    worksheet.Cells[1, 2].Value = "عنوان";
                    worksheet.Cells[1, 3].Value = "وضعیت";
                    worksheet.Cells[1, 4].Value = "تگ ها";
                    worksheet.Cells[1, 5].Value = "کد رهگیری";
                    worksheet.Cells[1, 6].Value = "واحد";
                    worksheet.Cells[1, 7].Value = "نمونه برگ";
                    worksheet.Cells[1, 8].Value = "امتیاز";

                    var apiurl = _config["Sites:Self"];

                    foreach (var (attr, index) in getparamsComplex.selectedAttrs.OrderBy(c => c.UnitId).ThenBy(c => c.Order).WithIndex())
                    {
                        worksheet.Cells[1, 8 + (index + 1)].Value = attr.Title;
                    }

                    foreach (var (item, index) in items.WithIndex())
                    {
                        worksheet.Cells[1 + (index + 1), 1].Value = item.Id;
                        worksheet.Cells[1 + (index + 1), 2].Value = item.Title;
                        worksheet.Cells[1 + (index + 1), 3].Value = item.IsActive ? "فعال" : "غیر فعال";
                        worksheet.Cells[1 + (index + 1), 4].Value = item.Tags;
                        worksheet.Cells[1 + (index + 1), 5].Value = item.RahCode;
                        worksheet.Cells[1 + (index + 1), 6].Value = db.Units.Single(c => c.Id == item.UnitId).Title;
                        worksheet.Cells[1 + (index + 1), 7].Value = cat.Title;
                        worksheet.Cells[1 + (index + 1), 8].Value = getTotalScoreForItem(getparamsComplex.selectedAttrs.ToList(), item.ItemAttribute.ToList());

                        foreach (var (attr, i) in getparamsComplex.selectedAttrs.OrderBy(c => c.UnitId).ThenBy(c => c.Order).WithIndex())
                        {

                            ItemAttribute itemAttr = null;

                            if (allitemAttrs.Where(c => c.ItemId == item.Id).Any(c => c.AttributeId == attr.Id))
                            {
                                itemAttr = allitemAttrs.Where(c => c.ItemId == item.Id && c.AttributeId == attr.Id).First();
                            }

                            if (itemAttr == null)
                            {
                                worksheet.Cells[1 + (index + 1), 8 + (i + 1)].Value = "";
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(itemAttr.AttributeFilePath))
                                {
                                    worksheet.Cells[1 + (index + 1), 8 + (i + 1)].Value = apiurl + itemAttr.AttributeFilePath;
                                }
                                else if (itemAttr.Attribute.AttrType == AttrType.date)
                                {
                                    var value = "";

                                    try
                                    {
                                        value = DateTime.Parse(itemAttr.AttrubuteValue).ToPersianDate();
                                    }
                                    catch { }

                                    worksheet.Cells[1 + (index + 1), 8 + (i + 1)].Value = value;
                                }
                                else if (itemAttr.Attribute.AttrType == AttrType.radiobutton || itemAttr.Attribute.AttrType == AttrType.combobox)
                                {
                                    var attrOptions = itemAttr.Attribute.AttributeOptions;

                                    var value = "";
                                    var isTrue = false;
                                    var haveTrue = false;

                                    if (attrOptions.Count != 0)
                                    {
                                        haveTrue = attrOptions.Any(c => c.IsTrue);

                                        if (attrOptions.Any(c => c.Id.ToString() == itemAttr.AttrubuteValue))
                                        {
                                            value = attrOptions.FirstOrDefault(c => c.Id.ToString() == itemAttr.AttrubuteValue).Title;
                                        }
                                        else
                                        {
                                            value = itemAttr.AttrubuteValue;
                                        }

                                        if (haveTrue)
                                        {
                                            isTrue = attrOptions.FirstOrDefault(c => c.IsTrue).Id.ToString() == itemAttr.AttrubuteValue;
                                        }
                                    }
                                    else
                                    {
                                        value = itemAttr.AttrubuteValue;
                                    }

                                    worksheet.Cells[1 + (index + 1), 8 + (i + 1)].Value = value;

                                    if (haveTrue)
                                    {
                                        worksheet.Cells[1 + (index + 1), 8 + (i + 1)].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        worksheet.Cells[1 + (index + 1), 8 + (i + 1)].Style.Fill.BackgroundColor.SetColor(isTrue ? Color.LightGreen : Color.IndianRed);
                                        worksheet.Cells[1 + (index + 1), 8 + (i + 1)].AddComment($"امتیاز این فیلد {itemAttr.Attribute.Score}", "Mabna");
                                    }
                                }
                                else
                                {
                                    worksheet.Cells[1 + (index + 1), 8 + (i + 1)].Value = itemAttr.AttrubuteValue;
                                }
                            }
                        }
                    }

                    fileContents = package.GetAsByteArray();

                    var guid = System.Guid.NewGuid().ToString();

                    PersianCalendar pc = new PersianCalendar();
                    var date = DateTime.Now;
                    var persianDateString = pc.GetYear(date).ToString() + pc.GetMonth(date).ToString("0#") + pc.GetDayOfMonth(date).ToString("0#") + pc.GetHour(date).ToString("0#") + pc.GetMinute(date).ToString("0#") + pc.GetSecond(date).ToString("0#");

                    var name = $"{cat.Title.Trim()}_{persianDateString}";

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, $"Items_" + name + ".xlsx");
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    System.IO.File.WriteAllBytes(path, fileContents);

                    var filepath = Path.Combine("/UploadFiles/" + guid + "/" + $"Items_" + name + ".xlsx");

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
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> ChangeActiveState([FromBody] ActiveStateParam activestateparam)
        {
            try
            {
                var item = await db.Items.SingleAsync(c => c.Id == activestateparam.id);

                item.IsActive = activestateparam.isActive;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [Role(RolePrefix.Edit, roleTitle, onlineExamRoleTitle)]
        public async Task<IActionResult> ChangeActiveStateGroup([FromBody] ActiveStateGroupParam activestateparam)
        {
            try
            {
                foreach (var id in activestateparam.ids)
                {
                    var item = await db.Items.SingleAsync(c => c.Id == id);

                    item.IsActive = activestateparam.isActive;
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

    public class removeItemAttrFileParam
    {
        public int attrId { get; set; }
        public int itemId { get; set; }
    }

    public class SetItemAttributeScoreParam
    {
        public ItemAttribute itemAttr { get; set; }
        public double score { get; set; }
    }

    public class LoginForRegisterCatParam
    {
        public int catId { get; set; }

        public string username { get; set; }
        public string password { get; set; }
    }

    public class getExcelParamComplex
    {
        public getparams param { get; set; }

        public string catName { get; set; }

        public List<AttrValSearch> attrvalsearch { get; set; }

        public string state { get; set; }

        public int catId { get; set; }

        public List<Model.Attribute> selectedAttrs { get; set; }

    }

    public class setTagsGroupParam
    {
        public int[] itemsIds { get; set; }

        public string tags { get; set; }

    }

    public class ItemAttrForFileGroupParams
    {
        public int[] itemsIds { get; set; }

        public int attrId { get; set; }

        public string inputValue { get; set; }

        public string fileFormat { get; set; }

        public string fileName { get; set; }
    }

    public class SetItemAttrGroupParams
    {
        public int[] itemsIds { get; set; }

        public int attrId { get; set; }

        public string inputValue { get; set; }
    }

    public class RahCodeParam
    {
        public long rahCode { get; set; }

        public int catId { get; set; }

    }

    public class ActiveStateGroupParam
    {
        public int[] ids { get; set; }

        public bool isActive { get; set; }
    }

    public class ActiveStateParam
    {
        public int id { get; set; }

        public bool isActive { get; set; }
    }

    public class getParamComplex
    {
        public getparams param { get; set; }

        public int? catId { get; set; }

        public List<AttrValSearch> attrvalsearch { get; set; }

        public string state { get; set; }

        public int Type { get; set; }
    }

    public class AttrValSearch
    {
        public int attrId { get; set; }

        public string val { get; set; }
    }

    public class getparams
    {
        public string sort { get; set; }

        public string direction { get; set; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public string q { get; set; }

    }

    public class CheckForUniqAttrParams
    {
        public int catId { get; set; }

        public int attrId { get; set; }

        public string val { get; set; }
    }

    public class ItemAttrForFileParams
    {
        public int itemId { get; set; }

        public int attrId { get; set; }

        public string inputValue { get; set; }

        public string fileFormat { get; set; }

        public string fileName { get; set; }
    }

    public class ItemWithAttrsParams
    {
        public IList<ItemAttributeCustom> itemAttrs { get; set; }

        public int catId { get; set; }

        public string authorizedFullName { get; set; }
        public string authorizedUsername { get; set; }
        public CategoryAuthorizeState authorizedType { get; set; }
    }

    public class ItemAttributeCustom
    {
        public int ItemId { get; set; }
        public int AttributeId { get; set; }
        public string AttrubuteValue { get; set; }
        public string AttributeFilePath { get; set; }
        public string FileName { get; set; }
    }

    public class EditItemParams
    {
        public int id { get; set; }

        public string title { get; set; }

        public bool isActive { get; set; }

        public string tags { get; set; }

        public int unitId { get; set; }

        public int categoryId { get; set; }
    }

    public class SetItemAttrParams
    {
        public int itemId { get; set; }

        public int attrId { get; set; }

        public string inputValue { get; set; }
    }
}