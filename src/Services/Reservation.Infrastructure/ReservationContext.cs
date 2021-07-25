﻿using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Restaurants;

namespace Reservation.Infrastructure.Databass
{
    public class ReservationContext:DbContext
    {
        // Uncomment when application layer is added
        public ReservationContext(DbContextOptions<ReservationContext> ops)
            :base(ops)
        {
            
        }

        // FOR migrations do the following
        // add default ctor
        // uncomment useSQLServer in OnConfiguring
        
        public DbSet<Restaurant> Restaurants { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Extract to settings
            // Uncomment if you want to add migrations
            // optionsBuilder.UseSqlServer("Data Source=darwin;Initial Catalog=ReservationApp;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}