using StackExchange.Redis;

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
IDatabase db = redis.GetDatabase(1);

var subscriber = redis.GetSubscriber();

subscriber.Subscribe("subscribeTest", (channel, message) =>
{
    Console.WriteLine((string)message);
});

subscriber.Publish("subscribeTest", "This is a subscribe test message.");

Console.ReadLine();



//namespace RedisPractice
//{
//    public class Program
//    {
//        public static async Task Main()
//        {
//            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
//            IDatabase db = redis.GetDatabase(1);

//            await db.StringSetAsync("count", 0);

//            var actions = new List<Action>();

//            for (int i = 0; i < 100; i++)
//            {
//                actions.Add(async () =>
//                {
//                    Program.CasAsync(() =>
//                    {
//                        var transaction = db.CreateTransaction();
//                        var result = db.StringGet("count");
//                        transaction.AddCondition(Condition.StringEqual("count", result));
//                        transaction.StringSetAsync("count", int.Parse(result) + 1);
//                        return transaction.Execute();
//                    });
//                });
//            }

//            Parallel.Invoke(actions.ToArray());
//            Thread.Sleep(5000);

//            var result = db.StringGet("count");
//            Console.WriteLine(result);
//        }

//        public static void CasAsync(Func<bool> action)
//        {
//            var retry = 0;
//            while (true)
//            {
//                try
//                {
//                    var result = action();

//                    if (result)
//                    {
//                        return;
//                    }
//                    else
//                    {
//                        if (retry >= 100)
//                        {
//                            throw new Exception("Cas Exception!");
//                        }
//                    }

//                    var delay = ++retry;
//                    Thread.Sleep(delay);
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                }
//            }
//        }
//    }
//}

////var result = db.StringGet("count");
////db.StringSet("count", int.Parse(result) + 1);
////transaction.AddCondition(Condition.StringEqual("count", 1 ));
////db.StringSetAsync("count", 3);
////transaction.StringSetAsync("count", 2);
////transaction.Execute();