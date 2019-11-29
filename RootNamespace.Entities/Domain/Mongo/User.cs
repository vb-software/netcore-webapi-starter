using System;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using RootNamespace.Entities.Domain.Mongo.Attributes;

namespace RootNamespace.Entities.Domain.Mongo
{
    [Serializable]
    [MongoDocument("users")]
    public class User : MongoIdentity
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        [BsonIgnoreIfDefault]
        public string Email { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        [BsonIgnoreIfDefault]
        public DateTime DateOfBirth { get; set; }
    }
}