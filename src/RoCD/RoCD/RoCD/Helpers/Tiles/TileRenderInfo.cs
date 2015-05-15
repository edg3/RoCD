using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RoCD.Helpers.Tiles
{
    public class TileRenderInfo
    {
        public Color BackColour = new Color();
        public Color TileColor = new Color();

        public byte TileX = 0;
        public byte TileY = 0;

        public void Write(BinaryWriter writer)
        {
            writer.Write(TileX);
            writer.Write(TileY);
            writer.Write(BackColour.A);
            writer.Write(BackColour.B);
            writer.Write(BackColour.G);
            writer.Write(BackColour.R);
            writer.Write(TileColor.A);
            writer.Write(TileColor.B);
            writer.Write(TileColor.G);
            writer.Write(TileColor.R);
        }

        public void Read(BinaryReader reader)
        {
            TileX = reader.ReadByte();
            TileY = reader.ReadByte();
            BackColour.A = reader.ReadByte();
            BackColour.B = reader.ReadByte();
            BackColour.G = reader.ReadByte();
            BackColour.R = reader.ReadByte();
            TileColor.A = reader.ReadByte();
            TileColor.B = reader.ReadByte();
            TileColor.G = reader.ReadByte();
            TileColor.R = reader.ReadByte();
        }
    }
}
