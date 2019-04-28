using Newtonsoft.Json;
using Queue.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RECEIVE.Helper;
using System;
using System.Text;

namespace RECEIVE
{
  
    class Program
    {
 
        static  void Main(string[] args)
        {
            EmailService _emailService = new EmailService();
            var factory = new ConnectionFactory() { HostName = "localhost" };
                using (IConnection connection = factory.CreateConnection())
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "OkanEmail",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Emails emails = JsonConvert.DeserializeObject<Emails>(message);

                        if( _emailService.SendEmail(emails))
                        {
                            Console.WriteLine($" Adı: {emails.To}  mesaj atıldı");

                        }
                        else
                        {
                            Console.WriteLine($" Adı: {emails.To}  mesaj atılamadı");
                        }
                    

                    };
                    channel.BasicConsume(queue: "OkanEmail",
                                         autoAck: true,
                                         consumer: consumer);
 
                    Console.ReadLine();
                
            }
        }
    }
}
