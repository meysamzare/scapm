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

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class WriterController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public WriterController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Writer writer)
        {
            try
            {

                var picdata = writer.PicData;
                var picname = writer.PicName;

                var guid = System.Guid.NewGuid().ToString();

                var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                byte[] bytes = Convert.FromBase64String(picdata);
                System.IO.File.WriteAllBytes(path, bytes);

                writer.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);

                await db.Writers.AddAsync(writer);

                await db.SaveChangesAsync();


                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Writer param)
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

                db.Writers.Update(param);

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

                var sl = db.Writers.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.FullName.Contains(query) || c.Desc.Contains(query));
                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("fullName"))
                    {
                        sl = sl.OrderBy(c => c.FullName);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("fullName"))
                    {
                        sl = sl.OrderByDescending(c => c.FullName);
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
                        haveAnyProduct = c.Products.Any()
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

                var sl = await db.Writers.Select(c => new { id = c.Id, fullName = c.FullName }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getWriter([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.Writers.FirstOrDefaultAsync(c => c.Id == id);

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
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    if (id != 0)
                    {

                        var sl = await db.Writers
                            .Include(c => c.Products)
                        .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (sl.haveAnyProduct)
                        {
                            return this.UnSuccessFunction(" نویسنده " + sl.FullName + " دارای محصولاتی است", "error");
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

                        db.Writers.Remove(sl);
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