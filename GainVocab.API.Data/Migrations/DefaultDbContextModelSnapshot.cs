﻿// <auto-generated />
using System;
using GainVocab.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GainVocab.API.Data.Migrations
{
    [DbContext(typeof(DefaultDbContext))]
    partial class DefaultDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GainVocab.API.Data.Models.APIUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.APIUserCourse", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("APIUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("APIUserId");

                    b.HasIndex("CourseId");

                    b.ToTable("APIUserCourse", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.Course", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("LanguageFromId")
                        .HasColumnType("bigint");

                    b.Property<long>("LanguageToId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex("LanguageFromId");

                    b.HasIndex("LanguageToId");

                    b.HasIndex(new[] { "PublicId" }, "IX_Courses_PublicId")
                        .IsUnique();

                    b.ToTable("Courses", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.CourseData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .IsFixedLength();

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex(new[] { "PublicId" }, "IX_CourseData_PublicId")
                        .IsUnique();

                    b.ToTable("CourseData", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.CourseDataExample", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<long>("CourseDataId")
                        .HasColumnType("bigint");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .IsFixedLength();

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Translation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseDataId");

                    b.HasIndex(new[] { "PublicId" }, "IX_CourseDataExample_PublicId")
                        .IsUnique();

                    b.ToTable("CourseDataExample", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.CourseProgress", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<int>("AmountOfErrors")
                        .HasColumnType("integer");

                    b.Property<long>("CurrentCourseDataId")
                        .HasColumnType("bigint");

                    b.Property<int>("PercentProgress")
                        .HasColumnType("integer");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .IsFixedLength();

                    b.Property<long>("UserCourseId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CurrentCourseDataId");

                    b.HasIndex("UserCourseId")
                        .IsUnique();

                    b.HasIndex(new[] { "PublicId" }, "IX_CourseProgress_PublicId")
                        .IsUnique();

                    b.ToTable("CourseProgress", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.Language", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PublicId" }, "IX_Languages_PublicId")
                        .IsUnique();

                    b.ToTable("Languages", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.SupportIssue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsResolved")
                        .HasColumnType("boolean");

                    b.Property<long?>("IssueEntityId")
                        .HasColumnType("bigint");

                    b.Property<string>("IssueMessage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("IssueTypeId")
                        .HasColumnType("bigint");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .IsFixedLength();

                    b.Property<string>("ReporterId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("IssueEntityId");

                    b.HasIndex("IssueTypeId");

                    b.HasIndex(new[] { "PublicId" }, "IX_SupportIssue_PublicId")
                        .IsUnique();

                    b.ToTable("SupportIssue", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.SupportIssueType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PublicId")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .HasDefaultValueSql("uuid_generate_v4()")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex(new[] { "PublicId" }, "IX_SupportIssueType_PublicId")
                        .IsUnique();

                    b.ToTable("SupportIssueType", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.APIUserCourse", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.APIUser", "APIUser")
                        .WithMany("Courses")
                        .HasForeignKey("APIUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GainVocab.API.Data.Models.Course", "Course")
                        .WithMany("Users")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("APIUser");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.Course", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.Language", "LanguageFrom")
                        .WithMany("CoursesFrom")
                        .HasForeignKey("LanguageFromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Course_LanguageFromId");

                    b.HasOne("GainVocab.API.Data.Models.Language", "LanguageTo")
                        .WithMany("CoursesTo")
                        .HasForeignKey("LanguageToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Course_LanguageToId");

                    b.Navigation("LanguageFrom");

                    b.Navigation("LanguageTo");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.CourseData", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.Course", "Course")
                        .WithMany("Data")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CourseData_CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.CourseDataExample", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.CourseData", "CourseData")
                        .WithMany("Examples")
                        .HasForeignKey("CourseDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CourseDataExample_CourseDataId");

                    b.Navigation("CourseData");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.CourseProgress", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.CourseData", "CurrentCourseData")
                        .WithMany("Progresses")
                        .HasForeignKey("CurrentCourseDataId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_CourseProgress_CurrentCourseDataId");

                    b.HasOne("GainVocab.API.Data.Models.APIUserCourse", "UserCourse")
                        .WithOne("CourseProgress")
                        .HasForeignKey("GainVocab.API.Data.Models.CourseProgress", "UserCourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentCourseData");

                    b.Navigation("UserCourse");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.SupportIssue", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.CourseData", "IssueEntity")
                        .WithMany("Issues")
                        .HasForeignKey("IssueEntityId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_SupportIssue_IssueEntityId");

                    b.HasOne("GainVocab.API.Data.Models.SupportIssueType", "IssueType")
                        .WithMany("Issues")
                        .HasForeignKey("IssueTypeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_SupportIssue_IssueTypeId");

                    b.Navigation("IssueEntity");

                    b.Navigation("IssueType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.APIUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.APIUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GainVocab.API.Data.Models.APIUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GainVocab.API.Data.Models.APIUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.APIUser", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.APIUserCourse", b =>
                {
                    b.Navigation("CourseProgress")
                        .IsRequired();
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.Course", b =>
                {
                    b.Navigation("Data");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.CourseData", b =>
                {
                    b.Navigation("Examples");

                    b.Navigation("Issues");

                    b.Navigation("Progresses");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.Language", b =>
                {
                    b.Navigation("CoursesFrom");

                    b.Navigation("CoursesTo");
                });

            modelBuilder.Entity("GainVocab.API.Data.Models.SupportIssueType", b =>
                {
                    b.Navigation("Issues");
                });
#pragma warning restore 612, 618
        }
    }
}
