using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Hubs;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class ChatController : Controller
    {
        public Data.DbContext db;
        private readonly IHubContext<ChatHub> chatHub;
        private IHostingEnvironment hostingEnvironment;

        public ChatController(Data.DbContext _db, IHubContext<ChatHub> _chatHub, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            chatHub = _chatHub;
            hostingEnvironment = _hostingEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] getchatParam getchat)
        {
            try
            {
                int size = 8;
                var user = await db.Users.FirstOrDefaultAsync(c => c.Username == User.Identity.Name);
                var userId = user.Id;

                var chats = await db.Chats.Where(c => (c.SenderId == userId && c.ReciverId == getchat.clientId) || (c.SenderId == getchat.clientId && c.ReciverId == userId))
                            .OrderByDescending(c => c.SendDate).Skip((getchat.page - 1) * size).Take(size)
                    .Include(c => c.ReciverUser)
                    .Include(c => c.SenderUser)
                .ToListAsync();

                return this.DataFunction(true, chats);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> addChat([FromBody] Chat chat)
        {
            try
            {
                chat.SendDate = DateTime.Now;
                chat.ReciveStatus = false;

                if (!string.IsNullOrEmpty(chat.FileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, chat.FileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(chat.FileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    chat.FileUrl = Path.Combine("/UploadFiles/" + guid + "/" + chat.FileName);

                    chat.FileStatus = true;
                }

                await db.Chats.AddAsync(chat);

                await db.SaveChangesAsync();

                return this.SuccessFunction(data: DateTime.Now.ToPersianDateWithTime());
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        [HttpPost]
        public async Task<IActionResult> readAllMessageConversation([FromBody] int senderId)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(c => c.Username == User.Identity.Name);
                var userId = user.Id;

                var unReadChats = db.Chats
                    .Where(c => (c.SenderId == senderId && c.ReciverId == userId) && c.ReciveStatus == false);

                await unReadChats.ForEachAsync(c => c.ReciveStatus = true);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getUnReadMessages()
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(c => c.Username == User.Identity.Name);
                var userId = user.Id;

                var unReadChats = await db.Chats
                        .Where(c => c.ReciverId == userId && c.ReciveStatus == false)
                    .Include(c => c.SenderUser)
                .Select(c => new INewMessage
                {
                    sendDate = c.SendDate.ToPersianDateWithTime(),
                    senderName = c.SenderUser.fullName,
                    messageText = c.Text,
                    senderId = c.SenderUser.Id
                }).ToListAsync();

                return this.SuccessFunction(data: unReadChats);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getLastMessageDate()
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(c => c.Username == User.Identity.Name);
                var userId = user.Id;

                var chat = await db.Chats.Where(c => (c.SenderId == userId || c.ReciverId == userId))
                            .OrderByDescending(c => c.SendDate).FirstOrDefaultAsync();

                if (chat == null)
                {
                    return this.SuccessFunction();
                }
                else
                {
                    return this.DataFunction(true, chat.SendDate.ToPersianDateWithTime());
                }
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

    }

    public class getchatParam
    {
        public int page { get; set; }

        public int clientId { get; set; }
    }
}