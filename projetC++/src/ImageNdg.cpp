#include <iostream>
#include <fstream>
#include <string>
#include <cmath>
#include <vector>

#include "ImageNdg.h"
#include "ImageDouble.h"

const double PI = 3.14159265358979323846;
// constructeurs et destructeur
CImageNdg::CImageNdg() {

	this->m_iHauteur  = 0;
	this->m_iLargeur  = 0;
	this->m_bBinaire  = false;
	this->m_sNom      = "vide";
	
	this->m_pucPixel  = NULL;
	this->m_pucPalette = NULL; 
}

CImageNdg::CImageNdg(int hauteur, int largeur, int valeur) {

	this->m_iHauteur = hauteur;
	this->m_iLargeur = largeur;
	this->m_bBinaire	= false; // Image Ndg par d�faut, binaire apr�s seuillage
	this->m_sNom      = "inconnu";

	this->m_pucPixel = new unsigned char[hauteur*largeur];
	this->m_pucPalette = new unsigned char[256*4];	
	choixPalette("grise"); // palette grise par d�faut, choix utilisateur 
	if (valeur != -1) 
		for (int i=0;i<this->lireNbPixels();i++)
			this->m_pucPixel[i] = valeur;
}

CImageNdg::CImageNdg(const std::string name) {

	BITMAPFILEHEADER header;
	BITMAPINFOHEADER infoHeader;
	
	std::ifstream f(name.c_str(),std::ios::in | std::ios::binary); 
		if (f.is_open()) {
			f.read((char*)&header.bfType,2);
			f.read((char*)&header.bfSize, 4);
			f.read((char*)&header.bfReserved1, 2);
			f.read((char*)&header.bfReserved2, 2);
			f.read((char*)&header.bfOffBits, 4);
			if (header.bfType != MAGIC_NUMBER_BMP) 
				throw std::string("ouverture format BMP impossible ..."); 
			else {
				f.read((char*)&infoHeader.biSize, 4);
				f.read((char*)&infoHeader.biWidth, 4);
				f.read((char*)&infoHeader.biHeight, 4);
				f.read((char*)&infoHeader.biPlanes, 2);
				f.read((char*)&infoHeader.biBitCount, 2);
				f.read((char*)&infoHeader.biCompression, 4);
				f.read((char*)&infoHeader.biSizeImage, 4);
				f.read((char*)&infoHeader.biXPelsPerMeter, 4);
				f.read((char*)&infoHeader.biYPelsPerMeter, 4);
				f.read((char*)&infoHeader.biClrUsed, 4);
				f.read((char*)&infoHeader.biClrImportant, 4);
				if (infoHeader.biCompression > 0) 
					throw std::string("Format compresse non supporte...");
				else {
					if (infoHeader.biBitCount == 8) {
						this->m_iHauteur = infoHeader.biHeight;
						this->m_iLargeur = infoHeader.biWidth;
						this->m_bBinaire = false;
						this->m_sNom.assign(name.begin(),name.end()-4);
						this->m_pucPalette = new unsigned char[256*4];	
						this->m_pucPixel = new unsigned char[infoHeader.biHeight * infoHeader.biWidth];

						// g�rer multiple de 32 bits via z�ros �ventuels ignor�s
						int complement = (((this->m_iLargeur-1)/4) + 1)*4 - this->m_iLargeur;
						for (int indice=0;indice<4*256;indice++) 
							f.read((char*)&this->m_pucPalette[indice],sizeof(char));

						for (int i= this->m_iHauteur-1; i >= 0; i--) {
							for (int j=0; j<this->m_iLargeur; j++) 
								f.read((char*)&this->m_pucPixel[i*this->m_iLargeur+j],sizeof(char));

							char inutile;
							for (int k=0; k< complement; k++)
								f.read((char*)&inutile,sizeof(char));
						}
					}	
					else {
						// cas d'une image couleur
						this->m_iHauteur = infoHeader.biHeight;
						this->m_iLargeur = infoHeader.biWidth;
						this->m_bBinaire = false;
						this->m_sNom.assign(name.begin(),name.end()-4);
						this->m_pucPalette = new unsigned char[256*4];	
						this->choixPalette("grise"); // palette grise par d�faut
						this->m_pucPixel = new unsigned char[infoHeader.biHeight * infoHeader.biWidth];

						// extraction plan luminance
						int complement = (((this->m_iLargeur*3-1)/4) + 1)*4 - this->m_iLargeur*3;
						for (int i= this->m_iHauteur-1; i >= 0; i--) {
							for (int j=0;j<this->m_iLargeur*3;j+=3) {
								unsigned char rouge,vert,bleu;
								f.read((char*)&rouge,sizeof(char));
								f.read((char*)&vert,sizeof(char)); 
								f.read((char*)&bleu,sizeof(char));
								this->m_pucPixel[i*this->m_iLargeur+j/3]=(unsigned char)(((int)rouge+(int)vert+(int)bleu)/3);
							}	

							char inutile;
							for (int k=0; k< complement; k++)
								f.read((char*)&inutile,sizeof(char));
						}
					}
				}
			}
			f.close();
		}
		else
			throw std::string("ERREUR : Image absente (ou pas ici en tout cas) !");
}

