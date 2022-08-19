// See https://aka.ms/new-console-template for more information
using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBPractice
{
    public class Program
    {
        public static async Task Main()
        {
            var program = new Program();
            var client = new AmazonDynamoDBClient();
            DynamoDBContext context = new DynamoDBContext(client);
            
            var counter = new Counter
            {
                Key = Guid.NewGuid().ToString(),
                Value = 0
            };

            await context.SaveAsync(counter);

            var actions = new List<Action>();

            for (int i = 0; i < 100; i++)
            {
                var index = i;
                actions.Add(async () =>
                {   
                    await program.CasAsync(async () =>
                    {
                        var result = await context.LoadAsync<Counter>(counter.Key);
                        result.Value += 1;
                        await context.SaveAsync(result);
                    });
                });
            }

            Parallel.Invoke(actions.ToArray());
            
            while (true)
            {
                
            }
        }

        public async Task CasAsync(Func<Task> action)
        {
            var retry = 0;
            while (true)
            {
                try
                {
                    await action();
                    return;
                }
                
                catch (Exception e)
                {
                    if (retry >= 100)
                        throw;
                }

                var delay = ++retry;
                await Task.Delay(delay);
            }
        }
    }
}
