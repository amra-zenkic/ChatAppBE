using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChatAppBE.Models
{
    [Collection("users")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Status { get; set; } = "online"; // or offline

        // list of user ids that the user has private chats with
        [BsonElement("privateChatUserIds")]
        public List<string> PrivateChatUserIds { get; set; } = new List<string>();
        
    }
}
