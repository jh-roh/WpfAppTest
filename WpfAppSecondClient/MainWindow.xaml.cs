using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppSecondClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    HubConnection hubConnection;
    


    public MainWindow()
    {
        InitializeComponent();
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7177/mychat")
            .WithAutomaticReconnect()
            .Build();
        hubConnection.On<string,string>("receive", (msg, user) =>
        {
            Dispatcher.Invoke(() =>
            {
                string message = $"{user}의 메시지 : {msg}";

                lstMessages.Items.Add(message + Environment.NewLine);

            });
        });

        hubConnection.On<string>("notify", (msg) =>
        {
            Dispatcher.Invoke(() =>
            {
                string message = $"{msg}";

                lstMessages.Items.Add(message );

            });
        });


    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        await hubConnection.InvokeAsync("Send", txtMessages.Text,"user1");
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            //btnSend.IsEnabled = false;

            await hubConnection.StartAsync();


            lstMessages.Items.Add("Connection started");
           // btnSend.IsEnabled = true;
        }
        catch (Exception ex)
        { 
            lstMessages.Items.Add(ex.Message);
        }
    }

    private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        await hubConnection.InvokeAsync("Send", txtMessages.Text + "/" + "userId", "");

        await hubConnection.StopAsync();

    }
}