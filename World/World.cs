using Sunshine.World.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunshine.World
{
    public class World
    {
        public int MapTilesWidth = 64;
        public int MapTilesHeight = 64;

        MapTile[,] MapTiles;

        public World(int width, int height)
        {
            MapTilesWidth = width;
            MapTilesHeight = height;
            MapTiles = new MapTile[MapTilesHeight + 1, MapTilesWidth + 1];
            GenerateMapHeightField();
        }

        /*private void GenerateMapRandomTiles()
        {
            Random randomizer = new Random();
            for (int y = 0; y < MapTilesHeight; y++)
                for (int x = 0; x < MapTilesWidth; x++)
                    MapTiles[y, x] = new MapTile()
                    {
                        Terrain = new Terrain.Terrain()
                        {
                            Type = new TerrainTile() { new TerrainTypeWeight((TerrainType)(randomizer.Next((int)TerrainType.LastType - 1) + 1), 100) }
                        },
                        X = x,
                        Y = y
                    };
        }*/

        const int StartMapDepthLow = 100000;//Maximum starting depth (0->StartMaxMapDepth)
        //const int StartMapDepthHigh = StartMapDepthLow + Overheight;//Maximum starting depth (0->StartMaxMapDepth)
        const int Overheight = 8848;
        const int Underheight = 11022;

        MapSliders StartSliders = new MapSliders()
        {
            UnderheightTh = 2800,
            OverheightTh = 2800,
            UnderheightLn = 23000,
            OverheightLn = 23000,
            MinimumDistance = 12000,
        };

        struct MapSliders
        {
            public int OverheightTh;//How much terrain can overheight its parents(neighbours) in thousandth
            public int UnderheightTh;//How much terrain can underheight its parents(neighbours) in thousandth

            public int OverheightLn;//How much terrain can overheight its parents(neighbours) in units (addition)
            public int UnderheightLn;//How much terrain can underheight its parents(neighbours) in units (addition)

            public int MinimumDistance;

            internal MapSliders Next(int x, int y)
            {
                var result = new MapSliders()
                {
                    //UnderheightTh = (int)(Math.Sqrt(UnderheightTh) * 24), OverheightTh = (int)(Math.Sqrt(OverheightTh) * 24),
                    //UnderheightTh = (int)Math.Pow(UnderheightTh, (double)98 / 100),//* 80 / 100,
                    UnderheightTh = UnderheightTh* 75 / 100,
                    //OverheightTh = (int)Math.Pow(OverheightTh, (double)98 / 100),//OverheightTh * 80 / 100,
                    OverheightTh = OverheightTh * 75 / 100,
                    //UnderheightLn = (int)Math.Pow(UnderheightLn, (double)90 / 100),//UnderheightLn * 00 / 100,
                    UnderheightLn = UnderheightLn * 45 / 100,
					//UnderheightLn = UnderheightLn - 4000,
					//OverheightLn = (int)Math.Pow(OverheightLn, (double)90 / 100),//OverheightLn * 00 / 100,
                    OverheightLn = OverheightLn * 45 / 100,
					//OverheightLn = OverheightLn - 4000,
					//MinimumDistance = (int)Math.Pow(MinimumDistance, (double)00 / 100),//OverheightLn * 00 / 100,
                    MinimumDistance = MinimumDistance * 00 / 100,
                };
                result.Init();
                return result;
            }

            private void Init()
            {
                OverheightTh = Math.Max(0, OverheightTh);
                UnderheightTh = Math.Max(0, UnderheightTh);

                OverheightLn = Math.Max(0, OverheightLn);
                UnderheightLn = Math.Max(0, UnderheightLn);
            }
        }

        class DepthToTerrain
        {
            public TerrainType Type;
            public int Depth;
        }

        public const int HeightTerrainTypeDeepWater = StartMapDepthLow - Underheight * 2;
        public const int HeightTerrainTypeWater = StartMapDepthLow - Underheight;
        public const int HeightTerrainTypeSand = StartMapDepthLow;
        public const int HeightTerrainTypeSwamp = StartMapDepthLow + Overheight * 020 / 100;
        public const int HeightTerrainTypeGrass = StartMapDepthLow + Overheight * 030 / 100;
        public const int HeightTerrainTypeForest = StartMapDepthLow + Overheight * 060 / 100;
        public const int HeightTerrainTypeRock = StartMapDepthLow + Overheight * 100 / 100;
        public const int HeightTerrainTypeSnow = StartMapDepthLow + Overheight * 130 / 100;
        public const int HeightTerrainTypeIce = StartMapDepthLow + Overheight * 180 / 100;

        List<DepthToTerrain> DepthToTerrainList = new List<DepthToTerrain>()
        {
                new DepthToTerrain() { Type = TerrainType.None,     Depth = int.MinValue, },
                new DepthToTerrain() { Type = TerrainType.DeepWater,     Depth = HeightTerrainTypeDeepWater, },
                new DepthToTerrain() { Type = TerrainType.Water,    Depth = HeightTerrainTypeWater, },
                new DepthToTerrain() { Type = TerrainType.Sand,     Depth = HeightTerrainTypeSand, },
                new DepthToTerrain() { Type = TerrainType.Swamp,    Depth = HeightTerrainTypeSwamp, },
                new DepthToTerrain() { Type = TerrainType.Grass,    Depth = HeightTerrainTypeGrass, },
                new DepthToTerrain() { Type = TerrainType.Forest,   Depth = HeightTerrainTypeForest, },
                new DepthToTerrain() { Type = TerrainType.Rock,     Depth = HeightTerrainTypeRock, },
                new DepthToTerrain() { Type = TerrainType.Snow,     Depth = HeightTerrainTypeSnow, },
                new DepthToTerrain() { Type = TerrainType.Ice,      Depth = HeightTerrainTypeIce, },
        };

        Random Randomizer = new Random(0);
        private void GenerateMapHeightField()
        {

            /*MapTiles[0, 0] = GenerateTile(HeightTerrainTypeSnow);
            MapTiles[MapTilesHeight, 0] = GenerateTile(HeightTerrainTypeDeepWater);
            MapTiles[0, MapTilesWidth] = GenerateTile(HeightTerrainTypeDeepWater);
            MapTiles[MapTilesHeight, MapTilesWidth] = GenerateTile(HeightTerrainTypeGrass);*/

            MapTiles[0, 0] = GenerateTile(HeightTerrainTypeSand);
            MapTiles[MapTilesHeight, 0] = GenerateTile(HeightTerrainTypeSand);
            MapTiles[0, MapTilesWidth] = GenerateTile(HeightTerrainTypeSand);
            MapTiles[MapTilesHeight, MapTilesWidth] = GenerateTile(HeightTerrainTypeSand);
            GenerateMap(0, 0, MapTilesWidth, MapTilesHeight, StartSliders/*.Next()*/);
            GenerateNormals();
        }

        private void GenerateNormals()
        {
            for (var y = 0; y <= MapTilesHeight; y++)
            {
                for (var x = 0; x <= MapTilesWidth; x++)
                {
                    int aZ, bZ, cZ, dZ;
                    aZ = (x > 0) ? MapTiles[y, x - 1].Depth : MapTiles[y, x].Depth;
                    bZ = (y > 0) ? MapTiles[y - 1, x].Depth : MapTiles[y, x].Depth;
                    cZ = (x < MapTilesWidth) ? MapTiles[y, x + 1].Depth : MapTiles[y, x].Depth;
                    dZ = (y < MapTilesHeight) ? MapTiles[y + 1, x].Depth : MapTiles[y, x].Depth;

                    MapTiles[y, x].Data = new TileData(aZ, bZ, cZ, dZ);//new TileData(aZ - cZ, bZ - dZ, 2);
                }
            }
        }

        private void GenerateMap(int left, int top, int right, int bottom, MapSliders sliders)
        {
            var lt = MapTiles[top, left].Terrain.Depth;
            var lb = MapTiles[bottom, left].Terrain.Depth;
            var rt = MapTiles[top, right].Terrain.Depth;
            var rb = MapTiles[bottom, right].Terrain.Depth;

            int midV = (top + bottom) / 2;
            int midH = (left + right) / 2;


            if (MapTiles[midV, left] == null)
                MapTiles[midV, left] = GenerateTile(GenerateDepthSide(lt, lb, sliders));//Mid Left
            if (MapTiles[midV, right] == null)
                MapTiles[midV, right] = GenerateTile(GenerateDepthSide(rt, rb, sliders));//Mid Right 
            if (MapTiles[top, midH] == null)
                MapTiles[top, midH] = GenerateTile(GenerateDepthSide(lt, rt, sliders));//Mid Top
            if (MapTiles[bottom, midH] == null)
                MapTiles[bottom, midH] = GenerateTile(GenerateDepthSide(lb, rb, sliders));//Mid Bottom

            //MapTiles[midV, midH] = GenerateTile(GenerateDepthMid(lt, lb, rt, rb, sliders));//Mid 
            MapTiles[midV, midH] = GenerateTile(GenerateDepthMid(
                MapTiles[midV, left].Terrain.Depth,
                MapTiles[midV, right].Terrain.Depth,
                MapTiles[top, midH].Terrain.Depth,
                MapTiles[bottom, midH].Terrain.Depth, sliders));//Mid 

            if ((bottom - top) > 2)
            {
                var newSliders = sliders.Next(left, top);
                GenerateMap(left, top, midH, midV, newSliders);
                GenerateMap(midH, top, right, midV, newSliders);
                GenerateMap(left, midV, midH, bottom, newSliders);
                GenerateMap(midH, midV, right, bottom, newSliders);
            }
        }

        private int Max(int lt, int lb, int rt, int rb)
        {
            return Math.Max(lt, Math.Max(lb, Math.Max(rt, rb)));
        }

        private int Min(int lt, int lb, int rt, int rb)
        {
            return Math.Min(lt, Math.Min(lb, Math.Min(rt, rb)));
        }

        private int GenerateDepthMid(int limit1, int limit2, int limit3, int limit4, MapSliders sliders)
        {
            int middle = (limit1 + limit2 + limit3 + limit4) / 4;

            int deltaO = Max(limit1, limit2, limit3, limit4) - middle;
            int deltaU = middle - Min(limit1, limit2, limit3, limit4);

            int overweight = deltaO * sliders.OverheightTh / 1000 + sliders.OverheightLn;
            int underweight = deltaU * sliders.UnderheightTh / 1000 + sliders.UnderheightLn;

            return middle + Randomizer.Next(overweight + underweight) - underweight;

            return (limit1 + limit2 + limit3 + limit4) / 4;
        }

        private int GenerateDepthSide(int limit1, int limit2, MapSliders sliders)
        {
            //var depth = (limit1 + limit2) / 2;

            int delta = Math.Abs(limit1 - limit2) / 2;
            int overweight = delta * sliders.OverheightTh / 1000 + sliders.OverheightLn;
            int underweight = delta * sliders.UnderheightTh / 1000 + sliders.UnderheightLn;
            int slide = Randomizer.Next(overweight + underweight - sliders.MinimumDistance) - underweight;
            slide = slide + ((slide>0)?sliders.MinimumDistance/2: -sliders.MinimumDistance/2);
            return (limit1 + limit2) / 2 + slide;
            //return (limit1 + limit2) / 2 + Randomizer.Next(sliders.Overheight + sliders.Underheight) - sliders.Underheight;
        }

        private MapTile GenerateTile(int depth)
        {
            return new MapTile() { Terrain = new Terrain.Terrain() { Depth = depth, Type = GenerateTileTerrain(depth), } };
        }


        private TerrainTile GenerateTileTerrain(int depth)
        {
            int i = DepthToTerrainList.Count - 2;
            while (i > 0 && DepthToTerrainList[i].Depth > depth) { i--; };
            var tl = DepthToTerrainList[i];
            var th = DepthToTerrainList[i + 1];
            return new TerrainTile() 
				{
					//new TerrainTypeWeight( TerrainType.Dirt, 1 ),
					//new TerrainTypeWeight( tl.Type, 1 ),
					new TerrainTypeWeight( tl.Type, depth - tl.Depth ),
					new TerrainTypeWeight( th.Type, th.Depth - depth ),
				};// {  DepthToTerrainList[i].Type;
        }

        public MapTile GetMapTile(int x, int y) { return MapTiles[y, x]; }

        public MapTile ShiftTileType(int x, int y)
        {
            var result = MapTiles[y, x];
            result.Terrain.Type = new TerrainTile(result.Terrain.Type.Select((ttw) =>
                {
                    var nextTerrainType = (int)ttw.Type + 1;
                    if (nextTerrainType == (int)TerrainType.LastType)
                        nextTerrainType = (int)TerrainType.None;
                    return new TerrainTypeWeight((TerrainType)nextTerrainType, ttw.Weight);
                }));
            return result;
        }

        public void SetTileType(int x, int y, TerrainTile type)
        {
            MapTiles[y, x].Terrain.Type = type;
        }


    }
}
