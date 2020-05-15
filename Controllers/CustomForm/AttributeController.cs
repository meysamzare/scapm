﻿using System;
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
        public async Task<IActionResult> Add([FromBody] AddAttrParam param)
        {
            try
            {
                var attr = param.attr;
                attr.AttributeOptions = param.options.ToList();

                db.Attributes.Add(attr);

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
                db.Update(addAttrParam.attr);

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
                var sl = await db.Attributes
                    .Select(c => new
                    {
                        id = c.Id,
                        title = c.Title + " (" + c.Category.Title.Trim() + ")"
                    })
                .ToListAsync();

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

                        var attr = await db.Attributes
                            .Include(c => c.AttributeOptions)
                            .Include(c => c.ItemAttribute)
                        .SingleAsync(c => c.Id == id);
                        if (attr == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.ItemAttributes.RemoveRange(attr.ItemAttribute.ToList());
                        db.AttributeOptions.RemoveRange(attr.AttributeOptions);

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

                var attr = attrList.OrderBy(c => c.Order).ThenBy(c => c.Id).Select(c => new
                {
                    Id = c.Id,
                    AttrTypeInt = c.AttrTypeToInt(c.AttrType),
                    UnitId = c.UnitId,
                    IsRequired = c.IsRequired,
                    IsUniq = c.IsUniq,
                    Title = c.Title,
                    Placeholder = c.Placeholder,
                    Desc = c.Desc,
                    Values = c.Values,
                    MaxFileSize = c.MaxFileSize,
                    CategoryId = c.CategoryId,
                    AttributeOptions = c.AttributeOptions == null ? new List<AttributeOption>() : c.AttributeOptions.ToList(),
                    Score = c.Score
                });

                return this.DataFunction(true, attr);
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

                var attr = attrList.Where(c => c.IsInClient == false).OrderBy(c => c.Order).ThenBy(c => c.Id).Select(c => new
                {
                    Id = c.Id,
                    AttrTypeInt = c.AttrTypeToInt(c.AttrType),
                    UnitId = c.UnitId,
                    IsRequired = c.IsRequired,
                    IsUniq = c.IsUniq,
                    Title = c.Title,
                    Placeholder = c.Placeholder,
                    Desc = c.Desc,
                    Values = c.Values,
                    MaxFileSize = c.MaxFileSize,
                    CategoryId = c.CategoryId,
                    Score = c.Score,
                    AttributeOptions = c.AttributeOptions == null ? new List<AttributeOption>() : c.AttributeOptions.ToList()
                });

                return this.DataFunction(true, attr);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        // return Attributes for register item
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAttrsForCat_C([FromBody] int catId)
        {
            try
            {
                var attrlist = await getAttrForCat(catId);

                var attrListSelected = attrlist.Where(c => c.IsInClient == true).OrderBy(c => c.UnitId).ThenBy(c => c.Order)
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
                    Values = c.Values,
                    MaxFileSize = c.MaxFileSize,
                    AttributeOptions = c.AttributeOptions == null ? new List<AttributeOption>() : c.AttributeOptions.Select(option => new AttributeOption { Id = option.Id, Title = option.Title }).ToList()
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
            var cat = await db.Categories
                .Include(c => c.Attributes)
                    .ThenInclude(c => c.AttributeOptions)
                .Include(c => c.ParentCategory)
            .SingleAsync(c => c.Id == cat1);
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
        public async Task<IActionResult> AddAttributeOption([FromBody] AttributeOption option)
        {
            try
            {
                await db.AttributeOptions.AddAsync(option);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAttributeOption([FromBody] AttributeOption option)
        {
            try
            {
                var attributeOption = await db.AttributeOptions.FirstOrDefaultAsync(c => c.Id == option.Id);

                db.AttributeOptions.Remove(attributeOption);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAttributeOption([FromBody] AttributeOption option)
        {
            try
            {
                var attributeOption = await db.AttributeOptions.FirstOrDefaultAsync(c => c.Id == option.Id);

                attributeOption.Title = option.Title;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAttributeOptions([FromBody] int attrId)
        {
            try
            {
                var attrOptions = await db.AttributeOptions.Where(c => c.AttributeId == attrId).ToListAsync();

                return this.DataFunction(true, attrOptions);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> setTrueOption([FromBody] AttributeOption option)
        {
            try
            {
                var isTrue = !option.IsTrue;

                await db.AttributeOptions.Where(c => c.AttributeId == option.AttributeId).ForEachAsync(attrOption =>
                {
                    attrOption.IsTrue = false;
                });

                var attributeOption = await db.AttributeOptions.FirstOrDefaultAsync(c => c.Id == option.Id);

                attributeOption.IsTrue = isTrue;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

    }

    public class getparamsAttr
    {
        public getparams getparams { get; set; }

        public int selectedCatId { get; set; }
    }

    public class AddAttrParam
    {
        public Model.Attribute attr { get; set; }

        public AttributeOption[] options { get; set; }
    }
}