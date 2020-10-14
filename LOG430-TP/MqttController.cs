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
    /*
     * Used this git repo to get inspiration: https://github.com/MassylTaleb/LOG430-Lab.git
     */
    public class MqttController
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
        private IApplicationMessageRepository _repository;

        public MqttController(MainViewModel mainViewModel)
        {
            this.initiateComponents();
            this.InitiateHandlers();
            this.mainViewModel = mainViewModel;
            _repository = new ApplicationMessageRepository();
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

            var appMessage = new ApplicationMessage
            {
                Topic = message.ApplicationMessage.Topic,
                Payload = Encoding.UTF8.GetString(message.ApplicationMessage.Payload),
                QualityOfServiceLevel = (int)message.ApplicationMessage.QualityOfServiceLevel,
                Retain = message.ApplicationMessage.Retain

            };

            _repository.Add(appMessage);
            return appMessage;
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


        /// <summary>
        /// subribes to a topic
        /// </summary>
        /// <param name="topic"></param>
        /// <returns>true if the subscription has been sent, false if the format was wrong</returns>
        public bool subscribe(string topic)
        {

            topic = topic.ToLower();
            var regexItem = new Regex("^[a-z0-9-+#_/]*$");

            if (regexItem.IsMatch(topic))
            {
                // Subscribe to a topic
                this.client.SubscribeAsync(new TopicFilterBuilder().WithTopic(BaseTopic + topic).Build());
                return true;
            }
            return false;
        }

        /// <summary>
        /// subscribe to all topics
        /// </summary>
        public void subscribeALL()
        {
            this.client.SubscribeAsync(new TopicFilterBuilder().WithTopic(BaseTopic + "#").Build());
        }

        /// <summary>
        /// unsubscire from the topic
        /// </summary>
        /// <param name="topic">the topic to unsubsccribe to</param>
        /// <returns>returns true on successful unsubscribe, else false</returns>
        public bool unsubscribe(string topic)
        {

            topic = topic.ToLower();
            var regexItem = new Regex("^[a-z0-9-+#_/]*$");

            if (regexItem.IsMatch(topic))
            {
                this.client.UnsubscribeAsync(BaseTopic + topic);
                return true;
            }
            return false;

        }


        /// <summary>
        /// unsubscribe from all topics
        /// </summary>
        public void unsubscribeALL()
        {

            this.client.UnsubscribeAsync(BaseTopic + "#");

        }
    }
}
