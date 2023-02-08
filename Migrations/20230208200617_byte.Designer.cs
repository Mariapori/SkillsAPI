﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillsAPI.Data;

#nullable disable

namespace SkillsAPI.Migrations
{
    [DbContext(typeof(TietokantaContext))]
    [Migration("20230208200617_byte")]
    partial class @byte
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SkillsAPI.Data.Kayttaja", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Kayttajanimi")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Salasana")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.HasKey("Id");

                    b.ToTable("Kayttajat");
                });
#pragma warning restore 612, 618
        }
    }
}
