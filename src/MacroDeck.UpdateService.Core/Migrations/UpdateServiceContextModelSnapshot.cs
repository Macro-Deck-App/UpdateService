﻿// <auto-generated />
using System;
using MacroDeck.UpdateService.Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MacroDeck.UpdateService.Core.Migrations
{
    [DbContext(typeof(UpdateServiceContext))]
    partial class UpdateServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MacroDeck.UpdateService.Core.DataAccess.Entities.FileDownloadEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_timestamp");

                    b.Property<int>("DownloadReason")
                        .HasColumnType("integer")
                        .HasColumnName("download_reason");

                    b.Property<int>("VersionFileId")
                        .HasColumnType("integer")
                        .HasColumnName("version_file_ref");

                    b.HasKey("Id");

                    b.HasIndex("DownloadReason");

                    b.HasIndex("VersionFileId");

                    b.ToTable("file_downloads", "updateservice");
                });

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

                    b.Property<bool>("IsPreviewVersion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_preview_version");

                    b.Property<int>("Major")
                        .HasColumnType("integer")
                        .HasColumnName("version_major");

                    b.Property<int>("Minor")
                        .HasColumnType("integer")
                        .HasColumnName("version_minor");

                    b.Property<int>("Patch")
                        .HasColumnType("integer")
                        .HasColumnName("version_patch");

                    b.Property<int?>("PreviewNo")
                        .HasColumnType("integer")
                        .HasColumnName("version_preview_no");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("version_string");

                    b.Property<int>("VersionState")
                        .HasColumnType("integer")
                        .HasColumnName("version_state");

                    b.HasKey("Id");

                    b.HasIndex("Version")
                        .IsUnique();

                    b.HasIndex("VersionState");

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

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint")
                        .HasColumnName("file_size");

                    b.Property<string>("OriginalFileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_name");

                    b.Property<int>("PlatformIdentifier")
                        .HasColumnType("integer")
                        .HasColumnName("platform_identifier");

                    b.Property<string>("SavedFileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("saved_name");

                    b.Property<int>("VersionId")
                        .HasColumnType("integer")
                        .HasColumnName("version_ref");

                    b.HasKey("Id");

                    b.HasIndex("VersionId");

                    b.ToTable("version_files", "updateservice");
                });

            modelBuilder.Entity("MacroDeck.UpdateService.Core.DataAccess.Entities.FileDownloadEntity", b =>
                {
                    b.HasOne("MacroDeck.UpdateService.Core.DataAccess.Entities.VersionFileEntity", "VersionFile")
                        .WithMany("FileDownloads")
                        .HasForeignKey("VersionFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VersionFile");
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

            modelBuilder.Entity("MacroDeck.UpdateService.Core.DataAccess.Entities.VersionFileEntity", b =>
                {
                    b.Navigation("FileDownloads");
                });
#pragma warning restore 612, 618
        }
    }
}
