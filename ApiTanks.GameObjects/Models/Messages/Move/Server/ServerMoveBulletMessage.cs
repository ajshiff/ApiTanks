namespace ApiTanks.GameObjects.Models.Messages
{
    public class ServerMoveBulletMessage : ServerMoveMessage
    {
        public string BulletId {get; set;}

        public ServerMoveBulletMessage (Bullet bullet)
            : base (bullet)
        {
            BulletId = bullet.Id;
        }
        
    }

}