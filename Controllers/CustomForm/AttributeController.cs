using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class AttributeController : Controller
    {
        public Data.DbContext db;

        public AttributeController(Data.DbContext _db)
        {
            db = _db;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddAttrParam addAttrParam)
        {
            try
            {
                var Attr = new Model.Attribute
                {
                    Title = addAttrParam.title,
                    CategoryId = addAttrParam.categoryId,
                    UnitId = addAttrParam.unitId,
                    AttrType = (AttrType)addAttrParam.attrTypeInt,
                    Desc = addAttrParam.desc,
                    Values = addAttrParam.values,
                    IsUniq = addAttrParam.isUniq,
                    Order = addAttrParam.orderInt,
                    IsInClient = addAttrParam.IsInClient,
                    IsInShowInfo = addAttrParam.IsInShowInfo,
                    OrderInInfo = addAttrParam.OrderInInfo,
                    IsInSearch = addAttrParam.IsInSearch,
                    Placeholder = addAttrParam.placeholder,
                    IsRequired = addAttrParam.isRequired,
                    IsMeliCode = addAttrParam.isMeliCode
                };

                db.Attributes.Add(Attr);


                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] AddAttrParam addAttrParam)
        {
            try
            {
                var attr = await db.Attributes.SingleAsync(c => c.Id == addAttrParam.id);

                attr.Title = addAttrParam.title;
                attr.CategoryId = addAttrParam.categoryId;
                attr.UnitId = addAttrParam.unitId;
                attr.AttrType = (AttrType)addAttrParam.attrTypeInt;
                attr.Desc = addAttrParam.desc;
                attr.Values = addAttrParam.values;
                attr.IsUniq = addAttrParam.isUniq;
                attr.Order = addAttrParam.orderInt;
                attr.IsInClient = addAttrParam.IsInClient;
                attr.IsInShowInfo = addAttrParam.IsInShowInfo;
                attr.OrderInInfo = addAttrParam.OrderInInfo;
                attr.IsInSearch = addAttrParam.IsInSearch;
                attr.Placeholder = addAttrParam.placeholder;
                attr.IsRequired = addAttrParam.isRequired;
                attr.IsMeliCode = addAttrParam.isMeliCode;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
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
                var sl = await db.Attributes.Select(c => new { id = c.Id, name = c.Title }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromBody] getparamsAttr getparams)
        {
            try
            {
                getparams.getparams.pageIndex += 1;

                int count;


                var query = getparams.getparams.q;

                var cls = db.Attributes
                    .Include(c => c.Category)
                    .Include(c => c.Unit)
                .AsQueryable();

                if (getparams.selectedCatId != 0)
                {
                    cls = cls.Where(c => c.CategoryId == getparams.selectedCatId);
                }

                if (!string.IsNullOrWhiteSpace(query))
                {

                    cls = cls.Where(c => c.Title.Contains(query) || c.Category.Title.Contains(query) ||
                                    c.Order.ToString().Contains(query) || c.Unit.Title.Contains(query) ||
                                    c.Id.ToString().Contains(query));
                }

                count = cls.Count();

                if (getparams.getparams.direction.Equals("asc"))
                {
                    if (getparams.getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderBy(c => c.Id);
                    }
                    if (getparams.getparams.sort.Equals("title"))
                    {
                        cls = cls.OrderBy(c => c.Title);
                    }
                    if (getparams.getparams.sort.Equals("catTitle"))
                    {
                        cls = cls.OrderBy(c => c.Category.Title);
                    }
                    if (getparams.getparams.sort.Equals("attrTypeString"))
                    {
                        cls = cls.OrderBy(c => c.AttrType);
                    }
                    if (getparams.getparams.sort.Equals("unitTitle"))
                    {
                        cls = cls.OrderBy(c => c.Unit.Title);
                    }
                }
                else if (getparams.getparams.direction.Equals("desc"))
                {
                    if (getparams.getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderByDescending(c => c.Id);
                    }
                    if (getparams.getparams.sort.Equals("title"))
                    {
                        cls = cls.OrderByDescending(c => c.Title);
                    }
                    if (getparams.getparams.sort.Equals("catTitle"))
                    {
                        cls = cls.OrderByDescending(c => c.Category.Title);
                    }
                    if (getparams.getparams.sort.Equals("attrTypeString"))
                    {
                        cls = cls.OrderByDescending(c => c.AttrType);
                    }
                    if (getparams.getparams.sort.Equals("unitTitle"))
                    {
                        cls = cls.OrderByDescending(c => c.Unit.Title);
                    }
                }
                else
                {
                    cls = cls.OrderBy(c => c.Id);
                }

                cls = cls.Skip((getparams.getparams.pageIndex - 1) * getparams.getparams.pageSize);
                cls = cls.Take(getparams.getparams.pageSize);

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
        public async Task<IActionResult> getAttribute([FromBody] int id)
        {
            try
            {
                if (id != 0)
                {

                    var attr = await db.Attributes.SingleAsync(c => c.Id == id);
                    if (attr == null)
                    {
                        return this.UnSuccessFunction("Data Not Found", "error");
                    }

                    return this.SuccessFunction(data: attr);
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

                        var attr = await db.Attributes.SingleAsync(c => c.Id == id);
                        if (attr == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.ItemAttributes.RemoveRange(db.ItemAttributes.Where(c => c.AttributeId == attr.Id).ToList());

                        db.Attributes.Remove(attr);
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
        public async Task<IActionResult> getAttrsForCat([FromBody] int catId)
        {
            try
            {
                var attrlist = await getAttrForCat(catId);

                return this.DataFunction(true, attrlist.OrderBy(c => c.Order).ThenBy(c => c.Id));
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getNonClientAttrsForCat([FromBody] int catId)
        {
            try
            {
                var attrlist = await getAttrForCat(catId);

                return this.DataFunction(true, attrlist.Where(c => c.IsInClient == false).OrderBy(c => c.Order).ThenBy(c => c.Id));
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAttrsForCat_C([FromBody] int catId)
        {
            try
            {
                var attrlist = await getAttrForCat(catId);

                var attrListSelected = attrlist.Where(c => c.IsInClient == true).OrderBy(c => c.Order).ThenBy(c => c.Id)
                .Select(c => new
                {
                    Id = c.Id,
                    AttrTypeInt = c.AttrTypeToInt(c.AttrType),
                    UnitId = c.UnitId,
                    IsRequired = c.IsRequired,
                    IsUniq = c.IsUniq,
                    Title = c.Title,
                    Placeholder = c.Placeholder,
                    Desc = c.Desc,
                    Values = c.Values
                })
                .ToList();

                return this.DataFunction(true, attrListSelected);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        List<Model.Attribute> attrList = new List<Model.Attribute>();

        public async Task<IEnumerable<Model.Attribute>> getAttrForCat(int cat1)
        {
            var cat = await db.Categories.Include(c => c.Attributes).Include(c => c.ParentCategory).SingleAsync(c => c.Id == cat1);
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







    }

    public class getparamsAttr
    {
        public getparams getparams { get; set; }

        public int selectedCatId { get; set; }
    }

    public class AddAttrParam
    {
        public int id { get; set; }

        public string title { get; set; }

        public int attrTypeInt { get; set; }

        public string desc { get; set; }

        public int orderInt { get; set; }

        public bool isUniq { get; set; }

        public int categoryId { get; set; }

        public int unitId { get; set; }

        public string values { get; set; }

        public bool isMeliCode { get; set; }

        public bool IsInClient { get; set; }
        public bool IsInShowInfo { get; set; }
        public bool IsInSearch { get; set; }

        public int OrderInInfo { get; set; }

        public string placeholder { get; set; }
        public bool isRequired { get; set; }

    }
}