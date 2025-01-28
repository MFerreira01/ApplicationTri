using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApplicationTri
{
    internal class ClTraitementIm
    {

        public static Bitmap ProcessImage(Bitmap image, int threshold)
        {
            Bitmap processed = new Bitmap(image.Width, image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);

                    // Convertir en niveaux de gris
                    int grayValue = (pixel.R + pixel.G + pixel.B) / 3;

                    // Binarisation : si le gris dépasse le seuil, mettre blanc, sinon noir
                    if (grayValue > threshold)
                    {
                        processed.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        processed.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return processed;
        }
        public static Bitmap StretchImageDynamic(string imagePath)
        {
            Bitmap bitmap = new Bitmap(imagePath);

            // Trouver les niveaux de gris min et max
            int minGray = 255;
            int maxGray = 0;

            // Première boucle pour déterminer les min et max
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);

                    int grayLevel = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);

                    if (grayLevel < minGray) minGray = grayLevel;
                    if (grayLevel > maxGray) maxGray = grayLevel;
                }
            }

            // Deuxième boucle pour appliquer le réajustement linéaire
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);

                    int grayLevel = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);

                    // Appliquer la normalisation linéaire
                    int stretchedGray = (int)((grayLevel - minGray) * 255.0 / (maxGray - minGray));

                    // Créer la couleur ajustée (en niveaux de gris)
                    Color stretchedColor = Color.FromArgb(stretchedGray, stretchedGray, stretchedGray);

                    bitmap.SetPixel(x, y, stretchedColor);  // Mettre à jour le pixel de l'image
                }
            }
            return bitmap;
        }
        public static Bitmap StretchDynamic(string imagePath)
        {
            Bitmap bitmap = new Bitmap(imagePath);

            // Trouver les niveaux de gris min et max
            int minGray = 255;
            int maxGray = 0;
            int moyenne = 0;

            // Première boucle pour déterminer les min et max
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);

                    int grayLevel = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);

                    moyenne += grayLevel;
                    if (grayLevel < minGray) minGray = grayLevel;
                    if (grayLevel > maxGray) maxGray = grayLevel;
                }
            }

            moyenne = moyenne /(bitmap.Height*bitmap.Width);

            // Deuxième boucle pour appliquer le réajustement linéaire
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);

                    int grayLevel = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);

                    if (grayLevel > moyenne) grayLevel = 255;
                    else
                    {
                        // Appliquer la normalisation linéaire
                        int stretchedGray = (int)((grayLevel - minGray) * 255.0 / (maxGray - minGray));

                        // Créer la couleur ajustée (en niveaux de gris)
                        Color stretchedColor = Color.FromArgb(stretchedGray, stretchedGray, stretchedGray);

                        bitmap.SetPixel(x, y, stretchedColor);  // Mettre à jour le pixel de l'image
                    } 
                }
            }
            return bitmap;
        }
        public static Bitmap inverser(Bitmap bitmap)
        {
            Color noir = Color.FromArgb(0, 0, 0);
            Color blanc = Color.FromArgb(255, 255, 255);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    // Vérifie si le pixel est blanc (255,255,255)
                    if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255) bitmap.SetPixel(x, y, noir);
                    else bitmap.SetPixel(x, y, blanc);
                }
            }
            return bitmap;
        }
        public static PointF GetCenterOfGravity(Bitmap bitmap)
        {
            // Variables pour le calcul du centre de gravité
            long sumX = 0, sumY = 0;
            long totalWhitePixels = 0;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);

                    // Vérifie si le pixel est blanc (255,255,255)
                    if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255)
                    {
                        sumX += x;
                        sumY += y;
                        totalWhitePixels++;
                    }
                }
            }

            if (totalWhitePixels == 0)
            {
                throw new Exception("Aucune zone blanche détectée dans l'image.");
            }

            // Calcul du centre de gravité
            float centerX = (float)sumX / totalWhitePixels;
            float centerY = (float)sumY / totalWhitePixels;

            return new PointF(centerX, centerY);
        }
        // Extraction des pixels d'intérêt
        public static List<PointF> GetPointsFromBitmap(Bitmap image, Color targetColor)
        {
            List<PointF> points = new List<PointF>();

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    if (image.GetPixel(x, y).ToArgb() == targetColor.ToArgb())
                    {
                        points.Add(new PointF(x, y));
                    }
                }
            }
            return points;
        }

        // Calcul de la moyenne des points
        public static double[] CalculateMean(List<PointF> points)
        {
            double meanX = points.Average(p => p.X);
            double meanY = points.Average(p => p.Y);
            return new double[] { meanX, meanY };
        }

        // Centrer les points par rapport à la moyenne
        public static double[,] CenterData(List<PointF> points, double[] mean)
        {
            double[,] centeredData = new double[points.Count, 2];

            for (int i = 0; i < points.Count; i++)
            {
                centeredData[i, 0] = points[i].X - mean[0];
                centeredData[i, 1] = points[i].Y - mean[1];
            }

            return centeredData;
        }

        // Calcul de la matrice de covariance
        public static double[,] ComputeCovarianceMatrix(double[,] centeredData)
        {
            int n = centeredData.GetLength(0);
            double sumXX = 0, sumXY = 0, sumYY = 0;

            for (int i = 0; i < n; i++)
            {
                sumXX += centeredData[i, 0] * centeredData[i, 0];
                sumXY += centeredData[i, 0] * centeredData[i, 1];
                sumYY += centeredData[i, 1] * centeredData[i, 1];
            }

            return new double[,]
            {
            { sumXX / (n - 1), sumXY / (n - 1) },
            { sumXY / (n - 1), sumYY / (n - 1) }
            };
        }

        // Résolution des vecteurs propres (approche simplifiée pour 2x2)
        public static (double[], double[,]) EigenDecomposition(double[,] covarianceMatrix)
        {
            double a = covarianceMatrix[0, 0];
            double b = covarianceMatrix[0, 1];
            double d = covarianceMatrix[1, 1];

            double trace = a + d;
            double determinant = (a * d) - (b * b);
            double lambda1 = trace / 2 + Math.Sqrt((trace * trace) / 4 - determinant);
            double lambda2 = trace / 2 - Math.Sqrt((trace * trace) / 4 - determinant);

            double[] eigenValues = { lambda1, lambda2 };

            // Calcul des vecteurs propres normalisés
            double v1x = b;
            double v1y = lambda1 - a;
            double norm1 = Math.Sqrt(v1x * v1x + v1y * v1y);

            double v2x = b;
            double v2y = lambda2 - a;
            double norm2 = Math.Sqrt(v2x * v2x + v2y * v2y);

            double[,] eigenVectors = {
            { v1x / norm1, v2x / norm2 },
            { v1y / norm1, v2y / norm2 }
        };

            return (eigenValues, eigenVectors);
        }

        // Fonction pour dessiner les axes sur l'image
        public static Bitmap DrawAxesOnImage(Bitmap image, double[] mean, double[,] eigenVectors, string outputPath, bool saveToFile = false, int scale = 100)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                Pen axisPen1 = new Pen(Color.Red, 2);  // Premier axe (par exemple, axe principal)
                Pen axisPen2 = new Pen(Color.Green, 2); // Deuxième axe (axe secondaire)

                int cx = (int)mean[0];  // Position du centre de l'image
                int cy = (int)mean[1];

                double vx1 = eigenVectors[0, 0], vy1 = eigenVectors[1, 0];
                double vx2 = eigenVectors[0, 1], vy2 = eigenVectors[1, 1];

                // Déterminer une échelle pour couvrir toute l'image
                int imageWidth = image.Width;
                int imageHeight = image.Height;

                // Étendre les lignes jusqu'aux bords de l'objet
                int length1 = Math.Max(imageWidth, imageHeight);
                int length2 = Math.Max(imageWidth, imageHeight);

                // Dessiner les axes principaux
                g.DrawLine(axisPen1, cx, cy, cx + (int)(vx1 * length1), cy + (int)(vy1 * length1));
                g.DrawLine(axisPen1, cx, cy, cx - (int)(vx1 * length1), cy - (int)(vy1 * length1));

                g.DrawLine(axisPen2, cx, cy, cx + (int)(vx2 * length2), cy + (int)(vy2 * length2));
                g.DrawLine(axisPen2, cx, cy, cx - (int)(vx2 * length2), cy - (int)(vy2 * length2));

                // Sauvegarder l'image si le paramètre 'saveToFile' est vrai
                if (saveToFile)
                {
                    image.Save(outputPath);
                    Console.WriteLine($"Image avec axes sauvegardée sous : {outputPath}");
                }
            }

            // Retourner l'image modifiée
            return image;
        }

        // Fonction pour calculer l'angle entre la verticale et l'axe principal
        public static double CalculerAngleVerticale(double[] axePrincipal)
        {
            // Calculer l'angle entre l'axe principal et l'axe vertical (axe Y)
            // L'axe vertical est supposé être [0, 1]

            // Calcul du produit scalaire entre l'axe principal et l'axe vertical
            double produitScalaire = axePrincipal[1]; // Nous prenons l'élément Y de l'axe principal

            // Calcul de la norme de l'axe principal (longueur de l'axe)
            double normeAxePrincipal = Math.Sqrt(Math.Pow(axePrincipal[0], 2) + Math.Pow(axePrincipal[1], 2));

            // Calcul de l'angle (en radians) entre l'axe principal et la verticale (axe Y)
            double angleEnRadians = Math.Acos(produitScalaire / normeAxePrincipal);

            // Convertir l'angle en degrés
            double angleEnDegres = angleEnRadians * (180 / Math.PI);

            return angleEnDegres;
        }
        public static double CalculerAngleVerticale(double[,] eigenVectors)
        {
            // Récupérer l'axe principal (le premier vecteur propre)
            double vx = eigenVectors[0, 0];  // Composante x du vecteur propre principal
            double vy = eigenVectors[1, 0];  // Composante y du vecteur propre principal

            // Calculer l'angle avec la verticale
            double[] axePrincipal = { vx, vy };

            return CalculerAngleVerticale(axePrincipal);
        }

        public static void DrawAngleArcOnImage(Bitmap image, double[] mean, double angleInDegrees, int radius, string outputPath)
        {
            // Calcul du centre de gravité (ou le centre des points)
            PointF centerOfGravityPointF = GetCenterOfGravity(image);

            // Convertir en double[]
            double[] centerOfGravity = new double[] { centerOfGravityPointF.X, centerOfGravityPointF.Y };

            using (Graphics g = Graphics.FromImage(image))
            {
                // Définir un stylo pour l'arc et pour la verticale
                Pen arcPen = new Pen(Color.Blue, 2);
                Pen verticalPen = new Pen(Color.Black, 2);

                // Récupérer les coordonnées du centre de gravité
                int cx = (int)centerOfGravity[0]; // Coordonnée x du centre de gravité
                int cy = (int)centerOfGravity[1]; // Coordonnée y du centre de gravité

                // Calculer l'angle de départ et l'angle de l'arc
                // L'angle est mesuré à partir de l'axe vertical, donc on part de 90° et on soustrait l'angle calculé
                float startAngle = 90 - (float)angleInDegrees;
                float sweepAngle = (float)angleInDegrees;

                // Définir le rectangle dans lequel l'arc sera tracé
                Rectangle arcBounds = new Rectangle(cx - radius, cy - radius, 2 * radius, 2 * radius);

                // Dessiner l'arc
                g.DrawArc(arcPen, arcBounds, startAngle, sweepAngle);

                // Dessiner la ligne verticale qui traverse toute l'image
                // La ligne verticale couvre toute la hauteur de l'image
                g.DrawLine(verticalPen, cx, 0, cx, image.Height);  // Ligne verticale de haut en bas

                // Sauvegarder l'image
                image.Save(outputPath);
                Console.WriteLine($"Image avec arc et verticale sauvegardée sous : {outputPath}");
            }

        }
    }
}
