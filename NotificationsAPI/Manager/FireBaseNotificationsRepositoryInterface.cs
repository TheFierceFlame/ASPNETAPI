using NotificationsAPI.DTO;
using System.Threading.Tasks;

namespace NotificationsAPI.Manager
{
    public interface FireBaseNotificationsRepositoryInterface
    {
        public Task<bool> SendPushNotification(MessageDTO Message);
        public Task<bool> SendGeneralPushNotification(MessageDTO Message);
    }
}
