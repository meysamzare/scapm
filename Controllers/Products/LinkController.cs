using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LinkController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public LinkController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost, DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> Add()
        {
            try
            {
                var files = Request.Form.Files;
                var param = JsonConvert.DeserializeObject<Link>(Request.Form.FirstOrDefault(c => c.Key == "object").Value);
                var fileExternalUrl = Request.Form.FirstOrDefault(c => c.Key == "fileExternalUrl").Value.First();

                if (files.Any())
                {
                    var file = files[0];
                    var guid = System.Guid.NewGuid().ToString();
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, fileName);

                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    param.FileUrl = Path.Combine("/UploadFiles", guid, fileName);
                    param.HaveExternalUrl = false;
                }
                else
                {
                    param.FileUrl = fileExternalUrl;
                    param.HaveExternalUrl = true;
                }

                param.Id = Guid.Empty;

                await db.AddAsync(param);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var param = JsonConvert.DeserializeObject<Link>(Request.Form.FirstOrDefault(c => c.Key == "object").Value);
                var fileExternalUrl = Request.Form.FirstOrDefault(c => c.Key == "fileExternalUrl").Value.First();

                var picurl = param.FileUrl;

                if (Request.Form.Files.Any() || !string.IsNullOrWhiteSpace(fileExternalUrl))
                {
                    if (Request.Form.Files.Any())
                    {
                        var file = Request.Form.Files[0];

                        var guid = System.Guid.NewGuid().ToString();
                        Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));
                        string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, fileName);


                        if (file.Length > 0)
                        {
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }

                        param.FileUrl = Path.Combine("/UploadFiles", guid, fileName);
                        param.HaveExternalUrl = false;
                    }
                    else
                    {
                        param.FileUrl = fileExternalUrl;
                        param.HaveExternalUrl = true;
                    }

                    if (!string.IsNullOrEmpty(picurl) && !param.HaveExternalUrl)
                    {
                        try
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                        }
                        catch { }
                    }
                }

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
        public async Task<IActionResult> Get([FromBody] getLinkParam param)
        {
            try
            {
                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var sl = db.Links
                        .Include(c => c.Product)
                    .Where(c => c.Product.TotalType == (ProductTotalType)param.totalType)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Title.Contains(query) || c.Desc.Contains(query) || c.Product.Title.Contains(query));
                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderBy(c => c.Title);
                    }
                    if (getparams.sort.Equals("productTitle"))
                    {
                        sl = sl.OrderBy(c => c.Product.Title);
                    }
                    if (getparams.sort.Equals("price"))
                    {
                        sl = sl.OrderBy(c => c.Price);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderByDescending(c => c.Title);
                    }
                    if (getparams.sort.Equals("productTitle"))
                    {
                        sl = sl.OrderByDescending(c => c.Product.Title);
                    }
                    if (getparams.sort.Equals("price"))
                    {
                        sl = sl.OrderByDescending(c => c.Price);
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
                        title = c.Title,
                        productTitle = c.Product.Title,
                        price = c.Price,
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
                var sl = await db.Links.Select(c => new { id = c.Id, Title = c.Title }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getLink([FromBody] Guid id)
        {
            try
            {

                var sl = await db.Links.FirstOrDefaultAsync(c => c.Id == id);

                return this.DataFunction(true, sl);

            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {

                    var sl = await db.Links
                        .FirstOrDefaultAsync(c => c.Id == id);

                    if (sl == null)
                    {
                        return this.UnSuccessFunction("Data Not Found", "error");
                    }

                    var picurl = sl.FileUrl;

                    if (!string.IsNullOrEmpty(picurl))
                    {
                        try
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                        }
                        catch { }
                    }

                    db.Links.Remove(sl);
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
        [AllowAnonymous]
        public async Task<IActionResult> setSee([FromBody] Guid id)
        {
            try
            {
                var link = await db.Links.FirstOrDefaultAsync(c => c.Id == id);

                link.ViewCount += 1;

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
        public async Task<IActionResult> setDownload([FromBody] Guid id)
        {
            try
            {
                var link = await db.Links.FirstOrDefaultAsync(c => c.Id == id);

                link.DownloadTime += 1;

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
        public async Task<IActionResult> setLike([FromBody] setLikeParam param)
        {
            try
            {
                var link = await db.Links.FirstOrDefaultAsync(c => c.Id == param.id);

                link.Like += param.like ? 1 : -1;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }



    }

    public class setLikeParam
    {
        public Guid id { get; set; }

        public bool like { get; set; }
    }

    public class getLinkParam
    {
        public getparams getparams { get; set; }

        public int totalType { get; set; }
    }
}