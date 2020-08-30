using System;
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
    public class ProductController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public ProductController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product param)
        {
            try
            {

                var picdata = param.PicData;
                var picname = param.PicName;

                var guid = System.Guid.NewGuid().ToString();

                var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                byte[] bytes = Convert.FromBase64String(picdata);
                System.IO.File.WriteAllBytes(path, bytes);

                param.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);

                await db.Products.AddAsync(param);

                await db.SaveChangesAsync();


                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Product param)
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
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(picdata);
                    System.IO.File.WriteAllBytes(path, bytes);

                    param.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);
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
        public async Task<IActionResult> Get([FromBody] getProductParam param)
        {
            try
            {
                var getparams = param.getparams;

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var sl = db.Products
                        .Include(c => c.Writer)
                        .Include(c => c.ProductCategory)
                    .Where(c => c.TotalType == (ProductTotalType)param.totalType)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Title.Contains(query) || c.Desc.Contains(query) || c.Writer.FullName.Contains(query)
                            || c.ProductCategory.Title.Contains(query) || c.TotalPrice.ToString().Contains(query));
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
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderBy(c => c.Type).ThenBy(c => c.Value);
                    }
                    if (getparams.sort.Equals("writer"))
                    {
                        sl = sl.OrderBy(c => c.Writer.FullName);
                    }
                    if (getparams.sort.Equals("productCategory"))
                    {
                        sl = sl.OrderBy(c => c.ProductCategory.Title);
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
                    if (getparams.sort.Equals("type"))
                    {
                        sl = sl.OrderByDescending(c => c.Type).ThenByDescending(c => c.Value);
                    }
                    if (getparams.sort.Equals("writer"))
                    {
                        sl = sl.OrderByDescending(c => c.Writer.FullName);
                    }
                    if (getparams.sort.Equals("productCategory"))
                    {
                        sl = sl.OrderByDescending(c => c.ProductCategory.Title);
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
                        Title = c.Title,
                        Type = c.Type,
                        Value = c.Value,
                        writerString = c.Writer.FullName,
                        productCategoryString = c.ProductCategory.Title,
                        haveAnyLink = c.Links.Any()
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
        public async Task<IActionResult> getAll([FromBody] int type)
        {
            try
            {
                var sl = await db.Products
                        .Where(c => c.TotalType == (ProductTotalType)type)
                    .Select(c => new { id = c.Id, Title = c.Title, Type = c.Type })
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
        public async Task<IActionResult> getProductTitle([FromBody] int id)
        {
            try
            {
                var product = await db.Products.FirstOrDefaultAsync(c => c.Id == id);

                return this.DataFunction(true, product.Title);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getIndex([FromBody] getIndexParam param)
        {
            try
            {
                var pageSize = 12;

                int count;

                var products = db.Products
                        .Include(c => c.Writer)
                        .Include(c => c.ProductCategory)
                        .Include(c => c.Links)
                    .Where(c => c.TotalType == (ProductTotalType)param.totalType)
                .AsQueryable();

                var query = param.searchText;

                if (!string.IsNullOrWhiteSpace(query))
                {
                    products = products.Where(c => c.Title.Contains(query) ||
                                c.Desc.Contains(query) || c.Tags.Contains(query) || c.Writer.FullName.Contains(query));
                }

                if (param.cats.Any())
                {
                    products = products.Where(c => param.cats.Contains(c.ProductCategory.Id));
                }

                count = await products.CountAsync();

                products = products.OrderByDescending(c => c.Id);

                if (param.sort.Equals("new"))
                {
                    products = products.OrderByDescending(c => c.Id);
                }

                if (param.sort.Equals("like"))
                {
                    products = products.OrderByDescending(c => c.Like);
                }

                if (param.sort.Equals("lowPrice"))
                {
                    products = products.OrderBy(c => c.TotalPrice);
                }

                if (param.sort.Equals("highPrice"))
                {
                    products = products.OrderByDescending(c => c.TotalPrice);
                }


                products = products.Skip((param.page - 1) * pageSize);
                products = products.Take(pageSize);

                var q = await products
                    .Select(c => new
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Type = c.Type,
                        Value = c.Links.Count,
                        TotalPrice = c.TotalPrice.ToString("#,##0"),
                        writerString = c.Writer.FullName,
                        writerPic = c.Writer.PicUrl,
                        PicUrl = c.PicUrl
                    })
                .ToListAsync();

                return Json(new jsondata
                {
                    success = true,
                    data = new
                    {
                        products = q,
                        totalCount = count
                    }
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> getProductIndex([FromBody] int id)
        {
            try
            {
                var product = await db.Products
                    .Include(c => c.Writer)
                    .Include(c => c.Links)
                    .Include(c => c.Links)
                    .Include(c => c.Comments)
                .FirstOrDefaultAsync(c => c.Id == id);

                product.Comments = product.Comments.Where(c => c.HaveComformed == true).ToList();

                return this.DataFunction(true, product);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getProduct([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.Products.FirstOrDefaultAsync(c => c.Id == id);

                    return this.SuccessFunction(data: sl);
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

                        var sl = await db.Products
                            .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        if (sl.haveAnyLink)
                        {
                            return this.UnSuccessFunction("برخی از محصولات دارای لینک هایی هستند!");
                        }

                        var picurl = sl.PicUrl;

                        if (!string.IsNullOrEmpty(picurl))
                        {
                            System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                        }

                        db.Products.Remove(sl);
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

    public class getProductParam
    {
        public getparams getparams { get; set; }

        public int totalType { get; set; }
    }

    public class getIndexParam
    {
        public int page { get; set; }
        public string sort { get; set; }
        public string searchText { get; set; }
        public int totalType { get; set; }
        public int[] cats { get; set; }
    }
}