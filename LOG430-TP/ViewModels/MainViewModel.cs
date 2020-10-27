using LOG430_TP.Commands;
using LOG430_TP.Models.StatisticComputers;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace LOG430_TP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged 
    {
        private static readonly string _ApplicationMessagePayloadValueRegexPattern = "\"Value\":(?<value>.+)}";
        private static readonly string _ApplicationMessagePayloadCreateUTCRegexPattern = "\"CreateUtc\":\"(?<createUtc>.+?)\"";
        private ApplicationMessageRepository repos;

        public ObservableCollection<ApplicationMessage> MessagesCache { get; set; }

        public MqttController Controller { get; private set; }

        private bool _IsScubscribedToAll;
        public bool IsScubscribedToAll
        {
            get => _IsScubscribedToAll;
            set => SetPropertyBackingField(ref _IsScubscribedToAll, value);
        }

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

        private bool _ShowStats;
        public bool ShowStats
        {
            get => _ShowStats;
            set => SetPropertyBackingField(ref _ShowStats, value);
        }

        private DateTime _StatsStartDateTime;
        public DateTime StatsStartDateTime
        {
            get => _StatsStartDateTime;
            set => SetPropertyBackingField(ref _StatsStartDateTime, value);
        }

        private DateTime _StatsEndDateTime;
        public DateTime StatsEndDateTime
        {
            get => _StatsEndDateTime;
            set => SetPropertyBackingField(ref _StatsEndDateTime, value);
        }

        private string _StatsTopicText;
        public string StatsTopicText
        {
            get => _StatsTopicText;
            set => SetPropertyBackingField(ref _StatsTopicText, value);
        }

        private Statistic _CurrentStatistic;
        public Statistic CurrentStatistic
        {
            get => _CurrentStatistic;
            set => SetPropertyBackingField(ref _CurrentStatistic, value);
        }

        private float _CurrentStatisticResult;
        public float CurrentStatisticResult
        {
            get => _CurrentStatisticResult;
            set => SetPropertyBackingField(ref _CurrentStatisticResult, value);
        }

        private Dictionary<Statistic, IStatisticComputer<float, float>> _StatisticComputers;

        public ICommand SubscribeCommand { get; }
        public ICommand UnsubscribeCommand { get; }
        public ICommand SubscribeAllCommand { get; }
        public ICommand UnsubscribeAllCommand { get; }
        public ICommand ClearMessagesCommand { get; }
        public ICommand ToggleShowStatsCommand { get; }
        public ICommand ComputeStatsCommand { get; }

        public MainViewModel()
        {
            Controller = new MqttController();
            Controller.ApplicationMessagedReceived += MessageReceived;
            MessagesCache = new ObservableCollection<ApplicationMessage>();
            repos = ApplicationMessageRepository.Instance;

            _IsScubscribedToAll = false;

            SubscribeCommand = new RelayCommand(Subscribe, () => CanSubscribe);
            UnsubscribeCommand = new RelayCommand(Unsubscribe, () => CanUnsubscribe);
            SubscribeAllCommand = new RelayCommand(SubscribeAll, () => !IsScubscribedToAll);
            UnsubscribeAllCommand = new RelayCommand(UnsubscribeAll, () => IsScubscribedToAll);
            ClearMessagesCommand = new RelayCommand(ClearMessages);
            ToggleShowStatsCommand = new RelayCommand(() => ShowStats = !ShowStats);
            ComputeStatsCommand = new RelayCommand(ComputeStats, () => !string.IsNullOrWhiteSpace(_StatsTopicText));

            _StatsStartDateTime = DateTime.Now.AddHours(-24);
            _StatsEndDateTime = DateTime.Now;

            _StatisticComputers = new Dictionary<Statistic, IStatisticComputer<float, float>>();
            _StatisticComputers.Add(Statistic.Median, new MedianComputer());
            _StatisticComputers.Add(Statistic.Mean, new MeanComputer());
            _StatisticComputers.Add(Statistic.StandardDeviation, new StandardDeviationComputer());

            var connection = Controller.connect();

            if (connection == false)
            {
                MessageBox.Show("Could not connect to mqttDatabase. Will now be in degraded mode", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            var testRepos = ApplicationMessageRepository.Instance;
            var startDate = DateTime.SpecifyKind(new DateTime(2020, 10, 26, 20, 20, 25), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(new DateTime(2020, 10, 26, 20, 20, 28), DateTimeKind.Utc);
            var x  = testRepos.GetApplicationMessages(startDate,endDate);
            x.Wait();

            var y = 4;

        }

        /// <summary>
        /// message received event handler
        /// </summary>
        /// <param name="applicationMessage"></param>
        public void MessageReceived(ApplicationMessage applicationMessage)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if(MessagesCache.Count >= 20)
                    MessagesCache.RemoveAt(0);

                MessagesCache.Add(applicationMessage);
                repos.Add(applicationMessage);
            });
        }

        private void ClearMessages()
        {
            MessagesCache.Clear();
        }

        private void Subscribe()
        {
            Controller.subscribe(_TopicSubscribeText);
            TopicSubscribeText = string.Empty;
        }

        private void Unsubscribe()
        {
            Controller.unsubscribe(_TopicUnsubscribeText);
            TopicSubscribeText = string.Empty;
        }

        private void SubscribeAll()
        {
            Controller.subscribeALL();
            IsScubscribedToAll = true;
        }

        private void UnsubscribeAll()
        {
            Controller.unsubscribeALL();
            IsScubscribedToAll = false;
        }



        private void ComputeStats()
        {

            List<ApplicationMessage> applicationMessages = new List<ApplicationMessage>();

            try
            {

                applicationMessages = repos.GetApplicationMessages(_StatsTopicText, _StatsStartDateTime, StatsEndDateTime).Result;

            }
            catch (Exception e)
            {
                MessageBox.Show("Could not connect to own database.", "error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            var values = this.applicationMessageValuesList(applicationMessages);
            if (!_StatisticComputers.TryGetValue(_CurrentStatistic, out IStatisticComputer<float, float> statisticComputer))
                return;

            // uses the good compute
            if (values.Count > 0)
                CurrentStatisticResult = statisticComputer.Compute(values);
        }

        /*
        private void ComputeStats()
        {

            var applicationMessages = ApplicationMessageRepository.Instance.getApplicationMessages(_StatsTopicText, _StatsStartDateTime, StatsEndDateTime);

            var values = this.applicationMessageValuesList(applicationMessages.Result);

            //CurrentStatisticResult = 0;

            //var messages = ApplicationMessageRepository.Instance.GetApplicationMessages()?.Result;

            //Regex valueRegex = new Regex(_ApplicationMessagePayloadValueRegexPattern);
            //Regex utcRegex = new Regex(_ApplicationMessagePayloadCreateUTCRegexPattern);

            //if (messages == null)
            //    return;

            //List<float> values = new List<float>();

            //foreach (var message in messages)
            //{
            //    if (message.Topic != _StatsTopicText)
            //        continue;

            //    Match valueMatch = valueRegex.Match(message.Payload);
            //    Match utcMatch = utcRegex.Match(message.Payload);

            //    if (!valueMatch.Success || !utcMatch.Success)
            //        continue;

            //    string value = valueMatch.Groups["value"].Value;
            //    string utcString = utcMatch.Groups["createUtc"].Value;


            //    // check date
            //    if (!DateTime.TryParse(utcString, out DateTime createUtcTime) || !float.TryParse(value, out float floatValue))
            //        continue;

            //    if (createUtcTime > StatsStartDateTime.ToUniversalTime() && createUtcTime < StatsEndDateTime.ToUniversalTime())
            //        values.Add(floatValue);
            //    else
            //        continue;
            //}

            if (!_StatisticComputers.TryGetValue(_CurrentStatistic, out IStatisticComputer<float, float> statisticComputer))
                return;

            // uses the good compute
            if(values.Count > 0)
                CurrentStatisticResult = statisticComputer.Compute(values);
        }*/


        private List<float> applicationMessageValuesList (List<ApplicationMessage> messages)
        {
            var values = new List<float>();

            Regex valueRegex = new Regex(_ApplicationMessagePayloadValueRegexPattern);

            foreach (ApplicationMessage message in messages)
            {
                Match valueMatch = valueRegex.Match(message.Payload);
                string value = valueMatch.Groups["value"].Value;


                if (!float.TryParse(value, out float floatValue))
                {
                    MessageBox.Show("Topic value is not a number", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new List<float>();
                }

                values.Add(floatValue);

            }

            return values;
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetPropertyBackingField<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            property = value;
            RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public enum Statistic
    { 
        Mean,
        Median,
        StandardDeviation
    }
}
