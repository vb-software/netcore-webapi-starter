using System;
using RootNamespace.Entities.Domain.Mongo.Attributes;

namespace RootNamespace.Entities.Domain.Mongo
{
    [Serializable]
    [MongoDocument("users")]
    public class User : MongoIdentity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}