using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDBPractice
{
    [DynamoDBTable("movie_table")]
    public class Movie
    {
        [DynamoDBHashKey]
        [DynamoDBProperty("year")]
        public string Year { get; set; }

        [DynamoDBRangeKey]
        [DynamoDBProperty("title")]
        public string Title { get; set; }

        [DynamoDBProperty("info")]
        public int info { get; set; }
    }
}
