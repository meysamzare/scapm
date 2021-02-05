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
    public class PostController : Controller
    {
        public Data.DbContext db;

        private IHostingEnvironment hostingEnvironment;

        public PostController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Post post)
        {
            try
            {
                post.DateCreate = DateTime.Now;
                post.SumRating = 0;
                post.RatingCount = 0;
                post.ViewCount = 0;


                if (!string.IsNullOrEmpty(post.HeaderPicData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, post.HeaderPicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(post.HeaderPicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    post.HeaderPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + post.HeaderPicName);
                }

                post.Url = post.Url.Trim().Replace(" ", "-").Replace("/", "-");

                await db.Posts.AddAsync(post);

                await db.SaveChangesAsync();

                return this.SuccessFunction(message: post.Id.ToString());
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Post post)
        {
            try
            {
                var po = await db.Posts.FirstOrDefaultAsync(c => c.Id == post.Id);

                po.DateEdited = DateTime.Now;
                po.Url = post.Url.Trim().Replace(' ', '-').Replace("/", "-");

                if (!string.IsNullOrEmpty(post.HeaderPicData))
                {
                    if (!string.IsNullOrEmpty(po.HeaderPicUrl))
                    {
                        try
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + po.HeaderPicUrl);
                        }
                        catch { }
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, post.HeaderPicName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(post.HeaderPicData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    po.HeaderPicUrl = Path.Combine("/UploadFiles/" + guid + "/" + post.HeaderPicName);
                }

                po.Content = post.Content;
                po.ShortContent = post.ShortContent;
                po.Title = post.Title;
                po.HaveComment = post.HaveComment;
                po.Name = post.Name;
                po.Order = post.Order;
                po.Tags = post.Tags;
                po.IsHighLight = post.IsHighLight;
                po.IsActive = post.IsActive;
                po.Type = post.Type;


                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromBody] getPostParam getPostParam)
        {
            try
            {

                var getparams = getPostParam.getParams;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var sl = db.Posts.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Name.Contains(query) || c.Content.Contains(query) || c.Author.Contains(query) || c.Title.Contains(query)
                     || c.Tags.Contains(query) || c.ShortContent.Contains(query));
                }

                if (getPostParam.postType.HasValue)
                {
                    sl = sl.Where(c => c.Type == (PostType)getPostParam.postType.Value);
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
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderBy(c => c.Title);
                    }
                    if (getparams.sort.Equals("author"))
                    {
                        sl = sl.OrderBy(c => c.Author);
                    }
                    if (getparams.sort.Equals("state"))
                    {
                        sl = sl.OrderBy(c => c.IsActive).ThenBy(c => c.IsHighLight);
                    }
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderBy(c => c.Type);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderBy(c => c.DateCreate).ThenBy(c => c.DateEdited);
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
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderByDescending(c => c.Title);
                    }
                    if (getparams.sort.Equals("author"))
                    {
                        sl = sl.OrderByDescending(c => c.Author);
                    }
                    if (getparams.sort.Equals("state"))
                    {
                        sl = sl.OrderByDescending(c => c.IsActive).ThenByDescending(c => c.IsHighLight);
                    }
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderByDescending(c => c.Type);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderByDescending(c => c.DateCreate).ThenByDescending(c => c.DateEdited);
                    }
                }
                else
                {
                    sl = sl.OrderBy(c => c.DateCreate);
                }

                sl = sl.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                sl = sl.Take(getparams.pageSize);

                var q = await sl
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Title = c.Title,
                        Author = c.Author,
                        dateCreateString = c.dateCreateString,
                        dateEditedString = c.dateEditedString,
                        IsActive = c.IsActive,
                        IsHighLight = c.IsHighLight,
                        Type = c.Type,
                        haveSchedules = c.Schedules.Any()
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
        [AllowAnonymous]
        public async Task<IActionResult> GetIndex([FromBody] getindexPram getparam)
        {
            try
            {
                var pageSize = 8;

                int count;

                var posts = db.Posts
                    .Include(c => c.Comments)
                .AsQueryable();

                if (getparam.type != 0)
                {
                    posts = posts.Where(c => c.Type == (PostType)getparam.type);
                }

                if (!string.IsNullOrWhiteSpace(getparam.searchText))
                {
                    posts = posts.Where(c => c.Title.Contains(getparam.searchText) ||
                                c.ShortContent.Contains(getparam.searchText) || c.Tags.Contains(getparam.searchText));
                }

                if (getparam.last15)
                {
                    posts = posts.Where(c => DateTime.Now.AddDays(-15) < c.DateCreate);
                }

                if (getparam.lastMonth)
                {
                    posts = posts.Where(c => DateTime.Now.getPersianDateMonthInt() == c.DateCreate.getPersianDateMonthInt());
                }

                if (getparam.tags.Any())
                {
                    foreach (var tag in getparam.tags)
                    {
                        posts = posts.Where(c => c.Tags.Contains(tag));
                    }
                }

                if (getparam.authors.Any())
                {
                    posts = posts.Where(c => getparam.authors.Contains(c.Author));
                }

                count = await posts.CountAsync();

                posts = posts.OrderByDescending(c => c.DateCreate);

                if (getparam.sort.Equals("new"))
                {
                    posts = posts.OrderByDescending(c => c.DateCreate);
                }

                if (getparam.sort.Equals("comment"))
                {
                    posts = posts.OrderByDescending(c => c.Comments.Where(l => l.HaveComformed == true).Count());
                }

                if (getparam.sort.Equals("view"))
                {
                    posts = posts.OrderByDescending(c => c.ViewCount);
                }

                posts = posts.Skip((getparam.page - 1) * pageSize);
                posts = posts.Take(pageSize);

                var q = await posts
                    .Select(c => new
                    {
                        Id = c.Id,
                        Url = c.Url,
                        HeaderPicUrl = c.HeaderPicUrl,
                        Type = c.Type,
                        Title = c.Title,
                        ShortContent = c.ShortContent,
                        ViewCount = c.ViewCount,
                        commentCount = c.Comments.Where(l => l.HaveComformed == true).Count(),
                        dateCreateString = c.DateCreate.ToPersianDate(),
                        Author = c.Author,
                        IsHighLight = c.IsHighLight
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

                var sl = await db.Posts.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getAllFeedIndex()
        {
            try
            {

                var feeds = await db.Posts
                    .Where(c => c.Type == PostType.feed && c.IsHighLight == true && c.IsActive == true)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Url = c.Url,
                        Title = c.Title,
                    })
                .ToListAsync();

                return this.DataFunction(true, feeds);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getLastNewsIndex()
        {
            try
            {

                var feeds = await db.Posts
                    .Where(c => c.Type == PostType.feed && c.IsActive == true)
                    .OrderByDescending(c => c.DateCreate)
                    .Take(7)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Url = c.Url,
                        HeaderPicUrl = c.HeaderPicUrl,
                        Title = c.Title,
                        ShortContent = c.ShortContent,
                        ViewCount = c.ViewCount,
                        commentCount = c.Comments.Where(l => l.HaveComformed == true).Count()
                    })
                .ToListAsync();

                return this.DataFunction(true, feeds);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getLastPostIndex()
        {
            try
            {

                var notHighLightPosts = await db.Posts
                    .Where(c => c.IsActive && !c.IsHighLight)
                    .OrderByDescending(c => c.DateCreate)
                        .Take(12)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Url = c.Url,
                        headerPicUrl = c.HeaderPicUrl,
                        Title = c.Title,
                        ShortContent = c.ShortContent,
                        ViewCount = c.ViewCount,
                        commentCount = c.Comments.Where(l => l.HaveComformed == true).Count(),
                        DateCreate = c.DateCreate,
                        IsHighLight = c.IsHighLight
                    })
                .ToListAsync();

                var lastTwoHighLightPosts = await db.Posts
                    .Where(c => c.IsActive && c.IsHighLight)
                        .OrderByDescending(c => c.DateCreate)
                        .Take(2)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Url = c.Url,
                        headerPicUrl = c.HeaderPicUrl,
                        Title = c.Title,
                        ShortContent = c.ShortContent,
                        ViewCount = c.ViewCount,
                        commentCount = c.Comments.Where(l => l.HaveComformed == true).Count(),
                        DateCreate = c.DateCreate,
                        IsHighLight = c.IsHighLight
                    })
                .ToListAsync();

                notHighLightPosts.AddRange(lastTwoHighLightPosts);

                notHighLightPosts = notHighLightPosts.OrderByDescending(c => Guid.NewGuid()).ToList();

                return this.DataFunction(true, notHighLightPosts);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getLastSpecialPostIndex()
        {
            try
            {

                var posts = await db.Posts
                    .Where(c => c.IsActive == true && c.IsHighLight == true)
                    .OrderByDescending(c => c.DateCreate)
                    .Take(15)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Url = c.Url,
                        HeaderPicUrl = c.HeaderPicUrl,
                        Title = c.Title,
                        ShortContent = c.ShortContent,
                        ViewCount = c.ViewCount,
                        commentCount = c.Comments.Where(l => l.HaveComformed == true).Count()
                    })
                .ToListAsync();

                return this.DataFunction(true, posts);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getLastFadakIndex()
        {
            try
            {

                var fadaks = await db.Posts
                    .Where(c => c.Type == PostType.fadak && c.IsActive == true)
                    .OrderByDescending(c => c.DateCreate)
                    .Take(10)
                    .Select(c => new
                    {
                        Id = c.Id,
                        Url = c.Url,
                        HeaderPicUrl = c.HeaderPicUrl,
                        Title = c.Title,
                        ShortContent = c.ShortContent,
                        ViewCount = c.ViewCount,
                        commentCount = c.Comments.Where(l => l.HaveComformed == true).Count()
                    })
                .ToListAsync();

                return this.DataFunction(true, fadaks);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getPost([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.Posts.FirstOrDefaultAsync(c => c.Id == id);

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
        public async Task<IActionResult> getPostTitle([FromBody] int id)
        {
            try
            {
                var post = await db.Posts.FirstOrDefaultAsync(c => c.Id == id);

                if (post == null)
                {
                    return this.DataFunction(false, "");
                }

                return this.DataFunction(true, post.Url.Trim());
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Postse([FromBody] int postId)
        {
            try
            {
                var post = await db.Posts.FirstOrDefaultAsync(c => c.Id == postId);

                post.ViewCount += 1;

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
        public async Task<IActionResult> getTagsAuthorsByType([FromBody] int type)
        {
            try
            {
                var tags = new List<string>();
                var authors = new List<string>();

                var postList = db.Posts.AsQueryable();

                if (type == 0)
                {
                    postList = postList.Where(c => c.IsActive == true);
                }
                else
                {
                    postList = postList.Where(c => c.Type == (PostType)type && c.IsActive == true);
                }

                var posttsgs = await postList.Select(c => c.Tags).ToArrayAsync();

                foreach (var posttag in posttsgs)
                {
                    var tgs = posttag.Split(",");

                    foreach (var tg in tgs)
                    {
                        if (!tags.Any(c => c == tg) && !string.IsNullOrWhiteSpace(tg))
                        {
                            tags.Add(tg);
                        }
                    }
                }

                var auths = await postList.Select(c => c.Author).ToArrayAsync();

                foreach (var auth in auths)
                {
                    if (!authors.Any(c => c == auth))
                    {
                        authors.Add(auth);
                    }
                }

                return this.DataFunction(true, new
                {
                    tags = tags,
                    authors = authors
                });
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

                        var sl = await db.Posts.Include(c => c.Schedules)
                            .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (sl.haveSchedules)
                        {
                            return this.UnSuccessFunction(" پست " + sl.Name + " دارای رویداد هایی است", "error");
                        }


                        var picurl = sl.HeaderPicUrl;

                        if (!string.IsNullOrEmpty(picurl))
                        {
                            try
                            {
                                System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                            }
                            catch { }
                        }

                        db.Posts.Remove(sl);
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
        public async Task<IActionResult> changeActiveState([FromBody] changeActiveParam param)
        {
            try
            {
                var post = await db.Posts.FirstOrDefaultAsync(c => c.Id == param.postId);

                post.IsActive = param.active;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

    }

    public class changeActiveParam
    {
        public int postId { get; set; }

        public bool active { get; set; }
    }

    public class getPostParam
    {
        public getparams getParams { get; set; }

        public int? postType { get; set; }
    }

    public class getindexPram
    {
        public int page { get; set; }

        public int type { get; set; }

        public string sort { get; set; }

        public string searchText { get; set; }

        public bool last15 { get; set; }
        public bool lastMonth { get; set; }

        public string[] tags { get; set; }
        public string[] authors { get; set; }
    }
}