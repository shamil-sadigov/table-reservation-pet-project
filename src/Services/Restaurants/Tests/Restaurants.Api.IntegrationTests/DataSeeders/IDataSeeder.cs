#region

using Microsoft.EntityFrameworkCore;

#endregion

namespace Restaurants.Api.IntegrationTests.DataSeeders
{
    public interface IDataSeeder
    {
        void Seed(DbContext context);
    }
}