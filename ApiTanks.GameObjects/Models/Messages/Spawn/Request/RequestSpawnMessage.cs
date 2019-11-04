namespace ApiTanks.GameObjects.Models.Messages
{
    public abstract class RequestSpawnMessage : SpawnMessage
    {
        protected RequestSpawnMessage (string teamId, string tankId) : base(teamId, tankId) {}
    }

}