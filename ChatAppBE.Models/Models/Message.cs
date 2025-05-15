namespace ChatAppBE.Models.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.EntityFrameworkCore;

    [Collection("messages")]
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // TODO: ID shoud never be null

        public string? Sender { get; set; }

        public string? Receiver { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
