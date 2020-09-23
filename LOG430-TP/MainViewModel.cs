using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace LOG430_TP
{
    public class MainViewModel
    {
        private IMqttClient _MqttClient;
        private IMqttClientOptions _MqttClientOptions;
        public ObservableCollection<ApplicationMessage> Messages { get; set; }

        public MainViewModel()
        {
            // Create a new MQTT client.
            var factory = new MqttFactory();
            _MqttClient = factory.CreateMqttClient();
            Messages = new ObservableCollection<ApplicationMessage>();
            // Use TCP connection.
            _MqttClientOptions = new MqttClientOptionsBuilder()
               .WithTcpServer("mqtt.cgmu.io", 1883) // Port is optional
                .Build();

            _MqttClient.UseConnectedHandler(OnConnectedFromServer);
            _MqttClient.UseDisconnectedHandler(OnDisconnectedFromServer);
            _MqttClient.UseApplicationMessageReceivedHandler(OnMessageReceived);

            ConnectAsync();

            Console.WriteLine(_MqttClientOptions.ClientId);

            Console.ReadLine();
        }

        private async Task ConnectAsync()
        {
            await _MqttClient.ConnectAsync(_MqttClientOptions, CancellationToken.None); // Since 3.0.5 with CancellationToken
        }

        private async void OnConnectedFromServer(MqttClientConnectedEventArgs e)
        {
            Console.WriteLine("### CONNECTED WITH SERVER ###");

            // Subscribe to a topic
            await _MqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("worldcongress2017/pilot_resologi/odtf1/ca/qc/mtl/mobil/infra/gateway/ipc0/gat-00000-01/heartbeat").Build());
            Console.WriteLine("### SUBSCRIBED ###");
        }

        private async void OnDisconnectedFromServer(MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine("### DISCONNECTED FROM SERVER ###");
            await Task.Delay(TimeSpan.FromSeconds(5));

            try
            {
                await _MqttClient.ConnectAsync(_MqttClientOptions, CancellationToken.None); // Since 3.0.5 with CancellationToken
            }
            catch
            {
                Console.WriteLine("### RECONNECTING FAILED ###");
            }
        }

        private void OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var message = new ApplicationMessage
            {
                Topic = e.ApplicationMessage.Topic,
                Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload),
                QualityOfServiceLevel = (int)e.ApplicationMessage.QualityOfServiceLevel,
                Retain = e.ApplicationMessage.Retain
            };
            Application.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(message);
            });
            //Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
            //Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
            //Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            //Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            //Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
            //Console.WriteLine();
        }
    }
}
