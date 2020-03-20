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
    [Migration("20190309180755_initUpdate5")]
    partial class initUpdate5
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

                    b.Property<DateTime>("DateExpire");

                    b.Property<DateTime>("DatePublish");

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

                    b.Property<bool>("Add_Item");

                    b.Property<bool>("Add_Role");

                    b.Property<bool>("Add_Unit");

                    b.Property<bool>("Add_User");

                    b.Property<bool>("Edit_Attribute");

                    b.Property<bool>("Edit_Category");

                    b.Property<bool>("Edit_Item");

                    b.Property<bool>("Edit_Role");

                    b.Property<bool>("Edit_Unit");

                    b.Property<bool>("Edit_User");

                    b.Property<string>("Name");

                    b.Property<bool>("Remove_Attribute");

                    b.Property<bool>("Remove_Category");

                    b.Property<bool>("Remove_Item");

                    b.Property<bool>("Remove_Role");

                    b.Property<bool>("Remove_Unit");

                    b.Property<bool>("Remove_User");

                    b.Property<bool>("Validation_User");

                    b.Property<bool>("View_Attribute");

                    b.Property<bool>("View_Category");

                    b.Property<bool>("View_Item");

                    b.Property<bool>("View_Role");

                    b.Property<bool>("View_Unit");

                    b.Property<bool>("View_User");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
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
                            Name = "مدیر سیستم",
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
                            View_User = true
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
                            DateAdd = new DateTime(2019, 3, 9, 21, 37, 54, 612, DateTimeKind.Local).AddTicks(4022),
                            DateEdit = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Firstname = "میثم",
                            GId = new Guid("51939f3e-b31a-466b-804d-c39e873402c8"),
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
