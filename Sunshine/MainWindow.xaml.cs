using Sunshine.Rendering;
using Sunshine.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sunshine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker RenderMapWorker = new BackgroundWorker();
        AutoResetEvent RenderEvent = new AutoResetEvent(false);

        World.World World;
        TileRender TileRender;
        Rect CurrentMapArea = new Rect(0, 0, 0, 0);

        DispatcherPriority Priority = DispatcherPriority.Send;

        public MainWindow()
        {
            InitializeComponent();
            InitializeContent();
            RenderMapWorker = new BackgroundWorker();
            RenderMapWorker.DoWork += new DoWorkEventHandler((obj, args) => RenderMapThread());
            RenderMapWorker.RunWorkerAsync();
            RenderEvent.Set();
        }

        private void InitializeContent()
        {
            Map.ClipToBounds = true;
            World = new World.World(64,64);
            TileRender = new TileRender(World, Map.Dispatcher);
        }

        int TileSize { get { return TileRender.Options.TileSize; } }
        int MapTilesWidth { get { return this.World.MapTilesWidth; } }
        int MapTilesHeight { get { return this.World.MapTilesHeight; } }
        int MapOffsetX { get { return TileRender.MapOffsetX; } set { TileRender.MapOffsetX = value; } }
        int MapOffsetY { get { return TileRender.MapOffsetY; } set { TileRender.MapOffsetY = value; } }


        void RenderMap()
        {
            RenderEvent.Set();
        }

        const int MapTileReserve = 0;
        private void RenderMapThread()
        {
            while (true)
            {
                RenderEvent.WaitOne(TimeSpan.FromSeconds(10));
                Debug.WriteLine(string.Format("InitializeMap: Size {0}, {1}", Map.ActualWidth, Map.ActualHeight));
                //var d = Dispatcher.Invoke(() => Dispatcher.DisableProcessing());
                //var d = Map.Dispatcher.DisableProcessing();


                int top = Math.Max(0, (int)(MapOffsetY - Map.ActualHeight / 2) / TileSize - MapTileReserve);
                int left = Math.Max(0, (int)(MapOffsetX - Map.ActualWidth / 2) / TileSize - MapTileReserve);
                int bottom = Math.Min(MapTilesHeight, (int)(MapOffsetY + Map.ActualHeight / 2) / TileSize + 1 + MapTileReserve);
                int right = Math.Min(MapTilesWidth, (int)(MapOffsetX + Map.ActualWidth / 2) / TileSize + 1 + MapTileReserve);

                Rect newMapArea = new Rect(left, top, right - left, bottom - top);

                var eliminatedTiles = CurrentMapArea.Substract(newMapArea); //Old tiles
                var introducedTiles = newMapArea.Substract(CurrentMapArea);

                CurrentMapArea.Intersect(newMapArea);
                var intersectMapArea = CurrentMapArea;

                CurrentMapArea = newMapArea;

                if (TileRender.NeedRefresh)
                    for (int y = (int)intersectMapArea.Top; y < intersectMapArea.Bottom; y++)
                        for (int x = (int)intersectMapArea.Left; x < intersectMapArea.Right; x++)
                        {
                            TouchTile(x, y);
                        }

                eliminatedTiles.ForEach((r) =>
                {
                    for (int y = (int)r.Top; y < r.Bottom; y++)
                        for (int x = (int)r.Left; x < r.Right; x++)
                        {
                            PopMapTile(x, y);
                        }
                });


                SetMapTransition();
                introducedTiles.ForEach((r) =>
                {
                    for (int y = (int)r.Top; y < r.Bottom; y++)
                        for (int x = (int)r.Left; x < r.Right; x++)
                        {
                            PushMapTile(x, y);
                        }
                });

                TileRender.NeedRefresh = false;
                //d.Dispose();
            }
        }

        void Invoke(Action action)
        {
            Map.Dispatcher.BeginInvoke(new Action(action), Priority);
        }

        private void PopMapTile(int tilePositionX, int tilePositionY)
        {
            var tileControl = TileRender.PopTile(tilePositionX, tilePositionY);
            Invoke(() => Map.Children.Remove(tileControl));
        }

        private void PushMapTile(int tilePositionX, int tilePositionY)
        {
            UIElement tb = TileRender.PushTile(tilePositionX, tilePositionY, World.GetMapTile(tilePositionX, tilePositionY));
            Invoke(() =>
            {
                Map.Children.Add(tb);
                Canvas.SetTop(tb, tilePositionY * TileSize);
                Canvas.SetLeft(tb, tilePositionX * TileSize);
            });
        }

        private void RefreshMapTile(int tilePositionX, int tilePositionY)
        {
            var tileControl = TileRender.PopTile(tilePositionX, tilePositionY);
            UIElement tb = TileRender.PushTile(tilePositionX, tilePositionY, World.GetMapTile(tilePositionX, tilePositionY));
            Invoke(() =>
                {
                    Map.Children.Remove(tileControl);
                    Map.Children.Add(tb);
                    Canvas.SetTop(tb, tilePositionY * TileSize);
                    Canvas.SetLeft(tb, tilePositionX * TileSize);
                });
        }

        private void TouchTile(int tilePositionX, int tilePositionY)
        {
            RefreshMapTile(tilePositionX, tilePositionY);
        }

        private void SetMapTransition()
        {
            Invoke(() =>
                {
                    TileRender.SetTransition((int)Map.ActualWidth, (int)Map.ActualHeight);
                });
        }

        private void SetMapScale(int delta)
        {
            TileRender.SetScale(delta);
            RenderMap();
            /*Map.Dispatcher.Invoke(() =>
            {
                TileRender.SetScale(delta);
            });*/
        }

        private void Map_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RenderMap();
        }

        Point startPoint = new Point(0, 0);
        bool mouseMoved = false;

        private void Map_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startTile = null;
            mouseMoved = false;
            var currentPosition = startPoint = e.GetPosition(Map);
            int tilePositionY = (int)((currentPosition.Y - Map.ActualHeight / 2 + MapOffsetY) / TileSize);
            int tilePositionX = (int)((currentPosition.X - Map.ActualWidth / 2 + MapOffsetX) / TileSize);

            Debug.WriteLine(string.Format("Map_MouseDown.startPoint: {0}, {1}", startPoint.X, startPoint.Y));
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    OnMapLeftMouseDown(tilePositionX, tilePositionY);
                    break;
                case MouseButton.Right:
                    OnMapRightMouseDown(tilePositionX, tilePositionY);
                    break;
            }
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPosition = e.GetPosition(Map);
            int offsetX = (int)(currentPosition.X - startPoint.X);
            int offsetY = (int)(currentPosition.Y - startPoint.Y);
            int tilePositionY = (int)((currentPosition.Y - Map.ActualHeight / 2 + MapOffsetY) / TileSize);
            int tilePositionX = (int)((currentPosition.X - Map.ActualWidth / 2 + MapOffsetX) / TileSize);


            if (e.LeftButton == MouseButtonState.Pressed && startPoint.X != 0)
                OnMapLeftDrag(offsetX, offsetY, tilePositionX, tilePositionY);

            if (e.RightButton == MouseButtonState.Pressed && startPoint.X != 0)
                OnMapRightDrag(offsetX, offsetY, tilePositionX, tilePositionY);

            mouseMoved = true;
            startPoint = currentPosition;
        }

        private void Map_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var currentPosition = e.GetPosition(Map);
            int tilePositionY = (int)((currentPosition.Y - Map.ActualHeight / 2 + MapOffsetY) / TileSize);
            int tilePositionX = (int)((currentPosition.X - Map.ActualWidth / 2 + MapOffsetX) / TileSize);

            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    if (!mouseMoved)
                        OnLeftMapClick(tilePositionX, tilePositionY);
                    break;
                case MouseButton.Right:
                    if (!mouseMoved)
                        OnRightMapClick(tilePositionX, tilePositionY);

                    break;
            }
            startPoint = new Point(0, 0);
            startTile = null;
        }

        MapTile startTile = null;
        private void OnMapLeftMouseDown(int tilePositionX, int tilePositionY)
        {
            Debug.WriteLine(string.Format("OnLeftMapMouseDown tile: {0}, {1}", tilePositionX, tilePositionY));
            startTile = World.GetMapTile(tilePositionX, tilePositionY);
        }

        private void OnMapRightMouseDown(int tilePositionX, int tilePositionY)
        {
            Debug.WriteLine(string.Format("OnRightMapMouseDown tile: {0}, {1}", tilePositionX, tilePositionY));
        }

        private void OnMapLeftDrag(int offsetX, int offsetY, int tilePositionX, int tilePositionY)
        {
            Debug.WriteLine(string.Format("OnRightMapDrag offset: {0}, {1}", offsetX, offsetY));
            if (startTile != null)
            {
                World.SetTileType(tilePositionX, tilePositionY, startTile.Terrain.Type);
                RefreshMapTile(tilePositionX, tilePositionY);
            }
        }

        private void OnMapRightDrag(int offsetX, int offsetY, int tilePositionX, int tilePositionY)
        {
            Debug.WriteLine(string.Format("OnRightMapDrag offset: {0}, {1}", offsetX, offsetY));
            MapOffsetX -= offsetX;
            MapOffsetY -= offsetY;
            RenderMap();
        }

        private void OnLeftMapClick(int tilePositionX, int tilePositionY)
        {
            Debug.WriteLine(string.Format("OnLeftMapClick tile: {0}, {1}", tilePositionX, tilePositionY));
            World.ShiftTileType(tilePositionX, tilePositionY);
            RefreshMapTile(tilePositionX, tilePositionY);
        }

        private void OnRightMapClick(int tilePositionX, int tilePositionY)
        {
            Debug.WriteLine(string.Format("OnRightMapClick tile: {0}, {1}", tilePositionX, tilePositionY));
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void Map_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void Window_MouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            SetMapScale(e.Delta);
        }
    }
}
