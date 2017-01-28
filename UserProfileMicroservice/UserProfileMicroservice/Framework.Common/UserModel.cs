using System;
using System.Collections.Generic;

namespace Framework.Common
{
    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }

    }

    public class UserModellResponse : UserModel
    {
        public string Resource { get; set; }
        public Guid Id { get; set; }
        public List<UserModellResponse> Friends { get; set; } = new List<UserModellResponse>();
    }
}
