using OTS2026_GrupaB.Exceptions;
using OTS2026_GrupaB.Models;


namespace OTS2026_GrupaB
{
    public enum Move
    {
        Up,
        Down,
        Left,
        Right,
        Back,
        Forward
    }

    public enum Score
    {
        Bad,
        Average,
        Good
    }

    public class Game
    {
        public Player Player { get; set; }
        public Space Map { get; set; }


        public Game(Position playerPosition, Position uncoverItemPosition)
        {
            Map = new Space();
            Map.InitializeMap();

            if (!ValidatePositionInsideMap(playerPosition) || !ValidatePositionInsideMap(uncoverItemPosition))
            {
                throw new PositionOutsideOfMapException("Positions must be valid!");
            }

            int itemX = uncoverItemPosition.X;
            int itemY = uncoverItemPosition.Y;
            int itemZ = uncoverItemPosition.Z;

            Map.Tiles[itemX, itemY, itemZ].Content = TileContent.UncoverItem;
            Player = new Player(playerPosition);
        }

        public void MovePlayer(Move move)
        {
            Position playerPositionAfterMove = Player.GetPositionAfterMove(move);
            bool positionIsValid = ValidatePosition(playerPositionAfterMove);
            if (positionIsValid)
            {
                Player.MakeMove(move);
            }
        }

        public bool ValidatePosition(Position position)
        {
            int x = position.X;
            int y = position.Y;
            int z = position.Z;

            if (!ValidatePositionInsideMap(position))
            {
                return false;
            }
            if(Map.Tiles[x, y, z].Type.Equals(TileType.Covered))
            {
                return Player.CanUncover;
            }
            else
            {
                return true;
            }
        }

        private bool ValidatePositionInsideMap(Position position)
        {
            int x = position.X;
            int y = position.Y;
            int z = position.Z;

            if (x < 0 || x >= Space.MapSize || y < 0 || y >= Space.MapSize)
            {
                return false;
            }
            if (Map.Tiles[x, y, z].Type.Equals(TileType.MapBarrier))
            {
                return false;
            }
            return true;
        }

        public void CollectItems()
        {
            int x = Player.Position.X;
            int y = Player.Position.Y;
            int z = Player.Position.Z;

            if (Map.Tiles[x, y, z].Content.Equals(TileContent.Gold))
            {
                if (Map.Tiles[x, y, z].Type.Equals(TileType.Covered))
                    Player.AmountOfCoveredGold++;
                else
                    Player.AmountOfGold++;
            }
            else if(Map.Tiles[x, y, z].Content.Equals(TileContent.UncoverItem))
            {
                Player.CanUncover = true;
            }

            Map.EmptyTileOnPosition(Player.Position);
        }


        public Score CalculateScore()
        {
            if(Player.AmountOfCoveredGold > 15)
            {
                return Score.Good;
            }
            if(Player.AmountOfGold >= 10 && Player.CanUncover)
            {
                if (Player.AmountOfCoveredGold >= 6)
                {
                    return Score.Good;
                }
                else
                {
                    return Score.Average;
                }
            }
            return Score.Bad;
        }

    }
}
