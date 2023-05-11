using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;

namespace MauiSignalRChatDemo.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly HubConnection _hubConnection;

        [ObservableProperty]
        string _name;

        [ObservableProperty]
        string _message;

        [ObservableProperty]
        ObservableCollection<string> _messages;

        [ObservableProperty]
        bool _isConnected;

        public MainPageViewModel()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://yoururlhere.com/chatHub")
                .Build();

            Messages ??= new ObservableCollection<string>();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Messages.Add($"{user} says {message}");
                });
            });
        }

        [RelayCommand]
        async Task Connect()
        {
            if (_hubConnection.State == HubConnectionState.Connecting ||
                _hubConnection.State == HubConnectionState.Connected) return;

            await _hubConnection.StartAsync();

            IsConnected = true;
        }

        [RelayCommand]
        async Task Disconnect()
        {
            
            if (_hubConnection.State == HubConnectionState.Disconnected) return;

            await _hubConnection.StopAsync();

            IsConnected = false;
        }

        [RelayCommand]
        async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Message)) return;

            await _hubConnection.InvokeAsync("SendMessage", Name, Message);

            Message = string.Empty;
        }
    }
}
