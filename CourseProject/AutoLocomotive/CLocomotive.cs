using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace AutoLocomotive
{
    public class CLocomotive: INotifyPropertyChanged
    {
        private int current_speed;
        private const int max_speed=95;
        private float distance_travelled;
        private string time_in_route;
        private float brake_line_pressure;
        public DateTime dispatch_time=DateTime.Now;
        private int positionKM;
        public double time=0.0001;

        public CLocomotive(int current_speed, float distance_travelled, float brake_line_pressure, int positionKM)
        {
            this.Current_Speed = current_speed;
            this.Distance_Travelled = distance_travelled;
            this.Brake_Line_Pressure = brake_line_pressure;
            this.PositionKM = positionKM;
        }


        public int Current_Speed
        {
            get { return current_speed; }
            set
            {
                if (current_speed >= 80)
                {
                    current_speed = 80;
                }
                else
                {
                    current_speed = value;
                }
                OnPropertyChanged("Current_Speed");
            }
        }

        public float Distance_Travelled
        {
            get { return distance_travelled; }
            set
            {
                distance_travelled = value;
                OnPropertyChanged("Distance_Travelled");
            }
        }

        public string Time_In_Route
        {
            get { return time_in_route; }
            set
            {
                time_in_route = value;
                OnPropertyChanged("Time_In_Route");
            }
        }      
        public float Brake_Line_Pressure
        {
            get { return brake_line_pressure; }
            set
            {
                brake_line_pressure = value;
                OnPropertyChanged("Brake_Line_Pressure");

            }
        }


        public int PositionKM
        {
            get { return positionKM; }
            set
            {
                positionKM = value;
                OnPropertyChanged("PositionKM");
            }
        }


        // key-speed
        // PositionKM-speed
       public static Dictionary<int, int> driver_controller_position = new Dictionary<int, int>
        {
            {0,0 },
            {1,3 },
            {2,5 },
            {3,10 },
            {4,20 },
            {5,30 },
            {6,40 },
            {7,45 },
            {8,50 },
            {9,60 },
            {10,70 },
            {11,75 },
            {12,80 },
            {13,85 },
            {14,90 },
            {15,95 }
        };
       
        public void Accelerate()
        {
            Current_Speed++; 
        }

        public void TimeInRoute()
        {
            Time_In_Route = (DateTime.Now.Subtract(dispatch_time)).ToString();
        }

        public void DistanceTravelled()
        {
            Distance_Travelled =(float)(time * Current_Speed);
            time = time + 0.0002;
        }

        public void DriverController()
        {
            foreach(var pair in driver_controller_position)
            {
                if(Current_Speed==pair.Value)
                {
                    PositionKM = pair.Key;
                    break;
                }    
            }        
        }


        static IEnumerable<int> speed = driver_controller_position.Values;
        //System.Collections.IEnumerator controller_speed = speed.GetEnumerator();

        static IEnumerable<int> pos_km = driver_controller_position.Keys;
        //System.Collections.IEnumerator controller_position = pos_km.GetEnumerator();

        //static public List<int> hw = new List<int>() { hw.AddRange(speed)};
        System.Collections.Generic.List<int> controller_speed = new List<int>(speed);
        System.Collections.Generic.List<int> controller_position = new List<int>(pos_km);

        public void StartMovingManualPositionKM()
        {
            if(PositionKM==controller_position[0])
            {
                PositionKM=controller_position[counter_for_posittion];
            }
        }


        public void StartMovingManualSpeed()
        {
                if(Current_Speed<controller_speed[counter_for_speed])
                {
                    Current_Speed++;                  
                }   
        }

        public int counter_for_posittion = 1;
        public void ControllerPositionPlus()
        {
            if (counter_for_posittion < controller_position[12])
            {
                counter_for_posittion++;
                PositionKM = controller_position[counter_for_posittion];
            }
        }

        public int counter_for_speed=1;
        public void ControllerSpeedPlus()
        {
            if(counter_for_posittion>counter_for_speed)
            {
                counter_for_speed++;
                if (Current_Speed < controller_speed[counter_for_speed])
                {
                    Current_Speed++;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this,new PropertyChangedEventArgs(prop));
        }

    }
}
