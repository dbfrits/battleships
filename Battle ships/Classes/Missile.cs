using Battle_ships.Enums;

namespace Battle_ships.Classes
{
    /// <summary>
    /// Creates a new blank missile to be used in the game.
    /// </summary>
    public class Missile()
    {

        #region Constructors

        /// <summary>
        /// Creates a new missile to be used in the game.
        /// </summary>
        /// <param name="ocean">The ocean to use for this missile.</param>
        /// <param name="type">Missile type. Value can be BasicMissile.</param>
        /// <param name="target">The target input variable.</param>
        public Missile(Ocean ocean, MissileTypes type, string target) : this()
        {
            Ocean = ocean;
            Type = type;
            Target = target;
        }

        #endregion

        #region Public Properties

        public Ocean? Ocean { get; } = null;
        public MissileTypes Type { get; } = MissileTypes.BasicMissile;
        public string Target { get; } = "";

        #endregion

        #region Methods

        /// <summary>
        /// Launch the missile at the specified target in the ocean.
        /// </summary>
        /// <returns>Returns the result of the missile. Value can be NotLaunched, InvalidOcean, InvalidTarget, UsedTarget, Missed, Hit, Sunk, GameOver.</returns>
        public MissileResults Launch()
        {
            MissileResults result = MissileResults.NotLaunched;

            if (Ocean == null) { result = MissileResults.InvalidOcean; }
            if (string.IsNullOrEmpty(Target)) { result = MissileResults.InvalidTarget; }

            if (result != MissileResults.NotLaunched) { return result; }

            result = Ocean!.ProcessMissile(this);

            return result;
        }

        #endregion

    }
}
