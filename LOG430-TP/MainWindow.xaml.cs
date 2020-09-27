﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LOG430_TP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Subscribe_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            var topic = vm.TopicSubscribeText;
            vm.Controller.subscribe(topic);
        }

        private void Unsubscribe_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            var topic = vm.TopicSubscribeText;
            vm.Controller.unsubscribe(topic);
        }
    }
}
