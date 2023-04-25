using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://mdqxnrpk:bKVsEpaaMzJNGAvfuXKvfvFL4nzVNEL_@toad.rmq.cloudamqp.com/mdqxnrpk");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //channel.QueueDeclare("hello-queue", true, false, false);

            channel.BasicQos(0, 1,false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume("hello-queue",false,consumer);



            //consumer.Received += Consumer_Received;
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen Mesaj : " + message);
                //BasicAckno
                channel.BasicAck(e.DeliveryTag,false);
            };


            Console.ReadKey();
        }

        //private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
