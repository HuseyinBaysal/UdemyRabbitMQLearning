using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            //RabbitMQ ile bağlantı kurulmalı
            //Connection factori oluşturulur
            var factory = new ConnectionFactory();
            //Gerçek uygulamalarda bu Uri AppSettingJson içinde tutulur
            factory.Uri = new Uri("amqps://mdqxnrpk:bKVsEpaaMzJNGAvfuXKvfvFL4nzVNEL_@toad.rmq.cloudamqp.com/mdqxnrpk");

            using var connection = factory.CreateConnection();
            //Bir kanal oluşturulur.
            var channel = connection.CreateModel();
            //RabbitMQ ile kanal üzerinden haberleşilir.
            //Channel üzerinden birde kuyruk oluşturulur.

            channel.QueueDeclare("hello-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x => 
            {
                string message = $"Mesaj {x}";

                //Bütün veriler byte dizisine çevrilir.
                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

                Console.WriteLine($"Mesaj Gönderilmiştir : {message}");
            });

            //Queue methodu içindeki durable property'si, durable false olursa, rabbitmq de oluşan kuyruklar Memory'de tutulur, eğer rabbitmq restrtlanırsa tüm kuyruk gidilir. Eğer durable true olursa, rabbitmq de oluşan kuyruklar storage'de tutulur rabbitmq restartlanırsa bile gitmez ve sırayla devam eder.

            //exclusive property'si, bu property true olursa, bu kuyruğa sadece bu kanaldan erişilebilir. eğer false olursa subscribe tarafında başka bir kanaldan erişilebilir.

            //autoDelete property'si bu property true olursa rabbitmq down olursa kuyruktaki herşey silinir. false olursa rabbitmq down olursa kuyruk silinmez yine de beklemeye devam eder.

            

            Console.ReadKey();
        }
    }
}
