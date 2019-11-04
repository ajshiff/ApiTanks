using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ApiTanks.GameObjects.Models
{
    public class Bullet : Entity
    {
        public long MaxLifetimeMs = 5000;
        private float size = 1;
        private float speed = 1;
        private readonly Stopwatch Timer = new Stopwatch();
        private bool HasCollided {get; set;}
        private Tank ParentTank {get; set;}
        public Bullet (Team team, Tank parentTank, string id) : base (team, id) 
        {
            Timer.Start();
            Size = size;
            Speed = speed;
            ParentTank = parentTank;
        }
        public Bullet (Team team, Tank parentTank, string id, Location pos) : base (team, id, pos) 
        {
            Timer.Start();
            Size = size;
            Speed = speed;
        }

        public Tank TankCollisionChecks (List<Tank> tanks)
        {
            var collidedTank = tanks.Where((tank) => CollisionCheck(tank)).FirstOrDefault();
            if (collidedTank != null )
            {
                HasCollided = true;
            }
            return collidedTank;
        }
        /************************************************************
        * A Bullet is still alive if its MaxLifetime(miliseconds)
        * has not been exceeded, or if it has not collided with
        * anything yet.
        *************************************************************/
        public bool StillAlive ()
        {
            if (Timer.ElapsedMilliseconds > MaxLifetimeMs || !HasCollided)
            {
                Timer.Stop();
                return false;
            }
            return true;
        }
    }

}