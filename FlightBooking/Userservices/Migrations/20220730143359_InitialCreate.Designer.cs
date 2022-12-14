// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Userservices.Models;

namespace Userservices.Migrations
{
    [DbContext(typeof(UserDBContext))]
    [Migration("20220730143359_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Userservices.Models.TicketBooking", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<string>("AirlineId");

                    b.Property<string>("AirlineName");

                    b.Property<string>("BoardingTime");

                    b.Property<string>("CouponApplied");

                    b.Property<string>("Destination");

                    b.Property<string>("EmailId");

                    b.Property<int>("FinalPrice");

                    b.Property<string>("Gender");

                    b.Property<int>("IsCancelled");

                    b.Property<string>("MealType");

                    b.Property<string>("Pnr");

                    b.Property<string>("SeatNumbers");

                    b.Property<string>("Source");

                    b.Property<string>("UserName");

                    b.HasKey("UserId");

                    b.ToTable("TicketBooking");
                });

            modelBuilder.Entity("Userservices.Models.UserCredentials", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int>("Age");

                    b.Property<long>("ContactNo");

                    b.Property<string>("Email");

                    b.Property<string>("Gender");

                    b.Property<string>("Password");

                    b.Property<string>("UserName");

                    b.HasKey("UserId");

                    b.ToTable("UserCredentials");
                });
#pragma warning restore 612, 618
        }
    }
}
