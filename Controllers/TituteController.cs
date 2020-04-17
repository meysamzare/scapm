using System.Collections.Generic;
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
    [Authorize]
    [ApiController]
    public class TituteController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public TituteController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] InsTitute titute)
        {
            try
            {
                if (titute.TituteCode == 0) {
                    titute.TituteCode = null;
                }
                await db.InsTitutes.AddAsync(titute);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] InsTitute titute)
        {
            try
            {
                var tit = await db.InsTitutes.SingleAsync(c => c.Id == titute.Id);

                if (titute.Id == titute.TituteCode)
                {
                    return this.UnSuccessFunction("این آموزشگاه نمیتواند انتخاب شود C1");
                }

                if (await isInChildTitute(titute.Id, titute.TituteCode))
                {
                    return this.UnSuccessFunction("این آموزشگاه نمیتواند انتخاب شود C2");
                }

                if (titute.TituteCode == 0) {
                    titute.TituteCode = null;
                }

                tit.Name = titute.Name;
                tit.OrgCode = titute.OrgCode;
                tit.OrgSection = titute.OrgSection;
                tit.OrgSex = titute.OrgSex;
                tit.PostCode = titute.PostCode;
                tit.State = titute.State;
                tit.Tell = titute.Tell;
                tit.TituteCode = titute.TituteCode;
                tit.Address = titute.Address;
                tit.City = titute.City;
                tit.Desc = titute.Desc;
                tit.Email = titute.Email;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        public bool isinchild { get; set; }

        public async Task<bool> isInChildTitute(int catId, int? selectedId)
        {
            var cat = await db.InsTitutes.Include(c => c.Children).SingleAsync(c => c.Id == catId);
            foreach (var i in cat.Children)
            {
                if (i.Id == selectedId)
                {
                    isinchild = true;
                }
                if (i.Children != null)
                {
                    if (i.Children.Any())
                    {
                        await isInChildTitute(i.Id, selectedId);
                    }
                }
            }

            return isinchild;
        }


        [HttpPost]
        public async Task<IActionResult> Get([FromBody] getparams getparams)
        {
            try
            {
                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var tit = db.InsTitutes.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    tit = tit.Where(c => c.Name.Contains(query) || c.OrgCode.ToString().Contains(query) ||
                                    c.OrgSection.Contains(query) || c.State.Contains(query) || c.City.Contains(query) ||
                                    c.Address.Contains(query) || c.PostCode.Contains(query) || c.Tell.Contains(query) ||
                                    c.Email.Contains(query) || c.Desc.Contains(query));

                }

                count = tit.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        tit = tit.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        tit = tit.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("orgCode"))
                    {
                        tit = tit.OrderBy(c => c.OrgCode);
                    }
                    if (getparams.sort.Equals("orgSection"))
                    {
                        tit = tit.OrderBy(c => c.OrgSection);
                    }
                    if (getparams.sort.Equals("postCode"))
                    {
                        tit = tit.OrderBy(c => c.PostCode);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        tit = tit.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        tit = tit.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("orgCode"))
                    {
                        tit = tit.OrderByDescending(c => c.OrgCode);
                    }
                    if (getparams.sort.Equals("orgSection"))
                    {
                        tit = tit.OrderByDescending(c => c.OrgSection);
                    }
                    if (getparams.sort.Equals("postCode"))
                    {
                        tit = tit.OrderByDescending(c => c.PostCode);
                    }
                }
                else
                {
                    tit = tit.OrderBy(c => c.Id);
                }

                tit = tit.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                tit = tit.Take(getparams.pageSize);

                var q = await tit
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        OrgCode = c.OrgCode,
                        OrgSection = c.OrgSection,
                        State = c.State,
                        City = c.City,
                        PostCode = c.PostCode,
                        OrgSex = c.OrgSex,
                        haveChildren = c.Children.Any(),
                        haveGrade = c.Grades.Any()
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
        public async Task<IActionResult> getTitute([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var tit = await db.InsTitutes.SingleAsync(c => c.Id == id);

                    if (tit.TituteCode == null) {
                        tit.TituteCode = 0;
                    }

                    return this.DataFunction(true, tit);
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
        public async Task<IActionResult> getAll()
        {
            try
            {

                var tits = await db.InsTitutes.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, tits);
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

                        var tit = await db.InsTitutes
                        .SingleAsync(c => c.Id == id);

                        if (tit == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.InsTitutes.Remove(tit);
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

        public IEnumerable<InsTitute> titutes
        {
            get
            {
                return db.InsTitutes.Include(c => c.Parent).Include(c => c.Children).ToList();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetTreeRoot()
        {
            List<JsTreeModel> items = GetParentTree();

            return Json(items);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public JsonResult GetTreeChildren(string id)
        {
            List<JsTreeModel> items = GetTree(id);

            return Json(items);
        }

        List<JsTreeModel> GetParentTree()
        {
            var items = new List<JsTreeModel>();

            foreach (var i in titutes.Where(c => c.TituteCode == null))
            {
                items.Add(new JsTreeModel
                {
                    id = i.Id.ToString(),
                    text = i.Name,
                    parent = "#",
                    children = i.Children.Any()
                });
            }

            return items;
        }

        List<JsTreeModel> GetTree(string id)
        {
            var isInt = int.TryParse(id, out int idd);
            var items = new List<JsTreeModel>();

            foreach (var i in titutes.Where(c => c.TituteCode == idd))
            {
                items.Add(new JsTreeModel
                {
                    id = i.Id.ToString(),
                    text = i.Name,
                    parent = i.Parent.Id.ToString(),
                    children = i.Children.Any()
                });
            }

            return items;
        }




    }
}