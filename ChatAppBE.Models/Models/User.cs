namespace ChatAppBE.Models.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.EntityFrameworkCore;

    [Collection("users")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string? Username { get; set; }

        public string Status { get; set; } = "online";

        [BsonElement("privateChatUserIds")]
        public List<string> PrivateChatUserIds { get; set; } = [];
    }
}
