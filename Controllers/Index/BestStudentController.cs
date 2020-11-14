using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model.Index;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class BestStudentController : Controller
    {
        public Data.DbContext db;

        private IHostingEnvironment hostingEnvironment;

        public BestStudentController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BestStudent param)
        {
            try
            {
                param.DateStart = param.DateStart.AddDays(1);
                param.DateEnd = param.DateEnd.AddDays(1);

                var picdata = param.PicData;
                var picname = param.PicName;

                var guid = System.Guid.NewGuid().ToString();

                var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                byte[] bytes = Convert.FromBase64String(picdata);
                System.IO.File.WriteAllBytes(path, bytes);

                param.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);

                await db.AddAsync(param);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] BestStudent param)
        {
            try
            {
                var picdata = param.PicData;
                var picname = param.PicName;
                var picurl = param.PicUrl;

                if (!string.IsNullOrEmpty(picdata))
                {
                    if (!string.IsNullOrEmpty(picurl))
                    {
                        try
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                        }
                        catch { }
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(picdata);
                    System.IO.File.WriteAllBytes(path, bytes);

                    param.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);
                }

                // param.DateStart = param.DateStart.AddDays(1);
                // param.DateEnd = param.DateEnd.AddDays(1);

                db.Update(param);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
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

                var sl = db.BestStudents.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.FullName.Contains(query) || c.Desc.Contains(query) || c.Title.Contains(query));
                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        sl = sl.OrderBy(c => c.FullName);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderBy(c => c.Title);
                    }
                    if (getparams.sort.Equals("class"))
                    {
                        sl = sl.OrderBy(c => c.Class);
                    }
                    if (getparams.sort.Equals("dateStart"))
                    {
                        sl = sl.OrderBy(c => c.DateStart);
                    }
                    if (getparams.sort.Equals("dateEnd"))
                    {
                        sl = sl.OrderBy(c => c.DateEnd);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        sl = sl.OrderByDescending(c => c.FullName);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderByDescending(c => c.Title);
                    }
                    if (getparams.sort.Equals("class"))
                    {
                        sl = sl.OrderByDescending(c => c.Class);
                    }
                    if (getparams.sort.Equals("dateStart"))
                    {
                        sl = sl.OrderByDescending(c => c.DateStart);
                    }
                    if (getparams.sort.Equals("dateEnd"))
                    {
                        sl = sl.OrderByDescending(c => c.DateEnd);
                    }
                }
                else
                {
                    sl = sl.OrderBy(c => c.Id);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl
                    .Select(c => new
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        Title = c.Title,
                        Class = c.Class,
                        dateStartString = c.DateStart.ToPersianDate(),
                        dateEndString = c.DateEnd.ToPersianDate(),
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
        public async Task<IActionResult> getAll()
        {
            try
            {

                var sl = await db.BestStudents.Select(c => new { id = c.Id, name = c.FullName + " - " + c.Title }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getBestStudent([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.BestStudents.FirstOrDefaultAsync(c => c.Id == id);

                    return this.DataFunction(true, sl);
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
        public async Task<IActionResult> getAllByDate()
        {
            try
            {
                var nowDate = DateTime.Now;

                var bestStudents = await db.BestStudents
                    .Where(c => c.DateStart <= nowDate && c.DateEnd >= nowDate)
                .ToListAsync();

                return this.DataFunction(true, bestStudents);
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

                        var sl = await db.BestStudents.FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        var picurl = sl.PicUrl;

                        if (!string.IsNullOrEmpty(picurl))
                        {
                            try
                            {
                                System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                            }
                            catch { }
                        }

                        db.BestStudents.Remove(sl);
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