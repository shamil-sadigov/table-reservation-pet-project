#region

using System;

#endregion

namespace Restaurants.Api.Options
{
    public class IdentityServiceOptions
    {
        public string Url { get; set; }

        public IdentityServiceOptions EnsureValid()
        {
            if (!Uri.IsWellFormedUriString(Url, UriKind.Absolute))
                throw new Exception("Uri of Identity Service should be absolute url");

            return this;
        }
    }
}