CImageNdg::CImageNdg(const CImageNdg& im) {

	this->m_iHauteur = im.lireHauteur();
	this->m_iLargeur = im.lireLargeur();
	this->m_bBinaire = im.lireBinaire(); 
	this->m_sNom     = im.lireNom();
	this->m_pucPixel = NULL; 
	this->m_pucPalette = NULL;

	if (im.m_pucPalette != NULL) {
		this->m_pucPalette = new unsigned char[256*4];
		memcpy(this->m_pucPalette,im.m_pucPalette,4*256);
	}
	if (im.m_pucPixel != NULL) {
		this->m_pucPixel = new unsigned char[im.lireHauteur() * im.lireLargeur()];
		memcpy(this->m_pucPixel,im.m_pucPixel,im.lireNbPixels());
	}
}

CImageNdg::~CImageNdg() {
	if (this->m_pucPixel) {
		delete[] this->m_pucPixel;
		this->m_pucPixel = NULL;
	}

	if (this->m_pucPalette) {
		delete[] this->m_pucPalette;
		this->m_pucPalette = NULL;
	}
}

void CImageNdg::sauvegarde(const std::string file) {
	BITMAPFILEHEADER header;
	BITMAPINFOHEADER infoHeader;

	if (this->m_pucPixel) {
		std::string nomFichier = "res/";

		if (file.empty())
			nomFichier += this->lireNom() + ".bmp";
		else
			nomFichier += file + ".bmp";

		std::ofstream f(nomFichier.c_str(),std::ios::binary);
		if (f.is_open()) {

			int complement = (((this->m_iLargeur-1)/4) + 1)*4 - this->m_iLargeur;

			header.bfType = MAGIC_NUMBER_BMP;
			f.write((char*)&header.bfType, 2);
			header.bfOffBits = 14 * sizeof(char) + 40 * sizeof(char) + 4 * 256 * sizeof(char); // palette
			header.bfSize = header.bfOffBits + (complement + lireLargeur())*lireHauteur()*sizeof(char);
			f.write((char*)&header.bfSize, 4);
			header.bfReserved1 = 0;
			f.write((char*)&header.bfReserved1, 2);
			header.bfReserved2 = 0;
			f.write((char*)&header.bfReserved2, 2);
			f.write((char*)&header.bfOffBits, 4);

			infoHeader.biSize = 40*sizeof(char);
			f.write((char*)&infoHeader.biSize, 4);
			infoHeader.biWidth = this->m_iLargeur;
			f.write((char*)&infoHeader.biWidth, 4);
			infoHeader.biHeight = this->m_iHauteur;
			f.write((char*)&infoHeader.biHeight, 4);
			infoHeader.biPlanes = 1;
			f.write((char*)&infoHeader.biPlanes, 2);
			infoHeader.biBitCount = 8;
			f.write((char*)&infoHeader.biBitCount, 2);
			infoHeader.biCompression = 0; // pas de compression
			f.write((char*)&infoHeader.biCompression, 4);
			infoHeader.biSizeImage = this->lireNbPixels();
			f.write((char*)&infoHeader.biSizeImage, 4);
			infoHeader.biXPelsPerMeter = 0;
			f.write((char*)&infoHeader.biXPelsPerMeter, 4);
			infoHeader.biYPelsPerMeter = 0;
			f.write((char*)&infoHeader.biYPelsPerMeter, 4);
			infoHeader.biClrUsed = 256;
			f.write((char*)&infoHeader.biClrUsed, 4);
			infoHeader.biClrImportant = 0;
			f.write((char*)&infoHeader.biClrImportant, 4);

			// on remplit la palette
			for (int indice=0;indice<4*256;indice ++) 
				f.write((char*)&this->m_pucPalette[indice],sizeof(char)); 

			for (int i= this->m_iHauteur-1; i >= 0; i--) {
				for (int j=0;j<m_iLargeur;j++) 
					f.write((char*)&this->m_pucPixel[i*m_iLargeur+j],sizeof(char));
					
				// g�rer multiple de 32 bits
				char inutile;
				for (int k=0; k< complement; k++)
					f.write((char*)&inutile,sizeof(char)); 
				}
		f.close();
		}
		else
			throw std::string("Impossible de creer le fichier de sauvegarde !");
	}
	else
		throw std::string("Pas de donnee a sauvegarder !");
}

