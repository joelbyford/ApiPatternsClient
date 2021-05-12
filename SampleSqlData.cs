using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiPatternsClient
{
    public class SampleSqlData
    {
        public Guid id {get; set;}

        public string Name {get; set;}

        public DateTime Date {get; set;}

        //basic constructor
        public SampleSqlData(string szName, DateTime dtDate)
        {
            Name = szName;
            Date = dtDate;
            id = Guid.NewGuid();
        }
        // The ToString() method is used to format the output, it's used for demo purpose only. 
        // It's not required by Azure Cosmos DB
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }
}