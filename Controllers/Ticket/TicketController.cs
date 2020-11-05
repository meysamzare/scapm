using System;
using System.Collections.Generic;
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
    public class TicketController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public TicketController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> getTickets([FromBody] getTickets getTickets)
        {
            try
            {
                var tickets = await db.Tickets
                .Where(c => (c.SenderId == getTickets.id && c.SenderType == getTickets.type)
                        || (c.ReciverId == getTickets.id && c.ReciverType == getTickets.type)
                    && c.IsRemoved == false)
                .ToListAsync();

                return this.DataFunction(true, tickets);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public IActionResult getUnreadTickets([FromBody] getTickets getTickets)
        {
            try
            {

                var userId = getTickets.id;
                var userType = getTickets.type;


                var tickets = db.Tickets
                .Where(c => (c.SenderId == getTickets.id && c.SenderType == getTickets.type)
                        || (c.ReciverId == getTickets.id && c.ReciverType == getTickets.type)
                    && c.IsRemoved == false)
                .Select(c => new
                {
                    Id = c.Id,
                    Subject = c.Subject,
                    Order = c.Order,
                    State = c.State,
                    SenderId = c.SenderId,
                    SenderName = c.SenderName,
                    SenderType = c.SenderType,
                    ReciverName = c.ReciverName,
                    ReciverType = c.ReciverType,
                    ReciverId = c.ReciverId,
                    newConversations = c.haveNewConversation(c, userId, userType, c.TicketConversations)
                })
                .Where(c => c.newConversations != 0)
                .OrderBy(c => c.Id).Take(10)
                .AsEnumerable();

                return this.DataFunction(true, tickets);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getOpenTickets([FromBody] getTickets getTickets)
        {
            try
            {
                var tickets = await db.Tickets
                .Where(c => (c.SenderId == getTickets.id && c.SenderType == getTickets.type)
                        || (c.ReciverId == getTickets.id && c.ReciverType == getTickets.type)
                    && c.IsRemoved == false)
                .Where(c => c.State == TicketState.Open || c.State == TicketState.WaitingForAnswer)
                .OrderBy(c => c.Id).Take(10)
                .ToListAsync();

                return this.DataFunction(true, tickets);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public IActionResult getUnreadTicketsCount([FromBody] getTickets getTickets)
        {
            try
            {

                var userId = getTickets.id;
                var userType = getTickets.type;


                var tickets = db.Tickets
                .Where(c => (c.SenderId == getTickets.id && c.SenderType == getTickets.type)
                        || (c.ReciverId == getTickets.id && c.ReciverType == getTickets.type)
                    && c.IsRemoved == false)
                .Select(c => new
                {
                    newConversations = c.haveNewConversation(c, userId, userType, c.TicketConversations)
                })
                .Where(c => c.newConversations != 0)
                .AsEnumerable();

                return this.DataFunction(true, tickets.Count());
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public IActionResult Get([FromBody] getTicketsParam getTicket)
        {
            try
            {
                getTicket.getParam.pageIndex += 1;

                int count;

                var query = getTicket.getParam.q;

                var userId = getTicket.id;
                var userType = getTicket.type;

                var sl = db.Tickets
                .Where(c => (c.SenderId == userId && c.SenderType == userType)
                        || (c.ReciverId == userId && c.ReciverType == userType))
                .Select(c => new
                {
                    Id = c.Id,
                    Subject = c.Subject,
                    Order = c.Order,
                    State = c.State,
                    SenderId = c.SenderId,
                    SenderName = c.SenderName,
                    SenderType = c.SenderType,
                    ReciverName = c.ReciverName,
                    ReciverType = c.ReciverType,
                    ReciverId = c.ReciverId,
                    newConversations = c.haveNewConversation(c, userId, userType, c.TicketConversations),
                    isSender = c.isSender(c, userId, userType),
                    isImport = c.isImport(c),
                    IsRemoved = c.IsRemoved
                }).AsEnumerable();


                var inboxCount = 0;
                var sendboxCount = 0;
                var draftCount = 0;
                var importCount = 0;

                inboxCount = sl.Where(c => c.isSender == false && c.newConversations != 0).Count();
                sendboxCount = sl.Where(c => c.isSender == true && c.newConversations != 0).Count();
                draftCount = sl.Where(c => c.State == TicketState.Close && c.newConversations != 0).Count();
                importCount = sl.Where(c => c.isImport == true && c.newConversations != 0).Count();


                if (!string.IsNullOrWhiteSpace(query))
                {
                    sl = sl.Where(c => c.Subject.Contains(query) || c.SenderName.Contains(query)
                        || c.ReciverName.Contains(query));
                }


                if (getTicket.onlyUnreads)
                {
                    sl = sl.Where(c => c.newConversations != 0);
                }

                if (getTicket.showType == "inbox")
                {
                    sl = sl.Where(c => c.isSender == false && c.IsRemoved == false);
                }

                if (getTicket.showType == "sendbox")
                {
                    sl = sl.Where(c => c.isSender == true && c.IsRemoved == false);
                }

                if (getTicket.showType == "import")
                {
                    sl = sl.Where(c => c.isImport == true && c.IsRemoved == false);
                }

                if (getTicket.showType == "draft")
                {
                    sl = sl.Where(c => c.IsRemoved == true);
                }


                count = sl.Count();


                if (getTicket.getParam.direction.Equals("asc"))
                {
                    if (getTicket.getParam.sort.Equals("id"))
                    {
                        sl = sl.OrderBy(c => c.Id);
                    }
                    if (getTicket.getParam.sort.Equals("subject"))
                    {
                        sl = sl.OrderBy(c => c.Subject);
                    }
                    if (getTicket.getParam.sort.Equals("order"))
                    {
                        sl = sl.OrderBy(c => c.Order);
                    }
                    if (getTicket.getParam.sort.Equals("state"))
                    {
                        sl = sl.OrderBy(c => c.State);
                    }
                    if (getTicket.getParam.sort.Equals("sender"))
                    {
                        sl = sl.OrderBy(c => c.SenderName);
                    }
                    if (getTicket.getParam.sort.Equals("reciver"))
                    {
                        sl = sl.OrderBy(c => c.ReciverName);
                    }
                }
                else if (getTicket.getParam.direction.Equals("desc"))
                {
                    if (getTicket.getParam.sort.Equals("id"))
                    {
                        sl = sl.OrderByDescending(c => c.Id);
                    }
                    if (getTicket.getParam.sort.Equals("subject"))
                    {
                        sl = sl.OrderByDescending(c => c.Subject);
                    }
                    if (getTicket.getParam.sort.Equals("order"))
                    {
                        sl = sl.OrderByDescending(c => c.Order);
                    }
                    if (getTicket.getParam.sort.Equals("state"))
                    {
                        sl = sl.OrderByDescending(c => c.State);
                    }
                    if (getTicket.getParam.sort.Equals("sender"))
                    {
                        sl = sl.OrderByDescending(c => c.SenderName);
                    }
                    if (getTicket.getParam.sort.Equals("reciver"))
                    {
                        sl = sl.OrderByDescending(c => c.ReciverName);
                    }
                }
                else
                {
                    sl = sl.OrderByDescending(c => c.Id);
                }

                sl = sl.Skip((getTicket.getParam.pageIndex - 1) * getTicket.getParam.pageSize);
                sl = sl.Take(getTicket.getParam.pageSize);

                var q = sl.ToList();


                return Json(new jsondata
                {
                    success = true,
                    data = new
                    {
                        data = q,
                        inboxCount = inboxCount,
                        sendboxCount = sendboxCount,
                        draftCount = draftCount,
                        importCount = importCount,
                    },
                    type = "" + count
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddConversation([FromBody] TicketConversation conversation)
        {
            try
            {
                conversation.Date = DateTime.Now;

                var FileData = conversation.FileData;
                var FileName = conversation.FileName;

                if (!string.IsNullOrEmpty(FileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, FileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(FileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    conversation.FileUrl = Path.Combine("/UploadFiles/" + guid + "/" + FileName);
                }

                await db.TicketConversations.AddAsync(conversation);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTicketWithConversation([FromBody] AddTicketWithConversationParam param)
        {
            try
            {
                var ticket = param.ticket;

                ticket.State = TicketState.WaitingForAnswer;

                await db.Tickets.AddAsync(ticket);

                await db.SaveChangesAsync();

                param.conversation.TicketId = ticket.Id;
                param.conversation.Date = DateTime.Now;


                var FileData = param.conversation.FileData;
                var FileName = param.conversation.FileName;

                if (!string.IsNullOrEmpty(FileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, FileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(FileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    param.conversation.FileUrl = Path.Combine("/UploadFiles/" + guid + "/" + FileName);
                }

                await db.TicketConversations.AddAsync(param.conversation);

                await db.SaveChangesAsync();


                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getTicket([FromBody] int id)
        {
            try
            {
                var ticket = await db.Tickets.FirstOrDefaultAsync(c => c.Id == id);

                return this.DataFunction(true, ticket);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getConversations([FromBody] int ticketId)
        {
            try
            {
                var conversations = await db.TicketConversations
                .Where(c => c.TicketId == ticketId)
                    .OrderByDescending(c => c.Date)
                .ToListAsync();


                return this.DataFunction(true, conversations);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> setAllConversationSeen([FromBody] setAllConversationSeenParam param)
        {
            try
            {
                var conversations = db.TicketConversations
                .Where(c => c.TicketId == param.ticketId && c.IsSender == param.isSender);

                await conversations.ForEachAsync(c => c.IsSeen = true);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> changeState([FromBody] changeStateParam param)
        {
            try
            {
                var ticket = await db.Tickets.FirstOrDefaultAsync(c => c.Id == param.ticketId);

                ticket.State = param.state;

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> closeTickets([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var ticket = await db.Tickets.FirstOrDefaultAsync(c => c.Id == id);

                    ticket.State = TicketState.Close;
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
        public async Task<IActionResult> removeTickets([FromBody] int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var ticket = await db.Tickets.Include(c => c.TicketConversations).FirstOrDefaultAsync(c => c.Id == id);

                    if (ticket.IsRemoved == true)
                    {
                        db.RemoveRange(ticket.TicketConversations);
                        db.Remove(ticket);
                    }
                    else
                    {
                        ticket.IsRemoved = true;
                        ticket.State = TicketState.Close;
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

    public class getTickets
    {
        public int id { get; set; }
        public TicketType type { get; set; }
    }

    public class changeStateParam
    {
        public int ticketId { get; set; }

        public TicketState state { get; set; }
    }

    public class setAllConversationSeenParam
    {
        public int ticketId { get; set; }

        public bool isSender { get; set; }
    }

    public class AddTicketWithConversationParam
    {
        public Ticket ticket { get; set; }

        public TicketConversation conversation { get; set; }
    }

    public class getTicketsParam
    {
        public getparams getParam { get; set; }

        public int id { get; set; }

        public TicketType type { get; set; }

        public bool onlyUnreads { get; set; }

        public string showType { get; set; }
    }
}