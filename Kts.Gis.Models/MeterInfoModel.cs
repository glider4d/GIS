using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Kts.Gis.Models
{
    public class MeterInfoModel : INotifyPropertyChanged
    {

        

        public double t1
        {
            get;set;
        }

        public double t2
        {
            get;set;
        }

        public double deltaT
        {
            get;set;
        }

        public DateTime dateTime
        {
            get;set;
        }

        public double q1
        {
            get;set;
        }

        public double q2
        {
            get;set;
        }

        public double deltaQ
        {
            get;set;
        }

        public double v1
        {
            get;set;
        }

        public double v2
        {
            get;set;
        }

        public double deltaV
        {
            get;set;
        }

        public double m1
        {
            get;set;
        }

        public double m2
        {
            get;set;
        }

        public double deltaM
        {
            get;set;
        }

        private double m_Param1;
        public double Param1
        {
            get
            {
                return m_Param1;
            }
            set
            {
                m_Param1 = value;
                OnPropertyChanged("Param1");
            }
        }

        private double m_Param2;
        public double Param2
        {
            get
            {
                return m_Param2;
            }
            set
            {
                m_Param2 = value;
                OnPropertyChanged("Param2");
            }
        }

        private double m_DeltaParam;
        public double DeltaParam
        {
            get
            {
                return m_DeltaParam;
            }
            set
            {
                m_DeltaParam = value;
                OnPropertyChanged("DeltaParam");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        public void selectParam(int indexOfParam)
        {
            switch (indexOfParam)
            {
                case 0: Param1 = t1; Param2 = t2; DeltaParam = deltaT; break;
                case 1: Param1 = v1; Param2 = v2; DeltaParam = deltaV; break;
                case 2: Param1 = q1; Param2 = q2; DeltaParam = deltaQ; break;
                case 3: Param1 = m1; Param2 = m2; DeltaParam = deltaM; break;
            }
        }

        public void setValue(int param, double value)
        {
            if (param == 1)
                t1 = value;
            if (param == 2)
                t2 = value;
            if (param == 47)
                deltaT = value;
            //1, 2, 47 : T_in, T_out, T_Delta;  5,6,50 : M_in, M_out, M_Delta;7,8,49 V_in, V_Out, V_Delta; 9,10,11: Q_in, Q_out, Q_Delta;
            if (param == 5)
                m1 = value;
            if (param == 6)
                m2 = value;
            if (param == 50)
                deltaM = value;

            if (param == 7)
                v1 = value;
            if (param == 8)
                v2 = value;
            if (param == 49)
                deltaV = value;

            if (param == 9)
                q1 = value;
            if (param == 10)
                q2 = value;
            if (param == 11)
                deltaQ = value;

        }

        public MeterInfoModel(double t1, double t2, double deltaT, DateTime dateTime)
        {
            this.t1 = t1;
            this.t2 = t2;
            this.deltaT = deltaT;
            this.dateTime = dateTime;
        }
    }
}