CImageNdg& CImageNdg::operator=(const CImageNdg& im) {

	if (&im == this)
		return *this;

	this->m_iHauteur = im.lireHauteur();
	this->m_iLargeur = im.lireLargeur();
	this->m_bBinaire = im.lireBinaire(); 
	this->m_sNom     = im.lireNom();

	if (this->m_pucPixel) 
		delete[] this->m_pucPixel;
	this->m_pucPixel = new unsigned char[this->m_iHauteur * this->m_iLargeur];

	if (this->m_pucPalette)
		delete[] this->m_pucPalette;
	this->m_pucPalette = new unsigned char[256*4];

	if (im.m_pucPalette != NULL)
		memcpy(this->m_pucPalette,im.m_pucPalette,4*256);
	if (im.m_pucPixel != NULL)
		memcpy(this->m_pucPixel,im.m_pucPixel,im.lireNbPixels());

return *this;
}

extern "C" __declspec(dllexport) void* creerhistoapartirtableau(int hauteur, int largeur, unsigned char* pixels, bool enregistrementcsv)
{	// cr�er une instance de cimagendg avec les dimensions de l'image
	CImageNdg* image = new CImageNdg(hauteur, largeur, -1); // -1 pour indiquer qu'on ne sp�cifie pas une valeur par d�faut

	// remplir l'image avec les donn�es des pixels
	for (int i = 0; i < hauteur * largeur; ++i) {
		image->m_pucPixel[i] = pixels[i]; // copier les pixels dans l'image
	}
	image->histogramme(true);
	// retourner l'instance de l'image
}

// Fonction pour lib�rer la m�moire (� utiliser depuis C# pour �viter les fuites m�moire)
extern "C" __declspec(dllexport) void libererImage(void* image) {
	delete static_cast<CImageNdg*>(image); // Lib�rer l'objet CImageNdg
}

extern "C" __declspec(dllexport) unsigned long* calculerHistogramme(CImageNdg * image, bool enregistrementCSV) {
	auto histogramme = image->histogramme(enregistrementCSV);
	unsigned long* result = new unsigned long[histogramme.size()];
	std::copy(histogramme.begin(), histogramme.end(), result);
	return result;
}

extern "C" __declspec(dllexport) void libererHistogramme(unsigned long* histogramme) {
	delete[] histogramme;
}

extern "C" __declspec(dllexport) void* CImageNdg::creerHistoApartirTableau(int hauteur, int largeur, unsigned char* pixels, bool enregistrementCSV)
{
	// Cr�er une instance de CImageNdg avec les dimensions de l'image
		CImageNdg * image = new CImageNdg(hauteur, largeur, -1); // -1 pour indiquer qu'on ne sp�cifie pas une valeur par d�faut

	// Remplir l'image avec les donn�es des pixels
	for (int i = 0; i < hauteur * largeur; ++i) {
		image->m_pucPixel[i] = pixels[i]; // Copier les pixels dans l'image
	}
	image->histogramme(true);
	// Retourner l'instance de l'image
	return image;
}

