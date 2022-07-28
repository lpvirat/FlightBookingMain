﻿// <auto-generated />
using System;
using AdminServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdminServices.Migrations
{
    [DbContext(typeof(AdminDBContext))]
    partial class AdminDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AdminServices.Models.AdminCredentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("AdminCredentials");
                });

            modelBuilder.Entity("AdminServices.Models.AirlineAddBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AirlineId");

                    b.Property<string>("Airlinename");

                    b.HasKey("Id");

                    b.ToTable("AirlineaddBlock");
                });

            modelBuilder.Entity("AdminServices.Models.Coupons", b =>
                {
                    b.Property<string>("CouponName")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Discount");

                    b.HasKey("CouponName");

                    b.ToTable("Coupon");
                });

            modelBuilder.Entity("AdminServices.Models.Flights", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Bcseats");

                    b.Property<string>("Destination");

                    b.Property<DateTime?>("EndTime");

                    b.Property<string>("FlightId");

                    b.Property<string>("FlightLogo");

                    b.Property<string>("FlightName");

                    b.Property<string>("MealType");

                    b.Property<int>("Nbcseats");

                    b.Property<int>("Price");

                    b.Property<int>("ScheduledDays");

                    b.Property<string>("Source");

                    b.Property<DateTime?>("StartTime");

                    b.Property<string>("TypeOfTrip");

                    b.HasKey("Id");

                    b.ToTable("Flights");
                });
#pragma warning restore 612, 618
        }
    }
}
