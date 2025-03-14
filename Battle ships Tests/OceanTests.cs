using Battle_ships.Classes;
using Battle_ships.Enums;
using System.Runtime.Serialization;

namespace Battle_ships_Tests
{
    public class OceanTests
    {

        [Fact]
        public void Create_Ocean()
        {
            var ocean = new Ocean(10, 10);

            Assert.IsType<Ocean>(ocean);
            Assert.Empty(ocean.Ships);
            Assert.False(ocean.AllShipsDestroyed);

        }

        [Fact]
        public void Generate_Targets()
        {
            var ocean = new Ocean(10, 10);
            Assert.Equal(100, ocean.Targets.Count);
            Assert.Equal("A1", ocean.Targets.First());
        }

        [Fact]
        public void Place_Battleship_random()
        {
            var battleship1 = new Ship(Battle_ships.Enums.ShipTypes.Battleship);
            var battleship2 = new Ship(Battle_ships.Enums.ShipTypes.Battleship);

            Ocean ocean = new(10, 10);
            ocean.PlaceShipRandom(battleship1);
            ocean.PlaceShipRandom(battleship2);

            Assert.True(battleship1.IsPlaced);
            Assert.True(battleship2.IsPlaced);
            Assert.NotEqual(battleship1.Targets, battleship2.Targets);
        }

        [Fact]
        public void Place_Destroyer_random()
        {
            var destroyer1 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);
            var destroyer2 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);

            Ocean ocean = new(10, 10);
            ocean.PlaceShipRandom(destroyer1);
            ocean.PlaceShipRandom(destroyer2);

            Assert.True(destroyer1.IsPlaced);
            Assert.True(destroyer2.IsPlaced);
            Assert.NotEqual(destroyer1.Targets, destroyer2.Targets);
        }

        [Fact]
        public void Process_Missile_InvalidTarget()
        {
            var destroyer1 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);

            Ocean ocean = new(10, 10);
            ocean.PlaceShipRandom(destroyer1);

            MissileResults result = MissileResults.NotLaunched;
            result = ocean.ProcessMissile(new Missile(ocean, Battle_ships.Enums.MissileTypes.BasicMissile, "A100"));

            Assert.Equal(MissileResults.InvalidTarget, result);
        }

        [Fact]
        public void Process_Missile_UsedTarget()
        {
            var destroyer1 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);

            Ocean ocean = new(10, 10);
            ocean.PlaceShipRandom(destroyer1);

            MissileResults result = MissileResults.NotLaunched;
            result = ocean.ProcessMissile(new Missile(ocean, Battle_ships.Enums.MissileTypes.BasicMissile, "A1"));
            result = ocean.ProcessMissile(new Missile(ocean, Battle_ships.Enums.MissileTypes.BasicMissile, "A1"));

            Assert.Equal(MissileResults.UsedTarget, result);
        }

        [Fact]
        public void Process_Missile_Missed()
        {
            var destroyer1 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);

            Ocean ocean = new(10, 10);
            ocean.PlaceShipRandom(destroyer1);

            MissileResults result = MissileResults.NotLaunched;
            while (result != MissileResults.Missed) {
                foreach (string target in ocean.Targets)
                {
                    result = ocean.ProcessMissile(new Missile(ocean, Battle_ships.Enums.MissileTypes.BasicMissile, target));
                }
            }

            Assert.Equal(MissileResults.Missed, result);
        }

        [Fact]
        public void Process_Missile_Hit() {
            var destroyer1 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);

            Ocean ocean = new(10, 10);
            ocean.PlaceShipRandom(destroyer1);

            Assert.Equal(MissileResults.Hit,ocean.ProcessMissile(new Missile(ocean, Battle_ships.Enums.MissileTypes.BasicMissile, destroyer1.Targets.First())));
        }

        [Fact]
        public void Process_Missile_Sunk()
        {
            var destroyer1 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);
            var destroyer2 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);

            Ocean ocean = new(10, 10);
            ocean.PlaceShipRandom(destroyer1);
            ocean.PlaceShipRandom(destroyer2);

            MissileResults result = MissileResults.NotLaunched;
            while (result != MissileResults.Sunk)
            {
                foreach (string target in ocean.Ships.First().Targets.ToList())
                {
                    result = ocean.ProcessMissile(new Missile(ocean, Battle_ships.Enums.MissileTypes.BasicMissile, target));
                }
            }

            Assert.Equal(MissileResults.Sunk, result);
        }

        [Fact]
        public void Process_Missile_GameOver()
        {
            var destroyer1 = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);

            Ocean ocean = new(10, 10);
            ocean.PlaceShipRandom(destroyer1);

            MissileResults result = MissileResults.NotLaunched;
            while (result != MissileResults.GameOver)
            {
                foreach (string target in ocean.Ships.First().Targets.ToList())
                {
                    result = ocean.ProcessMissile(new Missile(ocean, Battle_ships.Enums.MissileTypes.BasicMissile, target));
                }
            }

            Assert.Equal(MissileResults.GameOver, result);
        }

    }

}
