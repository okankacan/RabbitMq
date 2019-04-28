using Newtonsoft.Json;
using Queue.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Queue
{
    class Program
    {

       static List<Emails> _email = new List<Emails>()
        {
            new Emails{
                 To="a@dizayn34.com",
                 Subject="Example a Subject",
                  Content="Example a Content"
            },
             new Emails{
                 To="a2@dizayn34.com",
                 Subject="Example a Subject",
                  Content="Example a Content"
            },
              new Emails{
                 To="a3@dizayn34.com",
                 Subject="Example a Subject",
                 Content="Example a Content"
            }
        };

        static void Main(string[] args)
        {
            if(_email.Any())
            {

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "OkanEmail",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    foreach (var item in _email)
                    {
                        string message = JsonConvert.SerializeObject(item);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "OkanEmail",
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($"Gönderilen kişi: {item.To}");
                        Console.WriteLine(" İlgili kişi gönderildi...");
                    
                    }
                    Console.ReadLine();
                }

              

              
            }
        }
    }
}
