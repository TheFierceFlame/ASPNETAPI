using NotificationsAPI.DTO;
using NotificationsAPI.Manager;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NotificationsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly FireBaseNotificationsRepositoryInterface FirebaseNotificationsRepository;

        public NotificationsController(FireBaseNotificationsRepositoryInterface NotificationsRepository)
        {
            FirebaseNotificationsRepository = NotificationsRepository;
        }

        [HttpPost("/Enviar")]
        public async Task<IActionResult> Send(MessageDTO Message)
        {
            bool Result = await FirebaseNotificationsRepository.SendPushNotification(Message);

            return Result ? Ok(Message) : BadRequest();
        }

        [HttpPost("/EnviarGeneral")]
        public async Task<IActionResult> SendAll(MessageDTO Message)
        {
            bool Result = await FirebaseNotificationsRepository.SendGeneralPushNotification(Message);

            return Result ? Ok(Message) : BadRequest();
        }
    }
}
