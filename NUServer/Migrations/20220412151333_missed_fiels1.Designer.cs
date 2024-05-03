﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NUServer.Api.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NUServer.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220412151333_missed_fiels1")]
    partial class missed_fiels1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NUServer.Models.DB.ResourceDBModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ResourceSet");
                });

            modelBuilder.Entity("NUServer.Models.PackageModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AvtorId")
                        .HasColumnType("uuid");

                    b.Property<string>("AvtorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("DownloadCount")
                        .HasColumnType("bigint");

                    b.Property<string>("LatestVersion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Private")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("AvtorId");

                    b.ToTable("PackageSet");
                });

            modelBuilder.Entity("NUServer.Models.PackageVersionModel", b =>
                {
                    b.Property<Guid>("PackageId")
                        .HasColumnType("uuid");

                    b.Property<string>("Version")
                        .HasColumnType("text");

                    b.Property<long>("DownloadCount")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("PackageId", "Version");

                    b.ToTable("PackageVersionSet");
                });

            modelBuilder.Entity("NUServer.Models.UserModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PublishToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShareToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserSet");
                });

            modelBuilder.Entity("NUServer.Models.PackageModel", b =>
                {
                    b.HasOne("NUServer.Models.UserModel", "Avtor")
                        .WithMany("PackageList")
                        .HasForeignKey("AvtorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Avtor");
                });

            modelBuilder.Entity("NUServer.Models.PackageVersionModel", b =>
                {
                    b.HasOne("NUServer.Models.PackageModel", "Package")
                        .WithMany("VersionList")
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Package");
                });

            modelBuilder.Entity("NUServer.Models.PackageModel", b =>
                {
                    b.Navigation("VersionList");
                });

            modelBuilder.Entity("NUServer.Models.UserModel", b =>
                {
                    b.Navigation("PackageList");
                });
#pragma warning restore 612, 618
        }
    }
}