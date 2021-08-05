using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.Reservations;

namespace Reservations.Infrastructure.Configurations
{
    public static class ConversionExtensions
    {
        public static void WithConversion(this PropertyBuilder<RejectionReason> builder) 
            => builder.HasConversion(x => x.Value, str => RejectionReason.TryCreate(str).Value!);
        
        public static void WithConversion(this PropertyBuilder<AdministratorId> builder) 
            => builder.HasConversion(x => x.Value, guid => new AdministratorId(guid));
        
        public static void WithConversion(this PropertyBuilder<ReservationId> builder) 
            => builder.HasConversion(x => x.Value, guid => new ReservationId(guid));
        
        public static void WithConversion(this PropertyBuilder<RestaurantId> builder) 
            => builder.HasConversion(x => x.Value, guid => new RestaurantId(guid));
        
        public static void WithConversion(this PropertyBuilder<ReservationRequestId> builder) 
            => builder.HasConversion(x => x.Value, guid => new ReservationRequestId(guid));

        public static void WithConversion(this PropertyBuilder<TableId> builder) 
            => builder.HasConversion(x => x.Value, str => TableId.TryCreate(str).Value!);
        
        public static void WithUtcConversion(this PropertyBuilder<DateTime> builder) 
            => builder.HasConversion(dt => dt, dt => new DateTime(dt.Ticks, DateTimeKind.Utc));
        
        public static void WithUtcConversion(this PropertyBuilder<DateTime?> builder) 
            => builder.HasConversion(dt => dt, dt => WithUtcConversionInternal(dt));

        private static DateTime? WithUtcConversionInternal(DateTime? dt)
        {
            if (dt.HasValue)
                return new DateTime(dt.Value.Ticks, DateTimeKind.Utc);

            return null;
        }
    }
}