std::vector<unsigned long> CImageNdg::histogramme(bool enregistrementCSV) {

	std::vector<unsigned long> h;

	h.resize(256,0);
	for (int i=0;i<this->lireNbPixels();i++) 
		h[this->operator()(i)] += 1L; 

	if (enregistrementCSV) {
	 std::string fichier = "res/" + this->lireNom() + ".csv";
		std::ofstream f (fichier.c_str());

		if (!f.is_open())
			throw std::string("Impossible d'ouvrir le fichier en ecriture !");
		else {
			for (int i=0;i<(int)h.size();i++)
				f << h[i] << std::endl;
		}
		f.close();
	}

	return h;
}

extern "C" __declspec(dllexport) void GetHistogram(CImageNdg* image, bool enregistrementCSV, unsigned long* histogramme, int* taille)
{
			if (!image || !histogramme || !taille)
				throw std::invalid_argument("Param�tre invalide !");

			// Initialisation de l'histogramme
			*taille = 256;
			for (int i = 0; i < *taille; ++i) {
				histogramme[i] = 0;
			}

			// Calcul de l'histogramme
			for (int i = 0; i < image->lireNbPixels(); ++i) {
				histogramme[image->operator()(i)] += 1;
			}

			// Optionnel : Enregistrement en CSV
			if (enregistrementCSV) {
				std::string fichier = "res/" + image->lireNom() + ".csv";
				std::ofstream f(fichier.c_str());
				if (!f.is_open()) {
					throw std::runtime_error("Impossible d'ouvrir le fichier en �criture !");
				}
				else {
					for (int i = 0; i < *taille; ++i) {
						f << histogramme[i] << std::endl;
					}
				}
			}
}

// signatures globales
MOMENTS CImageNdg::signatures(const std::vector<unsigned long> h) {

	MOMENTS globales;
	
	// min
	int i=0;
	while ((i < (int)h.size()) && (h[i] == 0))
		i++;
	globales.minNdg = i;
		
	// max
	i=h.size()-1;
	while ((i > 0) && (h[i] == 0))
		i--;
	globales.maxNdg = i;

	// mediane
	int moitPop = this->lireNbPixels()/2;

	i=globales.minNdg;
	int somme = h[i];
	while (somme < moitPop) {
		i += 1;
		if (i < (int)h.size())
			somme += h[i];
	}
	globales.medianeNdg = i;

	// moyenne et �cart-type
	float moy=0,sigma=0;
	for (i=globales.minNdg;i<=globales.maxNdg;i++) {
		moy += ((float)h[i])*i;
		sigma += ((float)h[i])*i*i;
	}
	moy /= (float)this->lireNbPixels();
	sigma = sqrt(sigma/(float)this->lireNbPixels() - (moy*moy));
	globales.moyenneNdg = moy;
	globales.ecartTypeNdg = sigma;

	return globales;
}

MOMENTS CImageNdg::signatures() {
	
	MOMENTS globales = { 0,0,0,0,0 };

	std::vector<unsigned long> hist;
	hist = this->histogramme(false);

	globales = this->signatures(hist);

	return globales;
}

// op�rations ensemblistes images binaires
CImageNdg& CImageNdg::operation(const CImageNdg& im, const std::string methode) {

	if ((&im == this) || !(this->lireBinaire() && im.lireBinaire())) {
		throw std::string("Operation logique uniquement possible entre 2 images binaires");
		return *this;
	}

	this->m_iHauteur = im.lireHauteur();
	this->m_iLargeur = im.lireLargeur();
	this->m_bBinaire = im.lireBinaire(); 
	this->choixPalette("binaire"); // images binaires -> palettes binaires
	this->m_sNom     = im.lireNom()+"Op";

	if (methode.compare("et") == 0) {
		for (int i=0;i<this->lireNbPixels();i++)
			this->operator()(i) = this->operator()(i) && im(i);
	}
	else
		if (methode.compare("ou") == 0) {
			for (int i=0;i<this->lireNbPixels();i++)
				this->operator()(i) = this->operator()(i) || im(i);
		}
		else
			if (methode.compare("-") == 0) {
				for (int i = 0; i < this->lireNbPixels(); i++)
				{
					this->operator()(i) = this->operator()(i) - im(i); // vigilance sur les images op�r�r�es !
					if (this->operator()(i) < 0)
					{
						std::cout << "this->operator()(" << i << ") : " << this->operator()(i) << std::endl;
						this->operator()(i) = 0;
					}
				}
					
			}

return *this;
}

