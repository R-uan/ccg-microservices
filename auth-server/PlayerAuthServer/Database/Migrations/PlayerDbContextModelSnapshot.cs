﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PlayerAuthServer.Database;

#nullable disable

namespace PlayerAuthServer.Database.Migrations
{
    [ExcludeFromCodeCoverage]
    [DbContext(typeof(PlayerDbContext))]
    partial class PlayerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PlayerAuthServer.Entities.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Experience")
                        .HasColumnType("integer");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Level")
                        .HasColumnType("integer");

                    b.Property<int>("Losses")
                        .HasColumnType("integer");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Wins")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("players", (string)null);
                });

            modelBuilder.Entity("PlayerAuthServer.Models.CardCollection", b =>
                {
                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uuid");

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<Guid?>("PlayerGuid")
                        .HasColumnType("uuid");

                    b.HasKey("PlayerId", "CardId");

                    b.ToTable("card_collection", (string)null);
                });

            modelBuilder.Entity("PlayerAuthServer.Models.CardCollection", b =>
                {
                    b.HasOne("PlayerAuthServer.Entities.Player", "Player")
                        .WithMany("CardCollection")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("PlayerAuthServer.Entities.Player", b =>
                {
                    b.Navigation("CardCollection");
                });
#pragma warning restore 612, 618
        }
    }
}
