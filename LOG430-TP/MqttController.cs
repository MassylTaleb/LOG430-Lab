using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using System.Text.RegularExpressions;
using System.Collections;
using System.Windows;
using System.Threading.Tasks;

namespace LOG430_TP
{
    class MqttController
    {
        /// <summary>
        /// the client
        /// </summary>
        private IMqttClient client;

        /// <summary>
        /// the options
        /// </summary>
        private IMqttClientOptions options;

        /// <summary>
        /// broker address
        /// </summary>
        private const string broker = "mqtt.cgmu.io";

        /// <summary>
        /// broker address port
        /// </summary>
        private const int port = 1883;

        /// <summary>
        /// the base topic
        /// </summary>
        private const string BaseTopic = "worldcongress2017/pilot_resologi/";

        /// <summary>
        /// main view
        /// </summary>
        private MainViewModel mainViewModel;

        public MqttController(MainViewModel mainViewModel)
        {
            this.initiateComponents();
            this.InitiateHandlers();
            this.mainViewModel = mainViewModel;
        }

        /// <summary>
        /// initate components
        /// </summary>
        private void initiateComponents()
        {
            var factory = new MqttFactory();
            this.client = factory.CreateMqttClient();


            // Use TCP connection.
            this.options = new MqttClientOptionsBuilder()
               .WithTcpServer(broker, port) // Port is optional
                .Build();
        }

        /// <summary>
        /// initiate handlers
        /// </summary>
        private void InitiateHandlers()
        {

            this.InitiateConnectHandler();
            this.initiateMessageReceivedHandler();
        }


        private void initiateMessageReceivedHandler()
        {
            // add observer method to give payload info
            this.client.UseApplicationMessageReceivedHandler(e =>
            {
                mainViewModel.messageReceived(this.ApplicationMessageConverter(e));
            });
        }

        /// <summary>
        /// initiate connect handlers
        /// </summary>
        private void InitiateConnectHandler()
        {

            this.client.UseConnectedHandler(async e =>
            {
                // nothing to do when connnecting
            });
        }

        /// <summary>
        /// converts application message to a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private ApplicationMessage ApplicationMessageConverter(MqttApplicationMessageReceivedEventArgs message)
        {

            return new ApplicationMessage
            {
                Topic = message.ApplicationMessage.Topic,
                Payload = Encoding.UTF8.GetString(message.ApplicationMessage.Payload),
                QualityOfServiceLevel = (int)message.ApplicationMessage.QualityOfServiceLevel,
                Retain = message.ApplicationMessage.Retain

            };
        }

        /// <summary>
        /// connects to qtt.cgmu.io:1883
        /// </summary>
        public void connect()
        {
            Task connector = this.client.ConnectAsync(options, CancellationToken.None); // Since 3.0.5 with CancellationToken
            connector.Wait();
        }

        /// <summary>
        /// disconnect from the broker
        /// </summary>
        public async void disconnect()
        {
            await this.client.DisconnectAsync();
        }


        public void subscribe(string topic)
        {

            topic = topic.ToLower();

            var regexItem = new Regex("^[a-z0-9-+#]*$");

            //if (!regexItem.IsMatch(topic))
            //{
            //    Console.WriteLine("Bad Format in topic, forbidden character");
            //    return;
            //}

            // Subscribe to a topic
            this.client.SubscribeAsync(new TopicFilterBuilder().WithTopic(BaseTopic + topic).Build());
            Console.WriteLine("SUBSCRIBED to topic : " + BaseTopic + topic);

        }

        /// <summary>
        /// unsubscribres from a topic
        /// </summary>
        /// <param name="topic"></param>
        public void unsubscribe(string topic)
        {
            this.client.UnsubscribeAsync(BaseTopic + topic);
        }

    }
}
