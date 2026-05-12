
namespace OTS2026_GrupaB.Models
{

    public enum TileType
    {
        Standard,
        MapBarrier,
        Covered
    }

    public enum TileContent
    {
        Empty,
        UncoverItem,
        Gold
    }

    public  class Tile
    {
        public TileContent Content { get; set; }
        public TileType Type { get; set; }

        public Tile()
        {
            Content = TileContent.Empty;
            Type = TileType.Standard;
        }

        public Tile(TileContent content)
        {
            Content = content;
        }
    }
}
