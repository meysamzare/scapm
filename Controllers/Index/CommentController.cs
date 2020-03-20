using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model;
using SCMR_Api.Model.Index;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class CommentController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;
        private IHttpContextAccessor accessor;

        public CommentController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment, IHttpContextAccessor _accessor)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
            accessor = _accessor;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> setComment([FromBody] Comment comment)
        {
            try
            {
                if (await db.Comments.AnyAsync(c => c.PostId == comment.PostId && c.Email == comment.Email && c.Content == comment.Content))
                {
                    return this.UnSuccessFunction("شما قبلا همین نظر را برای این پست ارسال کرده اید");
                }

                comment.Date = DateTime.Now;
                comment.HaveComformed = false;
                comment.Ip = accessor.HttpContext.Connection.RemoteIpAddress.ToString();

                await db.Comments.AddAsync(comment);

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
        public async Task<IActionResult> setProductComment([FromBody] ProductComment comment)
        {
            try
            {
                if (await db.ProductComments.AnyAsync(c => c.ProductId == comment.ProductId && c.Email == comment.Email && c.Content == comment.Content))
                {
                    return this.UnSuccessFunction("شما قبلا همین نظر را برای این پست ارسال کرده اید");
                }

                comment.Date = DateTime.Now;
                comment.HaveComformed = false;
                comment.Ip = accessor.HttpContext.Connection.RemoteIpAddress.ToString();

                await db.ProductComments.AddAsync(comment);

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
        public async Task<IActionResult> getPostComments([FromBody] int postId)
        {
            try
            {
                var comments = await db.Comments
                    .Where(c => c.PostId == postId && c.HaveComformed == true)
                .ToListAsync();


                return this.DataFunction(true, comments);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getProductComments([FromBody] int productId)
        {
            try
            {
                var comments = await db.ProductComments
                    .Where(c => c.ProductId == productId && c.HaveComformed == true)
                .ToListAsync();


                return this.DataFunction(true, comments);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> setComformState([FromBody] commentStateParam commentstate)
        {
            try
            {
                dynamic comment = null;

                if (commentstate.totalType == 0)
                {
                    comment = await db.Comments.FirstOrDefaultAsync(c => c.Id == commentstate.id);
                    comment.HaveComformed = commentstate.comformed;
                }
                else
                {
                    comment = await db.ProductComments.FirstOrDefaultAsync(c => c.Id == commentstate.id);
                    comment.HaveComformed = commentstate.comformed;
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
        public async Task<IActionResult> Get([FromBody] getCommentParam param)
        {
            try
            {
                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var sl = db.Comments.AsQueryable();

                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.FullName.Contains(query) || c.Content.Contains(query) || c.Email.Contains(query)
                                        || c.Id.ToString().Contains(query));
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
                    if (getparams.sort.Equals("postTitle"))
                    {
                        sl = sl.OrderBy(c => c.Post.Title);
                    }
                    if (getparams.sort.Equals("haveComformed"))
                    {
                        sl = sl.OrderBy(c => c.HaveComformed);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderBy(c => c.Date);
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
                    if (getparams.sort.Equals("postTitle"))
                    {
                        sl = sl.OrderByDescending(c => c.Post.Title);
                    }
                    if (getparams.sort.Equals("haveComformed"))
                    {
                        sl = sl.OrderByDescending(c => c.HaveComformed);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderByDescending(c => c.Date);
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
                        productTitle = c.Post.Title,
                        Content = c.Content,
                        Email = c.Email,
                        dateString = c.Date.ToPersianDate(),
                        HaveComformed = c.HaveComformed,
                        haveChildren = c.Childrens.Any()
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
        public async Task<IActionResult> Get_Product([FromBody] getCommentParam param)
        {
            try
            {
                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;

                int totalType = param.totalType;



                var sl = db.ProductComments.Include(c => c.Product).AsQueryable();

                if (totalType == 1)
                {
                    sl = sl.Where(c => c.Product.TotalType == ProductTotalType.Docs);
                }

                if (totalType == 2)
                {
                    sl = sl.Where(c => c.Product.TotalType == ProductTotalType.VirtualLearn);
                }


                var query = getparams.q;

                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.FullName.Contains(query) || c.Content.Contains(query) || c.Email.Contains(query)
                                        || c.Id.ToString().Contains(query));
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
                    if (getparams.sort.Equals("postTitle"))
                    {
                        sl = sl.OrderBy(c => c.Product.Title);
                    }
                    if (getparams.sort.Equals("haveComformed"))
                    {
                        sl = sl.OrderBy(c => c.HaveComformed);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderBy(c => c.Date);
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
                    if (getparams.sort.Equals("postTitle"))
                    {
                        sl = sl.OrderByDescending(c => c.Product.Title);
                    }
                    if (getparams.sort.Equals("haveComformed"))
                    {
                        sl = sl.OrderByDescending(c => c.HaveComformed);
                    }
                    if (getparams.sort.Equals("date"))
                    {
                        sl = sl.OrderByDescending(c => c.Date);
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
                        productTitle = c.Product.Title,
                        Content = c.Content,
                        Email = c.Email,
                        dateString = c.Date.ToPersianDate(),
                        HaveComformed = c.HaveComformed,
                        haveChildren = c.Childrens.Any()
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
        public async Task<IActionResult> getRelatedComment([FromBody] getRelatedCommentParam param)
        {
            try
            {
                dynamic relatedComment = null;
                dynamic comment = null;


                if (param.totalType == 0)
                {
                    relatedComment = await getCommentParents(param.commentId);
                    comment = await db.Comments.Include(c => c.Childrens).FirstOrDefaultAsync(c => c.Id == param.commentId);
                }
                else
                {
                    relatedComment = await getCommentParents_Product(param.commentId);
                    comment = await db.ProductComments.Include(c => c.Childrens).FirstOrDefaultAsync(c => c.Id == param.commentId);
                }


                relatedComment.Add(comment);

                if (comment.Childrens.Count != 0)
                {
                    relatedComment.AddRange(comment.Childrens);
                }

                return Json(new jsondata
                {
                    success = true,
                    data = relatedComment
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        List<Comment> commentList = new List<Comment>();
        private async Task<List<Comment>> getCommentParents(int commentId)
        {
            var comment = await db.Comments
                .Include(c => c.Parent)
            .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment.ParentId != null)
            {
                commentList.Add(comment.Parent);

                await getCommentParents(comment.Parent.Id);
            }

            return commentList;
        }

        List<ProductComment> commentListProduct = new List<ProductComment>();
        private async Task<List<ProductComment>> getCommentParents_Product(int commentId)
        {
            var comment = await db.ProductComments
                .Include(c => c.Parent)
            .FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment.ParentId != null)
            {
                commentListProduct.Add(comment.Parent);

                await getCommentParents(comment.Parent.Id);
            }

            return commentListProduct;
        }


    }

    public class getRelatedCommentParam
    {
        public int commentId { get; set; }

        public int totalType { get; set; }
    }

    public class getCommentParam
    {
        public getparams getparams { get; set; }

        public int totalType { get; set; }
    }

    public class commentStateParam
    {
        public int id { get; set; }

        public bool comformed { get; set; }

        public int totalType { get; set; }
    }
}