using System;
using System.Collections.Generic;
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
    public class PictureController : Controller
    {
        public Data.DbContext db;

        private IHostingEnvironment hostingEnvironment;

        public PictureController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Picture picture)
        {
            try
            {
                var picdata = picture.PicData;
                var picname = picture.PicName;

                var guid = System.Guid.NewGuid().ToString();

                var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                byte[] bytes = Convert.FromBase64String(picdata);
                System.IO.File.WriteAllBytes(path, bytes);

                picture.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);


                await db.Pictures.AddAsync(picture);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup([FromBody] PictureAddGroupParam param)
        {
            try
            {
                var unSuccessCount = 0;

                foreach (var pic in param.pictures)
                {
                    try
                    {
                        var Picture = new Picture
                        {
                            Title = param.picData.Title,
                            Author = param.picData.Author,
                            PictureGalleryId = param.picData.PictureGalleryId,
                        };

                        var picdata = pic.picData;
                        var picname = pic.picName;

                        var guid = System.Guid.NewGuid().ToString();

                        var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                        Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                        byte[] bytes = Convert.FromBase64String(picdata);
                        System.IO.File.WriteAllBytes(path, bytes);

                        Picture.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);

                        await db.Pictures.AddAsync(Picture);
                    }
                    catch
                    {
                        unSuccessCount += 1;
                        continue;
                    }
                }

                await db.SaveChangesAsync();

                if (unSuccessCount != 0)
                {
                    return this.UnSuccessFunction("بارگذاری " + unSuccessCount + " عدد از تصاویر با مشکل مواجه شد!");
                }

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Picture picture)
        {
            try
            {
                var picdata = picture.PicData;
                var picname = picture.PicName;
                var picurl = picture.PicUrl;

                if (!string.IsNullOrEmpty(picdata))
                {
                    if (!string.IsNullOrEmpty(picurl))
                    {
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(picdata);
                    System.IO.File.WriteAllBytes(path, bytes);

                    picture.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);
                }

                db.Update(picture);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> clearGallery([FromBody] int id)
        {
            try
            {
                var picture = await db.Pictures.FirstOrDefaultAsync(c => c.Id == id);

                picture.PictureGalleryId = null;

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

                var sl = db.Pictures
                    .Include(c => c.PictureGallery)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Author.Contains(query) || c.PictureGallery.Name.Contains(query));
                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("galleryName"))
                    {
                        sl = sl.OrderBy(c => c.PictureGallery.Name);
                    }
                    if (getparams.sort.Equals("author"))
                    {
                        sl = sl.OrderBy(c => c.Author);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("galleryName"))
                    {
                        sl = sl.OrderByDescending(c => c.PictureGallery.Name);
                    }
                    if (getparams.sort.Equals("author"))
                    {
                        sl = sl.OrderByDescending(c => c.Author);
                    }
                }
                else
                {
                    sl = sl.OrderBy(c => c.Id);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl
                        .Include(c => c.PictureGallery)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        pictureGalleryName = c.PictureGallery == null ? "---" : c.PictureGallery.Name,
                        Author = c.Author,
                        PicUrl = c.PicUrl
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

                var sl = await db.Pictures.Select(c => new { id = c.Id, name = c.PictureGallery.Name + " " + c.Id }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAllByGallery([FromBody] int galleryId)
        {
            try
            {
                var pics = await db.Pictures.Where(c => c.PictureGalleryId == galleryId).ToListAsync();


                return this.DataFunction(true, pics);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getPicture([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.Pictures.FirstOrDefaultAsync(c => c.Id == id);

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

                        var sl = await db.Pictures.FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        var picurl = sl.PicUrl;

                        if (!string.IsNullOrEmpty(picurl))
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                        }

                        db.Pictures.Remove(sl);
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

    public class PictureAddGroupParam
    {
        public Picture picData { get; set; }

        public List<pictureListData> pictures { get; set; }
    }

    public class pictureListData
    {
        public string picData { get; set; }
        public string picName { get; set; }
    }
}