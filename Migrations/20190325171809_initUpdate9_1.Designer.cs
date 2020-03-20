﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCMR_Api.Data;

namespace SCMR_Api.Migrations
{
    [DbContext(typeof(Data.DbContext))]
    [Migration("20190325171809_initUpdate9_1")]
    partial class initUpdate9_1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SCMR_Api.Model.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttrType");

                    b.Property<int>("CategoryId");

                    b.Property<string>("Desc");

                    b.Property<Guid>("GId");

                    b.Property<bool>("IsUniq");

                    b.Property<int>("Order");

                    b.Property<string>("Title");

                    b.Property<int>("UnitId");

                    b.Property<string>("Values");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UnitId");

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("SCMR_Api.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("DateExpire");

                    b.Property<DateTime?>("DatePublish");

                    b.Property<string>("Desc");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("ParentId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("SCMR_Api.Model.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FileName");

                    b.Property<bool>("FileStatus");

                    b.Property<string>("FileUrl");

                    b.Property<DateTime?>("ReciveDate");

                    b.Property<bool>("ReciveStatus");

                    b.Property<int>("ReciverId");

                    b.Property<DateTime>("SendDate");

                    b.Property<int>("SenderId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("SCMR_Api.Model.ChatRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Allow");

                    b.Property<int>("RoleFromId");

                    b.Property<int>("RoleToId");

                    b.HasKey("Id");

                    b.ToTable("ChatRoles");
                });

            modelBuilder.Entity("SCMR_Api.Model.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Cls.Autonumber")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capasity")
                        .HasColumnName("Cls.Capasity");

                    b.Property<int>("Code")
                        .HasColumnName("Cls.Id");

                    b.Property<int>("GradeId")
                        .HasColumnName("Grd.Id");

                    b.Property<string>("Name")
                        .HasColumnName("Cls.Name");

                    b.Property<int>("Order")
                        .HasColumnName("Cls.Order");

                    b.Property<string>("Section")
                        .HasColumnName("Cls.Section");

                    b.HasKey("Id");

                    b.HasIndex("GradeId");

                    b.ToTable("sm.Class");
                });

            modelBuilder.Entity("SCMR_Api.Model.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Grd.Autonumber")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capasity")
                        .HasColumnName("Grd.Capasity");

                    b.Property<string>("Code")
                        .HasColumnName("Grd.Id");

                    b.Property<string>("Desc")
                        .HasColumnName("Grd.Desc");

                    b.Property<string>("InternalCode")
                        .HasColumnName("Grd.InternalCode");

                    b.Property<string>("Name")
                        .HasColumnName("Grd.Name");

                    b.Property<int>("Order")
                        .HasColumnName("Grd.Order");

                    b.Property<string>("OrgCode")
                        .HasColumnName("Grd.OrgCode");

                    b.Property<int>("TituteId")
                        .HasColumnName("Grd.insCode");

                    b.Property<int>("YeareducationId")
                        .HasColumnName("Grd.eduyearCode");

                    b.HasKey("Id");

                    b.HasIndex("TituteId");

                    b.HasIndex("YeareducationId");

                    b.ToTable("sm.Grade");
                });

            modelBuilder.Entity("SCMR_Api.Model.InsTitute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ins.AutoNum")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnName("ins.Address");

                    b.Property<string>("City")
                        .HasColumnName("ins.City");

                    b.Property<string>("Desc")
                        .HasColumnName("ins.Desc");

                    b.Property<string>("Email")
                        .HasColumnName("ins.Email");

                    b.Property<string>("Name")
                        .HasColumnName("ins.Name");

                    b.Property<int>("OrgCode")
                        .HasColumnName("ins.OrgCode");

                    b.Property<string>("OrgSection")
                        .HasColumnName("ins.OrgSection");

                    b.Property<int>("OrgSex")
                        .HasColumnName("ins.OrgSex");

                    b.Property<string>("PostCode")
                        .HasColumnName("ins.PostCode");

                    b.Property<string>("State")
                        .HasColumnName("ins.State");

                    b.Property<string>("Tell")
                        .HasColumnName("ins.Tell");

                    b.Property<int?>("TituteCode")
                        .HasColumnName("ins.Code");

                    b.HasKey("Id");

                    b.HasIndex("TituteCode");

                    b.ToTable("sm.insTitute");
                });

            modelBuilder.Entity("SCMR_Api.Model.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<bool>("IsActive");

                    b.Property<long>("RahCode");

                    b.Property<string>("Tags");

                    b.Property<string>("Title")
                        .HasMaxLength(50);

                    b.Property<int>("UnitId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UnitId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SCMR_Api.Model.ItemAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AttributeFilePath");

                    b.Property<int>("AttributeId");

                    b.Property<string>("AttrubuteValue");

                    b.Property<int>("ItemId");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemAttributes");
                });

            modelBuilder.Entity("SCMR_Api.Model.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("Desc");

                    b.Property<string>("Event");

                    b.Property<string>("Ip");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("SCMR_Api.Model.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Add_Attribute");

                    b.Property<bool>("Add_Category");

                    b.Property<bool>("Add_InsTitute");

                    b.Property<bool>("Add_Item");

                    b.Property<bool>("Add_Role");

                    b.Property<bool>("Add_Unit");

                    b.Property<bool>("Add_User");

                    b.Property<bool>("Add_Yeareducation");

                    b.Property<bool>("Edit_Attribute");

                    b.Property<bool>("Edit_Category");

                    b.Property<bool>("Edit_InsTitute");

                    b.Property<bool>("Edit_Item");

                    b.Property<bool>("Edit_Role");

                    b.Property<bool>("Edit_Unit");

                    b.Property<bool>("Edit_User");

                    b.Property<bool>("Edit_Yeareducation");

                    b.Property<string>("Name");

                    b.Property<bool>("Remove_Attribute");

                    b.Property<bool>("Remove_Category");

                    b.Property<bool>("Remove_InsTitute");

                    b.Property<bool>("Remove_Item");

                    b.Property<bool>("Remove_Role");

                    b.Property<bool>("Remove_Unit");

                    b.Property<bool>("Remove_User");

                    b.Property<bool>("Remove_Yeareducation");

                    b.Property<bool>("Validation_User");

                    b.Property<bool>("View_Attribute");

                    b.Property<bool>("View_Category");

                    b.Property<bool>("View_InsTitute");

                    b.Property<bool>("View_Item");

                    b.Property<bool>("View_Role");

                    b.Property<bool>("View_Unit");

                    b.Property<bool>("View_User");

                    b.Property<bool>("View_Yeareducation");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Add_Attribute = true,
                            Add_Category = true,
                            Add_InsTitute = false,
                            Add_Item = true,
                            Add_Role = true,
                            Add_Unit = true,
                            Add_User = true,
                            Add_Yeareducation = false,
                            Edit_Attribute = true,
                            Edit_Category = true,
                            Edit_InsTitute = false,
                            Edit_Item = true,
                            Edit_Role = true,
                            Edit_Unit = true,
                            Edit_User = true,
                            Edit_Yeareducation = false,
                            Name = "مدیر سیستم",
                            Remove_Attribute = true,
                            Remove_Category = true,
                            Remove_InsTitute = false,
                            Remove_Item = true,
                            Remove_Role = true,
                            Remove_Unit = true,
                            Remove_User = true,
                            Remove_Yeareducation = false,
                            Validation_User = true,
                            View_Attribute = true,
                            View_Category = true,
                            View_InsTitute = false,
                            View_Item = true,
                            View_Role = true,
                            View_Unit = true,
                            View_User = true,
                            View_Yeareducation = false
                        });
                });

            modelBuilder.Entity("SCMR_Api.Model.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EnTitle");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("SCMR_Api.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdd");

                    b.Property<DateTime>("DateEdit");

                    b.Property<string>("Firstname");

                    b.Property<Guid>("GId");

                    b.Property<string>("Lastname");

                    b.Property<string>("MeliCode");

                    b.Property<string>("Password");

                    b.Property<string>("ProfilePic");

                    b.Property<int>("RoleId");

                    b.Property<int>("UserState");

                    b.Property<string>("UserStateDesc");

                    b.Property<string>("Username");

                    b.Property<bool>("isLogedIn");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateAdd = new DateTime(2019, 3, 25, 21, 48, 8, 830, DateTimeKind.Local).AddTicks(5683),
                            DateEdit = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Firstname = "میثم",
                            GId = new Guid("8f5170c3-686b-46ec-a6ba-0a144bd9a797"),
                            Lastname = "زارع",
                            MeliCode = "2282795547",
                            Password = "12345678",
                            RoleId = 1,
                            UserState = 1,
                            UserStateDesc = "",
                            Username = "meysam1",
                            isLogedIn = false
                        });
                });

            modelBuilder.Entity("SCMR_Api.Model.Yeareducation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("edu.YeareduCode")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateEnd")
                        .HasColumnName("edu.DateEnd");

                    b.Property<DateTime>("DateStart")
                        .HasColumnName("edu.DateStart");

                    b.Property<string>("Desc")
                        .HasColumnName("edu.Desc");

                    b.Property<string>("Name")
                        .HasColumnName("edu.YeareduName");

                    b.HasKey("Id");

                    b.ToTable("sm.Yeareducation");
                });

            modelBuilder.Entity("SCMR_Api.Model.Attribute", b =>
                {
                    b.HasOne("SCMR_Api.Model.Category", "Category")
                        .WithMany("Attributes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SCMR_Api.Model.Unit", "Unit")
                        .WithMany("Attributes")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SCMR_Api.Model.Category", b =>
                {
                    b.HasOne("SCMR_Api.Model.Category", "ParentCategory")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("SCMR_Api.Model.Class", b =>
                {
                    b.HasOne("SCMR_Api.Model.Grade", "Grade")
                        .WithMany("Classes")
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SCMR_Api.Model.Grade", b =>
                {
                    b.HasOne("SCMR_Api.Model.InsTitute", "Titute")
                        .WithMany("Grades")
                        .HasForeignKey("TituteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SCMR_Api.Model.Yeareducation", "Yeareducation")
                        .WithMany("Grades")
                        .HasForeignKey("YeareducationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SCMR_Api.Model.InsTitute", b =>
                {
                    b.HasOne("SCMR_Api.Model.InsTitute", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("TituteCode");
                });

            modelBuilder.Entity("SCMR_Api.Model.Item", b =>
                {
                    b.HasOne("SCMR_Api.Model.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SCMR_Api.Model.Unit", "Unit")
                        .WithMany("Items")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SCMR_Api.Model.ItemAttribute", b =>
                {
                    b.HasOne("SCMR_Api.Model.Attribute", "Attribute")
                        .WithMany("ItemAttribute")
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SCMR_Api.Model.Item", "Item")
                        .WithMany("ItemAttribute")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SCMR_Api.Model.Log", b =>
                {
                    b.HasOne("SCMR_Api.Model.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("SCMR_Api.Model.User", b =>
                {
                    b.HasOne("SCMR_Api.Model.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
