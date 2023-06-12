﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjectManagementService.Data;

#nullable disable

namespace ProjectManagementService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230612174101_state_order_added")]
    partial class state_order_added
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectManagementService.Models.Dashboard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("AllowedUserRoles")
                        .HasColumnType("uuid[]");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Dashboards");
                });

            modelBuilder.Entity("ProjectManagementService.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("character varying(10000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("ProjectManagementService.Models.ProjectsIdentity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("IdentityId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("ProjectsIdentities");
                });

            modelBuilder.Entity("ProjectManagementService.Models.ProjectsRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("ProjectsRoles");
                });

            modelBuilder.Entity("ProjectManagementService.Models.Sprint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("date");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool?>("Finished")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Sprints");
                });

            modelBuilder.Entity("ProjectManagementService.Models.StateRelationship", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("StateCurrent")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StateNext")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("StateRelationships");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ab1d1a8a-0009-46ba-b539-b9ec6f7e9439"),
                            StateCurrent = new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8"),
                            StateNext = new Guid("c9124443-fab8-4696-a837-ecf7b421f4be")
                        },
                        new
                        {
                            Id = new Guid("5fb03a10-19cd-40be-a8f3-0ae4ce3d8644"),
                            StateCurrent = new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"),
                            StateNext = new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604")
                        },
                        new
                        {
                            Id = new Guid("e946eb51-a59a-47b8-9b59-a23066911db3"),
                            StateCurrent = new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"),
                            StateNext = new Guid("d2ab7572-2359-4698-ae87-14b5955101cf")
                        },
                        new
                        {
                            Id = new Guid("edf99163-e97f-4135-9964-177a31fe5c84"),
                            StateCurrent = new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"),
                            StateNext = new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c")
                        },
                        new
                        {
                            Id = new Guid("2b8d5fc5-170f-409e-a49c-1f472ab4095f"),
                            StateCurrent = new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"),
                            StateNext = new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604")
                        },
                        new
                        {
                            Id = new Guid("a0f99de4-16f5-44e3-99e9-ff4e2b20e80c"),
                            StateCurrent = new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"),
                            StateNext = new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814")
                        },
                        new
                        {
                            Id = new Guid("9a0e8ccd-f368-4659-bab9-56824d8727fd"),
                            StateCurrent = new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"),
                            StateNext = new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8")
                        },
                        new
                        {
                            Id = new Guid("50133267-24d8-473a-9502-91a15613d852"),
                            StateCurrent = new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"),
                            StateNext = new Guid("c9124443-fab8-4696-a837-ecf7b421f4be")
                        },
                        new
                        {
                            Id = new Guid("57701f91-fea4-40f4-8c25-68939f2703d6"),
                            StateCurrent = new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604"),
                            StateNext = new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17")
                        },
                        new
                        {
                            Id = new Guid("687782db-73f2-400e-a179-6dd6b1a90f52"),
                            StateCurrent = new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"),
                            StateNext = new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff")
                        },
                        new
                        {
                            Id = new Guid("c9557731-7df0-4b14-a8b2-5593846d0621"),
                            StateCurrent = new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"),
                            StateNext = new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8")
                        },
                        new
                        {
                            Id = new Guid("999d3e23-224f-484d-984e-b1d747ebf50e"),
                            StateCurrent = new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"),
                            StateNext = new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604")
                        },
                        new
                        {
                            Id = new Guid("f4790bbb-9bcb-4783-b5ed-448090e64e9c"),
                            StateCurrent = new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"),
                            StateNext = new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8")
                        },
                        new
                        {
                            Id = new Guid("c9724f8f-973e-4a76-8e21-88d0b77470a4"),
                            StateCurrent = new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"),
                            StateNext = new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff")
                        },
                        new
                        {
                            Id = new Guid("717ac234-3cc0-484d-8f05-b4a41459a7ab"),
                            StateCurrent = new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"),
                            StateNext = new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604")
                        },
                        new
                        {
                            Id = new Guid("c8c62654-d646-439c-a443-8c9f5f964bce"),
                            StateCurrent = new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"),
                            StateNext = new Guid("d2ab7572-2359-4698-ae87-14b5955101cf")
                        },
                        new
                        {
                            Id = new Guid("b92c18e9-872e-4f01-aa07-3b9f0951ea1e"),
                            StateCurrent = new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"),
                            StateNext = new Guid("69a34855-ad70-4b86-b2b6-fe801faf0f9f")
                        });
                });

            modelBuilder.Entity("ProjectManagementService.Models.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(10000)
                        .HasColumnType("character varying(10000)");

                    b.Property<int?>("EstimationInPoints")
                        .HasColumnType("integer");

                    b.Property<TimeSpan?>("EstimationInTime")
                        .HasColumnType("interval");

                    b.Property<Guid?>("PerformerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SprintId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.HasIndex("TypeId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ProjectManagementService.Models.TaskState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.Property<int?>("Order")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TaskStates");

                    b.HasData(
                        new
                        {
                            Id = new Guid("aeb8aaf7-0c48-4806-9840-eb48cccbbdf8"),
                            Name = "Анализ",
                            Order = 0
                        },
                        new
                        {
                            Id = new Guid("eadbc4f9-f1cf-426d-9ca1-06a69b09971c"),
                            Name = "Доработка",
                            Order = 7
                        },
                        new
                        {
                            Id = new Guid("69a34855-ad70-4b86-b2b6-fe801faf0f9f"),
                            Name = "Завершено",
                            Order = 8
                        },
                        new
                        {
                            Id = new Guid("d2ab7572-2359-4698-ae87-14b5955101cf"),
                            Name = "К доработке",
                            Order = 6
                        },
                        new
                        {
                            Id = new Guid("d31de2b7-103b-45fd-9731-964e7bc0a1ff"),
                            Name = "К работе",
                            Order = 2
                        },
                        new
                        {
                            Id = new Guid("ba5ebabd-89c7-4a3f-b997-ecdb4009a604"),
                            Name = "К тестированию",
                            Order = 4
                        },
                        new
                        {
                            Id = new Guid("c9124443-fab8-4696-a837-ecf7b421f4be"),
                            Name = "Оценка",
                            Order = 1
                        },
                        new
                        {
                            Id = new Guid("8981ee9e-e878-4b7a-b2d3-21e586eba814"),
                            Name = "Работа",
                            Order = 3
                        },
                        new
                        {
                            Id = new Guid("e6bfb790-c086-41b9-aba9-6511fb8e2e17"),
                            Name = "Тестирование",
                            Order = 5
                        });
                });

            modelBuilder.Entity("ProjectManagementService.Models.TaskType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.ToTable("TaskTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("dc9b0e23-5c4a-40fb-83ec-691bcd405a83"),
                            Name = "Задача"
                        },
                        new
                        {
                            Id = new Guid("1738a748-d9f2-4a92-8010-d69b9fb7fd67"),
                            Name = "Ошибка"
                        },
                        new
                        {
                            Id = new Guid("30755127-b34c-4b6f-9e44-57fbd8306bbc"),
                            Name = "Пользовательская история"
                        },
                        new
                        {
                            Id = new Guid("460509ef-ce6d-4b4d-a3cf-1c2161fe4e5e"),
                            Name = "Прочее"
                        });
                });

            modelBuilder.Entity("ProjectManagementService.Models.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ef7d3fc3-611f-46b0-98fd-5bf8e1005dec"),
                            Name = "Админ"
                        },
                        new
                        {
                            Id = new Guid("b68e2f0d-c7e8-4ba2-b5ff-093ac717cb9d"),
                            Name = "Стандартный"
                        });
                });

            modelBuilder.Entity("ProjectManagementService.Models.UserStory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Importance")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SprintId")
                        .HasColumnType("uuid");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SprintId");

                    b.ToTable("UserStories");
                });

            modelBuilder.Entity("ProjectManagementService.Models.Dashboard", b =>
                {
                    b.HasOne("ProjectManagementService.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectManagementService.Models.Task", b =>
                {
                    b.HasOne("ProjectManagementService.Models.TaskState", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectManagementService.Models.TaskType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("ProjectManagementService.Models.UserStory", b =>
                {
                    b.HasOne("ProjectManagementService.Models.Sprint", "Sprint")
                        .WithMany()
                        .HasForeignKey("SprintId");

                    b.Navigation("Sprint");
                });
#pragma warning restore 612, 618
        }
    }
}
