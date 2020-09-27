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
        public ObservableCollection<ApplicationMessage> Messages { get; set; }

        private MqttController controller;

        public MainViewModel()
        {

            this.controller = new MqttController(this);
            Messages = new ObservableCollection<ApplicationMessage>();

            this.controller.connect();

            this.controller.subscribeALL();
            Console.ReadLine();
        }

        /// <summary>
        /// message received event handler
        /// </summary>
        /// <param name="applicationMessage"></param>
        public void messageReceived(ApplicationMessage applicationMessage)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(applicationMessage);
            });

        }
    }
}
