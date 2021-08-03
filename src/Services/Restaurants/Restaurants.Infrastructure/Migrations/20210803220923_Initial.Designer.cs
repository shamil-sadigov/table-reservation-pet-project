﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurants.Infrastructure.Contexts;

namespace Restaurants.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210803220923_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Restaurants.Domain.Restaurants.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Restaurants", "restaurants");
                });

            modelBuilder.Entity("Restaurants.Domain.Tables.Table", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("_restaurantId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RestaurantId");

                    b.Property<string>("_state")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("State");

                    b.HasKey("Id");

                    b.HasIndex("_restaurantId");

                    b.ToTable("Tables", "restaurants");
                });

            modelBuilder.Entity("Restaurants.Domain.Visitors.Visitor", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Visitors", "restaurants");
                });

            modelBuilder.Entity("Restaurants.Domain.Restaurants.Restaurant", b =>
                {
                    b.OwnsOne("Restaurants.Domain.Restaurants.ValueObjects.RestaurantAddress", "_address", b1 =>
                        {
                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasColumnName("Address");

                            b1.HasKey("RestaurantId");

                            b1.ToTable("Restaurants");

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.OwnsOne("Restaurants.Domain.Restaurants.ValueObjects.RestaurantName", "_name", b1 =>
                        {
                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasColumnName("Name");

                            b1.HasKey("RestaurantId");

                            b1.ToTable("Restaurants");

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.OwnsOne("Restaurants.Domain.Restaurants.ValueObjects.RestaurantWorkingHours", "_workingHours", b1 =>
                        {
                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<TimeSpan>("FinishTime")
                                .HasPrecision(0)
                                .HasColumnType("time")
                                .HasColumnName("FinishWorkingAt");

                            b1.Property<TimeSpan>("StartTime")
                                .HasPrecision(0)
                                .HasColumnType("time")
                                .HasColumnName("StartWorkingAt");

                            b1.HasKey("RestaurantId");

                            b1.ToTable("Restaurants");

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.Navigation("_address")
                        .IsRequired();

                    b.Navigation("_name")
                        .IsRequired();

                    b.Navigation("_workingHours")
                        .IsRequired();
                });

            modelBuilder.Entity("Restaurants.Domain.Tables.Table", b =>
                {
                    b.HasOne("Restaurants.Domain.Restaurants.Restaurant", null)
                        .WithMany("_tables")
                        .HasForeignKey("_restaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Restaurants.Domain.Tables.ValueObjects.NumberOfSeats", "NumberOfSeats", b1 =>
                        {
                            b1.Property<string>("TableId")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<byte>("Value")
                                .HasColumnType("tinyint")
                                .HasColumnName("NumberOfSeats");

                            b1.HasKey("TableId");

                            b1.ToTable("Tables");

                            b1.WithOwner()
                                .HasForeignKey("TableId");
                        });

                    b.Navigation("NumberOfSeats")
                        .IsRequired();
                });

            modelBuilder.Entity("Restaurants.Domain.Restaurants.Restaurant", b =>
                {
                    b.Navigation("_tables");
                });
#pragma warning restore 612, 618
        }
    }
}
