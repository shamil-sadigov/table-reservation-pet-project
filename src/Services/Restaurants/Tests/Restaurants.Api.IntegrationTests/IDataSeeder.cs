using Microsoft.EntityFrameworkCore;
using Restaurants.Infrastructure.Contexts;

namespace Restaurants.Api.IntegrationTests
{
    public interface IDataSeeder
    {
        void Seed(DbContext context);
    }
}