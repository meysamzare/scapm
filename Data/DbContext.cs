using Microsoft.EntityFrameworkCore;
using SCMR_Api.Model;
using SCMR_Api.Model.Financial;
using SCMR_Api.Model.Index;
using System;
using System.Linq;

namespace SCMR_Api.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // modelBuilder.Entity<Student>()
            //     .HasOne(c => c.WorkBookHeader)
            //     .WithOne()
            //     .HasForeignKey<Student>(c => c.Code);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.ReciverUser)
                .WithMany(c => c.RecivedChat)
                .HasForeignKey(c => c.ReciverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.SenderUser)
                .WithMany(c => c.SendedChat)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Class>()
                .HasOne(c => c.Grade)
                .WithMany(c => c.Classes)
                .HasForeignKey(c => c.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimeSchedule>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ItemAttribute>()
                .HasOne(c => c.Item)
                .WithMany(c => c.ItemAttribute)
                .HasForeignKey(c => c.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ItemAttribute>()
                .HasOne(c => c.Attribute)
                .WithMany(c => c.ItemAttribute)
                .HasForeignKey(c => c.AttributeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StdClassMng>()
                .HasOne(c => c.Student)
                .WithMany(c => c.StdClassMngs)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StdClassMng>()
                .HasOne(c => c.Class)
                .WithMany(c => c.StdClassMngs)
                .HasForeignKey(c => c.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StdClassMng>()
                .HasOne(c => c.Grade)
                .WithMany(c => c.StdClassMngs)
                .HasForeignKey(c => c.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StdClassMng>()
                .HasOne(c => c.Yeareducation)
                .WithMany(c => c.StdClassMngs)
                .HasForeignKey(c => c.YeareducationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StdClassMng>()
                .HasOne(c => c.InsTitute)
                .WithMany(c => c.StdClassMngs)
                .HasForeignKey(c => c.InsTituteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Add_Attribute = true,
                    Add_Category = true,
                    Add_Item = true,
                    Add_Role = true,
                    Add_Unit = true,
                    Add_User = true,
                    Edit_Attribute = true,
                    Edit_Category = true,
                    Edit_Item = true,
                    Edit_Role = true,
                    Edit_Unit = true,
                    Edit_User = true,
                    Remove_Attribute = true,
                    Remove_Category = true,
                    Remove_Item = true,
                    Remove_Role = true,
                    Remove_Unit = true,
                    Remove_User = true,
                    Validation_User = true,
                    View_Attribute = true,
                    View_Category = true,
                    View_Item = true,
                    View_Role = true,
                    View_Unit = true,
                    View_User = true,
                    Name = "مدیر سیستم",
                    Id = 1
                });


            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = 1,
                    Username = "meysam",
                    Password = "12345678",
                    DateAdd = DateTime.Now,
                    Firstname = "میثم",
                    Lastname = "زارع",
                    MeliCode = "2282795547",
                    UserState = UserState.Active,
                    RoleId = 1,
                    UserStateDesc = ""
                });

            modelBuilder.Entity<Setting>()
                .HasData(new Setting
                {
                    Id = 1,
                    Key = "NowYeareducationId",
                    Value = "1"
                });

            modelBuilder.Entity<ILogSystem>()
                .Property(e => e.TableObjectIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Category>()
                .Property(e => e.TeachersIdAccess)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToArray());


            modelBuilder.Entity<OnlineClass>()
                .Property(e => e.AllowedAdminIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToArray());

            modelBuilder.Entity<OnlineClass>()
                .Property(e => e.AllowedStudentIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)).ToArray());

        }


        public DbSet<Setting> Settings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Log> Logs { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }


        public DbSet<ILogSystem> ILogSystems { get; set; }

        public DbSet<Unit> Units { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemAttribute> ItemAttributes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Model.Attribute> Attributes { get; set; }

        public DbSet<RegisterItemLogin> RegisterItemLogins { get; set; }


        public DbSet<AttributeOption> AttributeOptions { get; set; }

        public DbSet<ChatRole> ChatRoles { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<InsTitute> InsTitutes { get; set; }
        public DbSet<Yeareducation> Yeareducations { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<OrgChart> OrgCharts { get; set; }
        public DbSet<OrgPerson> OrgPeople { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TimeandDays> TimeandDays { get; set; }
        public DbSet<TimeSchedule> TimeSchedules { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentInfo> StudentInfos { get; set; }
        public DbSet<StdClassMng> StdClassMngs { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<ExamScore> ExamScores { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<StdPayment> StdPayments { get; set; }

        public DbSet<StudentType> StudentTypes { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketConversation> TicketConversations { get; set; }


        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationAgent> NotificationAgents { get; set; }
        public DbSet<NotificationSeen> NotificationSeens { get; set; }

        public DbSet<MobileChat> MobileChats { get; set; }

        public DbSet<ClassBook> ClassBooks { get; set; }


        public DbSet<Workbook> Workbooks { get; set; }


        // INDEX ------------------------------------------------


        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MainSlideShow> MainSlideShows { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Advertising> Advertisings { get; set; }

        public DbSet<BestStudent> BestStudents { get; set; }

        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureGallery> PictureGalleries { get; set; }


        // End_INDEX ------------------------------------------------


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Writer> Writers { get; set; }

        public DbSet<ScoreThemplate> ScoreThemplates { get; set; }
        public DbSet<StudentScore> StudentScores { get; set; }



        public DbSet<WorkBookHeader> WorkBookHeaders { get; set; }
        public DbSet<WorkBookDetail> WorkBookDetails { get; set; }


        public DbSet<OnlineClass> OnlineClasses { get; set; }

        public DbSet<OnlineClassLogin> OnlineClassLogins { get; set; }

        public DbSet<OnlineClassServer> OnlineClassServers { get; set; }

        public DbSet<StudentDailySchedule> StudentDailySchedules { get; set; }


        public DbSet<DescriptiveScore> DescriptiveScores { get; set; }
    }
}
