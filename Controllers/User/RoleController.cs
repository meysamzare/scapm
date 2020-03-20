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
    public class RoleController : Controller
    {
        Data.DbContext db;

        public RoleController(Data.DbContext _db)
        {
            db = _db;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Role role)
        {
            try
            {
                role.Name = role.Name.Trim();
                if (string.IsNullOrEmpty(role.Name))
                {
                    return this.UnSuccessFunction("نام سطح دسترسی را وارد کنید");
                }



                if (db.Roles.Any(c => c.Name.Equals(role.Name)))
                {
                    return this.UnSuccessFunction("این نام سطح دسترسی قبلا وارد شده است");
                }

                db.Roles.Add(role);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Role role)
        {
            try
            {
                var thisroleList = db.Roles.Where(c => c.Id == role.Id);
                if (db.Roles.Except(thisroleList).Any(c => c.Name.Equals(role.Name)))
                {
                    return this.UnSuccessFunction("این نام قبلا ثبت شده است");
                }

                db.Update(role);

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
                var sl = await db.Roles.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllHaveUser()
        {
            try
            {
                var roles = await db.Roles
                    .Select(c => new
                    {
                        Id = c.Id,
                        haveAnyUser = c.Users.Any(),
                        Name = c.Name
                    })
                .Where(c => c.haveAnyUser)
                .ToListAsync();

                return this.DataFunction(true, roles);
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

                var cls = db.Roles.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    cls = cls.Where(c => c.Name.Contains(query) ||
                                    c.Id.ToString().Contains(query));
                }

                count = cls.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        cls = cls.OrderBy(c => c.Name);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        cls = cls.OrderByDescending(c => c.Name);
                    }
                }
                else
                {
                    cls = cls.OrderBy(c => c.Id);
                }

                cls = cls.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                cls = cls.Take(getparams.pageSize);

                var q = await cls
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        haveAnyUser = c.Users.Any()
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
        public async Task<IActionResult> GetRole([FromBody] int id)
        {
            try
            {
                if (id != 0)
                {
                    var role = await db.Roles.SingleAsync(c => c.Id == id);

                    if (role == null)
                    {
                        return this.UnSuccessFunction("Data Not Found", "error");
                    }

                    return this.DataFunction(true, role);
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
        public async Task<IActionResult> Delete([FromBody]int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    if (id != 0)
                    {
                        var role = await db.Roles.SingleAsync(c => c.Id == id);
                        if (role.Users == null || !role.Users.Any())
                        {
                            db.Roles.Remove(role);
                        }
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