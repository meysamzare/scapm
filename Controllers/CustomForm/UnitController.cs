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
    public class UnitController : Controller
    {
        public Data.DbContext db;
        const string roleTitle = "Unit";

        public UnitController(Data.DbContext _db)
        {
            db = _db;
        }


        [HttpPost]
        [Role(RolePrefix.Add, roleTitle)]
        public async Task<IActionResult> Add([FromBody] Unit unit)
        {
            try
            {
                db.Units.Add(unit);

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
        public async Task<IActionResult> Edit([FromBody]  Unit unit)
        {
            try
            {
                db.Units.Update(unit);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAll()
        {
            try
            {
                var unitList = await db.Units
                    .OrderBy(c => c.Order).ThenBy(c => c.Id)
                .Select(c => new { id = c.Id, title = c.Title }).ToListAsync();

                return this.DataFunction(true, unitList);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        

        [HttpPost]
        [Role(RolePrefix.View, roleTitle)]
        public async Task<IActionResult> Get([FromBody] getparams getparams)
        {
            try
            {
                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var cls = db.Units
                    .Include(c => c.Items)
                    .Include(c => c.Attributes)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    cls = cls.Where(c => c.Title.Contains(query) || c.EnTitle.Contains(query) || c.Id.ToString().Contains(query));
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
                    if (getparams.sort.Equals("enTitle"))
                    {
                        cls = cls.OrderBy(c => c.EnTitle);
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
                    if (getparams.sort.Equals("enTitle"))
                    {
                        cls = cls.OrderByDescending(c => c.EnTitle);
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
        [Role(RolePrefix.View, roleTitle)]
        public async Task<IActionResult> getUnit([FromBody] int id)
        {
            try
            {
                if (id != 0)
                {

                    var unit = await db.Units.SingleAsync(c => c.Id == id);
                    if (unit == null)
                    {
                        return this.UnSuccessFunction("Data Not Found", "error");
                    }

                    return this.SuccessFunction(data: unit);
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
        [Role(RolePrefix.Remove, roleTitle)]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    if (id != 0)
                    {

                        var unit = await db.Units.SingleAsync(c => c.Id == id);
                        if (unit == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (unit.Items.Any() || unit.Attributes.Any())
                        {
                            return this.UnSuccessFunction("این واحد به فیلد ها یا اطلاعاتی اختصاص داده شده است", "warning");
                        }

                        db.Units.Remove(unit);
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

    }
}