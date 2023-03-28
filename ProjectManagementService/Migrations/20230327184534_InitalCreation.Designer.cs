﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectManagementService.Data;

#nullable disable

namespace ProjectManagementService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230327184534_InitalCreation")]
    partial class InitalCreation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectManagementService.Models.DashboardModel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int[]>("AllowedUserTypes")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectID")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("ID");

                    b.HasIndex("ProjectID");

                    b.ToTable("DashboardModels");
                });

            modelBuilder.Entity("ProjectManagementService.Models.ProjectModel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("ProjectModels");
                });

            modelBuilder.Entity("ProjectManagementService.Models.TaskModel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DashboardID")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("character varying(10000)");

                    b.Property<Guid?>("PerformerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StatusID")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("TypeID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ID");

                    b.HasIndex("DashboardID");

                    b.HasIndex("StatusID");

                    b.HasIndex("TypeID");

                    b.ToTable("TaskModels");
                });

            modelBuilder.Entity("ProjectManagementService.Models.TaskStatusModel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("TaskStatusModels");
                });

            modelBuilder.Entity("ProjectManagementService.Models.TaskTypeModel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("ID");

                    b.ToTable("TaskTypeModels");
                });

            modelBuilder.Entity("ProjectManagementService.Models.UserModel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uuid");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.ToTable("UserModels");
                });

            modelBuilder.Entity("ProjectManagementService.Models.UserRoleModel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.ToTable("UserRoleModels");
                });

            modelBuilder.Entity("ProjectManagementService.Models.DashboardModel", b =>
                {
                    b.HasOne("ProjectManagementService.Models.ProjectModel", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManagementService.Models.TaskModel", b =>
                {
                    b.HasOne("ProjectManagementService.Models.DashboardModel", "Dashboard")
                        .WithMany("Tasks")
                        .HasForeignKey("DashboardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementService.Models.TaskStatusModel", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementService.Models.TaskTypeModel", "Type")
                        .WithMany()
                        .HasForeignKey("TypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dashboard");

                    b.Navigation("Status");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("ProjectManagementService.Models.UserModel", b =>
                {
                    b.HasOne("ProjectManagementService.Models.UserRoleModel", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ProjectManagementService.Models.DashboardModel", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
