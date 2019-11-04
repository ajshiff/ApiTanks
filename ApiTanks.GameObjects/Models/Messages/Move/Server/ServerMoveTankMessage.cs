namespace ApiTanks.GameObjects.Models.Messages
{
    public class ServerMoveTankMessage : ServerMoveMessage
    {
        public ServerMoveTankMessage (Tank tank)
            : base (tank)
        {
            
        }
    }

}