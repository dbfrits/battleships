using Battle_ships.Enums;

namespace Battle_ships.Classes
{

    /// <summary>
    /// Creates a new blank ship to be used in the game.
    /// </summary>
    public class Ship()
    {

        #region Constructors

        /// <summary>
        /// Creates a new ship of the specified type to be used in the game.
        /// </summary>
        /// <param name="shipType">The type of ship. Value can Battleship, Destroyer.</param>
        public Ship(ShipTypes shipType) : this()
        {
            Type = shipType;

            switch (Type)
            {
                case ShipTypes.Battleship:
                    TargetSize = 5;
                    break;
                case ShipTypes.Destroyer:
                    TargetSize = 4;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Public Properties

        public ShipTypes Type { get; }
        /// <summary>
        /// How many target does this ship occupy.
        /// </summary>
        public int TargetSize { get; }
        /// <summary>
        /// Flag to indicate if the ship has been placed in the ocean.
        /// </summary>
        public bool IsPlaced { get; set; }

        /// <summary>
        /// List of the ocean targets used by this ship when it has been placed.
        /// </summary>
        public List<string> Targets = new List<string>();

        #endregion

    }

}