// seuillage
CImageNdg CImageNdg::seuillage(const std::string methode, int seuilBas, int seuilHaut) {
	
	if (!this->m_bBinaire) {
		CImageNdg out(this->lireHauteur(),this->lireLargeur());
		out.choixPalette("binaire"); // palette binaire par d�faut
		out.m_bBinaire = true;

		// cr�ation lut pour optimisation calcul
		std::vector<int> lut;
		lut.resize(256);

		// recherche valeur seuil
		// cas "manuel" 
		if (methode.compare("manuel") == 0)
		{
			out.m_sNom = this->lireNom() + "SeMa";
		}
		else 
			if (methode.compare("otsu") == 0)
			{
				out.m_sNom = this->lireNom() + "SeAu";
				// recherche seuil via Otsu
				std::vector<unsigned long> hist = this->histogramme(false);
				std::vector<unsigned long> histC; // histogramme cumul�
				histC.resize(256, 0);
				histC[0] = hist[0];
				for (int i = 1; i < (int)hist.size(); i++)
					histC[i] = histC[i - 1] + hist[i];

				MOMENTS globales = this->signatures(hist);
				int min = globales.minNdg,
					max = globales.maxNdg;

				// f(s)
				std::vector<double> tab;
				tab.resize(256, 0);

				double M1, M2, w1;

				// initialisation
				M1 = min;
				seuilBas = min;
				seuilHaut = 255;

				w1 = (double)histC[min] / (double)(this->lireNbPixels());
				M2 = 0;
				for (int i = min + 1; i <= max; i++)
					M2 += (double)hist[i] * i;
				M2 /= (double)(histC[max] - hist[min]);
				tab[min] = w1*(1 - w1)*(M1 - M2)*(M1 - M2);

				for (int i = min + 1; i < max; i++) {
					M1 = ((double)histC[i - 1] * M1 + (double)hist[i] * i) / histC[i];
					M2 = ((double)(histC[255] - histC[i - 1])*M2 - hist[i] * i) / (double)(histC[255] - histC[i]);
					w1 = (double)histC[i] / (double)(this->lireNbPixels());
					tab[i] = w1*(1 - w1)*(M1 - M2)*(M1 - M2);
					if (tab[i] > tab[seuilBas])
						seuilBas = i;
				}
			}
			else {
				if (methode.compare("moyenne") == 0) {
					out.m_sNom = this->lireNom() + "SeMoy";
					seuilBas = this->signatures().moyenneNdg;
					seuilHaut = 255;
				}
				if (methode.compare("mediane") == 0) {
					out.m_sNom = this->lireNom() + "SeMed";
					seuilBas = this->signatures().medianeNdg;
					seuilHaut = 255;
				}
			}
 

		// g�n�ration lut
		for (int i = 0; i < seuilBas; i++)
			lut[i] =  0; 
		for (int i = seuilBas; i <= seuilHaut; i++)
			lut[i] = 1;
		for (int i = seuilHaut+1; i <= 255; i++)
			lut[i] = 0;

		// cr�ation image seuill�e
		//std::cout << "Seuillage des pixels entre " << seuilBas << " et " << seuilHaut << std::endl;
		for (int i=0; i < out.lireNbPixels(); i++) 
			out(i) = lut[this->operator ()(i)]; 

		return out;
		}
	else {
		throw std::string("Seuillage image binaire impossible");
		return (*this);
	}
}

