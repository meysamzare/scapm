using System;
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
    public class OrgChartController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public OrgChartController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrgChart orgchart)
        {
            try
            {
                await db.OrgCharts.AddAsync(orgchart);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] OrgChart orgchart)
        {
            try
            {
                var och = await db.OrgCharts.SingleAsync(c => c.Id == orgchart.Id);

                if (orgchart.Id == orgchart.ParentId)
                {
                    return this.UnSuccessFunction("این چارت سازمانی نمیتواند انتخاب شود C1");
                }

                if (await isInChildOrgChart(orgchart.Id, orgchart.ParentId))
                {
                    return this.UnSuccessFunction("این چارت سازمانی نمیتواند انتخاب شود C2");
                }

                och.Name = orgchart.Name;
                och.Order = orgchart.Order;
                och.ParentId = orgchart.ParentId;
                och.Code = orgchart.Code;
                och.Desc = orgchart.Desc;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        public bool isinchild { get; set; }

        public async Task<bool> isInChildOrgChart(int ochId, int? selectedId)
        {
            var cat = await db.OrgCharts.Include(c => c.Children).SingleAsync(c => c.Id == ochId);
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
                        await isInChildOrgChart(i.Id, selectedId);
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

                var och = db.OrgCharts.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    och = och.Where(c => c.Name.Contains(query) || c.Desc.Contains(query) || c.Order.ToString().Contains(query) ||
                                    c.Code.ToString().Contains(query));

                }

                count = och.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        och = och.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        och = och.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        och = och.OrderBy(c => c.Code);
                    }
                    if (getparams.sort.Equals("parentTitle"))
                    {
                        och = och.OrderBy(c => c.Parent.Name);
                    }
                    if (getparams.sort.Equals("desc"))
                    {
                        och = och.OrderBy(c => c.Desc);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        och = och.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        och = och.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        och = och.OrderByDescending(c => c.Code);
                    }
                    if (getparams.sort.Equals("parentTitle"))
                    {
                        och = och.OrderByDescending(c => c.Parent.Name);
                    }
                    if (getparams.sort.Equals("desc"))
                    {
                        och = och.OrderByDescending(c => c.Desc);
                    }
                }
                else
                {
                    och = och.OrderBy(c => c.Id);
                }

                och = och.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                och = och.Take(getparams.pageSize);

                var q = await och
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Code = c.Code,
                        parentTitle = c.ParentId == null ? "ریشه" : c.Parent.Name,
                        haveChildren = c.Children.Any()
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
        public async Task<IActionResult> getChart([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var och = await db.OrgCharts.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, och);
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

                var och = await db.OrgCharts.Select(c => new { id = c.Id, name = c.Name }).ToListAsync();

                return this.DataFunction(true, och);
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

                        var och = await db.OrgCharts
                            .SingleAsync(c => c.Id == id);

                        if (och == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (och.haveChildren)
                        {
                            return this.UnSuccessFunction("نمیتوان چارت " + och.Name + " را حذف کرد", "error");
                        }

                        db.OrgCharts.Remove(och);
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

        public IEnumerable<OrgChart> orgcharts
        {
            get
            {
                return db.OrgCharts.Include(c => c.Parent).Include(c => c.Children).ToList();
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

            foreach (var i in orgcharts.Where(c => c.ParentId == null))
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

            foreach (var i in orgcharts.Where(c => c.ParentId == idd))
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