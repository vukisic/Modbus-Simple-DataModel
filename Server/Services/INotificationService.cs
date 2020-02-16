using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface INotificationService
    {
        void ShowNotification(string title, string message, NotificationType type);
    }
}
