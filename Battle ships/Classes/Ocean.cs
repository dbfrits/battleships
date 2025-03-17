using Battle_ships.Enums;
using System.Diagnostics;

namespace Battle_ships.Classes
{
    /// <summary>
    /// Creates a new Ocean for the game
    /// </summary>
    /// <param name="xsize">Horisonal (x-axis) size of the ocean. Max allowed is 26, e.g. A to Z</param>
    /// <param name="ysize">Vertical (y-axis) size of the ocean. No upper limit.</param>
    public class Ocean(int xsize, int ysize)
    {

        #region Public Properties

        /// <summary>
        /// All the ships in the ocean.
        /// </summary>
        public List<Ship> Ships = new List<Ship>();

        /// <summary>
        /// Game Over flag to indicate that all ships have been destroyed.
        /// </summary>
        public bool AllShipsDestroyed;

        /// <summary>
        /// All possible targets within the ocean.
        /// </summary>
        public List<string> Targets
        {
            get
            {
                if (targets.Count == 0)
                {
                    GenerateTargets();
                }
                return targets;
            }
        }

        #endregion

        #region Private Properties

        private List<string> targets = new List<string>();
        private List<string> freeTargets = new List<string>();
        private List<string> spentTargets = new List<string>();

        #endregion

        #region Methods

        /// <summary>
        /// Generate all the targets based on the required grid size.
        /// Example: A1, A2, A3, B1, B2, B3 etc.
        /// </summary>
        private void GenerateTargets()
        {
            for (char prefix = 'A'; prefix <= 'A' + xsize - 1; prefix++)
            {
                for (int yi = 1; yi <= ysize; yi++)
                {
                    targets.Add(string.Concat(prefix.ToString(), yi.ToString()));
                }

            }
        }

        /// <summary>
        /// Place a ship at random anywhere in the ocean, with a random orientation.
        /// It also validates that the location does not overlap with any other ships already in the ocean.
        /// </summary>
        /// <param name="ship">New ship to to be added to the ocean.</param>
        public void PlaceShipRandom(Ship ship)
        {
            if (freeTargets.Count() == 0)
            {
                freeTargets.AddRange(Targets);
            }

            Random rand = new Random();

            ShipOrientations Orientation = (ShipOrientations)rand.Next(0, 2);

            //Keep trying till the ship was placed without error
            while (!ship.IsPlaced)
            {
                if (Orientation == ShipOrientations.Horizontal)
                {
                    //There's no point getting a x position that won't allow for the ship size
                    int x = rand.Next(1, xsize - ship.TargetSize + 1);
                    int y = rand.Next(1, ysize + 1);
                    char yc = (char)(y + 64);
                    string StartTarget = string.Concat(yc.ToString(), x);

                    if (freeTargets.Contains(StartTarget))
                    {
                        //We have a free Start Target, now lets check the rest

                        List<string> shipTargets = new List<string>();
                        shipTargets.Add(StartTarget);

                        bool Blocked = false;
                        for (int i = x + 1; i < x + ship.TargetSize; i++)
                        {
                            string nextTarget = string.Concat(yc.ToString(), i);
                            if (!freeTargets.Contains(nextTarget))
                            {
                                Blocked = true;
                                break;
                            }
                            else
                            {
                                shipTargets.Add(nextTarget);
                            }
                        }
                        if (Blocked)
                        {
                            ship.IsPlaced = false;
                            ship.Targets.Clear();
                        }
                        else
                        {
                            foreach (string target in shipTargets)
                            {
                                ship.Targets.Add(target);
                                freeTargets.Remove(target);
                            }
                            ship.IsPlaced = true;
                            Ships.Add(ship);
                        }
                    }
                }
                else
                {
                    //There's no point getting a y position that won't allow for the ship size
                    int x = rand.Next(1, xsize + 1);
                    int y = rand.Next(1, ysize - ship.TargetSize + 1);
                    char yc = (char)(y + 64);
                    string StartTarget = string.Concat(yc.ToString(), x);

                    if (freeTargets.Contains(StartTarget))
                    {
                        //We have a free Start Target, now lets check the rest

                        List<string> shipTargets = new List<string>();
                        shipTargets.Add(StartTarget);

                        bool Blocked = false;
                        for (int i = y + 1; i < y + ship.TargetSize; i++)
                        {
                            yc = (char)(i + 64);
                            string nextTarget = string.Concat(yc.ToString(), x);
                            if (!freeTargets.Contains(nextTarget))
                            {
                                Blocked = true;
                                break;
                            }
                            else
                            {
                                shipTargets.Add(nextTarget);
                            }
                        }
                        if (Blocked)
                        {
                            ship.IsPlaced = false;
                            ship.Targets.Clear();
                        }
                        else
                        {
                            foreach (string target in shipTargets)
                            {
                                ship.Targets.Add(target);
                                freeTargets.Remove(target);
                            }
                            ship.IsPlaced = true;
                            Ships.Add(ship);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Processes the missile actions.
        /// </summary>
        /// <param name="missile"></param>
        /// <returns>Returns the result of the missile. Value can be NotLaunched, InvalidOcean, InvalidTarget, UsedTarget, Missed, Hit, Sunk, GameOver.</returns>
        public MissileResults ProcessMissile(Missile missile)
        {
            if (!Targets.Contains(missile.Target)) { return MissileResults.InvalidTarget; }
            if (spentTargets.Contains(missile.Target)) { return MissileResults.UsedTarget; } else { spentTargets.Add(missile.Target); }
            if (freeTargets.Contains(missile.Target)) { return MissileResults.Missed; } else { spentTargets.Add(missile.Target); }
              
            bool Hit = false;
            bool Sunk = false;
            foreach (var ship in Ships.Where(s => s.Targets.Count > 0))
            {
                if (ship.Targets.Contains(missile.Target)) { Hit = true; ship.Targets.Remove(missile.Target); }
                if (ship.Targets.Count == 0) { Sunk = true; }
            }

            if (Ships.Where(s => s.Targets.Count > 0).Count() == 0)
            {
                AllShipsDestroyed = true;
                return MissileResults.GameOver;
            }

            if (Sunk) { return MissileResults.Sunk; }
            if (Hit) { return MissileResults.Hit; }

            return MissileResults.Missed;
        }

        #endregion
    }
}
