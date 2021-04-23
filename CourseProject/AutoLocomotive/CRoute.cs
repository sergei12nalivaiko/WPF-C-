using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace AutoLocomotive
{
    public  class CRoute: INotifyPropertyChanged
    {
        private string previous_name_of_station;
        private string next_name_of_station;
        private int current_km;
        private int current_picket;
        public int max_speed_route=80;


        public string Previous_Name_Of_Station
        {
            get { return previous_name_of_station; }
            set
            {
                previous_name_of_station = value;
                OnPropertyChanged("Previous_Name_Of_Station");
            }
        }

        public string Next_Name_Of_Station
        {
            get { return next_name_of_station; }
            set
            {
                next_name_of_station = value;
                OnPropertyChanged("Next_Name_Of_Station");
            }
        }

        public int Current_KM
        {
            get { return current_km; }
            set
            {
                current_km = value;
                OnPropertyChanged("Current_KM");
            }
        }

        public int Current_Picket
        {
            get { return current_picket; }
            set
            {
                if (value > 9)
                {
                    current_picket = 0;
                }
                else current_picket = value;
                OnPropertyChanged("Current_Picket");
            }
        }


        public CRoute(string next_name_of_station, string previous_name_of_station, int current_km,int current_picket)
        {
            this.Next_Name_Of_Station = next_name_of_station;
            this.Previous_Name_Of_Station = previous_name_of_station;
            this.Current_KM = current_km;
            this.Current_Picket = current_picket;
        }




        public void Create_Route(StreamGeometry geometry, List<Point> p)
        {
            using (StreamGeometryContext context = geometry.Open())
            {
                context.BeginFigure(p[0], true /*isFilled*/, false /*isClosed*/);
                context.PolyQuadraticBezierTo(p, true, false);
            }
        }


        public Label Station(string name, int width, int height, double left, double top, double right, double bottom)
        {
            Label newLabel = new Label();
            newLabel.Content = name;
            newLabel.Width = width;
            newLabel.Height = height;
            newLabel.Margin = new Thickness(left, top, right, bottom);
            newLabel.FontSize = 13;
            newLabel.FontWeight = FontWeights.Bold;
            newLabel.Foreground = Brushes.Black;
            return newLabel;
        }

        public Line LStation(int x1, int y1, int x2, int y2)
        {
            Line newLine = new Line();
            newLine.Stroke = System.Windows.Media.Brushes.Black;
            newLine.X1 = x1;
            newLine.Y1 = y1;
            newLine.X2 = x2;
            newLine.Y2 = y2;
            return newLine;
        }

        public  Dictionary<Point, string> station_in_route = new Dictionary<Point, string>
        {
            {new Point() { X=100},"Верейцы"},
            {new Point() { X=240},"Талька"},
            {new Point() { X=550},"Пуховичи"},
            {new Point() { X=740},"Руденск" },
            {new Point() { X=970},"Колядичи"},
            {new Point() { X=1040},"Минск"},
        };


        public int counter_for_km = 1;
        public void Kilometr(List<Point> points, Point start)
        {
            if(start.X>=points[counter_for_km].X)
            {
                Current_KM++;
                counter_for_km++;
            }
        }


       public Point picket;
       
       public int counter_for_picket = 0;
        public void Picket(List<Point> points, Point start)
        {
            while (start.X >= points[counter_for_picket].X && start.X <= points[counter_for_picket+1].X)
            {
                Current_Picket++;
                counter_for_picket++;
                break;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
