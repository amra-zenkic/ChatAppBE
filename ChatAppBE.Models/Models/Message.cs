using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;

namespace ChatAppBE.Models.Models
{
    [Collection("messages")]
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
