using System;

namespace ApiTanks.GameObjects.Models
{
    public class Location
    {

        public Location ()
        {
            var rand = new Random();
            X = rand.Next(0, Gameboard.Width);
            Y = rand.Next(0, Gameboard.Height);
            R = rand.Next(0, (int)Rotation.MaxRotation);
        }

        public Location (double xIn = 0, double yIn = 0, double rIn = 0)
        {
            X = xIn;
            Y = yIn;
            R = rIn;
        }
        public double X 
        {
            get {return x;                   }
            set {x = value % Gameboard.Width;}
        }
        public double Y 
        {
            get {return y;                    }
            set {y = value % Gameboard.Height;}
        }
        public double R 
        {
            get { return r;        }
            // Rotation is measured in degrees. Cannot Exceed 360 Degrees
            set { r = value % (int)Rotation.MaxRotation; }
        }
        private double x;
        private double y;
        private double r;

        public static Location operator + (Location initial, Location delta)
        {
            return new Location(
                initial.X + delta.X,
                initial.Y + delta.Y,
                initial.R + delta.R
            );
        }
        public static Location operator * (Location initial, Location delta)
        {
            return new Location(
                initial.X * delta.X,
                initial.Y * delta.Y,
                initial.R * delta.R
            );
        }
        public void InvertPosition (bool invertX = true, bool invertY = true, bool invertR = false)
        {
            if (invertX)
                X *= -1;
            if (invertY)
                Y *= -1;
            if (invertR)
                R *= -1;
        }
    }


}