// transformation
CImageNdg CImageNdg::transformation(const std::string methode) {

	CImageNdg out(this->lireHauteur(),this->lireLargeur());

	std::vector<int> map = this->lirePalette();
	out.choixPalette(map); // conservation de la palette
	out.m_bBinaire = this->m_bBinaire; // conservation du type

	if (methode.compare("complement") == 0) {
		out.m_sNom = this->lireNom() + "TComp";
		// ndg -> 255-ndg
		// cr�ation lut pour optimisation calcul
		std::vector<int> lut;
		lut.resize(256);

		if (this->lireBinaire())
		{
			for (int i = 0; i < 2; i++)
				lut[i] = (int)(1 - i);
		}
		else
		{
			for (int i = 0; i < 256; i++)
				lut[i] = (int)(255 - i);
		}
		for (int i = 0; i < out.lireNbPixels(); i++)
			out(i) = lut[this->operator()(i)];
	}
	else
		if (methode.compare("expansion") == 0) {
			out.m_sNom = this->lireNom() + "TExp";
			int min = 255, max = 0;
			for (int i = 0; i < this->lireNbPixels(); i++) {
				if (this->operator()(i) > max)
					max = this->operator()(i);
				if (this->operator()(i) < min)
					min = this->operator()(i);
			}

			double a = 255 / (double)(max - min);
			double b = -a*min;

			std::vector<int> lut;
			lut.resize(256);

			for (int i = 0; i < 256; i++)
				lut[i] = (int)(a*i + b);
			for (int i = 0; i < out.lireNbPixels(); i++)
				out(i) = lut[this->operator()(i)];
		}
		else
			if (methode.compare("egalisation") == 0) {
				out.m_sNom = this->lireNom() + "TEga";
				std::vector<unsigned long> h = this->histogramme(false);
				std::vector<unsigned long> hC = h;
				for (int i = 1; i<(int)h.size(); i++)
					hC[i] = hC[i - 1] + h[i];

				// recherche min et max image
				int min = 0, max = 255;
				int i = 0;
				while (h[i] == 0)
					i++;
				min = i;
				i = 255;
				while (h[i] == 0)
					i--;
				max = i;

				std::vector<int> lut;
				lut.resize(256);

				for (int i = min; i <= max; i++)
					lut[i] = (int)(((double)hC[i] / (double)this->lireNbPixels())*(double)255);

				for (int i = 0; i < out.lireNbPixels(); i++)
					out(i) = lut[this->operator()(i)];
			}

	return out;
}

