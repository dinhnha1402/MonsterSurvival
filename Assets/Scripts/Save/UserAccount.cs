using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Save
{
    public class UserAccount : RealmObject
    {
        [PrimaryKey]
        public string Username { get; set; }

        public string Password { get; set; }
        // Giả định Password đã được mã hóa trước khi lưu trữ
    }
}
