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
    public class OrgPersonController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public OrgPersonController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrgPerson person)
        {
            try
            {
                person.BirthDate = person.BirthDate.AddDays(1);

                await db.OrgPeople.AddAsync(person);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] OrgPerson person)
        {
            try
            {
                person.BirthDate.AddDays(1);

                var pe = await db.OrgPeople.SingleAsync(c => c.Id == person.Id);

                var BirthDateBefore = pe.BirthDate;

                pe.BirthDate = person.BirthDate;

                if (BirthDateBefore != person.BirthDate)
                {
                    pe.BirthDate = pe.BirthDate.AddDays(1);
                }

                pe.Name = person.Name;
                pe.Code = person.Code;
                pe.LastName = person.LastName;
                pe.FatherName = person.FatherName;
                pe.sex = person.sex;
                pe.IdNum = person.IdNum;
                pe.IdNumber = person.IdNumber;
                pe.IdSerial = person.IdSerial;
                pe.Marrage = person.Marrage;
                pe.Child = person.Child;
                pe.InsuranceCode = person.InsuranceCode;
                pe.Type = person.Type;
                pe.TypeYear = person.TypeYear;
                pe.Address = person.Address;
                pe.Tell = person.Tell;
                pe.Phone = person.Phone;
                pe.Email = person.Email;
                pe.OrgChartId = person.OrgChartId;
                pe.SalaryId = person.SalaryId;
                pe.EducationId = person.EducationId;
                pe.InsuranceId = person.InsuranceId;

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

                var pe = db.OrgPeople.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    pe = pe.Where(c => c.Name.Contains(query) || c.Code.ToString().Contains(query) ||
                                    c.LastName.Contains(query) || c.FatherName.Contains(query) || c.IdNum.Contains(query) ||
                                    c.IdNumber.Contains(query) || c.IdSerial.Contains(query) || c.Child.ToString().Contains(query) ||
                                    c.InsuranceCode.Contains(query) || c.Type.Contains(query) || c.TypeYear.Contains(query) ||
                                    c.Address.Contains(query) || c.Tell.Contains(query) || c.Phone.Contains(query) || c.Email.Contains(query));
                }

                count = pe.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        pe = pe.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        pe = pe.OrderBy(c => c.Code);
                    }
                    if (getparams.sort.Equals("namelastname"))
                    {
                        pe = pe.OrderBy(c => c.Name).ThenBy(c => c.LastName);
                    }
                    if (getparams.sort.Equals("fatherName"))
                    {
                        pe = pe.OrderBy(c => c.FatherName);
                    }
                    if (getparams.sort.Equals("idNum"))
                    {
                        pe = pe.OrderBy(c => c.IdNum);
                    }
                    if (getparams.sort.Equals("phone"))
                    {
                        pe = pe.OrderBy(c => c.Phone);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        pe = pe.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        pe = pe.OrderByDescending(c => c.Code);
                    }
                    if (getparams.sort.Equals("namelastname"))
                    {
                        pe = pe.OrderByDescending(c => c.Name).ThenByDescending(c => c.LastName);
                    }
                    if (getparams.sort.Equals("fatherName"))
                    {
                        pe = pe.OrderByDescending(c => c.FatherName);
                    }
                    if (getparams.sort.Equals("idNum"))
                    {
                        pe = pe.OrderByDescending(c => c.IdNum);
                    }
                    if (getparams.sort.Equals("phone"))
                    {
                        pe = pe.OrderByDescending(c => c.Phone);
                    }
                }
                else
                {
                    pe = pe.OrderBy(c => c.Id);
                }

                pe = pe.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                pe = pe.Take(getparams.pageSize);

                var q = await pe
                    .Select(c => new
                    {
                        Id = c.Id,
                        Name = c.Name,
                        LastName = c.LastName,
                        FatherName = c.FatherName,
                        Code = c.Code,
                        IdNum = c.IdNum,
                        haveTeachers = c.Teachers.Any()
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

                var pe = await db.OrgPeople.Select(c => new { id = c.Id, name = c.Name + " " + c.LastName + " - " + c.Code }).ToListAsync();

                return this.DataFunction(true, pe);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getOrgPerson([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var pe = await db.OrgPeople.SingleAsync(c => c.Id == id);

                    return this.DataFunction(true, pe);
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

                        var pe = await db.OrgPeople
                            .SingleAsync(c => c.Id == id);

                        if (pe == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.OrgPeople.Remove(pe);
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
}