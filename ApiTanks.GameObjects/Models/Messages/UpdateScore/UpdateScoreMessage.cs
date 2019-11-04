namespace ApiTanks.GameObjects.Models.Messages
{
    public class UpdateScoreMessage : Message
    {
        public UpdateScoreMessage (Team team)
        {
            TeamId = team.Id;
            Points = team.Points;
        }
        public string TeamId {get; set;}
        public int Points {get; set;}
    }

}