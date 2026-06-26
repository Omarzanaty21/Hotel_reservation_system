using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelReservation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationExclusiveConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE EXTENSION IF NOT EXISTS btree_gist;
            """);

            migrationBuilder.Sql("""
                ALTER TABLE "Reservations"
                ADD CONSTRAINT no_overlapping_reservations
                EXCLUDE USING gist
                (
                    "RoomId" WITH =,
                    daterange("CheckIn", "CheckOut", '[)') WITH &&
                );
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
            ALTER TABLE "Reservations"
            DROP CONSTRAINT IF EXISTS no_overlapping_reservations;
            """);
        }
    }
}
