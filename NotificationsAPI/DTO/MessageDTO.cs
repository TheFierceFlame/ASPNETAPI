namespace NotificationsAPI.DTO
{
    public class MessageDTO
    {
        public string[] RegistrationIDs { get; set; }
        public NotificationDTO Notification { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
