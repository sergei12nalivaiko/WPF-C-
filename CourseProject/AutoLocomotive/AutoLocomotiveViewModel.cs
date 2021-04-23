using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace AutoLocomotive
{
    public class AutoLocomotiveViewModel:Window, INotifyPropertyChanged
    {

        //Start CLocomotive

        StreamGeometry geometry;
        EllipseGeometry myEllipseGeometry;
        Path myPath;
        PointAnimation myPointAnimation = new PointAnimation();
        Storyboard ellipse = new Storyboard();
        List<Point> points;
        Path myPath1 = new Path();
        Canvas c;
        public int coordinates = 0;
        public double t = 0.05;
        Point start = new Point(20, 95);

        private CLocomotive locomotive_parameters;
        
        public CLocomotive Locomotive_Parameters
        {
            get { return locomotive_parameters; }
            set
            {
                locomotive_parameters = value;
                OnPropertyChanged("Locomotive_Parameters");
            }
        }


        private CRoute route_parameters;

        public CRoute Route_Parameters
        {
            get { return route_parameters; }
            set
            {
                route_parameters = value;
                OnPropertyChanged("Route_Parameters");
            }
        }

        public AutoLocomotiveViewModel()
        {
            locomotive_parameters = new CLocomotive(0, 0, 5, 0);
            route_parameters = new CRoute("Верейцы","Осиповичи",386, 0);
        }
    
 
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private ICommand _SubmitCommand;
        public ICommand SubmitCommand
        {
            get
            {
                if (_SubmitCommand==null)
                {
                    _SubmitCommand = new RelayCommand(SubmitExecute, CanSubmitExecute);
                }
                return _SubmitCommand;
            }

        }

      

        private void SubmitExecute()
        {
            Moving();
        }

        private bool CanSubmitExecute()
        {
            return true;
        }

        DispatcherTimer timer = new DispatcherTimer();
        //Moving on map
        DispatcherTimer timer1 = new DispatcherTimer();


        //Auto moving
        public void DispatcherTimerSample()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            timer1.Interval = TimeSpan.FromSeconds(8);
            timer1.Tick += Location;
            timer1.Start();
        }

        //Manual moving
        public void DispatcherTimerSampleMoving_Manual()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += manual;
            timer.Start();

            timer1.Interval = TimeSpan.FromSeconds(8);
            timer1.Tick += Location;
            timer1.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
           locomotive_parameters.Accelerate();
           locomotive_parameters.TimeInRoute();
           locomotive_parameters.DistanceTravelled();
           locomotive_parameters.DriverController();
           //route_parameters.Kilometr(points, start);
           //route_parameters.Picket(points, start);
        }

        void manual(object sender, EventArgs e)
        {
            locomotive_parameters.TimeInRoute();
            locomotive_parameters.DistanceTravelled();
            locomotive_parameters.StartMovingManualSpeed();
            //locomotive_parameters.ControllerSpeedPlus();
            //route_parameters.Kilometr(points, start);
            //route_parameters.Picket(points, start);
        }

        public void Moving()
        {
            DispatcherTimerSample();
            myEllipseGeometry = new EllipseGeometry();
            NameScope.SetNameScope(this,new NameScope());
            myEllipseGeometry.Center = points[0];
            myEllipseGeometry.RadiusX = 6;
            myEllipseGeometry.RadiusY = 6;
            this.RegisterName("my", myEllipseGeometry);
            myPath1.Fill = Brushes.Red;
            myPath1.Margin = new Thickness(0);
            myPath1.Data = myEllipseGeometry;
            myPointAnimation.Duration = TimeSpan.FromHours(2);
            myPointAnimation.RepeatBehavior = RepeatBehavior.Forever;
            c.Children.Add(myPath1);

            Storyboard.SetTargetName(myPointAnimation, "my");
            Storyboard.SetTargetProperty(myPointAnimation, new PropertyPath(EllipseGeometry.CenterProperty));
            ellipse.Children.Add(myPointAnimation);
            ellipse.Begin(this);
        }

        public void MovingManual()
        {
            locomotive_parameters.StartMovingManualPositionKM();
            DispatcherTimerSampleMoving_Manual();
            myEllipseGeometry = new EllipseGeometry();
            NameScope.SetNameScope(this, new NameScope());
            myEllipseGeometry.Center = points[0];
            myEllipseGeometry.RadiusX = 6;
            myEllipseGeometry.RadiusY = 6;
            this.RegisterName("my", myEllipseGeometry);
            myPath1.Fill = Brushes.Red;
            myPath1.Margin = new Thickness(0);
            myPath1.Data = myEllipseGeometry;
            myPointAnimation.Duration = TimeSpan.FromHours(2);
            myPointAnimation.RepeatBehavior = RepeatBehavior.Forever;
            c.Children.Add(myPath1);
            Storyboard.SetTargetName(myPointAnimation, "my");
            Storyboard.SetTargetProperty(myPointAnimation, new PropertyPath(EllipseGeometry.CenterProperty));
            ellipse.Children.Add(myPointAnimation);
            ellipse.Begin(this);
        }


        public void Location(object sender, EventArgs e)
        {
            Point a = new Point(); ;
            Point b = new Point();
            Point d = new Point();

            Point a1 = new Point(); ;
            Point b1 = new Point();
            Point d1 = new Point();

            IEnumerable <string> values= route_parameters.station_in_route.Values;
            System.Collections.IEnumerator v = values.GetEnumerator();

            if (coordinates + 2 < points.Count - 1 & locomotive_parameters.Current_Speed > 0)
            {
                if (t <= 1)
                {
                    a.X = ((1 - t) * points[coordinates].X) + (t * points[coordinates + 1].X);
                    a.Y = ((1 - t) * points[coordinates].Y) + (t * points[coordinates + 1].Y);

                    b.X = ((1 - t) * points[coordinates + 1].X) + (t * points[coordinates + 2].X);
                    b.Y = ((1 - t) * points[coordinates + 1].Y) + (t * points[coordinates + 2].Y);

                    d.X = ((1 - t) * a.X) + (t * b.X);
                    d.Y = ((1 - t) * a.Y) + (t * b.Y);

                    a1.X = ((1 - t + t) * points[coordinates].X) + ((t + t) * points[coordinates + 1].X);
                    a1.Y = ((1 - t + t) * points[coordinates].Y) + ((t + t) * points[coordinates + 1].Y);

                    b1.X = ((1 - t + t) * points[coordinates + 1].X) + ((t + t) * points[coordinates + 2].X);
                    b1.Y = ((1 - t + t) * points[coordinates + 1].Y) + ((t + t) * points[coordinates + 2].Y);

                    d1.X = ((1 - t + t) * a1.X) + (t + t * b1.X);
                    d1.Y = ((1 - t + t) * a1.Y) + (t + t * b1.Y);

                    t = t + 0.05;
                }
                if (t > 1)
                {
                    coordinates = coordinates + 2;
                    t = 0.05;
                }
                myPointAnimation.From = start;
                myPointAnimation.To = d;
                ellipse.Begin(this);
                start = d;
                d = d1;
                route_parameters.Kilometr(points, start);
                route_parameters.Picket(points, start);
            }
            foreach (var pair in route_parameters.station_in_route)
            {
                if (start.X >= pair.Key.X)
                {
                    route_parameters.Previous_Name_Of_Station = pair.Value;
                }                            
            }


            foreach(string s in values)
            {
                if(v.MoveNext() && route_parameters.Previous_Name_Of_Station==v.Current.ToString())
                {
                    if(v.MoveNext())
                    {
                        route_parameters.Next_Name_Of_Station =(string) v.Current;
                    }
                }
            }
        }
        //End CLocomotive

        //Start CRoute


        public void Build_Route()
        {
            geometry = new StreamGeometry();
            myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 5;
            geometry.FillRule = FillRule.EvenOdd;
            myPath.Data = geometry;
            c = Application.Current.MainWindow.FindName("RailwayRoute") as Canvas;
            c.Children.Add(myPath);

                                                          //Осиповичи-Верейцы
            points = new List<Point>() { new Point(20, 95), new Point(30, 95), new Point(40, 90),  new Point(50, 85),
                                         new Point(60, 90),new Point(70, 95),new Point(80, 90),new Point(90,87),new Point (100,90),
                                                           //Верейцы-Талька
                                         new Point (110,92),new Point (120,93),new Point (130,94),new Point (140,92),new Point (150,94),
                                         new Point (160,95),new Point (180,95),new Point (190,96),new Point (200,96),new Point (210,97),
                                         new Point (220,98),new Point (230,98),new Point (240,95),
                                                           //Талька-Пуховичи
                                         new Point (250,94),new Point (260,93),new Point (270,92),new Point (280,91),new Point (290,90),
                                         new Point (300,89),new Point (310,88),new Point (320,91),new Point (330,94),new Point (340,94),
                                         new Point (350,93),new Point (360,92),new Point (370,91),new Point (380,90),new Point (390,90),
                                         new Point (400,90),new Point (410,89),new Point (420,89),new Point (430,88),new Point (440,88),
                                         new Point (450,87),new Point (460,85),new Point (470,80),new Point (480,87),new Point (490,85),
                                         new Point (500,85),new Point (510,80),new Point (520,80),new Point (530,78),new Point (540,80),
                                         new Point (550,82),
                                                           //Пуховичи-Руденск
                                         new Point (550,85),new Point (560,90),new Point (570,90),new Point (580,85),
                                         new Point (590,85),new Point (600,87),new Point (610,90),new Point (620,85),new Point (630,80),
                                         new Point (640,80),new Point (650,80),new Point (660,85),new Point (670,87),new Point (680,90),
                                         new Point (690,92),new Point (700,95),new Point (710,95),new Point (720,92),new Point (730,92),
                                         new Point (740,87),
                                                           //Руденск-Колядичи
                                         new Point (750,90),new Point (760,90),new Point (770,85),new Point (780,85),new Point (790,80),
                                         new Point (800,80),new Point (810,85),new Point (820,90), new Point (830,90),new Point (840,95),
                                         new Point (850,95),new Point (860,95),new Point (870,90),new Point (880,90),new Point (890,85),
                                         new Point (900,85), new Point (910,80),new Point (920,75),new Point (930,70),new Point (940,70),
                                         new Point (960,75),new Point (970,75),
                                                           //Колядичи-Минск
                                         new Point (980,80),new Point (990,80),new Point (1000,85),new Point (1010,85),new Point (1020,90),
                                         new Point (1030,90), new Point (1030,87),new Point (1040,85)
            };

            route_parameters.Create_Route(geometry, points);
            Label osipovichi = new Label();
            osipovichi = route_parameters.Station("ст.Осиповичи", 100, 40, 0, 20, 0, 20);
            c.Children.Add(osipovichi);
            Line line_osipovichi = new Line();
            line_osipovichi = route_parameters.LStation(20, 40, 20, 95);
            c.Children.Add(line_osipovichi);

            Label vereitsy = new Label();
            vereitsy = route_parameters.Station("ст.Верейцы", 100, 40, 70, 40, 0, 20);
            c.Children.Add(vereitsy);
            Line line_vereitsy = new Line();
            line_vereitsy = route_parameters.LStation(90, 60, 90, 90);
            c.Children.Add(line_vereitsy);

            Label talka = new Label();
            talka = route_parameters.Station("ст.Талька", 100, 40, 220, 20, 0, 20);
            c.Children.Add(talka);
            Line line_talka = new Line();
            line_talka = route_parameters.LStation(238, 40, 238, 95);
            c.Children.Add(line_talka);

            Label puhovichi = new Label();
            puhovichi = route_parameters.Station("ст.Пуховичи", 100, 40, 513, 20, 0, 20);
            c.Children.Add(puhovichi);
            Line line_puhovichi = new Line();
            line_puhovichi = route_parameters.LStation(540, 40, 540, 80);
            c.Children.Add(line_puhovichi);

            Label rudensk = new Label();
            rudensk = route_parameters.Station("ст.Руденск", 100, 40, 710, 20, 0, 20);
            c.Children.Add(rudensk);
            Line line_rudensk = new Line();
            line_rudensk = route_parameters.LStation(730, 40, 730, 90);
            c.Children.Add(line_rudensk);

            Label Kolydichi = new Label();
            Kolydichi = route_parameters.Station("ст.Колядичи", 100, 40, 920, 20, 0, 20);
            c.Children.Add(Kolydichi);
            Line line_Kolydichi = new Line();
            line_Kolydichi = route_parameters.LStation(960, 40, 960, 75);
            c.Children.Add(line_Kolydichi);

            Label Minsk = new Label();
            Minsk = route_parameters.Station("ст.Минск", 100, 40, 970, 6, 0, 20);
            c.Children.Add(Minsk);
            Line line_Minsk = new Line();
            line_Minsk = route_parameters.LStation(1030, 30, 1030, 90);
            c.Children.Add(line_Minsk);
        }



        private ICommand _DrawMap;
        public ICommand DrawMap
        {
            get
            {
                if (_DrawMap == null)
                {
                    _DrawMap = new RelayCommand(SubmitExecute1, CanSubmitExecute1);
                }
                return _DrawMap;
            }

        }

        private void SubmitExecute1()
        {
            Build_Route();
        }

        private bool CanSubmitExecute1()
        {
            return true;
        }



        // start moving manual control
        private ICommand _ManualControl;
        public ICommand ManualControl
        {
            get
            {
                if (_ManualControl == null)
                {
                    _ManualControl = new RelayCommand(SubmitExecute3, CanSubmitExecute3);
                }
                return _ManualControl;
            }

        }

        private void SubmitExecute3()
        {
            MovingManual();
        }

        private bool CanSubmitExecute3()
        {
            return true;
        }



        // +position
        private ICommand _PositionPlus;
        public ICommand PositionPlus
        {
            get
            {
                if (_PositionPlus == null)
                {
                    _PositionPlus = new RelayCommand(SubmitExecute2, CanSubmitExecute2);
                }
                return _PositionPlus;
            }

        }

        private void SubmitExecute2()
        {
            locomotive_parameters.ControllerPositionPlus();
            locomotive_parameters.ControllerSpeedPlus();
        }

        private bool CanSubmitExecute2()
        {
            return true;
        }

        private ICommand _RadioButton_Manual;
        public ICommand RadioButton_Manual
        {
            get
            {
                if (_RadioButton_Manual == null)
                {
                    _RadioButton_Manual = new RelayCommand(SubmitExecute4, CanSubmitExecute4);
                }
                return _RadioButton_Manual;
            }

        }

        public void control_color_button_manual()
        {
            Button start_moving;
            start_moving = Application.Current.MainWindow.FindName("StartMoving") as Button;
            start_moving.Background = Brushes.White;
            Button braking;
            braking = Application.Current.MainWindow.FindName("Braking") as Button;
            braking.Background = Brushes.White;
            Button emergency_braking;
            emergency_braking = Application.Current.MainWindow.FindName("EmergencyBraking") as Button;
            emergency_braking.Background = Brushes.White;
            Button type_position;
            type_position = Application.Current.MainWindow.FindName("TypePosition") as Button;
            type_position.Background = Brushes.White;
            Button reset_position;
            reset_position = Application.Current.MainWindow.FindName("ResetPosition") as Button;
            reset_position.Background = Brushes.White;

            Button start_moving_auto;
                start_moving_auto = Application.Current.MainWindow.FindName("StartMovingAuto") as Button;
                start_moving_auto.Background = Brushes.Gray;
                Button emergency_braking_auto;
                emergency_braking_auto = Application.Current.MainWindow.FindName("EmergencyBrakingAuto") as Button;
                emergency_braking_auto.Background = Brushes.Gray;
        }

        private void SubmitExecute4()
        {
            control_color_button_manual();
        }

        private bool CanSubmitExecute4()
        {
            return true;
        }

        public void control_color_button_auto()
        {
            
            Button start_moving_auto;
            start_moving_auto = Application.Current.MainWindow.FindName("StartMovingAuto") as Button;
            start_moving_auto.Background = Brushes.White;
            Button emergency_braking_auto;
            emergency_braking_auto = Application.Current.MainWindow.FindName("EmergencyBrakingAuto") as Button;
            emergency_braking_auto.Background = Brushes.White;

            Button start_moving;
            start_moving = Application.Current.MainWindow.FindName("StartMoving") as Button;
            start_moving.Background = Brushes.Gray;
            Button braking;
            braking = Application.Current.MainWindow.FindName("Braking") as Button;
            braking.Background = Brushes.Gray;
            Button emergency_braking;
            emergency_braking = Application.Current.MainWindow.FindName("EmergencyBraking") as Button;
            emergency_braking.Background = Brushes.Gray;
            Button type_position;
            type_position = Application.Current.MainWindow.FindName("TypePosition") as Button;
            type_position.Background = Brushes.Gray;
            Button reset_position;
            reset_position = Application.Current.MainWindow.FindName("ResetPosition") as Button;
            reset_position.Background = Brushes.Gray;
           



        }


        private ICommand _RadioButton_Auto;
        public ICommand RadioButton_Auto
        {
            get
            {
                if (_RadioButton_Auto == null)
                {
                    _RadioButton_Auto = new RelayCommand(SubmitExecute5, CanSubmitExecute5);
                }
                return _RadioButton_Auto;
            }

        }
        private void SubmitExecute5()
        {
            control_color_button_auto();
        }

        private bool CanSubmitExecute5()
        {
            return true;
        }

    }
    
}
