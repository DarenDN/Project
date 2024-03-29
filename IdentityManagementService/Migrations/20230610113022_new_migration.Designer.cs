﻿// <auto-generated />
using System;
using IdentityManagementService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IdentityManagementService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230610113022_new_migration")]
    partial class new_migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IdentityManagementService.Models.Identity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<Guid?>("RefreshTokenId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserInfoId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RefreshTokenId");

                    b.HasIndex("UserInfoId");

                    b.ToTable("Identities");
                });

            modelBuilder.Entity("IdentityManagementService.Models.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("IdentityManagementService.Models.UserInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("RoleId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("IdentityManagementService.Models.Identity", b =>
                {
                    b.HasOne("IdentityManagementService.Models.RefreshToken", "RefreshToken")
                        .WithMany()
                        .HasForeignKey("RefreshTokenId");

                    b.HasOne("IdentityManagementService.Models.UserInfo", "UserInfo")
                        .WithMany()
                        .HasForeignKey("UserInfoId");

                    b.Navigation("RefreshToken");

                    b.Navigation("UserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
