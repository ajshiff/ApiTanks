namespace ApiTanks.GameObjects.Models.Messages
{
    public class ServerDestroyTankMessage : DestroyMessage
    {
        public string TankTeamId {get; set;}
        public string TankId {get; set;}
        public string BulletTeamId {get; set;}
        public string BulletId {get; set;}

        public ServerDestroyTankMessage (Tank hitTank, Bullet hitBullet)
        {
            TankTeamId = hitTank.Team.Id;
            TankId = hitTank.Id;
            BulletTeamId = hitBullet.Team.Id;
            BulletId = hitBullet.Id;
        }
    }

}