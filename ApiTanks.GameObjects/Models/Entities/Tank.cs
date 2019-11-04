using System.Collections.Generic;

namespace ApiTanks.GameObjects.Models
{
    public class Tank : Entity
    {
        public List<Bullet> Bullets {get; set;} = new List<Bullet>();
        private int MaxBullets {get; set;} = 5;
        private int LifetimeBulletCount {get; set;} = 0;
        private float size = 4;
        private float speed = 1;
        public Tank (Team team, string id) : base (team, id) 
        {
            Size = size;
            Speed = speed;
        }
        public Tank (Team team, string id, Location pos) : base (team, id, pos) 
        {
            Size = size;
            Speed = speed;
        }

        public void SetRotation (double rotation)
        {
            Position.R = rotation;
        }

        public void Move (Direction direction, double newRotation)
        {
            Position.R = newRotation;
            var deltaMove = DeltaMove();
            if (direction == Direction.Backward)
                deltaMove.InvertPosition();
            else if (direction == Direction.Stay)
                return;
            Position += deltaMove;

        }

        public Bullet FireBullet ()
        {
            if ((Bullets.Count) > MaxBullets)
            {
                return null;
            }
            var bulletId = $"{Id}_{++LifetimeBulletCount}";
            var newBullet = new Bullet(Team, this, bulletId, Position);
            Bullets.Add(newBullet);
            return newBullet;
        }
    }

}