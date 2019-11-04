namespace ApiTanks.GameObjects.Models.Messages
{
    public abstract class RequestSpawnBulletMessage : SpawnMessage
    {
        public RequestSpawnBulletMessage (string teamId, string tankId) : base(teamId, tankId) {}
    }

}