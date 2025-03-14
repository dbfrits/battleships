using Battle_ships.Classes;

namespace Battle_ships_Tests
{
    public class ShipTests
    {

        [Fact]
        public void Create_Battleship()
        {
            var battleship = new Ship(Battle_ships.Enums.ShipTypes.Battleship);

            Assert.IsType<Ship>(battleship);
            Assert.Equal(5, battleship.TargetSize);
            Assert.Empty(battleship.Targets);
        }

        [Fact]
        public void Create_Destroyer()
        {
            var destroyer = new Ship(Battle_ships.Enums.ShipTypes.Destroyer);

            Assert.IsType<Ship>(destroyer);
            Assert.Equal(4, destroyer.TargetSize);
            Assert.Empty(destroyer.Targets);
        }


    }
}
