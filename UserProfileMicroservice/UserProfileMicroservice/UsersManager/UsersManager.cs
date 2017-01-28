using System.Collections.Generic;
using Framework.Common;
using UsersDBUpdate;

namespace UsersManager
{
    public class UsersManager
    {
        public UsersManager()
        {
            ManagerUpdateDb = new ManagerUpdateDb();
        }

        public ManagerUpdateDb ManagerUpdateDb { get; set; }


        public UserModellResponse AddUser(UserModel user)
        {
           return ManagerUpdateDb.AddUser(user);

        }

        public List<UserModellResponse> GetAll()
        {
            return ManagerUpdateDb.GetAll();
        }
    }
}