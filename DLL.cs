using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ApplicationTri
{
    internal class DLL
    {
        [DllImport("VotreDLL.dll", EntryPoint = "CalculerHistogramme", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CalculerHistogramme(
            IntPtr image, // Pointeur vers l'objet C++
            bool enregistrementCSV,
            [Out] ulong[] histogramme,
            ref int taille);

        public static ulong[] CalculerHistogrammeManaged(IntPtr image, bool enregistrementCSV)
        {
            int taille = 0;
            ulong[] histogramme = new ulong[256]; // Taille maximale supposée
            CalculerHistogramme(image, enregistrementCSV, histogramme, ref taille);

            // Ajuster la taille si nécessaire
            ulong[] resultat = new ulong[taille];
            Array.Copy(histogramme, resultat, taille);
            return resultat;
        }
    }
}
