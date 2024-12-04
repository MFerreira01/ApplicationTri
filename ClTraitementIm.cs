using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTri
{
    internal class ClTraitementIm
    {
        public IntPtr Img;

        public ClTraitementIm()
        {
            Img = IntPtr.Zero;
        }


        // Déclaration de la fonction native sans paramètres par défaut
        [DllImport("DllProj.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr histogramme(bool enregistrementCSV);

        public IntPtr HistogrammeCS(bool enregistrementCSV = false)
        {
            Img = histogramme(enregistrementCSV);
            return Img;
        }

        [DllImport("DllProj.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr creerImageApartirTableau(int hauteur, int largeur, byte[] pixels);

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
        public IntPtr ConvertirEnImageNDG(Bitmap capturedImage)
        {
            // Convertir l'image en un tableau d'octets
            byte[] pixelData = BitmapToByteArray(capturedImage);

            // Créer l'image dans la DLL à partir du tableau
            IntPtr imagePtr = ClTraitementIm.creerImageApartirTableau(capturedImage.Height, capturedImage.Width, pixelData);

            return imagePtr;
        }

    }
}
