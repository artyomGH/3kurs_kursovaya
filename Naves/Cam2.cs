using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using System.Threading;

namespace Naves
{
    public class Cam2
    {
        public StreamWriter file;
        public Cam2()
        {
            file = new StreamWriter(@"d:\WriteText.txt", true);
        }

        private Capture capture;
        double righteye_coor_x,righteye_coor_y;
        private HaarCascade haarCascade;
        public Thread myThread;
        public static double moviendo_X, moviendo_Y;
        public double tmp_X_new, tmp_X_old, tmp_Y_new, tmp_Y_old;
        public int numbers = 0;
        public void Window_Loaded()
        {
            capture = new Capture();
            haarCascade = new HaarCascade(@"haarcascade_mcs_eyepair_small.xml");
            myThread = new Thread(timer_Tick);
            myThread.IsBackground = false;
            myThread.Start();
        }
        public void timer_Tick()
        {
            Stopwatch stopWatch = new Stopwatch();
            
            //file.WriteLine("haarcascade_lefteye_2splits.xml");
            while (true)
            {
                Image<Bgr, Byte> currentFrame = capture.QueryFrame();
                if (currentFrame != null)
                {
                    Image<Gray, Byte> grayFrame = currentFrame.Convert<Gray, Byte>();

                    //stopWatch.Reset();
                    //stopWatch.Start();
                    var detectedFaces = grayFrame.DetectHaarCascade(haarCascade)[0];
                    //stopWatch.Stop();

                    // Get the elapsed time as a TimeSpan value.
                    //TimeSpan ts = stopWatch.Elapsed;
                    
                    //file.WriteLine( ts.ToString());
                    numbers = detectedFaces.Length;
                    if (numbers > 0)
                    {
                        //tmp_X_new = 1000;
                        righteye_coor_x = 0;
                        for (int i = 0; i < numbers; i++)
                        {
                            if (righteye_coor_x < detectedFaces[i].rect.Width)
                            {
                                righteye_coor_x = detectedFaces[i].rect.Width;
                                tmp_X_new = detectedFaces[i].rect.X +
                                    detectedFaces[i].rect.Width / 2;
                                tmp_Y_new = detectedFaces[i].rect.Y +
                                    (detectedFaces[i].rect.Height) / 2;
                            }

                        }
                    }
                    //if (numbers > 0 )
                    //{
                    //    tmp_X_new = detectedFaces[0].rect.X + (detectedFaces[0].rect.Width) / 2;
                    //    tmp_Y_new = detectedFaces[0].rect.Y + (detectedFaces[0].rect.Height) / 2;
                    //}
                    moviendo_X = tmp_X_old - tmp_X_new;
                    moviendo_Y = tmp_Y_old - tmp_Y_new;
                }
                Thread.Sleep(0);
                
            }
            
        }
        public void setXY()
        {
            tmp_X_old = tmp_X_new;
            tmp_Y_old = tmp_Y_new;
        }
    }
}
