using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Drawing;
using Emgu.CV.XFeatures2D;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace Appl
{
    public class DMatchFinder
    {
        public DMatchFinder( MDMatch match, int ind0, int ind1 )
        {
            Match = match;
            Ind0 = ind0;
            Ind1 = ind1;
        }

        public MDMatch Match { get; set; }
        public int Ind0 { get; set; }
        public int Ind1 { get; set; }
    }

    public class ImageHelper
    {
        static ConfigParametersClass conf = MainWindow.Model.ConfigParameter;

        public static void ResizeImageRef( ImageRef img )
        {
            int width = img.Image.ROI.Width;
            int height = img.Image.ROI.Height;
            int sizeRight = img.Image.ROI.X + width;
            int sizeBottom = img.Image.ROI.Y + height;

            int minX = 10000, maxX = 0, minY = 10000, maxY = 0;

            byte[, ,] data = img.Image.Data;
            for ( int indW = img.Image.ROI.X; indW < sizeRight; indW++ )
            {
                for ( int indH = img.Image.ROI.Y; indH < sizeBottom; indH++ )
                {
                    var c = data[indH, indW, 0];

                    if ( c < conf.ThresholdIntensityHigh.Value )
                    {
                        if ( minX > indW )
                            minX = indW;

                        if ( minY > indH )
                            minY = indH;

                        if ( maxX < indW )
                            maxX = indW;

                        if ( maxY < indH )
                            maxY = indH;
                    }
                }
            }

            int delta = 50;
            int x, y, w, h;

            if ( minX - delta > 0 )
                x = minX - delta;
            else
                x = 0;

            if ( minY - delta > 0 )
                y = minY - delta;
            else
                y = 0;

            if ( maxX + delta < img.Image.ROI.Right )
                w = maxX - minX + delta;
            else
                w = img.Image.ROI.Right - minX;

            if ( maxY + delta < img.Image.ROI.Bottom )
                h = maxY - minY + delta;
            else
                h = img.Image.ROI.Bottom - minY;

            var roi = new System.Drawing.Rectangle( x, y, w + delta, h + delta );
            img.Image.ROI = roi;
        }

        public static double GetSize( Image<Gray, Byte> image )
        {
            int nbrPx = 0;
            int width = image.Width;
            int height = image.Height;

            Image<Gray, double> gray_image = image.Convert<Gray, double>();

            double[, ,] data = gray_image.Data;

            for ( int indW = 0; indW < width; indW++ )
            {
                for ( int indH = 0; indH < height; indH++ )
                {
                    var c = gray_image[indH, indW];
                    var a = gray_image[indH, indW];

                    if ( a.Intensity > conf.ThresholdIntensityLow.Value && a.Intensity < conf.ThresholdIntensityHigh.Value )
                    {
                        //image.Data[indH, indW, 0] = 0;
                        nbrPx++;
                    }
                }
            }

            return nbrPx;
        }
        
        public static Image<Gray, byte> RemoveNoise( Image<Gray, byte> img )
        {
            int width = img.Width;
            int height = img.Height;
            byte[, ,] data = img.Data;
            for ( int indW = 0; indW < width; indW++ )
            {
                for ( int indH = 0; indH < height; indH++ )
                {
                    //var c = img.[indH, indW];
                    //var a = img.[indH, indW];
                    if ( data[indH, indW, 0] > conf.ThresholdIntensityHigh.Value )
                        img.Data[indH, indW, 0] = 255;
                }
            }
            return img;
        }

        public static Image<Bgr, byte> RemoveBackGroundColorForAcq( Image<Bgr, byte> img, WindowsAquisition acq )
        {
            Image<Bgr, byte> back = MainWindow.Model.BackgroundColor;

            int col = 3;
            int width = acq.Width;
            int height = acq.Height;
            int sizeRight = acq.PosX + width;
            int sizeBottom = acq.PosY + height;

            int seuil = (int)MainWindow.Model.ConfigParameter.DeltaIntensityRemoveBack.Value;

            byte[,,] data = img.Data;
            for ( int indW = acq.PosX; indW < sizeRight; indW++ )
            {
                for ( int indH = acq.PosY; indH < sizeBottom; indH++ )
                {
                    
                    if( Math.Abs( back.Data[indH, indW, 0] - img.Data[indH, indW, 0] ) < seuil &&
                        Math.Abs( back.Data[indH, indW, 1] - img.Data[indH, indW, 1] ) < seuil &&
                        Math.Abs( back.Data[indH, indW, 2] - img.Data[indH, indW, 2] ) < seuil  )
                    {
                        img.Data[indH, indW, 0] = (byte)( 255 );
                        img.Data[indH, indW, 1] = (byte)( 255 );
                        img.Data[indH, indW, 2] = (byte)( 255 );
                    }
                    
                    /*
                    bool use = false;

                    for ( int d = 0; d < col; d++ )
                    {
                        var b = back.Data[indH, indW, d];
                        var c = img.Data[indH, indW, d];

                        var diff = Math.Abs( c - b );
                        if ( diff > MainWindow.Model.ConfigParameter.DeltaIntensityRemoveBack.Value )
                            use = true;
                    }

                    if ( !use )
                    {
                        img.Data[indH, indW, 0] = (byte)( 255 );
                        img.Data[indH, indW, 1] = (byte)( 255 );
                        img.Data[indH, indW, 2] = (byte)( 255 );
                    }*/
                }
            }
            return img;

        }

        public static Image<Bgr, byte> RemoveBackGroundColor( Image<Bgr, byte> img )
        {
            Image<Bgr, byte> back = MainWindow.Model.BackgroundColor;

            int col = 3;
            int width = img.Width;
            int height = img.Height;
            byte[,,] data = img.Data;


            for ( int indW = 0; indW < width; indW++ )
            {
                for ( int indH = 0; indH < height; indH++ )
                {
                    bool use = false;
                    var diff = 0;
                    
                    for ( int d = 0; d < col; d++ )
                    {
                        var b = back.Data[indH, indW, d];
                        var c = img.Data[indH, indW, d];

                        diff += Math.Abs( c - b );
                    }

                    if ( diff < MainWindow.Model.ConfigParameter.DeltaIntensityRemoveBack.Value )
                    {
                        img.Data[indH, indW, 0] = (byte)( 255 );
                        img.Data[indH, indW, 1] = (byte)( 255 );
                        img.Data[indH, indW, 2] = (byte)( 255 );
                    }

                }
            }

            return img;
        }

        public static Image<Bgr, byte> StandardizeBackGroundColor( Image<Bgr,byte> img )
        {
            Image<Bgr, byte> back = MainWindow.Model.BackgroundColor;

            int col = 3;
            int width = img.Width;
            int height = img.Height;
            byte[,,] data = img.Data;

            for ( int d = 0; d < col; d++ )
            {
                for ( int indW = 0; indW < width; indW++ )
                {
                    for ( int indH = 0; indH < height; indH++ )
                    {
                        var b = back.Data[indH, indW, d];
                        var c = img.Data[indH, indW, d];

                        var diff = c + 255 - b;
                        var val = diff > 255 ? 255 : diff;

                        if ( indH == 500 && indW == 900 )
                        {
                            int zz = 0;
                        }

                        img.Data[indH, indW, d] = (byte)( val );
                    }
                }
            }

            return img;

        }

        public static Image<Gray, byte> UseBackGround( Image<Gray, byte> img, ImageRef back )
        {
            int width = img.Width;
            int height = img.Height;
            byte[,,] data = img.Data;

            for ( int indW = 0; indW < width; indW++ )
            {
                for ( int indH = 0; indH < height; indH++ )
                {
                    var b = back.Image.Data[indH, indW, 0];
                    var c = img.Data[indH, indW, 0];
                    
                    var diff = c + 255 - b;
                    var val = diff > 255 ? 255 : diff;

                    if ( indH == 500 && indW == 900 )
                    {
                        int zz = 0;
                    }

                    img.Data[indH, indW, 0] = (byte)(val);
                }
            }

            return img;
        }

    }
}