// morphologie
CImageNdg CImageNdg::morphologie(const std::string methode, const std::string eltStructurant) {

	CImageNdg out(this->lireHauteur(), this->lireLargeur());

	out.choixPalette(this->lirePalette()); // conservation de la palette

	out.m_bBinaire = this->m_bBinaire; // conservation du type

	if (methode.compare("erosion") == 0) {
		out.m_sNom = this->lireNom() + "MEr";
		CImageNdg agrandie(this->lireHauteur() + 2, this->lireLargeur() + 2);

		// gestion des bords
		if (this->lireBinaire()) {
			int pix;

			for (pix = 0; pix<agrandie.lireLargeur(); pix++) {
				agrandie(0, pix) = 1;
				agrandie(this->lireHauteur() - 1, pix) = 1;
			}
			for (pix = 1; pix<agrandie.lireHauteur() - 1; pix++) {
				agrandie(pix, 0) = 1;
				agrandie(pix, this->lireLargeur() - 1) = 1;
			}
		}
		else {
			int pix;

			for (pix = 0; pix<agrandie.lireLargeur(); pix++) {
				agrandie(0, pix) = 255;
				agrandie(this->lireHauteur() - 1, pix) = 255;
			}
			for (pix = 1; pix<agrandie.lireHauteur() - 1; pix++) {
				agrandie(pix, 0) = 255;
				agrandie(pix, this->lireLargeur() - 1) = 255;
			}
		}

		// gestion du coeur
		for (int i = 0; i<this->lireHauteur(); i++)
			for (int j = 0; j<this->lireLargeur(); j++) {
				agrandie(i + 1, j + 1) = this->operator()(i, j);
			}

		if (eltStructurant.compare("V4") == 0) {
			for (int i = 1; i<agrandie.lireHauteur() - 1; i++)
				for (int j = 1; j<agrandie.lireLargeur() - 1; j++) {
					int minH = std::min(agrandie(i, j - 1), agrandie(i, j + 1));
					int minV = std::min(agrandie(i - 1, j), agrandie(i + 1, j));
					int minV4 = std::min(minH, minV);
					out(i - 1, j - 1) = std::min(minV4, (int)agrandie(i, j));
				}
		}
		else {
			if (eltStructurant.compare("V8") == 0) {
				for (int i = 1; i<agrandie.lireHauteur() - 1; i++)
					for (int j = 1; j<agrandie.lireLargeur() - 1; j++) {
						int minH = std::min(agrandie(i, j - 1), agrandie(i, j + 1));
						int minV = std::min(agrandie(i - 1, j), agrandie(i + 1, j));
						int minV4 = std::min(minH, minV);
						int minD1 = std::min(agrandie(i - 1, j - 1), agrandie(i + 1, j + 1));
						int minD2 = std::min(agrandie(i - 1, j + 1), agrandie(i + 1, j - 1));
						int minD = std::min(minD1, minD2);
						int minV8 = std::min(minV4, minD);
						out(i - 1, j - 1) = std::min(minV8, (int)agrandie(i, j));
					}
			}
		}
	}
	else {
		if (methode.compare("dilatation") == 0) {
			out.m_sNom = this->lireNom() + "MDi";
			CImageNdg agrandie(this->lireHauteur() + 2, this->lireLargeur() + 2);

			// gestion des bords
			int pix;

			for (pix = 0; pix<agrandie.lireLargeur(); pix++) {
				agrandie(0, pix) = 0;
				agrandie(agrandie.lireHauteur() - 1, pix) = 0;
			}
			for (pix = 1; pix<agrandie.lireHauteur() - 1; pix++) {
				agrandie(pix, 0) = 0;
				agrandie(pix, agrandie.lireLargeur() - 1) = 0;
			}

			// gestion du coeur
			for (int i = 0; i<this->lireHauteur(); i++)
				for (int j = 0; j<this->lireLargeur(); j++) {
					agrandie(i + 1, j + 1) = this->operator()(i, j);
				}

			if (eltStructurant.compare("V4") == 0) {
				for (int i = 1; i<agrandie.lireHauteur() - 1; i++)
					for (int j = 1; j<agrandie.lireLargeur() - 1; j++) {
						int maxH = std::max(agrandie(i, j - 1), agrandie(i, j + 1));
						int maxV = std::max(agrandie(i - 1, j), agrandie(i + 1, j));
						int maxV4 = std::max(maxH, maxV);
						out(i - 1, j - 1) = std::max(maxV4, (int)agrandie(i, j));
					}
			}
			else {
				if (eltStructurant.compare("V8") == 0) {
					for (int i = 1; i<agrandie.lireHauteur() - 1; i++)
						for (int j = 1; j<agrandie.lireLargeur() - 1; j++) {
							int maxH = std::max(agrandie(i, j - 1), agrandie(i, j + 1));
							int maxV = std::max(agrandie(i - 1, j), agrandie(i + 1, j));
							int maxV4 = std::max(maxH, maxV);
							int maxD1 = std::max(agrandie(i - 1, j - 1), agrandie(i + 1, j + 1));
							int maxD2 = std::max(agrandie(i - 1, j + 1), agrandie(i + 1, j - 1));
							int maxD = std::max(maxD1, maxD2);
							int maxV8 = std::max(maxV4, maxD);
							out(i - 1, j - 1) = std::max(maxV8, (int)agrandie(i, j));
						}
				}
			}
		}
	}

	return out;
}

