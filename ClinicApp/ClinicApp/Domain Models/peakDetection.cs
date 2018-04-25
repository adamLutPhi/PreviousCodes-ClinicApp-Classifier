using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicApp.Domain_Models
{
    public class peakDetection
    {
        public List<double> Time;
        public int fs = 250; // M.I.T Standard
        public int w = 20;
        public List<double> INT1;
        public List<double> INT2;
        public List<double> S1;
        public List<double> S2;
        public double Max_S1;
        public double Max_S2;
        public List<double> D1;
        public List<double> D2;
        public List<double> DD1;
        public List<double> DD2;
        public List<double> Impulse1;
        public List<double> Impulse2;
        public int Beg;

       public peakDetection(List<SamplingRecord> recs)
        {
            doMagic(recs);
        }

       public peakDetection(List<SamplingRecord> recs, int fs)
        {
            this.fs = fs;
            doMagic(recs);

        }
       public peakDetection(List<SamplingRecord> recs, int fs, int w)
        {
            this.fs = fs;
            this.w = w;

            doMagic(recs);

        }

        public void doMagic(List<SamplingRecord> recs)
        {
            this.Time = new List<double>();
            this.S1 = new List<double>();
            this.S2 = new List<double>();

            foreach (var record in recs) // Adding Values to S1, S2
            {
                int i = recs.IndexOf(record);
                //this.Time.Add(record.SamplingNumber / fs);
                //this.S1.Add(record.LeadII);
                //this.S2.Add(record.LeadV2);
                this.Time.Insert(i, record.SamplingNumber / fs);
                this.S1.Insert(i, record.LeadII);
                this.S2.Insert(i, record.LeadV2);
            }

            this.Max_S1 = S1.Max();
            this.Max_S2 = S2.Max();

            #region Normalization // Data Normalization

            foreach(var value in S1)
            {
                int i = S1.IndexOf(value);
                S1.Insert(i, S1.ElementAt(i) / Max_S1);
                S2.Insert(i, S2.ElementAt(i) / Max_S2);
            }

            #endregion

            #region Plotting //GDI+ Implementation Goes Here...

            #endregion

            #region Differentiation
            
            Differentiate(S1);
            Differentiate(S2);
           
            #endregion

            #region Squaring
            foreach(var val in D1)
            {
                int i = D1.IndexOf(val);
                DD1.Add(D1.ElementAt(i));
                DD2.Add(D2.ElementAt(i));
            }

            #endregion

            #region Integration
            int beg = w / 2;
            for (int i = beg; i < DD1.Count() - beg; i++ )
            {

                INT1.Insert(i, Sum(DD1, i - beg, i + beg));
                INT1.Insert(i, INT1.ElementAt(i) / w);

                INT2.Insert(i, Sum(DD1, i - beg, i + beg));
                INT2.Insert(i, INT2.ElementAt(i) / w);
            }
            double mean1 = Mean(INT1);
            double mean2 = Mean(INT2);

            double max1 = INT1.Max();
            double max2 = INT2.Max();
            foreach(var value in INT1)
            {
              int i = INT1.IndexOf(value);
              INT1.Insert(i, INT1.ElementAt(i) - mean1);
              INT1.Insert(i, INT1.ElementAt(i) / max1);

              INT2.Insert(i, INT2.ElementAt(i) - mean2);
              INT2.Insert(i, INT2.ElementAt(i) / max2);
            }
            #endregion

            #region Thresholding
            double comparison = 0.3*INT1.Max();
            
            foreach(var val in INT1)
            {
                int i = INT1.IndexOf(val);
               
                if(INT1.ElementAt(i)>comparison)
                {
                    Impulse1.Add(INT1.ElementAt(i));
                }

                if(INT2.ElementAt(i)>comparison)
                {
                    Impulse2.Add(INT2.ElementAt(i));
                }

            }
            #endregion

            #region  Plotting //GDI+ Implementation Goes Here...
            #endregion

        }
   #region CoreFunctions
        public List<double> Differentiate(List<double>Input)
        {
            for (int i = 0; i < Input.Count; i++)
            {
                Input.Insert(i, Input.ElementAt(i + 1) - Input.ElementAt(i));
            }

                //foreach (var val in Input)
                //{
                //    int i = Input.IndexOf(val);
                //    if (i == Input.Count)
                //    { break; }
                //    else
                //    {
                //        Input.Insert(i, Input.ElementAt(i + 1) - Input.ElementAt(i));
                //    }
                //}


            return Input;

        }
        public double Sum(List<double>Input,int Start,int Finish)
        {
            double total = 0.0;

            for (int i = Start; i <= Finish; i++)
            {
                total += Input.ElementAt(i);
            }
            return total;
        }
        
        public double Mean(List<double> Input)
        {
          return  Input.Sum() / Input.Count();
        }

        //public void STD()
        //{ }
   #endregion

    }
}
