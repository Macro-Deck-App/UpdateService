﻿// <auto-generated />
using System;
using MacroDeck.UpdateService.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MacroDeck.UpdateService.Core.Migrations
{
    [DbContext(typeof(UpdateServiceContext))]
    [Migration("20230605141244_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MacroDeck.UpdateService.Core.DataAccess.Entities.VersionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_timestamp");

                    b.Property<bool>("IsBetaVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_pre_release_version");

                    b.Property<int>("Major")
                        .HasColumnType("integer")
                        .HasColumnName("version_major");

                    b.Property<int>("Minor")
                        .HasColumnType("integer")
                        .HasColumnName("version_minor");

                    b.Property<int>("Patch")
                        .HasColumnType("integer")
                        .HasColumnName("version_patch");

                    b.Property<int?>("PreReleaseNo")
                        .HasColumnType("integer")
                        .HasColumnName("version_pre_release_no");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("version_string");

                    b.HasKey("Id");

                    b.HasIndex("Version")
                        .IsUnique();

                    b.ToTable("versions", "updateservice");
                });

            modelBuilder.Entity("MacroDeck.UpdateService.Core.DataAccess.Entities.VersionFileEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_timestamp");

                    b.Property<string>("FileHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hash");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_name");

                    b.Property<int>("FileProvider")
                        .HasColumnType("integer")
                        .HasColumnName("file_provider");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint")
                        .HasColumnName("file_size");

                    b.Property<int>("PlatformIdentifier")
                        .HasColumnType("integer")
                        .HasColumnName("platform_identifier");

                    b.Property<int>("VersionId")
                        .HasColumnType("integer")
                        .HasColumnName("version_ref");

                    b.HasKey("Id");

                    b.HasIndex("VersionId");

                    b.ToTable("version_files", "updateservice");
                });

            modelBuilder.Entity("MacroDeck.UpdateService.Core.DataAccess.Entities.VersionFileEntity", b =>
                {
                    b.HasOne("MacroDeck.UpdateService.Core.DataAccess.Entities.VersionEntity", "Version")
                        .WithMany("Files")
                        .HasForeignKey("VersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Version");
                });

            modelBuilder.Entity("MacroDeck.UpdateService.Core.DataAccess.Entities.VersionEntity", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
