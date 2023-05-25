using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Webcam
{
    internal class Webcam_Handler
    {
        private readonly VideoCapture capture;

        public Webcam_Handler()
        {
            capture = new VideoCapture();
        }

        public bool Start()
        {
            // Open webcam and start reading camera frames
            capture.Open(0, VideoCaptureAPIs.DSHOW);

            if (!capture.IsOpened())
            {
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            // Remove the lock on the camera
            capture.Dispose();
        }

        public byte[] Read()
        {
            using var mat = capture.RetrieveMat();

            return mat.ToBytes(
                ".jpg",
                new int[] { (int)ImwriteFlags.JpegQuality, 50 });

            //return mat.ToBytes(); //extracting raw frames without compression causes GC pressure
        }
    }
}
