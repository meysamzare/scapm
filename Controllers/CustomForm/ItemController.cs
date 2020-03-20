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

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ItemController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        private IConfiguration _config;
        public ItemController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IConfiguration config)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;

            _config = config;
        }


        [HttpPost]
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
        public IActionResult Get([FromBody] getParamComplex getparamsComplex)
        {
            try
            {
                getparamsComplex.param.pageIndex += 1;

                int count;


                var query = getparamsComplex.param.q;

                var items = db.Items
                    .Include(c => c.Category)
                .AsQueryable();


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
                    items = items.OrderBy(c => c.Id);
                }

                items = items.Skip((getparamsComplex.param.pageIndex - 1) * getparamsComplex.param.pageSize);
                items = items.Take(getparamsComplex.param.pageSize);

                var q = items.ToList();

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
                    var it = await db.Items.SingleAsync(c => c.Id == itemId);

                    Categories = await db.Categories.Include(c => c.ParentCategory).Include(c => c.Attributes)
                                            .ToListAsync();

                    var attrs = await getAttrForCat(it.CategoryId);

                    return this.DataFunction(true, attrs.Where(c => c.IsInShowInfo == true).OrderBy(c => c.OrderInInfo).ThenBy(c => c.Id));
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
                        // .Include(c => c.ItemAttribute)
                        .Include(c => c.Category)
                        .Include(c => c.Unit)
                    .SingleAsync(c => c.Id == id);

                    Categories = await db.Categories.Include(c => c.ParentCategory).Include(c => c.Attributes)
                                            .ToListAsync();

                    var attrs = await getAttrForCat(it.Category.Id);

                    if (it == null)
                    {
                        return this.UnSuccessFunction("Data Not Found", "error");
                    }

                    return this.DataFunction(true, new { item = it, attrs = attrs.OrderBy(c => c.Order).ThenBy(c => c.Id) });
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

                    var itemattrs = await db.ItemAttributes.Where(c => c.ItemId == id).ToListAsync();

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
                                    System.IO.File.Delete(itemAttr.AttrubuteValue);
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
        public async Task<IActionResult> setItemAttrForFiles([FromBody] ItemAttrForFileParams itemAttrForFileParams)
        {
            try
            {


                var itemid = itemAttrForFileParams.itemId;
                var attrid = itemAttrForFileParams.attrId;
                string guidd = "";

                var item = await db.Items.SingleAsync(c => c.Id == itemid);
                var attr = await db.Attributes.SingleAsync(c => c.Id == attrid);

                if (await db.ItemAttributes.AnyAsync(c => c.AttributeId == attrid && c.ItemId == itemid))
                {
                    if (string.IsNullOrEmpty(itemAttrForFileParams.inputValue))
                    {

                        var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                        System.IO.File.Delete(ia.AttrubuteValue);

                        db.ItemAttributes.Remove(ia);

                        await db.SaveChangesAsync();

                    }
                    else
                    {
                        var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                        //Remove Old File And Save New File

                        System.IO.File.Delete(ia.AttrubuteValue);

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


                return this.SuccessFunction(data: guidd);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
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

                            System.IO.File.Delete(ia.AttrubuteValue);

                            db.ItemAttributes.Remove(ia);

                            await db.SaveChangesAsync();

                        }
                        else
                        {
                            var ia = await db.ItemAttributes.SingleAsync(c => c.AttributeId == attrid && c.ItemId == itemid);

                            //Remove Old File And Save New File

                            System.IO.File.Delete(ia.AttrubuteValue);

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
                var cat = await db.Categories.FirstOrDefaultAsync(c => c.Id == param.catId);

                var isValid = false;

                var anyTeacher = await db.Teachers.Include(c => c.OrgPerson)
                            .AnyAsync(c => c.OrgPerson.IdNum == param.username && c.Password == param.password);

                var anyParent = await db.Students
                            .AnyAsync(c => c.IdNumber2 == param.username && c.ParentsPassword == param.password);

                var anyStudent = await db.Students
                            .AnyAsync(c => c.IdNumber2 == param.username && c.StudentPassword == param.password);

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
                            userFullname = std.Name + " " + std.LastName;
                        }
                        break;
                    case CategoryAuthorizeState.SMA:
                        isValid = anyStudent;
                        if (anyStudent)
                        {
                            userType = CategoryAuthorizeState.SMA;
                            var std = await db.Students.FirstOrDefaultAsync(c => c.IdNumber2 == param.username && c.ParentsPassword == param.password);
                            userFullname = std.Name + " " + std.LastName;
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
                            userFullname = std.Name + " " + std.LastName;
                        }
                        else if (anyStudent)
                        {
                            userType = CategoryAuthorizeState.SMA;
                            var std = await db.Students.FirstOrDefaultAsync(c => c.IdNumber2 == param.username && c.ParentsPassword == param.password);
                            userFullname = std.Name + " " + std.LastName;
                        }
                        break;

                    default:
                        isValid = false;
                        break;
                }

                var jwt = BuildToken(param.username, 15);

                if (!isValid)
                {
                    return this.UnSuccessFunction("نام کاربری و یا کلمه عبور شما اشتباه است و یا شما مجوز مشاهده این نمون برگ را ندارید!");
                }

                return this.SuccessFunction(data: new
                {
                    jwt = jwt,
                    userType = userType,
                    userFullname = userFullname
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SetItemeWithAttrs([FromBody] ItemWithAttrsParams itemWithAttrs)
        {
            try
            {
                var item = new Item
                {
                    CategoryId = itemWithAttrs.catId,
                    IsActive = false,
                    Tags = "",
                    Title = itemWithAttrs.itemAttrs.FirstOrDefault().AttrubuteValue,
                    UnitId = db.Units.FirstOrDefault().Id,
                    DateAdd = DateTime.Now,
                    // AuthorizedUsername = itemWithAttrs.authorizedBy
                };

                db.Items.Add(item);

                await db.SaveChangesAsync();

                var itemid = item.Id;

                var itemAttrs = new List<ItemAttribute>();

                foreach (var itemAttr in itemWithAttrs.itemAttrs)
                {
                    var filepath = "";
                    if (itemAttr.AttributeFilePath.Equals("1"))
                    {
                        if (!string.IsNullOrWhiteSpace(itemAttr.AttrubuteValue))
                        {
                            var guid = System.Guid.NewGuid().ToString();

                            var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, itemAttr.FileName);
                            Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                            byte[] bytes = Convert.FromBase64String(itemAttr.AttrubuteValue);
                            System.IO.File.WriteAllBytes(path, bytes);

                            filepath = Path.Combine("/UploadFiles/" + guid + "/" + itemAttr.FileName);

                            itemAttr.AttrubuteValue = path;
                        }
                    }

                    itemAttrs.Add(new ItemAttribute
                    {
                        ItemId = itemid,
                        AttributeId = itemAttr.AttributeId,
                        AttrubuteValue = itemAttr.AttrubuteValue,
                        AttributeFilePath = filepath
                    });
                }

                await db.ItemAttributes.AddRangeAsync(itemAttrs);

                Random random = new System.Random();
                int value = random.Next(1111111, 99999999);

                item.RahCode = value;

                await db.SaveChangesAsync();

                var cat = await db.Categories.SingleAsync(c => c.Id == itemWithAttrs.catId);

                return this.CustomFunction(true, cat.EndMessage, value.ToString(), null);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
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
                            if (itemAttr.AttributeId == uniqAttrParams.attrId && itemAttr.AttrubuteValue.Equals(uniqAttrParams.val))
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
        public async Task<IActionResult> getItemsToExel([FromBody] int catId)
        {
            try
            {
                var items = await db.Items
                .Where(c => c.CategoryId == catId)
                    .Include(c => c.ItemAttribute)
                        .ThenInclude(c => c.Attribute)
                .ToListAsync();

                var cat = await db.Categories.Include(c => c.Attributes).SingleAsync(c => c.Id == catId);

                Categories = await db.Categories
                        .Include(c => c.ParentCategory)
                        .Include(c => c.Attributes)
                    .ToListAsync();

                var attrs = await getAttrForCat(catId);

                byte[] fileContents;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.DefaultColWidth = 20;
                    worksheet.DefaultRowHeight = 20;


                    worksheet.Cells[1, 1].Value = "ردیف";
                    worksheet.Cells[1, 2].Value = "عنوان";
                    worksheet.Cells[1, 3].Value = "وضعیت";
                    worksheet.Cells[1, 4].Value = "تگ ها";
                    worksheet.Cells[1, 5].Value = "کد رهگیری";
                    worksheet.Cells[1, 6].Value = "واحد";
                    worksheet.Cells[1, 7].Value = "نمونه برگ";

                    var apiurl = "http://api.taha.sch.ir";

                    foreach (var (attr, index) in attrs.OrderBy(c => c.Order).WithIndex())
                    {
                        worksheet.Cells[1, 7 + (index + 1)].Value = attr.Title;
                    }

                    foreach (var (item, index) in items.WithIndex())
                    {
                        worksheet.Cells[1 + (index + 1), 1].Value = item.Id;
                        worksheet.Cells[1 + (index + 1), 2].Value = item.Title;
                        worksheet.Cells[1 + (index + 1), 3].Value = item.IsActive ? "فعال" : "غیر فعال";
                        worksheet.Cells[1 + (index + 1), 4].Value = item.Tags;
                        worksheet.Cells[1 + (index + 1), 5].Value = item.RahCode;
                        worksheet.Cells[1 + (index + 1), 6].Value = item.UnitString;
                        worksheet.Cells[1 + (index + 1), 7].Value = item.CategoryString;

                        foreach (var (attr, i) in attrs.OrderBy(c => c.Order).WithIndex())
                        {
                            ItemAttribute itemAttr = null;

                            if (item.ItemAttribute.Any(c => c.AttributeId == attr.Id))
                            {
                                itemAttr = item.ItemAttribute.Where(c => c.AttributeId == attr.Id).First();
                            }

                            if (itemAttr == null)
                            {
                                worksheet.Cells[1 + (index + 1), 7 + (i + 1)].Value = "";
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(itemAttr.AttributeFilePath))
                                {
                                    worksheet.Cells[1 + (index + 1), 7 + (i + 1)].Value = apiurl + itemAttr.AttributeFilePath;
                                }
                                else if (itemAttr.Attribute.AttrType == AttrType.date)
                                {
                                    worksheet.Cells[1 + (index + 1), 7 + (i + 1)].Value = DateTime.Parse(itemAttr.AttrubuteValue).ToPersianDate();
                                }
                                else
                                {
                                    worksheet.Cells[1 + (index + 1), 7 + (i + 1)].Value = itemAttr.AttrubuteValue;
                                }
                            }
                        }
                    }

                    worksheet.View.RightToLeft = true;
                    worksheet.Cells.Style.Font.Name = "B Yekan";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    fileContents = package.GetAsByteArray();

                    var guid = System.Guid.NewGuid().ToString();

                    var name = DateTime.Now.ToLongDateString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, $"ite_" + name + ".xlsx");
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    System.IO.File.WriteAllBytes(path, fileContents);

                    var filepath = Path.Combine("/UploadFiles/" + guid + "/" + $"ite_" + name + ".xlsx");

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

                var cat = await db.Categories.Include(c => c.Attributes).SingleAsync(c => c.Id == getparamsComplex.catId);

                var allitemAttrs = db.ItemAttributes.AsQueryable();

                byte[] fileContents;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.DefaultColWidth = 20;
                    worksheet.DefaultRowHeight = 20;


                    worksheet.Cells[1, 1].Value = "ردیف";
                    worksheet.Cells[1, 2].Value = "عنوان";
                    worksheet.Cells[1, 3].Value = "وضعیت";
                    worksheet.Cells[1, 4].Value = "تگ ها";
                    worksheet.Cells[1, 5].Value = "کد رهگیری";
                    worksheet.Cells[1, 6].Value = "واحد";
                    worksheet.Cells[1, 7].Value = "نمونه برگ";

                    var apiurl = "http://api.taha.sch.ir";

                    foreach (var (attr, index) in getparamsComplex.selectedAttrs.WithIndex())
                    {
                        worksheet.Cells[1, 7 + (index + 1)].Value = attr.Title;
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

                        foreach (var (attr, i) in getparamsComplex.selectedAttrs.WithIndex())
                        {

                            ItemAttribute itemAttr = null;

                            if (allitemAttrs.Where(c => c.ItemId == item.Id).Any(c => c.AttributeId == attr.Id))
                            {
                                itemAttr = allitemAttrs.Where(c => c.ItemId == item.Id && c.AttributeId == attr.Id).First();
                            }

                            if (itemAttr == null)
                            {
                                worksheet.Cells[1 + (index + 1), 7 + (i + 1)].Value = "";
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(itemAttr.AttributeFilePath))
                                {
                                    worksheet.Cells[1 + (index + 1), 7 + (i + 1)].Value = apiurl + itemAttr.AttributeFilePath;
                                }
                                else if (itemAttr.Attribute.AttrType == AttrType.date)
                                {
                                    worksheet.Cells[1 + (index + 1), 7 + (i + 1)].Value = DateTime.Parse(itemAttr.AttrubuteValue).ToPersianDate();
                                }
                                else
                                {
                                    worksheet.Cells[1 + (index + 1), 7 + (i + 1)].Value = itemAttr.AttrubuteValue;
                                }
                            }
                        }
                    }
                    worksheet.View.RightToLeft = true;
                    worksheet.Cells.Style.Font.Name = "B Yekan";
                    worksheet.Cells.Style.Font.Size = 11;
                    worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    fileContents = package.GetAsByteArray();

                    var guid = System.Guid.NewGuid().ToString();

                    var name = DateTime.Now.ToLongDateString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, $"ite_" + name + ".xlsx");
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    System.IO.File.WriteAllBytes(path, fileContents);

                    var filepath = Path.Combine("/UploadFiles/" + guid + "/" + $"ite_" + name + ".xlsx");

                    // db.Logs.Add(new Log
                    // {
                    // 	Date = DateTime.Now,
                    // 	Event = "Excel Output for CatId/CatTitle: " + cat.Id + "/" + cat.Title,
                    // 	Desc = "File path = " + filepath + ", UserName = " + User.Identity.Name,
                    //     agentId =
                    // });

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

        public string catName { get; set; }

        public List<AttrValSearch> attrvalsearch { get; set; }

        public string state { get; set; }

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