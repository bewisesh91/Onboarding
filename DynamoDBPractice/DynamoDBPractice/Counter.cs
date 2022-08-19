using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDBPractice
{
    [DynamoDBTable("music")]
    public class Counter
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("key")]
        public string Key { get; set; }

        [DynamoDBProperty("value")]
        public int Value { get; set; }

        [DynamoDBVersion]
        public int? VersinoNumber { get; set; }
    }
}
