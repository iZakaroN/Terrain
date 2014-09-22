using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Sunshine.World.Terrain;
using Sunshine.World;

namespace Sunshine.Rendering
{
    public class TileRender
    {
        public Options Options = new Options();
        public TranslateTransform TranslateTile = new TranslateTransform(0, 0);
        public ScaleTransform ScaleTile;
        public Transform TransformTile;
        //int[,] MapIndex;

        UIElement[,] MapControls;
        World.World World;

        Dispatcher Dispatcher;

        Dictionary<TerrainType, TileBitmap> TerrainImages;

        public int MapOffsetX;
        public int MapOffsetY;
        public bool NeedRefresh = false;

        public TileRender(World.World world, Dispatcher dispatcher)
        {
            World = world;
            Dispatcher = dispatcher;
            MapControls = new UIElement[World.MapTilesHeight, World.MapTilesWidth];

            MapOffsetX = 50 * Options.TileSize - Options.TileSize / 2;
            MapOffsetY = 50 * Options.TileSize - Options.TileSize / 2;
            ScaleTile = new ScaleTransform(1, 1, MapOffsetX, MapOffsetY);
            TransformTile = new TransformGroup() { Children = new TransformCollection() { ScaleTile, TranslateTile } };

            InitializeTerrainBitmaps();
            InitializeTerrain();
        }

        private void InitializeTerrainBitmaps()
        {
            TerrainImages = new Dictionary<TerrainType, TileBitmap>()
            {
                { TerrainType.None,     new TileBitmap("System/X.gif", Options) },
                { TerrainType.Dirt,     new TileBitmap("Dirt/S2.jpg", Options)  },
                { TerrainType.DeepWater,     new TileBitmap("Water/S2.jpg", Options)  },
                { TerrainType.Water,    new TileBitmap("Water/S1.jpg", Options)  },
                { TerrainType.Sand,     new TileBitmap("Sand/W3.jpg", Options)  },
                { TerrainType.Swamp,     new TileBitmap("Swamp/S1.jpg", Options)  },
                { TerrainType.Grass,    new TileBitmap("Grass/S1.jpg", Options)  },
                { TerrainType.Forest,    new TileBitmap("Forest/S1.jpg", Options)  },
                { TerrainType.Rock,     new TileBitmap("Rock/R1.jpg", Options)  },
                { TerrainType.Snow,     new TileBitmap("Snow/S1.png", Options)  },
                { TerrainType.Ice,     new TileBitmap("Ice/S1.jpg", Options)  },
            };
        }

        private void InitializeTerrain()
        {
            for (int y = 0; y < World.MapTilesHeight; y++)
                for (int x = 0; x < World.MapTilesWidth; x++)
                    PushTile(x, y);
        }

        internal UIElement PopTile(int tilePositionX, int tilePositionY)
        {
            Debug.Assert(MapControls[tilePositionY, tilePositionX] != null);
            var result = MapControls[tilePositionY, tilePositionX];
            //MapControls[y, x] = null;
            return result;
        }

        internal UIElement PushTile(int tilePositionX, int tilePositionY)
        {
            return PushTile(tilePositionX, tilePositionY, World.GetMapTile(tilePositionX, tilePositionY));
        }

        internal UIElement PushTile(int tilePositionX, int tilePositionY, MapTile tile)
        {
            if (MapControls[tilePositionY, tilePositionX] == null)
                MapControls[tilePositionY, tilePositionX] = CreateMapTileUI(tile);
            return MapControls[tilePositionY, tilePositionX];
            //var result = CreateMapTileUI(tile);
            //Debug.Assert(MapControls[y, x] == null);
            //MapControls[y, x] = result;
            //return result;
        }

        private UIElement CreateMapTileUI(MapTile tile)
        {
			return this.Dispatcher.Invoke(() =>
				{

					/*var ttw = tile.Terrain.Type.First();
					var result = CreateMapTileImage(TerrainImages[ttw.Type].Bitmap, 1);
					return InitializeTileUI(result);*/

					var result = InitializeTileUI(new Canvas() { ClipToBounds = true });

					var totalWeight = tile.Terrain.Type.Aggregate(0, (seed, ttw) => seed += ttw.Weight); //Calculate total weight if the tile
					tile.Terrain.Type.ForEach((ttw) => 
						{
							result.Children.Add(CreateMapTileImage(TerrainImages[ttw.Type].Bitmap, (double)ttw.Weight / totalWeight)); // Create tile image with weight->opacity
						});
					return result;
				});
        }

		private T InitializeTileUI<T>(T element) 
			where T : FrameworkElement
		{
			element.Width = Options.TileSize + 1;
			element.Height = Options.TileSize;
			element.RenderTransform = TransformTile;
			return element;
		}

        private Image CreateMapTileImage(ImageSource source, double opacity)
        {
            return new Image()
                {
                    Source = source,
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Stretch,
					Opacity = opacity,
                };
        }

        public void SetTransition(int mapWidth, int mapHeight)
        {
            TranslateTile.Y = mapHeight / 2 - MapOffsetY;
            TranslateTile.X = mapWidth / 2 - MapOffsetX;
        }

        public void SetScale(int delta)
        {
            /*ScaleTile.CenterX = MapOffsetX;
            ScaleTile.CenterY = MapOffsetY;
            ScaleTile.ScaleX += (double)delta/1200;
            ScaleTile.ScaleY += (double)delta / 1200;*/
            double factor = Options.TileSize;
            Options.TileSize += (int)((double)delta / 120) * 10;
            factor = Options.TileSize / factor;
            MapOffsetX = (int)(MapOffsetX * factor);
            MapOffsetY = (int)(MapOffsetY * factor);

            NeedRefresh = true;

        }

    }
}
