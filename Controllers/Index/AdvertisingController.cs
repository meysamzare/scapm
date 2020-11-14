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
using SCMR_Api.Model.Financial;
using SCMR_Api.Model.Index;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class AdvertisingController : Controller
    {
        public Data.DbContext db;

        private IHostingEnvironment hostingEnvironment;

        public AdvertisingController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Advertising ad)
        {
            try
            {

                if (!string.IsNullOrEmpty(ad.PicData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, ad.PicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(ad.PicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    ad.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + ad.PicName);
                }

                await db.Advertisings.AddAsync(ad);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Advertising ad)
        {
            try
            {
                var adv = await db.Advertisings.FirstOrDefaultAsync(c => c.Id == ad.Id);

                if (!string.IsNullOrEmpty(ad.PicData))
                {
                    if (!string.IsNullOrEmpty(ad.PicUrl))
                    {
                        try
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + ad.PicUrl);
                        }
                        catch { }
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, ad.PicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(ad.PicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    adv.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + ad.PicName);
                }

                adv.Name = ad.Name;
                adv.Type = ad.Type;
                adv.IsActive = ad.IsActive;
                adv.Url = ad.Url;

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

                var sl = db.Advertisings.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Name.Contains(query) || c.Url.Contains(query));
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
                        sl = sl.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("isActive"))
                    {
                        sl = sl.OrderBy(c => c.IsActive);
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
                        sl = sl.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("isActive"))
                    {
                        sl = sl.OrderByDescending(c => c.IsActive);
                    }
                }
                else
                {
                    sl = sl.OrderBy(c => c.Id);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl
                    .Select(c => new Advertising
                    {
                        Id = c.Id,
                        Name = c.Name,
                        IsActive = c.IsActive
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

                var sl = await db.Advertisings.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAllByType([FromBody] int type)
        {
            try
            {
                var sl = await db.Advertisings
                .Where(c => c.IsActive == true && c.Type == (AdType)type)
                    .OrderBy(c => c.Type)
                .ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAllMobileByType([FromBody] int type)
        {
            try
            {
                var sl = await db.Advertisings
                .Where(c => c.IsActive == true && (c.Type == (AdType)type || c.Type == AdType.AllMobileApp))
                    .OrderBy(c => c.Type)
                .ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getAd([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.Advertisings.FirstOrDefaultAsync(c => c.Id == id);

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

                        var sl = await db.Advertisings.FirstOrDefaultAsync(c => c.Id == id);

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

                        db.Advertisings.Remove(sl);
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
        public async Task<IActionResult> changeActiveState([FromBody] changeActiveStateParam param)
        {
            try
            {
                var ad = await db.Advertisings.FirstOrDefaultAsync(c => c.Id == param.id);

                ad.IsActive = param.active;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }




    }

    public class changeActiveStateParam
    {
        public int id { get; set; }

        public bool active { get; set; }
    }
}