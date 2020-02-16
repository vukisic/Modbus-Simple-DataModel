using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class NotificationService : INotificationService, IDisposable
    {
        private NotificationManager notificationManager;
        
        public  NotificationService()
        {
            notificationManager = new NotificationManager();
        }

        public void Dispose()
        {
            notificationManager = null;
        }

        public void ShowNotification(string title, string message, NotificationType type)
        {
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type
            });
        }
    }
}
