using System;
using System.Collections.Generic;
using System.Text;

namespace MazeAnalyzer
{
    public class MeasureItem
    {
        public double time=0;
        public double pathLength = 0;
        public double velocity = 0;
        public int reEntry = 0;
        public bool lastPosIn = false;

        //private int vCounter = 0;
        //private double vTotal = 0;
        //public void AddVelocityDataPoint(double inp)
        //{
        //    vCounter++;
        //    vTotal += inp;
        //}

        //public double Velocity
        //{
        //    get 
        //    { 
        //        if(vCounter>0) 
        //            return vTotal / vCounter;
        //        else
        //            return 0;
        //    }
        //}

        public List<double> accessTimes = new List<double>();
        public List<double> accessTimePathLength = new List<double>();
        public List<string> accessDescription = new List<string>();

    }


}
