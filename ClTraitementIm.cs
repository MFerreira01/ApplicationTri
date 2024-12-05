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
        [DllImport("DllProj.dll", EntryPoint = "histogramme", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr histogramme(bool enregistrementCSV);

        public IntPtr HistogrammeCS(bool enregistrementCSV = false)
        {
            Img = histogramme(enregistrementCSV);
            return Img;
        }

        [DllImport("DllProj.dll", EntryPoint = "creerHistogramme", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr creerHistogramme(bool enregistrementCSV);

        [DllImport("DllProj.dll", EntryPoint = "libererTableau", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libererTableau(IntPtr tableau);


        [DllImport("DllProj.dll", EntryPoint = "creerHistoApartirTableau", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr creerHistoApartirTableau(int hauteur, int largeur, byte[] pixels, bool enregistrementCSV);


        [DllImport("DllProj.dll", EntryPoint = "libererImage", CallingConvention = CallingConvention.Cdecl)]
        private static extern void LibererImage(IntPtr image);

        // Méthode publique pour appeler l'histogramme à partir d'un tableau
        public IntPtr HistogrammeAPartirTableau(int hauteur, int largeur, byte[] pixels, bool enregistrementCSV = false)
        {
            // Appeler la méthode native
            IntPtr imagePtr = creerHistoApartirTableau(hauteur, largeur, pixels, true);

            return imagePtr;
        }

        // Méthode pour libérer la mémoire native si nécessaire
/*        public void LibererHisto()
        {
            if (histo != IntPtr.Zero)
            {
                LibererImage(histo);
                histo = IntPtr.Zero;
            }
        }*/
    }
}
