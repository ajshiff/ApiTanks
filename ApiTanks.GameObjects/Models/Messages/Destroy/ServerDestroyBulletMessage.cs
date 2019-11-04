namespace ApiTanks.GameObjects.Models.Messages
{
    public class ServerDestroyBulletMessage : DestroyMessage
    {
        public string BulletId {get; set;}

        public ServerDestroyBulletMessage (Bullet bullet)
        {
            BulletId = bullet.Id;

        }
    }

}