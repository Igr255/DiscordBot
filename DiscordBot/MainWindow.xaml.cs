using DiscordBot.BotFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DiscordBot
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

        BotStart bot = new BotStart();
       
        ///BUTTONS///
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {                    
            bot.Test1();
        } 
        
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try { BotStart.thread.Abort(); }
            catch (Exception ex) { }
            
            Utils.potkan.d.Quit();
            Application.Current.Shutdown();
        }

        ///TEXTBOXES///
        private void ServerNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            bot.SetServerName(textBox.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Utils.potkan.SetStartingVoiceChannelID(int.Parse(textBox.Text));
        }

        private void ChannelCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Utils.potkan.SetVoiceChannelCount(int.Parse(textBox.Text));
        }

        private void NameChange_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Utils.potkan.SetCustomName(textBox.Text);
        }

        ///CHECKBOXES///        
        private void SetWalkSequence_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Utils.potkan.SetWalkSequence((bool)checkBox.IsChecked);
        }

        private void SetFollowSequence_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox1 = (CheckBox)sender;
            Utils.potkan.SetFollowSequence((bool)checkBox1.IsChecked);
        }

        private void SetSmoothFollow_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox1 = (CheckBox)sender;
            Utils.potkan.SetSmoothFollow((bool)checkBox1.IsChecked);
        }

        

        ///

    }
}
