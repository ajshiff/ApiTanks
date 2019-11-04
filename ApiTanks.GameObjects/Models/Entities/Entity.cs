using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiTanks.GameObjects.Models
{
    public abstract class Entity
    {
        public Team Team {get; set;}
        public string Id {get; set;}
        public Location Position {get; set;}
        public double Size {get; set;}
        public double Speed {get; set;}
        
        protected Entity (){}
        public Entity (Team team, string id)
        {
            Team = team;
            Id = id;
            Position = new Location();
        }
        public Entity (Team team, string id, Location position)
        {
            Team = team;
            Id = id;
            Position = position;
        }


        /************************************************************
        * Move
        * Edits the Position of the Enity 
        *************************************************************/
        public virtual void Move ()
        {
            Position += DeltaMove();
        }
        public virtual Location DeltaMove ()
        {
            double dX = Speed * Math.Cos(Position.R);
            double dY = Speed * Math.Sin(Position.R);
            return new Location(dX, dY, 0);
        }

        public virtual bool CollisionCheck(Entity entity)
        {
            return hitBoxesDoTouch(entity);

            bool hitBoxesDoTouch (Entity otherEntity)
            {
                Entity thisEntity = this;
                var distance = distanceBetweenEntities(thisEntity.Position, otherEntity.Position);
                return distance < (thisEntity.Size + otherEntity.Size);
            }

            double distanceBetweenEntities (Location thisLocation, Location otherLocation)
            {
                return Math.Sqrt((thisLocation.X - otherLocation.X) + (thisLocation.Y - thisLocation.Y));
            }
        }


    }

}