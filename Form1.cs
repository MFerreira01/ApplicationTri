using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace ApplicationTri
{
    public partial class Form1 : Form
    {
        smcs.IDevice m_device;
        Rectangle m_rect;
        PixelFormat m_pixelFormat;
        UInt32 m_pixelType;

        smcs.IImageProcAPI m_imageProcApi;
        smcs.IAlgorithm m_changeBitDepthAlg;
        smcs.IParams m_changeBitDepthParams;
        smcs.IImageBitmap m_changeBitDepthBitmap;

        private Bitmap capturedImage; // Variable pour stocker l'image capturée
        private ClTraitementIm Histogramme;

        public Form1()
        {
            InitializeComponent();
            Histogramme = new ClTraitementIm();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            // initialize GigEVision API
            smcs.CameraSuite.InitCameraAPI();
            smcs.ICameraAPI smcsVisionApi = smcs.CameraSuite.GetCameraAPI();

            smcs.CameraSuite.InitImageProcAPI();
            m_imageProcApi = smcs.CameraSuite.GetImageProcAPI();

            if (!smcsVisionApi.IsUsingKernelDriver())
            {
                Text = Text + " (Warning: Smartek Filter Driver not loaded.)";
            }

            m_changeBitDepthAlg = m_imageProcApi.GetAlgorithmByName("ChangeBitDepth");
            m_changeBitDepthAlg.CreateParams(ref m_changeBitDepthParams);
            m_imageProcApi.CreateBitmap(ref m_changeBitDepthBitmap);


            label1.Text = "No camera connected";

            // discover all devices on network
            smcsVisionApi.FindAllDevices(3.0);
            smcs.IDevice[] devices = smcsVisionApi.GetAllDevices();
            if (devices.Length <= 0) return;
            m_device = devices[0];

            if (m_device == null || !m_device.Connect()) return;

            label1.Text = "Camera address:" + Common.IpAddrToString(m_device.GetIpAddress());

            // disable trigger mode
            bool status = m_device.SetStringNodeValue("TriggerMode", "Off");
            // set continuous acquisition mode
            status = m_device.SetStringNodeValue("AcquisitionMode", "Continuous");
            // start acquisition
            status = m_device.SetIntegerNodeValue("TLParamsLocked", 1);
            status = m_device.CommandNodeExecute("AcquisitionStart");
            timer1.Enabled = true;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            smcs.IImageInfo imageInfo = null;
            if (!m_device.GetImageInfo(ref imageInfo)) return;


            UInt32 pixelType;
            imageInfo.GetPixelType(out pixelType);
            var depth = smcs.CameraSuite.GvspGetBitsDepth((smcs.GVSP_PIXEL_TYPES)pixelType);
            var image = (smcs.IImageBitmap)imageInfo;

            if (depth > 8)
            {
                // to show image we change bit depth to 8 bit
                m_changeBitDepthParams.SetIntegerNodeValue("BitDepth", 8);
                m_imageProcApi.ExecuteAlgorithm(m_changeBitDepthAlg, image, m_changeBitDepthBitmap, m_changeBitDepthParams, null);
                image = m_changeBitDepthBitmap;
            }



            Bitmap bitmap = (Bitmap)pbImageCam.Image;
            BitmapData bd = null;

            ImageUtils.CopyToBitmap(image, ref bitmap, ref bd, ref m_pixelFormat, ref m_rect, ref m_pixelType);

            if (bitmap != null)
            {
                pbImageCam.Height = bitmap.Height;
                pbImageCam.Width = bitmap.Width;
                pbImageCam.Image = bitmap;
            }

            // display image
            if (bd != null)
                bitmap.UnlockBits(bd);

            pbImageCam.Invalidate();
            // remove (pop) image from image buffer
            m_device.PopImage(imageInfo);
            // empty buffer
            m_device.ClearImageBuffer();
        }

        private void OnClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
            if (m_device != null && m_device.IsConnected())
            {
                m_device.CommandNodeExecute("AcquisitionStop");
                m_device.SetIntegerNodeValue("TLParamsLocked", 0);
                m_device.Disconnect();
            }

            m_changeBitDepthAlg.DestroyParams(m_changeBitDepthParams);
            m_imageProcApi.DestroyBitmap(m_changeBitDepthBitmap);

            smcs.CameraSuite.ExitImageProcAPI();
            smcs.CameraSuite.ExitCameraAPI();
        }
        private Bitmap CaptureImage()
        {
            smcs.IImageInfo imageInfo = null;
            if (!m_device.GetImageInfo(ref imageInfo)) return null;

            UInt32 pixelType;
            imageInfo.GetPixelType(out pixelType);
            var depth = smcs.CameraSuite.GvspGetBitsDepth((smcs.GVSP_PIXEL_TYPES)pixelType);
            var image = (smcs.IImageBitmap)imageInfo;

            if (depth > 8)
            {
                // Convertit en 8 bits pour l'affichage
                m_changeBitDepthParams.SetIntegerNodeValue("BitDepth", 8);
                m_imageProcApi.ExecuteAlgorithm(m_changeBitDepthAlg, image, m_changeBitDepthBitmap, m_changeBitDepthParams, null);
                image = m_changeBitDepthBitmap;
            }

            Bitmap bitmap = null;
            BitmapData bd = null;

            ImageUtils.CopyToBitmap(image, ref bitmap, ref bd, ref m_pixelFormat, ref m_rect, ref m_pixelType);

            if (bd != null)
                bitmap.UnlockBits(bd);

            // Libère l'image de la mémoire tampon de la caméra
            m_device.PopImage(imageInfo);
            m_device.ClearImageBuffer();

            return bitmap;
        }

        private void BoutonACQ_Click(object sender, EventArgs e)
        {
            // Capture une image
            capturedImage = CaptureImage();

            // Affiche l'image capturée dans le PictureBox
            if (capturedImage != null)
            {
                pbImageCam.Image = capturedImage;
                MessageBox.Show("Image capturée et stockée en mémoire !");

/*                // Convertir l'image capturée en une image de type CImageNdg
                CImageNdg ImNDG = ConvertirEnImageNDG(capturedImage);

                // Appeler la méthode HistogrammeCS sur l'objet CImageNdg
                ImNDG.HistogrammeCS(true);*/

                /* // Calcul de l'histogramme
                 var histogramData = Histogramme(capturedImage);

                 // Affichage ou utilisation de l'histogramme
                 AfficherHistogramme(histogramData);*/

                Histogramme.HistogrammeCS(true);
            }
            else
            {
                MessageBox.Show("Échec de la capture de l'image.");
            }
        }

        private void buttonInit_Click(object sender, EventArgs e)
        {
            if (m_device !=null)
            {
                label1.Text = "Adresse IP:" + Common.IpAddrToString(m_device.GetIpAddress());
            }
            else
            {
                label1.Text = "pas connecté";
            }

        }

    }
}
