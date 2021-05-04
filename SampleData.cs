using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace ApiPatternsClient
{
    public class SampleData
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id {get; set;}

        [BsonElement("Name")]
        public string Name {get; set;}

        [BsonElement("Date")]
        public DateTime Date {get; set;}

        //basic constructor
        public SampleData(string szName, DateTime dtDate)
        {
            Name = szName;
            Date = dtDate;
        }

    }
}