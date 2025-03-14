using Battle_ships.Enums;
using Battle_ships.Classes;
using System.Diagnostics;
using System.Net.Security;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;


try
{

    Console.WriteLine("\nWelcome to Battle ships!\n");

    //Get all the configuration settings from appsettings.json and validate them
    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
    var config = builder.Build();
    var Ocean_xsize = config["Ocean:xsize"];
    var Ocean_ysize = config["Ocean:ysize"];
    string? environment = config["Environment"];
    var Battleships = config["Ships:Battleships"];
    var Destroyers = config["Ships:Destroyers"];
    if (Ocean_xsize == null || !int.TryParse(Ocean_xsize, out int xsize)) { throw new Exception("Config Error - Ocean:xsize missing or invalid. Please check the configuration in appsettings.json."); }
    if (xsize > 26) { throw new Exception("Config Error - Ocean:xsize invalid. Application only supports an xsize of 26. Please check the configuration in appsettings.json."); }
    if (Ocean_ysize == null || !int.TryParse(Ocean_ysize, out int ysize)) { throw new Exception("Config Error - Ocean:ysize missing or invalid. Please check the configuration in appsettings.json."); }
    if (environment == null || string.IsNullOrEmpty(environment)) { throw new Exception("Config Error - Environment missing or invalid. Please check the configuration in appsettings.json."); }
    if (Battleships == null || !int.TryParse(Battleships, out int BattleshipCount)) { throw new Exception("Config Error - Ships:Battleships missing or invalid. Please check the configuration in appsettings.json."); }
    if (Destroyers == null || !int.TryParse(Destroyers, out int DestroyerCount)) { throw new Exception("Config Error - Ships:Destroyers missing or invalid. Please check the configuration in appsettings.json."); }

    //Create the Ocean, specifying the grid size
    Ocean ocean = new(xsize, ysize);

    //Create and add the ships to the ocean at random locations
    for (int i = 0; i < BattleshipCount; i++) { ocean.PlaceShipRandom(new Ship(ShipTypes.Battleship)); }
    for (int i = 0; i < DestroyerCount; i++) { ocean.PlaceShipRandom(new Ship(ShipTypes.Destroyer)); }

    string IntroMessage1 = "The ocean has " + ocean.Targets.Count() + " possible targets, ranging from " + ocean.Targets.First() + " to " + ocean.Targets.Last();
    string IntroMessage2 = "There are " + ocean.Ships.Count() + " enemy ships in total, good luck hunting.\n";

    // Loop until the Ocean Status confirms all ships have been destroyed
    while (!ocean.AllShipsDestroyed)
    {
        Console.Clear();

        Console.WriteLine(IntroMessage1);
        Console.WriteLine(IntroMessage2);

        //DEBUG message to show ship locations (only if a debugger is attached)
        if (environment!.ToUpper().Trim() == "DEVELOPMENT")
        {
            foreach (Ship ship in ocean.Ships.Where(s => s.Targets.Count > 0))
            {
                Console.WriteLine(String.Format("DEBUG - {0} with targets still to hit {1}", ship.Type.ToString(), String.Join(", ", ship.Targets)));
            }
            Console.WriteLine("");
        }

        //Ask the player for a target to hit
        Console.WriteLine("Enter your target location, e.g. (C5):");
        string? InputTarget = Console.ReadLine();
        string Target = "";
        if (InputTarget != null) { Target = InputTarget.ToUpper().Trim(); }

        Console.WriteLine("");

        Missile missile = new(ocean, MissileTypes.BasicMissile, Target);

        //Fire the missile and handle the responses
        switch (missile.Launch())
        {
            case MissileResults.InvalidOcean:
                Console.WriteLine("Error: The ocean is not valid, closing the application.");
                Environment.Exit(1);
                break;
            case MissileResults.InvalidTarget:
                Console.WriteLine("Error: You need to enter a valid target location, for examle B9 or F3.");
                break;
            case MissileResults.UsedTarget:
                Console.WriteLine("You have already tried that target. Please try again.");
                break;
            case MissileResults.Missed:
                Console.WriteLine("Sorry, you missed.");
                break;
            case MissileResults.Hit:
                Console.WriteLine("BOOM!! Direct hit");
                break;
            case MissileResults.Sunk:
                Console.WriteLine("SHIP DESTROYED !!");
                break;
            case MissileResults.GameOver:
                //Show Ocean Status - To get here all ships need to be sunk
                Console.WriteLine("Well done, all ships have been destroyed. Thanks for playing.");
                break;
            default:
                break;
        }

        Console.WriteLine("\nPress enter to continue.\n");
        Console.ReadLine();


    }

}
catch (Exception ex)
{
    Console.WriteLine("\n\nOpps, something went wrong. Exception : " + ex.Message + "\n\n");
}
