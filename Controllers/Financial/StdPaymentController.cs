using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model.Financial;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class StdPaymentController : Controller
    {
        public Data.DbContext db;

        public StdPaymentController(Data.DbContext _db)
        {
            db = _db;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] StdPayment stdpayment)
        {
            try
            {
                await db.StdPayments.AddAsync(stdpayment);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] StdPayment stdpayment)
        {
            try
            {
                var stdpay = await db.StdPayments.FirstOrDefaultAsync(c => c.Id == stdpayment.Id);

                stdpay.RefNum = stdpayment.RefNum;
                stdpay.Bank = stdpayment.Bank;
                stdpay.Hesab = stdpayment.Hesab;
                stdpay.Shobe = stdpayment.Shobe;
                stdpay.Price = stdpayment.Price;

                stdpay.PaymentTypeId = stdpayment.PaymentTypeId;
                stdpay.StudentId = stdpayment.StudentId;
                stdpay.StdClassMngId = stdpayment.StdClassMngId;
                stdpay.ContractId = stdpayment.ContractId;

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

                var sl = db.StdPayments.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    sl = sl.Where(c => c.PaymentType.Title.Contains(query) || c.Price.ToString().Contains(query) || c.Bank.Contains(query) || c.Contract.Title.Contains(query) ||
                                      c.Student.Name.Contains(query) || c.Student.LastName.Contains(query) || c.Hesab.Contains(query) || c.Shobe.Contains(query) || c.RefNum.Contains(query));

                }

                count = sl.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("contract"))
                    {
                        sl = sl.OrderBy(c => c.Contract.Title);
                    }
                    if (getparams.sort.Equals("student"))
                    {
                        sl = sl.OrderBy(c => c.Student.LastName).ThenBy(c => c.Student.Name);
                    }
                    if (getparams.sort.Equals("paymenttype"))
                    {
                        sl = sl.OrderBy(c => c.PaymentType.Title);
                    }
                    if (getparams.sort.Equals("price"))
                    {
                        sl = sl.OrderBy(c => c.Price);
                    }
                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("contract"))
                    {
                        sl = sl.OrderByDescending(c => c.Contract.Title);
                    }
                    if (getparams.sort.Equals("student"))
                    {
                        sl = sl.OrderByDescending(c => c.Student.LastName).ThenByDescending(c => c.Student.Name);
                    }
                    if (getparams.sort.Equals("paymenttype"))
                    {
                        sl = sl.OrderByDescending(c => c.PaymentType.Title);
                    }
                    if (getparams.sort.Equals("price"))
                    {
                        sl = sl.OrderByDescending(c => c.Price);
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
                        contractTitle = c.Contract.Title,
                        studentFullName = c.Student.Name + " " + c.Student.LastName,
                        paymentTypeTitle = c.PaymentType.Title,
                        priceString = c.priceString
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

                var sl = await db.StdPayments.Select(c => new
                {
                    id = c.Id,
                    name = "قرارداد " +
                        c.Contract + " و دانش آموز " + c.Student.Name + " " + c.Student.LastName
                }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getStdPayment([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.StdPayments.FirstOrDefaultAsync(c => c.Id == id);

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

                        var sl = await db.StdPayments.FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }

                        db.StdPayments.Remove(sl);
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