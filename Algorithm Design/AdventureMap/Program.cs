using System;
using System.Collections.Generic;

namespace AdventureMap
{
    class Program
    {
        /// <summary>
        /// Entry point. Generates a new random map each time the user presses a key.
        /// Press Escape to exit.
        /// </summary>
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                DrawMap(width: 60, height: 20);

                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Escape)
                    break;
            }

            Console.ResetColor();
            Console.CursorVisible = true;
        }

        // =====================================================================
        //  CURVE GENERATION (ABSTRACTION)
        // =====================================================================

        /// <summary>
        /// Generates a curved vertical element by producing one X position per row (Y).
        /// The curve starts at startX and then has a chance to move left or right by 1 step each row.
        /// 
        /// This method is abstract: it does not care if the curve represents a river or a wall.
        /// The caller decides how to draw it (color, width, symbols, etc.).
        /// </summary>
        /// <param name="startX">Initial X position at row 0.</param>
        /// <param name="length">How many rows to generate (typically the map height).</param>
        /// <param name="curveChance">Probability (0..1) of turning left/right on each row.</param>
        /// <param name="minX">Minimum allowed start X (clamping keeps the curve inside the border).</param>
        /// <param name="maxX">Maximum allowed start X (clamping keeps the curve inside the border).</param>
        /// <param name="random">Random generator passed in to avoid re-seeding issues.</param>
        /// <returns>A list where curve[y] is the start X of the curve on row y.</returns>
        static List<int> GenerateCurve(int startX, int length, double curveChance, int minX, int maxX, Random random)
        {
            var curveValues = new List<int>(capacity: length);

            // Keep track of the current X position while building the curve.
            int currentX = Clamp(startX, minX, maxX);

            for (int y = 0; y < length; y++)
            {
                curveValues.Add(currentX);

                // With a certain probability, curve left or right by one step.
                if (random.NextDouble() < curveChance)
                {
                    int direction = random.Next(2); // 0 or 1
                    if (direction == 0) currentX--;
                    else currentX++;

                    currentX = Clamp(currentX, minX, maxX);
                }
            }

            return curveValues;
        }

        /// <summary>
        /// Draws the appropriate curve character ('/', '\', or '|') based on how the curve moves
        /// from the current row to the next row.
        /// </summary>
        /// <param name="curve">The generated curve X positions (one per row).</param>
        /// <param name="y">Current row.</param>
        static void DrawCurve(List<int> curve, int y)
        {
            // For the last row, we cannot look ahead safely, so draw straight.
            if (y >= curve.Count - 1)
            {
                Console.Write("|");
                return;
            }

            // Compare current X with next row X to determine curve direction.
            int dx = curve[y + 1] - curve[y];

            if (dx < 0) Console.Write("/");
            else if (dx > 0) Console.Write("\\");
            else Console.Write("|");
        }

        // =====================================================================
        //  MAP DRAWING
        // =====================================================================

        /// <summary>
        /// Draws the full ASCII adventure map.
        /// The map is drawn row-by-row and character-by-character (similar to fractal drawing),
        /// while using precomputed data (curves and roads) for consistent geometry.
        /// </summary>
        static void DrawMap(int width, int height)
        {
            // ---------------------------
            // Preparation phase
            // ---------------------------

            // We use one Random instance for the entire map to keep randomness stable.
            var random = new Random();

            // Helpful boundaries for positioning elements.
            int leftQuarterEnd = width / 4;
            int rightQuarterStart = (width * 3) / 4;

            // Main road baseline (roughly in the middle).
            int mainRoadBaseY = height / 2;

            // Curves must fit inside the border area (border is at x=0 and x=width-1).
            // We clamp the START position of a curve so that its full width stays inside the map.
            const int riverWidth = 3;
            const int wallWidth = 2;

            int minRiverX = 1;
            int maxRiverX = (width - 2) - (riverWidth - 1);

            int minWallX = 1;
            int maxWallX = (width - 2) - (wallWidth - 1);

            // Generate river and wall paths using the same abstract curve generator.
            // River curves more than the wall.
            List<int> riverStart = GenerateCurve(
                startX: rightQuarterStart,
                length: height,
                curveChance: 0.50,
                minX: minRiverX,
                maxX: maxRiverX,
                random: random
            );

            List<int> wallStart = GenerateCurve(
                startX: leftQuarterEnd,
                length: height,
                curveChance: 0.10,
                minX: minWallX,
                maxX: maxWallX,
                random: random
            );

            // Generate a slightly wavy main road by storing the Y position for each X.
            // This is an example of precomputing an element because it depends on avoiding obstacles.
            List<int> roadY = new List<int>(capacity: width);
            int currentRoadY = mainRoadBaseY;

            for (int x = 0; x < width; x++)
            {
                roadY.Add(currentRoadY);

                // Avoid moving the road too close to the river near its crossing zone.
                // This keeps the road readable and leaves space for bridge/turrets.
                int riverXAtRoad = riverStart[currentRoadY];
                if (x >= riverXAtRoad - 2 && x <= riverXAtRoad + 6)
                    continue;

                // Avoid moving the road too close to the wall near its crossing zone.
                int wallXAtRoad = wallStart[currentRoadY];
                if (x >= wallXAtRoad - 1 && x <= wallXAtRoad + 2)
                    continue;

                // Randomly nudge the road up or down sometimes to create a more organic path.
                // Small probability keeps the road mostly straight.
                int direction = random.Next(7);
                if (direction == 0 && currentRoadY > 1) currentRoadY--;
                if (direction == 1 && currentRoadY < height - 2) currentRoadY++;
            }

            // Find where the "river road" should start (5 left of the river).
            // We search from left to right until we are past the intended river-road column.
            int roadIntersectionX = 0;
            for (int x = 0; x < width; x++)
            {
                int yOnRoad = roadY[x];
                int riverRoadX = riverStart[yOnRoad] - 5;

                if (x > riverRoadX)
                {
                    roadIntersectionX = x;
                    break;
                }
            }
            int roadIntersectionY = roadY[roadIntersectionX];

            // Title positioning: center it horizontally.
            string title = "ADVENTURE MAP";
            int titleX = (width - title.Length) / 2;

            // ---------------------------
            // Drawing phase (priority order)
            // ---------------------------

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 1) Border (always visible)
                    bool onVerticalBorder = x == 0 || x == width - 1;
                    bool onHorizontalBorder = y == 0 || y == height - 1;

                    if (onVerticalBorder || onHorizontalBorder)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        // Corners use '+', horizontal uses '-', vertical uses '|'.
                        if (onVerticalBorder && onHorizontalBorder)
                            Console.Write("+");
                        else if (onHorizontalBorder)
                            Console.Write("-");
                        else
                            Console.Write("|");

                        continue;
                    }

                    // 2) Title (written once and then we skip ahead)
                    if (y == 1 && x == titleX)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(title);

                        // Skip to the end of the title to keep the for-loop consistent.
                        x += title.Length - 1;
                        continue;
                    }

                    // 3) Bridge-like element / Turrets near the wall at road crossing
                    // Turrets are drawn on the rows adjacent to the road to create a "gate" feel.
                    if (y == roadY[x] - 1 || y == roadY[x] + 1)
                    {
                        int wallX = wallStart[roadY[x]];

                        // Draw the two turret characters at the wall position.
                        if (x == wallX)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("[");
                            continue;
                        }
                        if (x == wallX + 1)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("]");
                            continue;
                        }

                        // Bridge over the river: '=' near the river crossing on rows adjacent to the road.
                        int riverX = riverStart[roadY[x]];
                        if (x > riverX - 3 && x < riverX + 5)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("=");
                            continue;
                        }
                    }

                    // 4) Main road
                    if (y == roadY[x])
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("#");
                        continue;
                    }

                    // 5) Road by the river (starts after intersection)
                    int riverRoadColumn = riverStart[y] - 5;
                    if (y > roadIntersectionY && x == riverRoadColumn)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("#");
                        continue;
                    }

                    // 6) River (blue, width 3)
                    if (x >= riverStart[y] && x < riverStart[y] + riverWidth)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        DrawCurve(riverStart, y);
                        continue;
                    }

                    // 7) Wall (gray, width 2)
                    if (x >= wallStart[y] && x < wallStart[y] + wallWidth)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        DrawCurve(wallStart, y);
                        continue;
                    }

                    // 8) Forest (random in left quarter, sparser to the right)
                    if (x < leftQuarterEnd)
                    {
                        // The chance decreases as x increases, making the forest fade out.
                        int forestInverseChance = Math.Max(2, x + 1);

                        if (random.Next(forestInverseChance) == 0)
                        {
                            string trees = "AT@%()";

                            // Slight color variation makes the forest look more alive.
                            Console.ForegroundColor = (random.Next(2) == 0)
                                ? ConsoleColor.Green
                                : ConsoleColor.DarkGreen;

                            Console.Write(trees[random.Next(trees.Length)]);
                            continue;
                        }
                    }

                    // Empty space
                    Console.ResetColor();
                    Console.Write(" ");
                }

                Console.ResetColor();
                Console.WriteLine();
            }
        }

        // =====================================================================
        //  SMALL HELPERS
        // =====================================================================

        /// <summary>
        /// Keeps a value inside a specified range.
        /// This prevents curves from leaving the playable area or overlapping the border.
        /// </summary>
        static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
