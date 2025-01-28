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
using static System.Net.Mime.MediaTypeNames;

using System.Net;
using System.Net.Sockets;
using smcs;

using System.IO;


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

        private IPAddress m_ipAdrServeur;
        private IPAddress m_ipAdrClient;
        private int m_numPort;
        private int m_numImg = 1;

        private Bitmap capturedImage; // Variable pour stocker l'image capturée
        private Bitmap bufferedImage;
        /*        private ClTraitementIm Histogramme;*/

        string imagePath;

        public Form1()
        {
            InitializeComponent();
            /*Histogramme = new ClTraitementIm();*/

            m_ipAdrServeur = IPAddress.Parse("192.168.56.2");  // Adresse locale
            m_ipAdrClient = IPAddress.Parse("192.168.1.150");   // Adresse distante
            m_numPort = 8001;
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


            labelAdressIP.Text = "No camera connected";

            // discover all devices on network
            smcsVisionApi.FindAllDevices(3.0);
            smcs.IDevice[] devices = smcsVisionApi.GetAllDevices();
            if (devices.Length <= 0) return;
            m_device = devices[0];

            if (m_device == null || !m_device.Connect()) return;

            labelAdressIP.Text = "Camera address:" + Common.IpAddrToString(m_device.GetIpAddress());

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
                pbImageCam.Image = bitmap;
                bufferedImage = bitmap;
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

        private delegate void affImage(PictureBox Pbx, Bitmap bmp);
        private void GetImg(PictureBox Pb, Bitmap btmp)
        {
            if (Pb.InvokeRequired)
            {
                affImage d;
                d = new affImage(GetImg);
                this.Invoke(d, new object[] { Pb, btmp });
            }
            else btmp = new Bitmap(Pb.Image);
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
       
        public static byte[] BitmapToByteArray(Bitmap bitmap)
        // On transforme l'image Bitmap en tableau pour pouvoir créer une imageNDG et utiliser la fonction Histogramme
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            // Verifiez que l'image est en niveau de gris, ou effectuez la conversion si nécessaire
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height),
                                              ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            int bytes = data.Stride * data.Height;
            byte[] pixelData = new byte[bytes];
            Marshal.Copy(data.Scan0, pixelData, 0, bytes);
            bitmap.UnlockBits(data);

            return pixelData;
        }

        static double CalculerMoyenneNDG(Bitmap image)
        {
            double sommeNDG = 0;
            int totalPixels = image.Width * image.Height;

            // Parcourir tous les pixels de l'image
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // Obtenir la couleur du pixel
                    Color pixelColor = image.GetPixel(x, y);

                    // Calculer le niveau de gris du pixel (moyenne des composantes RGB)
                    int niveauGris = (int)((pixelColor.R + pixelColor.G + pixelColor.B) / 3.0);

                    // Ajouter à la somme des niveaux de gris
                    sommeNDG += niveauGris;
                }
            }

            // Calculer la moyenne
            return sommeNDG / totalPixels;
        }
        static double CalculerMin(Bitmap image)
        {
            double min = 255;

            // Parcourir tous les pixels de l'image
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // Obtenir la couleur du pixel
                    Color pixelColor = image.GetPixel(x, y);

                    // Calculer le niveau de gris du pixel (moyenne des composantes RGB)
                    int niveauGris = (int)((pixelColor.R + pixelColor.G + pixelColor.B) / 3.0);

                    // Ajouter à la somme des niveaux de gris
                    if(niveauGris<min) min = niveauGris;
                }
            }

            return min;
        }
        static double CalculerMax(Bitmap image)
        {
            double max = 0;

            // Parcourir tous les pixels de l'image
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // Obtenir la couleur du pixel
                    Color pixelColor = image.GetPixel(x, y);

                    // Calculer le niveau de gris du pixel (moyenne des composantes RGB)
                    int niveauGris = (int)((pixelColor.R + pixelColor.G + pixelColor.B) / 3.0);

                    // Ajouter à la somme des niveaux de gris
                    if (niveauGris > max) max = niveauGris;
                }
            }

            return max;
        }

        private void buttonInit_Click(object sender, EventArgs e)
        {

            return;
/*            if (m_device !=null)
            {
                labelAdressIP.Text = "Adresse IP:" + Common.IpAddrToString(m_device.GetIpAddress());
                this.lblConnection.BackColor = Color.LimeGreen;
                this.lblConnection.Text = "Connection établie";
            }
            else
            {
                labelAdressIP.Text = "pas connecté";
                this.lblConnection.BackColor = Color.Orange;
                this.lblConnection.Text = "Connection en cours";
            }

            bool enregistrementCSV = true;

            ulong[] histogramme = DLL.CalculerHistogrammeManaged(image, enregistrementCSV);

            // Affichage des résultats
            for (int i = 0; i < histogramme.Length; i++)
            {
                Console.WriteLine($"Valeur {i} : {histogramme[i]}");
            }*/

        }

        private void envoieInfo(String str)

        {
            TcpClient tcpClient = new TcpClient();
            this.tbCom.AppendText("Connexion en cours...\r\n");

            tcpClient.Connect(m_ipAdrClient, m_numPort);

            this.tbCom.AppendText("Connexion etablie\r\n");

            NetworkStream resStream = tcpClient.GetStream();

            ASCIIEncoding asciiEncod = new ASCIIEncoding();
            byte[] asciiCode = asciiEncod.GetBytes(str);
            resStream.Write(asciiCode, 0, asciiCode.Length);
            this.tbCom.AppendText("Transmission : " + str + "\r\n");

            this.tbCom.AppendText("Reception : \r\n");
            byte[] mesRecu = new byte[1024];
            int k = resStream.Read(mesRecu, 0, 1024);
            for (int i = 0; i < k; i++)
                this.tbCom.AppendText(Convert.ToChar(mesRecu[i]).ToString() + "\r\n");
            str = "";
            for (int i = 0; i < k; i++)
                str = str + Convert.ToChar(mesRecu[i]);
            this.tbCom.AppendText("Message recu : " + str + "\r\n");

            tcpClient.Close();
        }
        private String recevoirInfo()
        {
            TcpListener tcpList;
            Socket sock;
            String message="";

            /* Initializes the Listener */
            tcpList = new TcpListener(m_ipAdrServeur, m_numPort);

            /* Start Listeneting at the specified port */
            tcpList.Start();
            this.tbCom.AppendText("Le serveur en cours d'execution...\r\n");
            this.tbCom.AppendText("Le point de terminaison local  :" + tcpList.LocalEndpoint.ToString() + "\r\n");
            this.tbCom.AppendText("Attente de connexion.....\r\n");

            sock = tcpList.AcceptSocket();
            this.tbCom.AppendText("Connexion acceptee de " + sock.RemoteEndPoint + "\r\n");

            byte[] b = new byte[1024];
            int k = sock.Receive(b);
            this.tbCom.AppendText("Reception...\r\n");
            for (int i = 0; i < k; i++)
            {
                this.tbCom.AppendText(Convert.ToChar(b[i]).ToString() + "\r\n");
                message += Convert.ToChar(b[i]).ToString();
            }
                
            ASCIIEncoding asen = new ASCIIEncoding();
            sock.Send(asen.GetBytes("Information recue par le serveur.\r\n"));
            this.tbCom.AppendText("\r\nAccusé de reception envoyé.\r\n");
            tcpList.Stop();
            sock.Close();

            return message;
        }
        private void ACQ()
        {
            try
            {
                string message = recevoirInfo();
                if (message != "1")
                {
                    MessageBox.Show("Message reçu != 1, l'acquisition ne sera pas lancé");
                }
                else
                {
                    // Capture une image
                    // Essayer de capturer une image
                    try
                    {
                        capturedImage = CaptureImage();
                    }
                    // Si le buffer est vide et que l'on ne peut donc pas prendre l'image, on prend la dernière image bufferisé (nouvelle image toutes les 5 secondes)
                    catch
                    {
                        capturedImage = bufferedImage;
                    }

                    // Affiche l'image capturée dans le PictureBox
                    if (capturedImage != null)
                    {
                        GetImg(pbImageCam, capturedImage);
                        pbImageCapture.Image = capturedImage;
                        MessageBox.Show("Image capturée et stockée en mémoire !");
                        bool obj;

                        double moyenneNDG = CalculerMoyenneNDG(capturedImage);

                        if (moyenneNDG > 128)
                        {
                            labelDécision.Text = "Décision : Objet blanc";
                            obj = true;
                        }
                        else
                        {
                            labelDécision.Text = "Décision : Objet noir";
                            obj = false;
                        }
                        envoieInfo(obj.ToString());
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void SaveImage(Bitmap image, string filePath)
        {
            try
            {
                using (Bitmap copy = new Bitmap(image))
                {
                    copy.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
                }
                MessageBox.Show($"Image sauvegardée dans : {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde de l'image : {ex.Message}");
            }
        }

        private void BoutonACQ_Click(object sender, EventArgs e)
        {
            // Capture une image
            
            /* capturedImage = chargerImage();*/
            capturedImage = CaptureImage();

            // Affiche l'image capturée dans le PictureBox
            if (capturedImage != null)
            {
                GetImg(pbImageCam, capturedImage);
                pbImageCapture.Image = capturedImage;
                MessageBox.Show("Image capturée et stockée en mémoire !");
                bool obj;

                double moyenneNDG = CalculerMoyenneNDG(capturedImage);

                if (moyenneNDG > 128)
                {
                    labelDécision.Text = "Décision : Objet blanc";
                    obj = true;
                }
                else
                {
                    labelDécision.Text = "Décision : Objet noir";
                    obj = false;
                }

                // sauvegarder l'image
                SaveImage(capturedImage, $"C:\\Users\\maria\\Downloads\\imgProjVision\\imgCapturee_{m_numImg}.bmp");
                m_numImg++;

                /*envoieInfo(obj.ToString());*/

                // Calculer l'histogramme

                /*  IntPtr resultat = Histogramme.HistogrammeAPartirTableau(capturedImage.Height, capturedImage.Width, BitmapToByteArray(capturedImage), enregistrementCSV: true);

                   // Faire quelque chose avec `resultat`
                   Console.WriteLine("Histogramme calculé et instance native créée.");*/
            }
            else
            {
                capturedImage = bufferedImage;
               
                pbImageCapture.Image = capturedImage;
                MessageBox.Show("Image capturée et stockée en mémoire !");
                bool obj;

                double moyenneNDG = CalculerMoyenneNDG(capturedImage);

                if (moyenneNDG > 128)
                {
                    labelDécision.Text = "Décision : Objet blanc";
                    obj = true;
                }
                else
                {
                    labelDécision.Text = "Décision : Objet noir";
                    obj = false;
                }
            }
        }

        System.Drawing.Image img;
        private void buttonOuvrir_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";

                if (ouvrirImage.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    imagePath = ouvrirImage.FileName;
                }
                img = System.Drawing.Image.FromFile(ouvrirImage.FileName);
                pbImageCam.Image = new Bitmap(img);

                MessageBox.Show(imagePath);
            }
        }

        private void buttonTraiter_Click(object sender, EventArgs e)
        {
            Bitmap img = ClTraitementIm.StretchImageDynamic(imagePath);
            pbImageCapture.Image = img;
            pbImageCapture.Image = ClTraitementIm.ProcessImage(img, 40);
            img = ClTraitementIm.ProcessImage(img, 40);
            img = ClTraitementIm.inverser(img);

            // Détection des pixels blancs
            List<PointF> points = ClTraitementIm.GetPointsFromBitmap(img, Color.White);

            if (points.Count == 0)
            {
                Console.WriteLine("Aucun point blanc détecté.");
                return;
            }

            // Étape 1: Calcul de la moyenne
            double[] mean = ClTraitementIm.CalculateMean(points);

            // Étape 2: Centrage des données
            double[,] centeredData = ClTraitementIm.CenterData(points, mean);

            // Étape 3: Calcul de la matrice de covariance
            double[,] covarianceMatrix = ClTraitementIm.ComputeCovarianceMatrix(centeredData);

            // Étape 4: Décomposition propre
            (double[] eigenValues, double[,] eigenVectors) = ClTraitementIm.EigenDecomposition(covarianceMatrix);

            /// Étape 5: Dessin des axes principaux
            string outputPath = "";
            pbImageCapture.Image = ClTraitementIm.DrawAxesOnImage(img, mean, eigenVectors, outputPath, saveToFile: false);

            // Récupérer l'axe principal (le premier vecteur propre)
            double vx = eigenVectors[0, 0]; // Composante x du vecteur propre principal
            double vy = eigenVectors[1, 0]; // Composante y du vecteur propre principal

            // Calculer l'angle entre la verticale et l'axe principal
            double[] axePrincipal = { vx, vy };
            double angle = ClTraitementIm.CalculerAngleVerticale(axePrincipal);

            MessageBox.Show($"L'angle entre la verticale et l'axe principal est : {angle} degrés \n centre de gravité : " + ClTraitementIm.GetCenterOfGravity(img));

            // Dessiner l'arc sur l'image
            string sortie = "C:\\Users\\maria\\Downloads\\imgProjVision\\image_modifiee_avec_arc.jpg";
            int radius = 150;  
            ClTraitementIm.DrawAngleArcOnImage(img, axePrincipal, angle, radius, sortie);


            /*double moyenneNDG = CalculerMoyenneNDG((Bitmap)img);
            double min = CalculerMin((Bitmap)img);
            double max = CalculerMax((Bitmap)img);
            MessageBox.Show("moyenne " + moyenneNDG + "\n min " + min + "\n max " + max);*/
        }

    }
}
