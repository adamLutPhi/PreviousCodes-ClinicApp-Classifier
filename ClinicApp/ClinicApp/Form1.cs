using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ClinicApp.Domain_Models;
using System.IO;

namespace ClinicApp
{
    public partial class Form1 : Form
    {
        string path;
        string[] chunk;
        List<SamplingRecord> RecordChunk;
        int currentpointer;
        double estimateTime;
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("Warning:\nData Grid View is For Demo Purposes For Only Dania\n"+
            "It Enables her to Test the Authenticity of the Array Values \n"+
            "It Also Tests the Performance of Arrays & Lists in For & Foreach Loop Scenarios");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string path = Browse();
            string[] lines = File.ReadAllLines(path);
            RecordChunk = new List<SamplingRecord>();
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();

            for (int i = 0; i < lines.Count(); i++)
            {
                chunk = lines[i].Split(' ');           
                SamplingRecord a = new SamplingRecord();

                for (int j = 0; j < chunk.Length; j++)
                {
                    string sample = chunk[j];
                    if (sample != "")
                    {
                        a.SamplingNumber = Convert.ToInt32(sample);
                        currentpointer = j + 1;
                        break;
                    }

                }
                for (int j = currentpointer; j < chunk.Length; j++)
                {
                    string lead2 = chunk[j];
                    if (lead2 != "")
                    {
                        a.LeadII = Convert.ToDouble(lead2);
                        currentpointer = j + 1;
                        break;
                    }

                }
                for (int j = currentpointer; j < chunk.Length; j++)
                {
                    string leadv2 = chunk[j];
                    if (leadv2 != "")
                    {
                        a.LeadV2 = Convert.ToDouble(leadv2);
                        break;
                    }

                }
                RecordChunk.Add(a);
            }
            timer.Stop();

            estimateTime = Convert.ToDouble(timer.ElapsedMilliseconds) / 1000;
            int cols = 3;
            int rows = RecordChunk.Count();
            MessageBox.Show("Processing Time:" + estimateTime + " Seconds \n" + "Record Dimensions are:\n" + cols + " Columns\n" + rows + " Rows");
            //Stopwatch List2ArrayTime = new Stopwatch();
            Stopwatch ListLoopTime = new Stopwatch();
            //Stopwatch ArrayLoopTime = new Stopwatch();

        //    #region Foreach_Performance_Test
        //    /////////////////
        //    ////////////////////////////////////////////////////////////
        //    ///////////////////Description: Uncomment the Following to Test List vs. Array w/ Foreach Performance
        //    ////////////////////////////////////////////////////////////
        //    /////////////////
        
        //Stopwatch ForeachListTime = new Stopwatch();

        //ForeachListTime.Start();
        //foreach (var record in RecordChunk)
        //{
        //    dataGridView1.Rows.Add(record.SamplingNumber, record.LeadII, record.LeadV2);
        //}
        //ForeachListTime.Stop();

        //MessageBox.Show("== Foreach Loop Processing Time ==\nList Time: " + (Convert.ToDouble(ForeachListTime.ElapsedMilliseconds) / 1000) + " Seconds \n");
   
        //    #endregion
        peakDetection pD = new peakDetection(RecordChunk);
        string im = "Impulse 1 values are:\n";
            foreach(var val in pD.Impulse1)
            {
                im += val;
            }
            im += "\nImpulse 2 values are:\n";
            foreach(var val in pD.Impulse2)
            {
                im += val;
            }
        }
        //Leave the Region Underneath Unchanged!
        #region CoreFunctions
        public string Browse()
        {
            // string path;
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.Title = "Open a Text File";
                file.Filter = "text files (*.txt)|*.txt|All files (*.*)|*.*";


                if (file.ShowDialog() == DialogResult.OK)
                {

                    path = file.FileName;
                }

            }
            return path;
        }
        #endregion
    }
}
