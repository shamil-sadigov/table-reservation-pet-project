using Microsoft.EntityFrameworkCore;

namespace Restaurants.Api.IntegrationTests.DataSeeders
{
    public interface IDataSeeder
    {
        void Seed(DbContext context);
    }
}