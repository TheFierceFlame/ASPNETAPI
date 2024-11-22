using PushNotificationsAPI.DTO;
using System.Threading.Tasks;

namespace PushNotificationsAPI.Manager
{
    public interface FireBaseNotificationsRepositoryInterface
    {
        public Task<bool> SendPushNotification(MessageDTO Message);
        public Task<bool> SendGeneralPushNotification(MessageDTO Message);
    }
}
