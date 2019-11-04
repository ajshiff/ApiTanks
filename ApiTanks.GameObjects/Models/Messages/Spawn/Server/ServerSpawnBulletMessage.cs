namespace ApiTanks.GameObjects.Models.Messages
{
    public class ServerSpawnBulletMessage : ServerSpawnMessage
    {
        public string BulletId {get; set;}
        
        public ServerSpawnBulletMessage (Bullet bullet) 
            : base(bullet) 
        {
            BulletId = bullet.Id;
        }
    }

}