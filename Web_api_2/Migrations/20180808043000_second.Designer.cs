﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web_api_2.Models;

namespace Web_api_2.Migrations
{
    [DbContext(typeof(NotesContext))]
    [Migration("20180808043000_second")]
    partial class second
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Web_api_2.Models.CheckList", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ListOne");

                    b.Property<string>("ListTwo");

                    b.Property<int?>("NoteID");

                    b.HasKey("ID");

                    b.HasIndex("NoteID");

                    b.ToTable("CheckList");
                });

            modelBuilder.Entity("Web_api_2.Models.Labels", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NoteID");

                    b.Property<string>("Text");

                    b.HasKey("ID");

                    b.HasIndex("NoteID");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("Web_api_2.Models.Note", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Pin");

                    b.Property<string>("PlainText");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Note");
                });

            modelBuilder.Entity("Web_api_2.Models.CheckList", b =>
                {
                    b.HasOne("Web_api_2.Models.Note")
                        .WithMany("CList")
                        .HasForeignKey("NoteID");
                });

            modelBuilder.Entity("Web_api_2.Models.Labels", b =>
                {
                    b.HasOne("Web_api_2.Models.Note")
                        .WithMany("Labels")
                        .HasForeignKey("NoteID");
                });
#pragma warning restore 612, 618
        }
    }
}
