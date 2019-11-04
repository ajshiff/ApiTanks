namespace ApiTanks.GameObjects.Models.Messages
{
    public abstract class RequestSpawnTankMessage : SpawnMessage
    {
        public RequestSpawnTankMessage (string teamId, string tankId) : base(teamId, tankId) {}
    }

}