using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SCMR_Api.Model;

namespace SCMR_Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        Data.DbContext db;
        private IConfiguration _config;

        private readonly static UserMapping<string> connections =
            new UserMapping<string>();

        private IHostingEnvironment hostingEnvironment;

        public UserController(Data.DbContext _db, IConfiguration config, IHostingEnvironment _hostingEnvironment)
        {
            db = _db;
            _config = config;
            hostingEnvironment = _hostingEnvironment;
        }



        [HttpPost]
        public IActionResult getLoginState()
        {
            return this.DataFunction(true, HttpContext.User.Identity.IsAuthenticated);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginParams loginPrams)
        {
            try
            {
                User user = null;

                if (await db.Users.AnyAsync(c => c.Username.Equals(loginPrams.username)))
                {
                    user = await db.Users.Include(c => c.Role).SingleAsync(c => c.Username.Equals(loginPrams.username));
                }

                if (user == null)
                {
                    return this.UnSuccessFunction("نام کاربری اشتباه است");
                }

                if (user.UserState != UserState.Active)
                {
                    return this.UnSuccessFunction("این نام کاربری توسط مدیر سیستم مسدود شده است");
                }

                var isPassOk = user.Password.Equals(loginPrams.password);

                if (!isPassOk)
                {
                    return this.UnSuccessFunction("کلمه عبور اشتباه است");
                }




                // if (connections.haveKey(loginPrams.username))
                // {
                //     return this.UnSuccessFunction("شخص دیگری با این نام کاربری وارد سیستم شده است");
                // }
                // else
                // {
                //     connections.Add(loginPrams.username, "1");
                // }

                var token = BuildToken(loginPrams.username);

                return Json(new jsondata
                {
                    success = true,
                    redirect = token,
                    data = user
                });
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }


        // Login Again with every page change
        [HttpPost]
        public IActionResult LoginA()
        {
            var username = User.Identity.Name;
            return this.SuccessFunction(BuildToken(username));
        }


        private string BuildToken(string username)
        {

            var claims = new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(40),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Logout([FromBody] string username)
        {
            try
            {
                // var user = db.Users.Single(c => c.Username.Equals(username));

                // user.isLogedIn = false;

                // db.SaveChanges();

                connections.removeFull(username);

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }




        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserAddParam user)
        {
            try
            {
                user.username = user.username.ToLower().Trim();

                if (db.Users.Any(c => c.MeliCode.Equals(user.meliCode)))
                {
                    return this.UnSuccessFunction("این کد ملی قبلا ثبت شده است");
                }
                if (db.Users.Any(c => c.Username.ToLower().Trim().Equals(user.username)))
                {
                    return this.UnSuccessFunction("این نام کاربری قبلا ثبت شده است");
                }

                var picdata = user.picData;
                var picname = user.picName;

                var guid = System.Guid.NewGuid().ToString();

                var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                byte[] bytes = Convert.FromBase64String(picdata);
                System.IO.File.WriteAllBytes(path, bytes);

                User u = new User
                {
                    DateAdd = DateTime.Now,
                    UserState = UserState.Unknown,
                    Firstname = user.firstname,
                    Lastname = user.lastname,
                    MeliCode = user.meliCode,
                    Username = user.username,
                    Password = user.password,
                    RoleId = user.roleId
                };

                u.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);

                db.Users.Add(u);

                await db.SaveChangesAsync();

                return this.SuccessFunction();
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] User user)
        {
            try
            {
                user.Username = user.Username.ToLower().Trim();

                var thisUserList = db.Users.Where(c => c.Id == user.Id);
                if (db.Users.Except(thisUserList).Any(c => c.Username.ToLower().Trim().Equals(user.Username)))
                {
                    return this.UnSuccessFunction("این نام کاربری قبلا ثبت شده است");
                }
                if (db.Users.Except(thisUserList).Any(c => c.MeliCode.Equals(user.MeliCode)))
                {
                    return this.UnSuccessFunction("این کد ملی قبلا ثبت شده است");
                }

                var userS = await db.Users.SingleAsync(c => c.Id == user.Id);

                userS.Username = user.Username;
                userS.Password = user.Password;
                userS.Firstname = user.Firstname;
                userS.Lastname = user.Lastname;
                userS.MeliCode = user.MeliCode;
                userS.UserState = user.UserState;
                userS.UserStateDesc = user.UserStateDesc;
                userS.RoleId = user.RoleId;
                userS.DateEdit = DateTime.Now;


                var picdata = user.PicData;
                var picname = user.PicName;
                var picurl = userS.PicUrl;

                if (!string.IsNullOrEmpty(picdata))
                {
                    if (!string.IsNullOrEmpty(picurl) && picurl.StartsWith("/UploadFiles/"))
                    {
                        System.IO.File.Delete(hostingEnvironment.ContentRootPath + picurl);
                    }

                    var guid = System.Guid.NewGuid().ToString();

                    var path = Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid, picname);
                    Directory.CreateDirectory(Path.Combine(hostingEnvironment.ContentRootPath, "UploadFiles", guid));

                    byte[] bytes = Convert.FromBase64String(picdata);
                    System.IO.File.WriteAllBytes(path, bytes);

                    userS.PicUrl = Path.Combine("/UploadFiles/" + guid + "/" + picname);
                }


                var agentId = user.Id;
                var agentType = TicketType.User;
                var agentName = user.Firstname + " " + user.Lastname;

                if (await db.Tickets.AnyAsync(c =>
                    (c.ReciverType == agentType && c.ReciverId == agentId) ||
                    (c.SenderType == agentType && c.SenderId == agentId)
                ))
                {
                    var tickets = db.Tickets.Where(c =>
                    (c.ReciverType == agentType && c.ReciverId == agentId) ||
                    (c.SenderType == agentType && c.SenderId == agentId));

                    await tickets.ForEachAsync(c =>
                    {
                        if (c.ReciverType == agentType && c.ReciverId == agentId)
                        {
                            c.ReciverName = agentName;
                        }
                        else if (c.SenderType == agentType && c.SenderId == agentId)
                        {
                            c.SenderName = agentName;
                        }
                    });
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
        public async Task<IActionResult> getAll()
        {
            try
            {
                var sl = await db.Users.Select(c => new { id = c.Id, name = c.fullName, fullName = c.fullName }).ToListAsync();

                return this.DataFunction(true, sl);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getAllByRole([FromBody] int roleId)
        {
            try
            {
                var users = await db.Users.Where(c => c.RoleId == roleId && c.UserState == UserState.Active)
                    .Select(c => new { id = c.Id, name = c.fullName, fullName = c.fullName })
                .ToListAsync();

                return this.DataFunction(true, users);
            }
            catch (System.Exception e)
            {
                return this.CatchFunction(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> getUsersChat()
        {
            try
            {
                var users = await db.Users
                .Except(db.Users.Where(c => c.Username == User.Identity.Name))
                    .Where(c => c.UserState == UserState.Active)
                .Select(c => new
                {
                    id = c.Id,
                    username = c.Username,
                    firstname = c.Firstname,
                    lastname = c.Lastname,
                    PicUrl = c.PicUrl
                })
                .ToListAsync();

                return this.DataFunction(true, users);
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

                var cls = db.Users.AsQueryable();


                if (!string.IsNullOrWhiteSpace(query))
                {

                    cls = cls.Where(c => c.Firstname.Contains(query) || c.Lastname.Contains(query) || c.fullName.Contains(query) ||
                                        c.Role.Name.Contains(query) || c.MeliCode.Contains(query) || c.Username.Contains(query));
                }

                count = cls.Count();


                if (getparams.direction.Equals("asc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderBy(c => c.Id);
                    }
                    if (getparams.sort.Equals("fullName"))
                    {
                        cls = cls.OrderBy(c => c.Lastname).ThenBy(c => c.Firstname);
                    }
                    if (getparams.sort.Equals("meliCode"))
                    {
                        cls = cls.OrderBy(c => c.MeliCode);
                    }
                    if (getparams.sort.Equals("username"))
                    {
                        cls = cls.OrderBy(c => c.Username);
                    }
                    if (getparams.sort.Equals("roleName"))
                    {
                        cls = cls.OrderBy(c => c.Role.Name);
                    }
                    if (getparams.sort.Equals("state"))
                    {
                        cls = cls.OrderBy(c => c.UserState);
                    }

                }
                else if (getparams.direction.Equals("desc"))
                {
                    if (getparams.sort.Equals("id"))
                    {
                        cls = cls.OrderByDescending(c => c.Id);
                    }
                    if (getparams.sort.Equals("fullName"))
                    {
                        cls = cls.OrderByDescending(c => c.Lastname).ThenByDescending(c => c.Firstname);
                    }
                    if (getparams.sort.Equals("meliCode"))
                    {
                        cls = cls.OrderByDescending(c => c.MeliCode);
                    }
                    if (getparams.sort.Equals("username"))
                    {
                        cls = cls.OrderByDescending(c => c.Username);
                    }
                    if (getparams.sort.Equals("roleName"))
                    {
                        cls = cls.OrderByDescending(c => c.Role.Name);
                    }
                    if (getparams.sort.Equals("state"))
                    {
                        cls = cls.OrderByDescending(c => c.UserState);
                    }
                }
                else
                {
                    cls = cls.OrderBy(c => c.Id);
                }

                cls = cls.Skip((getparams.pageIndex - 1) * getparams.pageSize);
                cls = cls.Take(getparams.pageSize);

                var q = await cls
                    .Select(c => new
                    {
                        Id = c.Id,
                        fullName = c.Firstname + " " + c.Lastname,
                        MeliCode = c.MeliCode,
                        Username = c.Username,
                        roleName = c.Role.Name,
                        UserState = c.UserState,
                        userStateString = c.userStateString,
                        userStateColor = c.userStateColor
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
        public async Task<IActionResult> GetUser([FromBody] int id)
        {
            try
            {
                if (id != 0)
                {
                    var role = await db.Users.Include(c => c.Role).SingleAsync(c => c.Id == id);

                    if (role == null)
                    {
                        return this.UnSuccessFunction("Data Not Found", "error");
                    }

                    return this.DataFunction(true, role);
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
        public async Task<IActionResult> GetUserByUsername([FromBody] string username)
        {
            try
            {
                var user = await db.Users.Include(c => c.Role).SingleAsync(c => c.Username.Equals(username));

                return this.DataFunction(true, user);
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
                        var user = await db.Users.SingleAsync(c => c.Id == id);

                        user.UserState = UserState.Removed;
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
        public async Task<IActionResult> changeState([FromBody] ChangeStateParam changeStateParam)
        {
            try
            {
                var user = await db.Users.SingleAsync(c => c.Id == changeStateParam.userId);

                lock (user)
                {
                    user.UserState = (UserState)changeStateParam.userState;
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

    public class ChangeStateParam
    {
        public int userId { get; set; }

        public int userState { get; set; }
    }

    public class UserAddParam
    {
        public string username { get; set; }

        public string password { get; set; }

        public string firstname { get; set; }
        public string lastname { get; set; }
        public string meliCode { get; set; }
        public int userState { get; set; }
        public int roleId { get; set; }

        public string picName { get; set; }
        public string picData { get; set; }
    }

    public class LoginParams
    {
        public string username { get; set; }

        public string password { get; set; }
    }

    public class UserMapping<T>
    {
        private readonly Dictionary<T, string> _connections =
            new Dictionary<T, string>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                _connections.Add(key, connectionId);
            }
        }

        public string GetConnections(T key)
        {

            if (_connections.ContainsKey(key))
            {
                return _connections[key];
            }

            return "";
        }

        public void changeValue(T key, string val)
        {
            lock (_connections)
            {
                _connections[key] = val;
            }
        }

        public bool haveKey(T key)
        {
            return _connections.ContainsKey(key);
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                string connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                _connections.Remove(key);
            }
        }

        public bool removeFull(T key)
        {
            lock (_connections)
            {
                if (_connections.ContainsKey(key))
                {
                    return _connections.Remove(key);
                }

                return false;
            }
        }
    }
}