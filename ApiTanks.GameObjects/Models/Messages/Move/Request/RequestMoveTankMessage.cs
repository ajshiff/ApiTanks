using ApiTanks.GameObjects.Models;

namespace ApiTanks.GameObjects.Models.Messages
{
    public class RequestMoveTankMessage : MoveMessage
    {
        private Direction direction;
        public Direction MeepDirection
        {
            get
            {
                return direction;
            }
            private set
            {
                direction = value;
            }
        }
        public int MovementDirection 
        {
            private get 
            {
                return (int)direction;
            }
            set
            {
                if (value > 0)
                    MeepDirection = Direction.Forward;
                else if (value < 0)
                    MeepDirection = Direction.Backward;
                else
                    MeepDirection = Direction.Stay;
            }
        }
        public double Rotation {get; set;}
        public RequestMoveTankMessage (string teamId, string tankId, int direction, double rotation)
            : base (teamId, tankId)
        {
            MovementDirection = direction;
            Rotation = rotation;
        }
    }

}