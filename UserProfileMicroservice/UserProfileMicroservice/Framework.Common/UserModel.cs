using System;

namespace Framework.Common
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

    }

    public class QRModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class UserModellResponse : UserModel
    {
        public string Resource { get; set; }
        public Guid Id { get; set; }
        public double Latitude {get; set; }
        public double Longitude { get; set; }
    }
}
