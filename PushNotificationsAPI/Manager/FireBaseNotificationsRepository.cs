using PushNotificationsAPI.DTO;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PushNotificationsAPI.Manager
{
    public class FireBaseNotificationsRepository : FireBaseNotificationsRepositoryInterface
    {
        public async Task<bool> SendPushNotification(MessageDTO Message)
        {
            try
            {
                FirebaseApp DefaultApp = FirebaseApp.DefaultInstance;

                if(DefaultApp == null)
                {
                    DefaultApp = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory, "key.json"
                        ))
                    });
                }

                FirebaseMessaging Messaging = FirebaseMessaging.GetMessaging(DefaultApp);

                foreach(string Device in Message.RegistrationIDs)
                {
                    string Result = await Messaging.SendAsync(new Message()
                    {
                        Data = new Dictionary<string, string>()
                        {
                            ["FirstName"] = Message.Name,
                            ["LastName"] = Message.LastName
                        },
                        Notification = new Notification
                        {
                            Title = Message.Notification.Title,
                            Body = Message.Notification.Body
                        },
                        Token = Device
                    });
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendGeneralPushNotification(MessageDTO Message)
        {
            try
            {
                FirebaseApp DefaultApp = FirebaseApp.DefaultInstance;

                if (DefaultApp == null)
                {
                    DefaultApp = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory, "key.json"
                        ))
                    });
                }

                FirebaseMessaging Messaging = FirebaseMessaging.GetMessaging(DefaultApp);

                string Result = await Messaging.SendAsync(new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                        ["FirstName"] = Message.Name,
                        ["LastName"] = Message.LastName
                    },
                    Notification = new Notification
                    {
                        Title = Message.Notification.Title,
                        Body = Message.Notification.Body
                    },
                    Topic = "Nueva funcionalidad de mensajes por WhatsApp"
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
