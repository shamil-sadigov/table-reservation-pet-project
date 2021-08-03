#region

using System;

#endregion

namespace Restaurants.Application
{
    public class Command
    {
        public Command(Guid id, string type, DateTime occuredOn)
        {
            Id = id;
            Type = type;
            OccuredOn = occuredOn;
        }

        public Guid Id { get; }
        public DateTime OccuredOn { get; }
        public string Type { get; set; }
    }
}