CImageNdg CImageNdg::hough(int threshold, bool colorForEachLine)
{
	/*-------------------- Images initialization ----------------------*/
	// Tresholded image
	CImageNdg imgSeuil = seuillage().transformation();
	CImageDouble imgDoubleSeuil(imgSeuil);
	// Contours image
	CImageNdg imgContours = imgDoubleSeuil.vecteurGradient("norme").toNdg();
	// Hough image
	CImageNdg res(lireHauteur(), lireLargeur(), 0);

	/*-------------------------- Parameters ----------------------------*/
	int width = lireLargeur();
	int height = lireHauteur();
	const int maxTheta = 180;  // Nombre d'angles possibles (degr�s)
	const int maxRho = static_cast<int>(sqrt(width * width + height * height));  // Distance maximale possible depuis l'origine

	/*-------------------------- Hough transform -----------------------*/
	std::vector<std::vector<int>> accumulator(maxTheta, std::vector<int>(maxRho, 0));
	// Go through the contours image
	for (int x = 0; x < height; ++x)
	{
		for (int y = 0; y < width; ++y)
		{
			// If current pixel is part of contours
			if (imgContours(x, y) > 0)
			{
				// Go through all possible angles
				for (int theta = 0; theta < maxTheta; ++theta)
				{
					// Get the angle in radians
					double radians = theta * PI / 180.0;
					// Compute rho from cartesian coordinates (rho = x * cos(theta) + y * sin(theta)
					int rho = static_cast<int>(y * cos(radians) + x * sin(radians));

					// Make sure rho is positive
					if (rho < 0)
					{
						rho = (rho + maxRho) % maxRho; // Shift the negative value to the positive range
					}
					if (theta >= 0 && theta < maxTheta && rho >= 0 && rho < maxRho)
					{
						accumulator[theta][rho]++;
					}
				}
			}
		}
	}

	CImageNdg hough(accumulator.size(), accumulator[0].size());

	for (int i = 0; i < hough.lireHauteur(); i++)
	{
		for (int j = 0; j < hough.lireLargeur(); j++)
		{
			hough(i, j) = accumulator[i][j];
		}
	}

	hough.sauvegarde("Sinus");


	/*-------------------------- Hough lines ---------------------------*/
	// Go through the accumulator
	std::vector<POINT_POLAIRE> houghLines;
	for (int theta = 0; theta < maxTheta; ++theta)
	{
		for (int rho = 0; rho < maxRho; ++rho)
		{
			// If the current value is above the threshold
			if (accumulator[theta][rho] > threshold)
			{
				bool isLocalMaxima = true;

				// Check the neighbors
				for (int dTheta = -1; dTheta <= 1; ++dTheta)
				{
					for (int dRho = -1; dRho <= 1; ++dRho)
					{
						int neighborTheta = theta + dTheta;
						int neighborRho = rho + dRho;

						// Skip if the neighbor is out of bounds
						if (neighborTheta < 0 || neighborTheta >= maxTheta || neighborRho < 0 || neighborRho >= maxRho)
						{
							continue;
						}

						// Check if the neighbor has a higher value
						if (accumulator[neighborTheta][neighborRho] > accumulator[theta][rho])
						{
							isLocalMaxima = false;
							break;
						}
					}
					if (!isLocalMaxima)
					{
						break;
					}
				}

				// Add the point to the list of lines if it's a local maxima
				if (isLocalMaxima)
				{
					houghLines.push_back({ theta, rho });
				}
			}
		}
	}

	/*-------------------------- Draw lines ----------------------------*/
	if (colorForEachLine)
	{
		int grayIncrement = 255 / houghLines.size();
		int currentGrayLevel = 1;

		for (const auto& line : houghLines)
		{
			int theta = line.rho;
			int rho = line.theta;

			// Convertir les coordonn�es polaires en coordonn�es cart�siennes
			double radians = theta * PI / 180.0;
			for (int y = 0; y < width; ++y)
			{
				int x = static_cast<int>((rho - y * cos(radians)) / sin(radians));
				if (x >= 0 && x < height)
				{
					// Dessiner un pixel sur l'image r�sultante
					res(x, y) = currentGrayLevel;
				}
			}
			for (int x = 0; x < height; ++x)
			{
				int y = static_cast<int>((rho - x * sin(radians)) / cos(radians));
				if (y >= 0 && y < width)
				{
					// Dessiner un pixel sur l'image r�sultante
					res(x, y) = 255;
				}
			}
			currentGrayLevel += grayIncrement;
			grayIncrement++;
			if (currentGrayLevel >= 255)
			{
				currentGrayLevel = 0;
			}
		}
	}
	else
	{
		for (const auto& line : houghLines)
		{
			int theta = line.rho;
			int rho = line.theta;

			// Convertir les coordonn�es polaires en coordonn�es cart�siennes
			double radians = theta * PI / 180.0;
			for (int y = 0; y < width; ++y)
			{
				int x = static_cast<int>((rho - y * cos(radians)) / sin(radians));
				if (x >= 0 && x < height)
				{
					// Dessiner un pixel sur l'image r�sultante
					res(x, y) = 255;
				}
			}
			for (int x = 0; x < height; ++x)
			{
				int y = static_cast<int>((rho - x * sin(radians)) / cos(radians));
				if (y >= 0 && y < width)
				{
					// Dessiner un pixel sur l'image r�sultante
					res(x, y) = 255;
				}
			}
		}
	}

	return res;
}
