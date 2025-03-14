using Battle_ships.Classes;
using Battle_ships.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_ships_Tests
{
    public class MissileTests
    {
        [Fact]
        public void Create_Missile()
        {
            var missile = new Missile();

            Assert.IsType<Missile>(missile);
            Assert.Null(missile.Ocean);
            Assert.True(missile.Type == Battle_ships.Enums.MissileTypes.BasicMissile);
            Assert.Empty(missile.Target);
        }

        [Fact]
        public void Create_Missile_WithParameters()
        {
            var missile = new Missile(new Ocean(10,10),Battle_ships.Enums.MissileTypes.BasicMissile,"A1");

            Assert.IsType<Missile>(missile);
            Assert.NotNull(missile.Ocean);
            Assert.IsType<Ocean>(missile.Ocean);
            Assert.True(missile.Type == Battle_ships.Enums.MissileTypes.BasicMissile);
            Assert.Equal("A1",missile.Target);
        }

        [Fact]
        public void Launch_Missile_InvalidTarget()
        {
            var missile = new Missile();
            Assert.Equal(MissileResults.InvalidTarget, missile.Launch());
        }

        [Fact]
        public void Launch_Missile_InvalidOcean()
        {
            Missile missile = new Missile(null!, MissileTypes.BasicMissile, "A1");
            Assert.Equal(MissileResults.InvalidOcean, missile.Launch());
        }

    }
}
