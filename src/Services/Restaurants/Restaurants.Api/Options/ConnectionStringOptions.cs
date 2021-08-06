using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Api.Options
{
    public class ConnectionStringOptions:IValidatableObject
    {
        public string Default { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Default.Contains("Data Source"))
                yield return new ValidationResult("ConnectionString should contain Data Source");
            
            if (!Default.Contains("Initial Catalog"))
                yield return new ValidationResult("ConnectionString should contain Initial Catalog");
        }
    }
}