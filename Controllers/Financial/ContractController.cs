using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model.Financial;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ContractController : Controller
    {
        public Data.DbContext db;

        private IHostingEnvironment hostingEnvironment;

        public ContractController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Contract contract)
        {
            try
            {
                if (!string.IsNullOrEmpty(contract.FileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, contract.FileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(contract.FileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    contract.FileUrl = Path.Combine("/UploadFiles/" + guid + "/" + contract.FileName);
                }

                contract.Date = contract.Date.AddDays(1);

                await db.Contracts.AddAsync(contract);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] Contract contract)
        {
            try
            {

                var cont = await db.Contracts.FirstOrDefaultAsync(c => c.Id == contract.Id);

                if (!string.IsNullOrEmpty(contract.FileData))
                {
                    if (!string.IsNullOrEmpty(cont.FileUrl))
                    {
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + cont.FileUrl);
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, contract.FileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(contract.FileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    cont.FileUrl = Path.Combine("/UploadFiles/" + guid + "/" + contract.FileName);
                }

                var dateB = cont.Date;

                cont.Date = contract.Date;

                if (dateB != contract.Date)
                {
                    cont.Date = cont.Date.AddDays(1);
                }

                cont.Title = contract.Title;
                cont.Code = contract.Code;
                cont.PartyContractId = contract.PartyContractId;
                cont.PartyContractName = contract.PartyContractName;
                cont.Price = contract.Price;
                cont.ContractTypeId = contract.ContractTypeId;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFile([FromBody] int id)
        {
            try
            {
                var cont = await db.Contracts.FirstOrDefaultAsync(c => c.Id == id);

                if (!string.IsNullOrEmpty(cont.FileUrl))
                {
                    System.IO.File.Delete(hostingEnvironment.ContentRootPath + cont.FileUrl);

                    cont.FileUrl = "";

                    await db.SaveChangesAsync();
                }

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

                var sl = db.Contracts.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    sl = sl.Where(c => c.Title.Contains(query) || c.Price.ToString().Contains(query) || c.Code.ToString().Contains(query));

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
                    if (getparams.sort.Equals("code"))
                    {
                        sl = sl.OrderBy(c => c.Code);
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
                    if (getparams.sort.Equals("title"))
                    {
                        sl = sl.OrderByDescending(c => c.Title);
                    }
                    if (getparams.sort.Equals("code"))
                    {
                        sl = sl.OrderByDescending(c => c.Code);
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
                        Title = c.Title,
                        Code = c.Code,
                        datePersian = c.Date.ToPersianDate(),
                        haveStdPayments = c.StdPayments.Any()
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

                var sl = await db.Contracts.Select(c => new { id = c.Id, title = c.Title }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getContract([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.Contracts.SingleAsync(c => c.Id == id);

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

                        var sl = await db.Contracts.Include(c => c.StdPayments)
                            .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (sl.haveStdPayments)
                        {
                            return this.UnSuccessFunction(" قرارداد " + sl.Title + " دارای پرداخت هایی است", "error");
                        }

                        db.Contracts.Remove(sl);
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
        public async Task<IActionResult> getPrintContent([FromBody] int Id)
        {
            try
            {
                var cont = await db.Contracts
                .Where(c => c.Id == Id)
                    .Include(c => c.ContractType)
                .FirstOrDefaultAsync();

                var conType = cont.ContractType;

                string HtmlContent = "";

                if (conType.Table == ContractTypeTable.Student)
                {
                    var std = await db.Students.FirstOrDefaultAsync(c => c.Id == cont.PartyContractId);

                    if (std != null)
                    {
                        HtmlContent = conType.Content;

                        HtmlContent = HtmlContent.Replace("{name}", std.Name);
                        HtmlContent = HtmlContent.Replace("{lastName}", std.LastName);
                        HtmlContent = HtmlContent.Replace("{fatherName}", std.FatherName);
                        HtmlContent = HtmlContent.Replace("{idNum}", std.IdNumber2);
                        HtmlContent = HtmlContent.Replace("{bdate}", std.birthDateString);
                        HtmlContent = HtmlContent.Replace("{date}", DateTime.Now.ToPersianDate());

                        return this.DataFunction(true, HtmlContent);
                    }

                    return this.UnSuccessFunction("دانش آموز مورد نظر یافت نشد");
                }
                else if (conType.Table == ContractTypeTable.Person)
                {
                    var prs = await db.OrgPeople.FirstOrDefaultAsync(c => c.Id == cont.PartyContractId);

                    if (prs != null)
                    {
                        HtmlContent = conType.Content;

                        HtmlContent = HtmlContent.Replace("{name}", prs.Name);
                        HtmlContent = HtmlContent.Replace("{lastName}", prs.LastName);
                        HtmlContent = HtmlContent.Replace("{fatherName}", prs.FatherName);
                        HtmlContent = HtmlContent.Replace("{idNum}", prs.IdNum);
                        HtmlContent = HtmlContent.Replace("{bdate}", prs.BirthDate.ToPersianDate());
                        HtmlContent = HtmlContent.Replace("{date}", DateTime.Now.ToPersianDate());

                        return this.DataFunction(true, HtmlContent);
                    }

                    return this.UnSuccessFunction("پرسنل مورد نظر یافت نشد");
                }
                else
                {
                    HtmlContent = conType.Content;

                    HtmlContent = HtmlContent.Replace("{name}", cont.PartyContractName);
                    HtmlContent = HtmlContent.Replace("{date}", DateTime.Now.ToPersianDate());

                    return this.DataFunction(true, HtmlContent);
                }
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

    }
}