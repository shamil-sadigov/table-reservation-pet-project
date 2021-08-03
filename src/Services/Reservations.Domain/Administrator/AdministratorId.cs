#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservations.Domain.Administrator
{
    public sealed class AdministratorId : GuidIdentity
    {
        public AdministratorId(Guid id) : base(id)
        {
        }
    }
    
    
    
    
}