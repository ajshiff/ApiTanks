using System;
using System.Collections.Generic;
using System.Linq;
using ApiTanks.GameObjects.Models;
using ApiTanks.GameObjects.Models.Messages;

namespace ApiTanks.GameObjects
{
    public class EventLoop
    {
        public Dictionary<string, Team> Teams {get; set;} 
            = new Dictionary<string, Team>();
        private long currentGameLoop;
        public long CurrentGameLoop
        {
            get
            {
                if (currentGameLoop > Int32.MaxValue)
                {
                    currentGameLoop = 0;
                }
                return currentGameLoop;
            } 
            set
            {
                currentGameLoop = value;
            }
        } 
        public EventLoop()
        {

        }

        public MessagesPacket RunLoop(
            List<RequestSpawnTankMessage> requestSpawnTankMessages,
            List<RequestSpawnBulletMessage> requestSpawnBulletMessages,
            List<RequestMoveTankMessage> requestMoveMessages)
        {
            // Spawn Tanks from Messages
            var serverSpawnTankMessages = requestSpawnTankMessages.Aggregate(
                new List<ServerSpawnTankMessage>(), (acc, requestSpawnTankMessage) => {
                var serverSpawnTankMessage = SpawnTank(requestSpawnTankMessage);
                if (serverSpawnTankMessage != null)
                    acc.Add(serverSpawnTankMessage);
                return acc;
            });

            // Spawn Bullets from Messages
            var serverSpawnBulletMessages = requestSpawnBulletMessages.Aggregate(
                new List<ServerSpawnBulletMessage>(), (acc, requestSpawnBulletMessage) => {
                var serverSpawnBulletMessage = SpawnBullet(requestSpawnBulletMessage);
                if (serverSpawnBulletMessage != null)
                    acc.Add(serverSpawnBulletMessage);
                return acc;
            });

            // Move Tanks from Messages
            var serverMoveTankMessages = requestMoveMessages.Aggregate(
                new List<ServerMoveTankMessage>(), (acc, moveMessage) => {
                var serverMoveTankMessage = MoveTank(moveMessage);
                if (serverMoveTankMessage != null)
                    acc.Add(serverMoveTankMessage);
                return acc;
            });

            // Select All Tanks
            var allTanks = Teams.SelectMany<KeyValuePair<string, Team>, Tank>((teamKvp) 
                => (IEnumerable<Tank>)teamKvp.Value.Tanks).ToList();
            // Select All Bullets
            var allBullets = allTanks.SelectMany<Tank, Bullet>((tank) => tank.Bullets);

            var serverMoveBulletMessages = new List<ServerMoveBulletMessage>();
            var serverDestroyTankMessages = new List<ServerDestroyTankMessage>();
            var serverDestroyBulletMessages = new List<ServerDestroyBulletMessage>();
            var updateScoreMessages = new List<UpdateScoreMessage>();
            foreach (var bullet in allBullets)
            {
                // Move Bullets
                serverMoveBulletMessages.Add(MoveBullet(bullet));
                // Detect Collisions Between Bullets and Tanks
                var hitTank = bullet.TankCollisionChecks(allTanks);
                if (hitTank == null)
                    break;
                serverDestroyTankMessages.Add( DestroyTank(hitTank, bullet) );
                serverDestroyBulletMessages.Add( DestroyBullet(bullet) );
                bullet.Team.Points += 1;
                updateScoreMessages.Add(UpdateScore(bullet.Team));
            }
            


            // Send End-Game-Tick Event
            var messagesPacket = new MessagesPacket()
            {
                ServerSpawnTankMessages = serverSpawnTankMessages,
                ServerSpawnBulletMessages = serverSpawnBulletMessages,
                ServerMoveTankMessages = serverMoveTankMessages,
                ServerMoveBulletMessages = serverMoveBulletMessages,
                ServerDestroyTankMessages = serverDestroyTankMessages,
                ServerDestroyBulletMessages = serverDestroyBulletMessages,
                UpdateScoreMessages = updateScoreMessages,
                OfficialGamestateUpdateMessage = UpdateGametick()
            };
            return messagesPacket;
        }

        public ServerSpawnTankMessage SpawnTank (RequestSpawnTankMessage spawnTankMessages)
        {
            Team team;
            var teamId = spawnTankMessages.TeamId;
            var tankId = spawnTankMessages.TankId;
            try 
            {
                team = Teams[teamId];
            }
            catch (KeyNotFoundException)
            {
                Teams.Add(teamId, new Team(teamId));
                team = Teams[teamId];
            }
            var spawnedTank = team.SpawnTank(tankId);
            return spawnedTank == null ? null : new ServerSpawnTankMessage(spawnedTank);
        }

        public ServerSpawnBulletMessage SpawnBullet (RequestSpawnBulletMessage spawnMessages)
        {
            Team team;
            Tank tank;
            var teamId = spawnMessages.TeamId;
            var tankId = spawnMessages.TankId;
            try
            {
                team = Teams[teamId];
                tank = team.Tanks[tankId];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            var newBullet = tank.FireBullet();
            return newBullet == null ? null : new ServerSpawnBulletMessage(newBullet);
        }
        public ServerMoveTankMessage MoveTank (RequestMoveTankMessage moveMessages)
        {
            Team team;
            Tank tank;
            var teamId = moveMessages.TeamId;
            var tankId = moveMessages.TankId;
            var movementDirection = moveMessages.MeepDirection;
            var newTankRotation = moveMessages.Rotation;
            try 
            {
                team = Teams[teamId];
                tank = team.Tanks[tankId];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
            tank.Move(movementDirection, newTankRotation);
            return new ServerMoveTankMessage(tank);
        }
        public ServerMoveBulletMessage MoveBullet (Bullet bullet)
        {
            bullet.Move();
            return new ServerMoveBulletMessage(bullet);
        }
        public ServerDestroyTankMessage DestroyTank (Tank tank, Bullet bullet)
        {
            return new ServerDestroyTankMessage(tank, bullet);
        }
        public ServerDestroyBulletMessage DestroyBullet (Bullet bullet)
        {
            return new ServerDestroyBulletMessage(bullet);
        }
        public UpdateScoreMessage UpdateScore (Team team)
        {
            return new UpdateScoreMessage(team);
        }
        public OfficialGamestateUpdateMessage UpdateGametick ()
        {
            return new OfficialGamestateUpdateMessage(CurrentGameLoop);
        }

    }
}
