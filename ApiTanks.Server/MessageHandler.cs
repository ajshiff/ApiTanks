using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using ApiTanks.GameObjects.Models.Messages;
using ApiTanks.GameObjects;
using ApiTanks;

namespace ApiTanks.Server
{

    class MessageHandler
    {
        public static Stopwatch Timer = new Stopwatch();
        public static List<RequestSpawnTankMessage> SpawnTankRequests = new List<RequestSpawnTankMessage>();
        public static List<RequestSpawnBulletMessage> SpawnBulletRequests = new List<RequestSpawnBulletMessage>();
        public static List<RequestMoveTankMessage> MoveTankRequests = new List<RequestMoveTankMessage>();
        private static EventLoop GameLoop = new EventLoop();
        public async static Task Main()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                // Standard Messaging
                channel.QueueDeclare(queue: "hello",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                // Incoming Requests
                channel.QueueDeclare(queue: "RequestSpawnTank",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "RequestFireBullet",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "RequestMoveTank",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                // Outgoing Messages
                channel.QueueDeclare(queue: "ServerSpawnTank",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "ServerSpawnBullet",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "ServerMoveTank",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "ServerMoveBullet",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "ServerDestroyTank",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "ServerDestroyBullet",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "ServerUpdateScore",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueDeclare(queue: "ServerUpdateGameloop",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                // Incoming / Consumer Processes
                var consumerHello = new EventingBasicConsumer(channel);
                var consumerRequestSpawnTank = new EventingBasicConsumer(channel);
                var consumerRequestFireBullet= new EventingBasicConsumer(channel);
                var consumerRequestMoveTank = new EventingBasicConsumer(channel);
                consumerHello.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                consumerRequestSpawnTank.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var requestObject = JsonConvert.DeserializeObject<RequestSpawnTankMessage>(message);
                    SpawnTankRequests.Add(requestObject);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                consumerRequestFireBullet.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var requestObject = JsonConvert.DeserializeObject<RequestSpawnBulletMessage>(message);
                    SpawnBulletRequests.Add(requestObject);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                consumerRequestMoveTank.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var requestObject = JsonConvert.DeserializeObject<RequestMoveTankMessage>(message);
                    MoveTankRequests.Add(requestObject);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "hello",
                                    autoAck: true,
                                    consumer: consumerHello);
                channel.BasicConsume(queue: "RequestSpawnTank",
                                    autoAck: true,
                                    consumer: consumerHello);
                channel.BasicConsume(queue: "RequestFireBullet",
                                    autoAck: true,
                                    consumer: consumerHello);
                channel.BasicConsume(queue: "RequestMoveTank",
                                    autoAck: true,
                                    consumer: consumerHello);

                // Outgoing Processes
                while (Timer.ElapsedMilliseconds < 360000)
                {
                    // Prep Data For Gameloop
                    var spawnTankRequests = SpawnTankRequests.Aggregate(new List<RequestSpawnTankMessage>(),
                        (acc, request) => {
                            return default;
                        }).ToList();
                    SpawnTankRequests.Clear();

                    var spawnBulletRequests = SpawnBulletRequests.Aggregate(new List<RequestSpawnBulletMessage>(),
                        (acc, request) => {
                            return default;
                        }).ToList();
                    SpawnBulletRequests.Clear();

                    var moveTankRequests = MoveTankRequests.Aggregate(new List<RequestMoveTankMessage>(),
                        (acc, request) => {
                            return default;
                        }).ToList();
                    MoveTankRequests.Clear();

                    // Game Loop
                    var serverMessages = GameLoop.RunLoop(
                        spawnTankRequests,
                        spawnBulletRequests,
                        MoveTankRequests
                    );
                    // Send Server Messages
                    SendServerMessages(serverMessages);
                    // Chill
                    await Task.Delay(20);
                }

                // Send Server Messages
                void SendServerMessages(MessagesPacket serverMessages)
                {
                    foreach (var message in serverMessages.ServerSpawnTankMessages)
                    {
                        channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: "ServerSpawnTank",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(message.GetMessageAsJToken().ToString())
                        );
                    }
                    foreach (var message in serverMessages.ServerSpawnBulletMessages)
                    {
                        channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: "ServerSpawnBullet",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(message.GetMessageAsJToken().ToString())
                        );
                    }
                    foreach (var message in serverMessages.ServerMoveTankMessages)
                    {
                        channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: "ServerMoveTank",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(message.GetMessageAsJToken().ToString())
                        );
                    }
                    foreach (var message in serverMessages.ServerMoveBulletMessages)
                    {
                        channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: "ServerMoveBullet",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(message.GetMessageAsJToken().ToString())
                        );
                    }
                    foreach (var message in serverMessages.ServerDestroyTankMessages)
                    {
                        channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: "ServerDestroyTank",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(message.GetMessageAsJToken().ToString())
                        );
                    }
                    foreach (var message in serverMessages.ServerDestroyBulletMessages)
                    {
                        channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: "ServerDestroyBullet",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(message.GetMessageAsJToken().ToString())
                        );
                    }
                    foreach (var message in serverMessages.UpdateScoreMessages)
                    {
                        channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: "ServerUpdateScore",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(message.GetMessageAsJToken().ToString())
                        );
                    }
                    channel.BasicPublish
                        (
                            exchange: "",
                            routingKey: "ServerUpdateScore",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(
                                serverMessages.OfficialGamestateUpdateMessage
                                    .GetMessageAsJToken()
                                    .ToString())
                        );
                }
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}

