using System;

namespace Framework.Common
{
    public class LoggedInUser
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
