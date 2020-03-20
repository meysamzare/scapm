using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMR_Api.Hubs;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class MobileChatController : Controller
    {
        public Data.DbContext db;
        private IHostingEnvironment hostingEnvironment;

        public MobileChatController(Data.DbContext _db, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            hostingEnvironment = _hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> AddChat([FromBody] MobileChat chat)
        {
            try
            {
                chat.SendDate = DateTime.Now;

                var fileData = chat.FileData;
                var fileName = chat.FileName;

                if (!string.IsNullOrEmpty(fileData))
                {
                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, fileName);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(fileData);
                    System.IO.File.WriteAllBytes(path, bytes);

                    chat.FileUrl = Path.Combine("/UploadFiles/" + guid + "/" + fileName);
                }

                await db.MobileChats.AddAsync(chat);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetConversations([FromBody] GetConversationsParam param)
        {
            try
            {
                var chats = db.MobileChats
                    .Where(c => (c.SenderId == param.id && c.SenderType == param.type) || (c.ReciverId == param.id && c.ReciverType == param.type))
                .AsQueryable();

                var pageSize = 10;

                var clientIds = new List<ClientDetile>();

                foreach (var chat in chats)
                {
                    var isSender = chat.detectIsSender(param.type, param.id);

                    if (isSender)
                    {
                        if (!clientIds.Any(c => c.Id == chat.ReciverId && c.Type == chat.ReciverType))
                        {
                            clientIds.Add(new ClientDetile
                            {
                                Id = chat.ReciverId,
                                Type = chat.ReciverType,
                                Name = chat.ReciverName
                            });
                        }
                    }
                    else
                    {
                        if (!clientIds.Any(c => c.Id == chat.SenderId && c.Type == chat.SenderType))
                        {
                            clientIds.Add(new ClientDetile
                            {
                                Id = chat.SenderId,
                                Type = chat.SenderType,
                                Name = chat.SenderName
                            });
                        }
                    }
                }

                var count = clientIds.Count;

                var ClientList = new List<MobileChatConversation>();

                foreach (var item in clientIds)
                {
                    var ClientChats = chats
                        .Where(c =>
                            ((c.SenderId == param.id && c.SenderType == param.type) && (c.ReciverId == item.Id && c.ReciverType == item.Type))
                            ||
                            ((c.ReciverId == param.id && c.ReciverType == param.type) && (c.SenderId == item.Id && c.SenderType == item.Type))
                        );

                    var allUnReads = ClientChats.Where(c => c.IsRead == false && c.detectIsSender(param.type, param.id) == false);

                    var lastChat = await ClientChats.OrderByDescending(c => c.SendDate).FirstAsync();

                    var imgUrl = "";

                    if (item.Type == MobileChatType.StudentParent)
                    {
                        var student = await db.Students.FirstOrDefaultAsync(c => c.Id == item.Id);

                        imgUrl = student.PicUrl;
                    }

                    ClientList.Add(new MobileChatConversation
                    {
                        clientId = item.Id,
                        clientType = (int)item.Type,
                        clientImg = imgUrl,
                        clientName = item.Name,
                        LastChatDateTime = lastChat.SendDate,
                        lastChatText = lastChat.Text,
                        unreadCount = await allUnReads.CountAsync()
                    });
                }

                var query = param.q;

                if (!string.IsNullOrEmpty(query))
                {
                    ClientList = ClientList
                        .Where(c => c.clientName.Contains(query) || c.lastChatText.Contains(query))
                    .ToList();
                }

                var lst = ClientList
                    .OrderByDescending(c => c.LastChatDateTime)
                    .Skip((param.page - 1) * pageSize)
                    .Take(pageSize).ToArray();

                return this.DataFunction(true, new
                {
                    conversations = lst,
                    count = count,
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getUnreadChatCount([FromBody] getUnreadChatCountParam param)
        {
            try
            {
                var chats = db.MobileChats
                    .Where(c => (c.SenderId == param.id && c.SenderType == param.type) || (c.ReciverId == param.id && c.ReciverType == param.type))
                .AsQueryable();

                var clientIds = new List<ClientDetile>();

                foreach (var chat in chats)
                {
                    var isSender = chat.detectIsSender(param.type, param.id);

                    if (isSender)
                    {
                        if (!clientIds.Any(c => c.Id == chat.ReciverId && c.Type == chat.ReciverType))
                        {
                            clientIds.Add(new ClientDetile
                            {
                                Id = chat.ReciverId,
                                Type = chat.ReciverType,
                                Name = chat.ReciverName
                            });
                        }
                    }
                    else
                    {
                        if (!clientIds.Any(c => c.Id == chat.SenderId && c.Type == chat.SenderType))
                        {
                            clientIds.Add(new ClientDetile
                            {
                                Id = chat.SenderId,
                                Type = chat.SenderType,
                                Name = chat.SenderName
                            });
                        }
                    }
                }

                var count = clientIds.Count;

                var unreadCount = 0;

                foreach (var item in clientIds)
                {
                    var ClientChats = chats
                        .Where(c =>
                            ((c.SenderId == param.id && c.SenderType == param.type) && (c.ReciverId == item.Id && c.ReciverType == item.Type))
                            ||
                            ((c.ReciverId == param.id && c.ReciverType == param.type) && (c.SenderId == item.Id && c.SenderType == item.Type))
                        );

                    var allUnReads = await ClientChats.Where(c => c.IsRead == false && c.detectIsSender(param.type, param.id) == false).CountAsync();
                    
                    unreadCount += allUnReads;
                }

                return this.DataFunction(true, unreadCount);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetChats([FromBody] GetMobileChatsParam param)
        {
            try
            {
                var pageSize = 10;

                var chats = db.MobileChats
                        .Where(c =>
                            ((c.SenderId == param.id && c.SenderType == param.type) && (c.ReciverId == param.clientId && c.ReciverType == param.clientType))
                            ||
                            ((c.ReciverId == param.id && c.ReciverType == param.type) && (c.SenderId == param.clientId && c.SenderType == param.clientType))
                        );

                var count = await chats.CountAsync();

                var lst = await chats
                    .OrderByDescending(c => c.SendDate)
                    .Skip((param.page - 1) * pageSize)
                    .Take(pageSize)
                .ToListAsync();

                return this.DataFunction(true, new
                {
                    chats = lst,
                    totalCount = count
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> setAllChatSeen([FromBody] setAllMobileChatSeenParam param)
        {
            try
            {
                var chats = db.MobileChats.Where(c =>
                    (c.ReciverId == param.id && c.ReciverType == param.type) && (c.SenderId == param.senderId && c.SenderType == param.senderType)
                );

                await chats.ForEachAsync(chat =>
                {
                    chat.IsRead = true;
                });

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }
    }

    public class getUnreadChatCountParam
    {
        public MobileChatType type { get; set; }

        public int id { get; set; }
    }

    public class setAllMobileChatSeenParam
    {
        public int id { get; set; }

        public MobileChatType type { get; set; }

        public int senderId { get; set; }

        public MobileChatType senderType { get; set; }
    }

    public class GetMobileChatsParam
    {
        public int id { get; set; }

        public MobileChatType type { get; set; }

        public int clientId { get; set; }

        public MobileChatType clientType { get; set; }

        public int page { get; set; }
    }

    public class GetConversationsParam
    {
        public int page { get; set; }

        public MobileChatType type { get; set; }

        public int id { get; set; }

        public string q { get; set; }
    }

    public class ClientDetile
    {
        public int Id { get; set; }

        public MobileChatType Type { get; set; }

        public string Name { get; set; }
    }
}