using System.Collections.Generic;
using System.Drawing;

namespace Snake;

internal static class GameLevels
{
    private static readonly List<string[]> levels = new()
    {
        new[]
        {
            "WWWEEEEEWWW",
            "WWEEEEEEEWW",
            "WEEEEEEEEEW",
            "EEEEEEEEEEE",
            "EEEEEEEEEEE",
            "EEEEEEEEEEE",
            "EEEEEEEEEEE",
            "EEEEEEEEEEE",
            "WEEEEEEEEEW",
            "WWEEEEEEEWW",
            "WWWEESEEWWW"
        },

        new[]
        {
            "WWWWWWWWWWW",
            "WEEEEEEEEEW",
            "WEEEEEEEEEW",
            "WEEWEEEWEEW",
            "WEEWEEEWEEW",
            "WEEWEEEWEEW",
            "WEEWEEEWEEW",
            "WEEWEEEWEEW",
            "WEEEEEEEEEW",
            "WEEEESEEEEW",
            "WWWWWWWWWWW"
        },

        new[]
        {
            "EEEWEEEWEEWWWW",
            "EEEWEEEWEEEEEW",
            "EEEWEEEWEEEEEW",
            "EEEWEEEWEEEEEE",
            "EEEWEEEWEEEEEE",
            "EEEWEEEWWWWWWW",
            "EEEWEEEEEEEEEW",
            "EEEWEEEEEEEEEW",
            "WWWWEEEWEEEEEE",
            "EEEWEEEWEEEEEE",
            "EEEWESEWEEEEEE",
            "EEEWEEEWEEEEEE"
        },

        new[]
        {
            "EEEWWEEEEEEE",
            "EEEWWEEEEEEE",
            "EEEWWEEEEEEE",
            "EEEWWEEEEEEE",
            "EEEWWEEWWWWW",
            "EEEWWEEWWWWW",
            "EEEEEEEEEEEE",
            "EEEEEEEEEEEE",
            "WWWWWEEWWEEE",
            "WWWWWEEWWEEE",
            "EEEEEEEWWEEE",
            "EEEEEEEWWEEE",
            "EEEEEEEWWEEE",
            "EEEEEEEWWESE"
        },

        new[]
        {
            "WWWWWWEEWWWWWW",
            "WEEEEEEEEEEEEW",
            "WEWWWWEEWWWWEW",
            "WEWEEEEEEEEWEW",
            "WEWEWWEEWWEWEW",
            "WEWEWEEEEWEWEW",
            "EEEEEEWWEEEEEE",
            "EEEEEEWWEEEEEE",
            "WEWEWEEEEWEWEW",
            "WEWEWWEEWWEWEW",
            "WEWEEEEEEEEWEW",
            "WEWWWWEEWWWWEW",
            "WEEEEEEEEEEEEW",
            "WWWWWWSEWWWWWW"
        }
    };

    public static int CountLevels => levels.Count;
    public static Point InitialSnakePosition { get; private set; }

    public static GameMap GetMap(int levelNumber)
    {
        var currentLevel = levels[levelNumber];
        var width = currentLevel[0].Length;
        var height = currentLevel.Length;
        var map = new GameMap(width, height);

        for (var i = 0; i < width; ++i)
        for (var j = 0; j < height; ++j)
            switch (currentLevel[j][i])
            {
                case 'W':
                    map[i, j] = Cell.Wall;
                    break;
                case 'S':
                    InitialSnakePosition = new Point(i, j);
                    map[i, j] = Cell.Snake;
                    break;
                default:
                    map[i, j] = Cell.Empty;
                    break;
            }

        return map;
    }
}