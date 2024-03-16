using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Save
{
    public class UserAccount
    {
        [BsonId] // Đánh dấu trường này là ID duy nhất của document.
        public ObjectId _id { get; set; }

        [BsonElement("username")] // Tùy chọn này chỉ rõ tên trường khi được lưu trong MongoDB.
        public string Username { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
