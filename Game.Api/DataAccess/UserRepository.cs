using Game.Api.DataAccess.Models.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Game.Api.DataAccess
{
    public class UserRepository
    {
        private static List<User> users = new List<User>
        {
            //password = 123
            new User { Id = 1, UserName = "sasha", Password = "@�\0\u0015c\b_�Qe2���\\^�۾�", Role = new Role{ Name = Role.UserRoleName } },
            new User { Id = 2, UserName = "dima", Password = "@�\0\u0015c\b_�Qe2���\\^�۾�", Role = new Role{ Name = Role.UserRoleName } }
        };

        public User GetById(long id)
        {
            return users.FirstOrDefault(it => it.Id == id);
        }

        public User GetByName(string name)
        {
            return users.FirstOrDefault(it => it.UserName == name);
        }

        public long Add(User user)
        {
            var id = users.Select(it => it.Id).Max() + 1;
            user.Id = id;
            users.Add(user);
            return id;
        }

        public void Update(User user)
        {
        }
    }
}
