﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PiServer.Context;

namespace PiServer.Migrations
{
    [DbContext(typeof(PiDbContext))]
    partial class PiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("PiServer.Models.Szenzor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("IP")
                        .HasColumnType("text");

                    b.Property<long>("RemoteId")
                        .HasColumnType("bigint");

                    b.Property<int>("Tipus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Szenzorok");
                });
#pragma warning restore 612, 618
        }
    }
}
