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
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace LOG430_TP
{
    public class MainViewModel : INotifyPropertyChanged 
    {
        public ObservableCollection<ApplicationMessage> Messages { get; set; }

        public MqttController Controller { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _TopicSubscribeText;
        public string TopicSubscribeText
        {
            get => _TopicSubscribeText;
            set
            {
                SetPropertyBackingField(ref _TopicSubscribeText, value);
                CanSubscribe = !string.IsNullOrEmpty(_TopicSubscribeText);
            }
        }

        private string _TopicUnsubscribeText;
        public string TopicUnsubscribeText
        {
            get => _TopicUnsubscribeText;
            set
            {
                SetPropertyBackingField(ref _TopicUnsubscribeText, value);
                CanUnsubscribe = !string.IsNullOrEmpty(_TopicUnsubscribeText);
            }
        }

        private bool _CanSubscribe;
        public bool CanSubscribe
        {
            get => _CanSubscribe;
            set => SetPropertyBackingField(ref _CanSubscribe, value);
        }

        private bool _CanUnsubscribe;
        public bool CanUnsubscribe
        {
            get => _CanUnsubscribe;
            set => SetPropertyBackingField(ref _CanUnsubscribe, value);
        }

        public MainViewModel()
        {

            this.Controller = new MqttController(this);
            Messages = new ObservableCollection<ApplicationMessage>();

            this.Controller.connect();
            
            //this.Controller.subscribe("wwodtf1/ca/qc/mtl/mobil/infra/gateway/ipc0/gat-00000-01/heartbeat");
            Console.ReadLine();
        }

        /// <summary>
        /// message received event handler
        /// </summary>
        /// <param name="applicationMessage"></param>
        public void messageReceived(ApplicationMessage message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Messages.Add(message);
            });

        }

        protected void SetPropertyBackingField<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            property = value;
            RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
