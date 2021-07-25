using System;

namespace Reservation.Application
{
    public class Command
    {
        public Guid Id { get; }
        public DateTime OccuredOn { get; }
        public string Type { get; set; }
        
        public Command(Guid id, string type, DateTime occuredOn)
        {
            Id = id;
            Type = type;
            OccuredOn = occuredOn;
        }
    }
}