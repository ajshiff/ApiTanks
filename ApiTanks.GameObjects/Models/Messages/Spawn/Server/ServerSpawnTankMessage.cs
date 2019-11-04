namespace ApiTanks.GameObjects.Models.Messages
{
    public class ServerSpawnTankMessage : ServerSpawnMessage
    {
        public ServerSpawnTankMessage (Tank tank) 
            : base(tank) {}
    }

}