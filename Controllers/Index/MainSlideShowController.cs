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
    public class MainSlideShowController : Controller
    {
        public Data.DbContext db;

        private IHostingEnvironment hostingEnvironment;

        public MainSlideShowController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MainSlideShow mainslideshow)
        {
            try
            {

                if (!string.IsNullOrEmpty(mainslideshow.imgData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, mainslideshow.imgName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(mainslideshow.imgData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    mainslideshow.imgUrl = Path.Combine("/UploadFiles/" + guid + "/" + mainslideshow.imgName);
                }

                mainslideshow.DatePublish = mainslideshow.DatePublish.AddDays(1);
                mainslideshow.DateExpire = mainslideshow.DateExpire.AddDays(1);

                await db.MainSlideShows.AddAsync(mainslideshow);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] MainSlideShow mainslideshow)
        {
            try
            {
                var mss = await db.MainSlideShows.FirstOrDefaultAsync(c => c.Id == mainslideshow.Id);


                if (!string.IsNullOrEmpty(mainslideshow.imgData))
                {
                    if (!string.IsNullOrEmpty(mss.imgUrl))
                    {
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + mss.imgUrl);
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, mainslideshow.imgName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(mainslideshow.imgData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    mss.imgUrl = Path.Combine("/UploadFiles/" + guid + "/" + mainslideshow.imgName);
                }


                var datePB = mss.DatePublish;

                mss.DatePublish = mainslideshow.DatePublish;

                if (datePB != mainslideshow.DatePublish)
                {
                    mss.DatePublish = mss.DatePublish.AddDays(1);
                }

                var dateEB = mss.DateExpire;

                mss.DateExpire = mainslideshow.DateExpire;

                if (dateEB != mainslideshow.DateExpire)
                {
                    mss.DateExpire = mss.DateExpire.AddDays(1);
                }

                mss.Name = mainslideshow.Name;
                mss.Page = mainslideshow.Page;
                mss.Title = mainslideshow.Title;
                mss.Desc = mainslideshow.Desc;
                mss.PostId = mainslideshow.PostId;
                mss.ShowState = mainslideshow.ShowState;

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

                var sl = db.MainSlideShows
                    .Include(c => c.Post)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    sl = sl.Where(c => c.Name.Contains(query) || c.Title.Contains(query) || c.Post.Name.Contains(query) ||
                            c.Desc.Contains(query));
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
                    if (getparams.sort.Equals("post"))
                    {
                        sl = sl.OrderBy(c => c.Post.Name);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderBy(c => c.DatePublish).ThenBy(c => c.DateExpire);
                    }
                    if (getparams.sort.Equals("state"))
                    {
                        sl = sl.OrderBy(c => c.ShowState);
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
                    if (getparams.sort.Equals("post"))
                    {
                        sl = sl.OrderByDescending(c => c.Post.Name);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderByDescending(c => c.DatePublish).ThenByDescending(c => c.DateExpire);
                    }
                    if (getparams.sort.Equals("state"))
                    {
                        sl = sl.OrderByDescending(c => c.ShowState);
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
                        Name = c.Name,
                        postName = c.Post.Name,
                        datePublishString = c.DatePublish.ToPersianDate(),
                        dateExpireString = c.DateExpire.ToPersianDate(),
                        ShowState = c.ShowState
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

                var sl = await db.MainSlideShows.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAllIndex()
        {
            try
            {

                var slides = await db.MainSlideShows.Where(c => c.ShowState == true &&
                         c.DatePublish <= DateTime.Now && c.DateExpire > DateTime.Now).OrderBy(c => c.Page)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        PostId = c.Post.Id,
                        postUrl = c.Post.Url,
                        imgUrl = c.imgUrl
                    })
                .ToListAsync();


                return this.DataFunction(true, slides);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getMSS([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.MainSlideShows.FirstOrDefaultAsync(c => c.Id == id);

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

                        var sl = await db.MainSlideShows
                            .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.MainSlideShows.Remove(sl);
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
        public async Task<IActionResult> changeShowState([FromBody] changeShowStateParam param)
        {
            try
            {
                var mss = await db.MainSlideShows.FirstOrDefaultAsync(c => c.Id == param.id);

                mss.ShowState = param.showState;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


    }

    public class changeShowStateParam
    {
        public int id { get; set; }

        public bool showState { get; set; }
    }
}