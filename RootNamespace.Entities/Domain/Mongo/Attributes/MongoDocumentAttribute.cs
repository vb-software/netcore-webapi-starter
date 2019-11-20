using System;
using System.Reflection;

namespace RootNamespace.Entities.Domain.Mongo.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MongoDocumentAttribute : Attribute
    {
        private readonly string _documentName;

        public MongoDocumentAttribute(string documentName)
        {
            _documentName = documentName;
        }

        public string DocumentName => _documentName;

        public static string GetDocumentName<T>()
        {
            return GetDocumentName(typeof(T));
        }

        public static string GetDocumentName(Type type)
        {
            var documentAttribute = type.GetTypeInfo().GetCustomAttribute(typeof(MongoDocumentAttribute), true) as MongoDocumentAttribute;
            return documentAttribute == null ? type.Name : documentAttribute.DocumentName;
        }
    }
}