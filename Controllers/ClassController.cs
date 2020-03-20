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
    [ApiController]
    [Authorize]
    public class ClassController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public ClassController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Class clas)
        {
            try
            {

                var grade = await db.Grades
                    .Include(c => c.Classes)
                .FirstOrDefaultAsync(c => c.Id == clas.GradeId);

                var registredCapasityCount = grade.Classes.Sum(c => c.Capasity);
                var gradeCapasity = grade.Capasity;
                var freeCapasity = gradeCapasity - registredCapasityCount;

                if (freeCapasity < clas.Capasity)
                {
                    return this.UnSuccessFunction("ظرفیت این پایه تحصیلی تکمیل است، ظرفیت باقی مانده: " + freeCapasity);
                }

                await db.Classes.AddAsync(clas);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Class clas)
        {
            try
            {
                var cl = await db.Classes.SingleAsync(c => c.Id == clas.Id);


                var grade = await db.Grades
                    .Include(c => c.Classes)
                .FirstOrDefaultAsync(c => c.Id == clas.GradeId);

                var registredCapasityCount = grade.Classes.Sum(c => c.Capasity);
                var gradeCapasity = grade.Capasity;
                var freeCapasity = gradeCapasity - registredCapasityCount;

                var addedCapasity = clas.Capasity - cl.Capasity;

                if ((addedCapasity > 0) && (freeCapasity < addedCapasity))
                {
                    return this.UnSuccessFunction("ظرفیت این پایه تحصیلی تکمیل است، ظرفیت باقی مانده: " + freeCapasity);
                }

                cl.Capasity = clas.Capasity;
                cl.Code = clas.Code;
                cl.GradeId = clas.GradeId;
                cl.Name = clas.Name;
                cl.Order = clas.Order;
                cl.Section = clas.Section;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getClass([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var cls = await db.Classes.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, cls);
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
        public async Task<IActionResult> getClassByGrade([FromBody] int gradeid)
        {
            try
            {

                if (gradeid != 0)
                {
                    var cls = await db.Classes.Where(c => c.GradeId == gradeid).ToListAsync();

                    return this.DataFunction(true, cls);
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
                var nowYeareducationId = await this.getActiveYeareducationId();

                var cls = await db.Classes
                        .Select(c => new { id = c.Id, name = c.Name, GradeId = c.GradeId, yeareducationId = c.Grade.YeareducationId })
                    .Where(c => c.yeareducationId == nowYeareducationId)
                .ToListAsync();

                return this.DataFunction(true, cls);
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

                        var cls = await db.Classes
                            .SingleAsync(c => c.Id == id);

                        if (cls == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.Classes.Remove(cls);
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
        public async Task<IActionResult> Get([FromBody] getparams getparams)
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                getparams.pageIndex += 1;

                int count;


                var query = getparams.q;

                var cls = db.Classes
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Code = c.Code,
                        Section = c.Section,
                        gradeName = c.Grade.Name,
                        Capasity = c.Capasity,
                        haveExam = c.Exams.Any(),
                        haveStdClassMngs = c.StdClassMngs.Any(),
                        YeareducationId = c.Grade.YeareducationId
                    })
                    .Where(c => c.YeareducationId == nowYeareducationId)
                .AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    cls = cls.Where(c => c.Name.Contains(query) || c.Section.Contains(query) ||
                                    c.Capasity.ToString().Contains(query) ||
                                    c.Code.ToString().Contains(query) || c.gradeName.Contains(query));
                }

                count = cls.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        cls = cls.OrderBy(c => c.Name);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        cls = cls.OrderBy(c => c.Code);
                    }
                    if (getparams.sort.Equals("section"))
                    {
                        cls = cls.OrderBy(c => c.Section);
                    }
                    if (getparams.sort.Equals("capacity"))
                    {
                        cls = cls.OrderBy(c => c.Capasity);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("name"))
                    {
                        cls = cls.OrderByDescending(c => c.Name);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        cls = cls.OrderByDescending(c => c.Code);
                    }
                    if (getparams.sort.Equals("section"))
                    {
                        cls = cls.OrderByDescending(c => c.Section);
                    }
                    if (getparams.sort.Equals("capacity"))
                    {
                        cls = cls.OrderByDescending(c => c.Capasity);
                    }
                }
                else
                {
                    cls = cls.OrderBy(c => c.Id);
                }

                cls = cls.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                cls = cls.Take(getparams.pageSize);

                var q = await cls.ToListAsync();

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
        public async Task<IActionResult> getPrintData()
        {
            try
            {
                var nowYeareducationId = await this.getActiveYeareducationId();

                var headers = new List<string>();
                var datas = new List<List<string>>();

                var title = "لیست کلاس ها";


                headers.Add("ردیف");
                headers.Add("کد کلاس");
                headers.Add("عنوان");
                headers.Add("شعبه");
                headers.Add("مقطع تحصیلی");
                headers.Add("ظرفیت");

                var classes = await db.Classes.Include(c => c.Grade).ToListAsync();

                classes.Where(c => c.Grade.YeareducationId == nowYeareducationId);

                classes.ForEach(cls =>
                {
                    var lstData = new List<string>();

                    lstData.Add(cls.Id.ToString());
                    lstData.Add(cls.Code.ToString());
                    lstData.Add(cls.Name);
                    lstData.Add(cls.Section);
                    lstData.Add(cls.Grade.Name);
                    lstData.Add(cls.Capasity.ToString());

                    datas.Add(lstData);
                });

                return this.DataFunction(true, new PrintData
                {
                    headers = headers,
                    datas = datas,
                    title = title
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


    }


    public class PrintData
    {
        public List<string> headers { get; set; }

        public List<List<string>> datas { get; set; }

        public string title { get; set; }
    }
}