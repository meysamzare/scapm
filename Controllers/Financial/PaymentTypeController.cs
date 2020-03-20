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
    public class PaymentTypeController : Controller
    {
        public Data.DbContext db;

        public PaymentTypeController(Data.DbContext _db)
        {
            db = _db;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PaymentType paymenttype)
        {
            try
            {
                if (await db.PaymentTypes.AnyAsync(c => c.Code == paymenttype.Code))
                {
                    return this.UnSuccessFunction("این کد پرداخت قبلا ثبت شده است");
                }
                await db.PaymentTypes.AddAsync(paymenttype);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] PaymentType paymenttype)
        {
            try
            {
                var paytyp = await db.PaymentTypes.FirstOrDefaultAsync(c => c.Id == paymenttype.Id);

                if (await db.PaymentTypes.Except(db.PaymentTypes.Where(c => c.Id == paymenttype.Id)).AnyAsync(c => c.Code == paymenttype.Code))
                {
                    return this.UnSuccessFunction("این کد پرداخت قبلا ثبت شده است");
                }

                paytyp.Code = paymenttype.Code;
                paytyp.Title = paymenttype.Title;
                paytyp.Desc = paymenttype.Desc;

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

                var sl = db.PaymentTypes.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Title.Contains(query) || c.Code.ToString().Contains(query) || c.Desc.Contains(query));
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
                        Code = c.Code,
                        Title = c.Title,
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

                var sl = await db.PaymentTypes.Select(c => new { id = c.Id, title = c.Title }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> getPaymentType([FromBody] int id)
        {
            try
            {

                if (id != 0)
                {
                    var sl = await db.PaymentTypes.FirstOrDefaultAsync(c => c.Id == id);

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

                        var sl = await db.PaymentTypes.Include(c => c.StdPayments)
                            .FirstOrDefaultAsync(c => c.Id == id);

                        if (sl == null)
                        {
                            return this.UnSuccessFunction("Data Not Found", "error");
                        }
                        if (sl.haveStdPayments)
                        {
                            return this.UnSuccessFunction(" نوع پرداخت " + sl.Title + " دارای پرداخت هایی است", "error");
                        }

                        db.PaymentTypes.Remove(sl);
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