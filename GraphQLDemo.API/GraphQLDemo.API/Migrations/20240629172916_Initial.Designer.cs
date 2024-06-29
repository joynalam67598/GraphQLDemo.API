﻿// <auto-generated />
using System;
using GraphQLDemo.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GraphQLDemo.API.Migrations
{
    [DbContext(typeof(SchoolDBContext))]
    [Migration("20240629172916_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("GraphQLDemo.API.DTOs.CourseDTO", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("InstructorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Subject")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("InstructorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("GraphQLDemo.API.DTOs.InstructorDTO", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.Property<double>("Salary")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Instructors");
                });

            modelBuilder.Entity("GraphQLDemo.API.DTOs.StudentDTO", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CourseDTOId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<double>("GPA")
                        .HasColumnType("REAL");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseDTOId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("GraphQLDemo.API.DTOs.CourseDTO", b =>
                {
                    b.HasOne("GraphQLDemo.API.DTOs.InstructorDTO", "Instructor")
                        .WithMany()
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("GraphQLDemo.API.DTOs.StudentDTO", b =>
                {
                    b.HasOne("GraphQLDemo.API.DTOs.CourseDTO", null)
                        .WithMany("Students")
                        .HasForeignKey("CourseDTOId");
                });

            modelBuilder.Entity("GraphQLDemo.API.DTOs.CourseDTO", b =>
                {
                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
