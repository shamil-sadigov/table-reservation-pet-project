﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reservation.Infrastructure.Databass.Contexts;

namespace Reservation.Infrastructure.Databass.Migrations
{
    [DbContext(typeof(ReservationContext))]
    partial class ReservationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Reservation.Domain.ReservationRequests.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("_approvedByAdministratorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ApprovedByAdministratorId");

                    b.Property<DateTime>("_approvedDateTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("ApprovedDateTime");

                    b.Property<Guid>("_reservationRequestId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ReservationRequestId");

                    b.HasKey("Id");

                    b.HasIndex("_reservationRequestId")
                        .IsUnique();

                    b.ToTable("Reservations", "reservation");
                });

            modelBuilder.Entity("Reservation.Domain.ReservationRequests.ReservationRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("_createdDateTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("CreatedDateTime");

                    b.Property<byte>("_numberOfRequestedSeats")
                        .HasColumnType("tinyint")
                        .HasColumnName("NumberOfRequestedSeats");

                    b.Property<string>("_state")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("State");

                    b.Property<Guid>("_tableId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("TableId");

                    b.Property<DateTime>("_visitingDateTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("VisitingDateTime");

                    b.Property<Guid>("_visitorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("VisitorId");

                    b.HasKey("Id");

                    b.HasIndex("_tableId");

                    b.HasIndex("_visitorId");

                    b.ToTable("ReservationRequests", "reservation");
                });

            modelBuilder.Entity("Reservation.Domain.ReservationRequests.ReservationRequestRejection", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("_reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Reason");

                    b.Property<Guid>("_rejectedByAdministratorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RejectedByAdministratorId");

                    b.Property<DateTime>("_rejectionDateTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("RejectionDateTime");

                    b.Property<Guid>("_reservationRequestId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ReservationRequestId");

                    b.HasKey("Id");

                    b.HasIndex("_reservationRequestId")
                        .IsUnique();

                    b.ToTable("ReservationRequestRejections", "reservation");
                });

            modelBuilder.Entity("Reservation.Domain.Restaurants.Restaurant", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.ToTable("Restaurants", "reservation");
                });

            modelBuilder.Entity("Reservation.Domain.Tables.Table", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("NumberOfSeats")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("_restaurantId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RestaurantId");

                    b.Property<string>("_state")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("State");

                    b.HasKey("Id");

                    b.HasIndex("_restaurantId");

                    b.ToTable("Tables", "reservation");
                });

            modelBuilder.Entity("Reservation.Domain.Visitors.Visitor", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Visitors", "reservation");
                });

            modelBuilder.Entity("Reservation.Domain.ReservationRequests.Reservation", b =>
                {
                    b.HasOne("Reservation.Domain.ReservationRequests.ReservationRequest", null)
                        .WithOne()
                        .HasForeignKey("Reservation.Domain.ReservationRequests.Reservation", "_reservationRequestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Reservation.Domain.ReservationRequests.ReservationRequest", b =>
                {
                    b.HasOne("Reservation.Domain.Tables.Table", null)
                        .WithMany()
                        .HasForeignKey("_tableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Reservation.Domain.Visitors.Visitor", null)
                        .WithMany()
                        .HasForeignKey("_visitorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Reservation.Domain.ReservationRequests.ReservationRequestRejection", b =>
                {
                    b.HasOne("Reservation.Domain.ReservationRequests.ReservationRequest", null)
                        .WithOne()
                        .HasForeignKey("Reservation.Domain.ReservationRequests.ReservationRequestRejection", "_reservationRequestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Reservation.Domain.Restaurants.Restaurant", b =>
                {
                    b.OwnsOne("Reservation.Domain.Restaurants.ValueObjects.RestaurantAddress", "_address", b1 =>
                        {
                            b1.Property<Guid>("RestaurantId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Address");

                            b1.HasKey("RestaurantId");

                            b1.ToTable("Restaurants");

                            b1.WithOwner()
                                .HasForeignKey("RestaurantId");
                        });

                    b.OwnsOne("Reservation.Domain.Restaurants.ValueObjects.RestaurantWorkingHours", "_workingHours", b1 =>
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

                    b.Navigation("_workingHours")
                        .IsRequired();
                });

            modelBuilder.Entity("Reservation.Domain.Tables.Table", b =>
                {
                    b.HasOne("Reservation.Domain.Restaurants.Restaurant", null)
                        .WithMany("_tables")
                        .HasForeignKey("_restaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Reservation.Domain.Restaurants.Restaurant", b =>
                {
                    b.Navigation("_tables");
                });
#pragma warning restore 612, 618
        }
    }
}
