﻿// <auto-generated />
using System;
using HospitalRegistrationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HospitalRegistrationSystem.Infrastructure.Migrations;

[DbContext(typeof(RepositoryContext))]
[Migration("20221026101448_InitialMigration")]
partial class InitialMigration
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.10")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

        modelBuilder.Entity("HospitalRegistrationSystem.Domain.Entities.Appointment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<int>("ClientId")
                    .HasColumnType("int");

                b.Property<string>("Diagnosis")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("DoctorId")
                    .HasColumnType("int");

                b.Property<bool>("IsVisited")
                    .HasColumnType("bit");

                b.Property<DateTime>("VisitTime")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("ClientId");

                b.HasIndex("DoctorId");

                b.ToTable("Appointments");
            });

        modelBuilder.Entity("HospitalRegistrationSystem.Domain.Entities.Client", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Gender")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnType("nvarchar(10)");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("MiddleName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Clients");
            });

        modelBuilder.Entity("HospitalRegistrationSystem.Domain.Entities.Doctor", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Gender")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnType("nvarchar(10)");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("MiddleName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Specialty")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("nvarchar(30)");

                b.HasKey("Id");

                b.ToTable("Doctors");
            });

        modelBuilder.Entity("HospitalRegistrationSystem.Domain.Entities.Appointment", b =>
            {
                b.HasOne("HospitalRegistrationSystem.Domain.Entities.Client", "Client")
                    .WithMany("Appointments")
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HospitalRegistrationSystem.Domain.Entities.Doctor", "Doctor")
                    .WithMany("Appointments")
                    .HasForeignKey("DoctorId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Client");

                b.Navigation("Doctor");
            });

        modelBuilder.Entity("HospitalRegistrationSystem.Domain.Entities.Client", b =>
            {
                b.Navigation("Appointments");
            });

        modelBuilder.Entity("HospitalRegistrationSystem.Domain.Entities.Doctor", b =>
            {
                b.Navigation("Appointments");
            });
#pragma warning restore 612, 618
    }
}
