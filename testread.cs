using UnityEngine;
//using System.Collections;
using System.IO;
using System.Net;

//using System.Text;
//using System.Linq;

public class testread : MonoBehaviour {
	
	//****************Declaration des variables globales*********************
	string lecturetext = @"C:\Users\anis\Documents\New Unity Project\surledash.txt";
	int positiondebutlecture = 0;
	int tramavecsat=0, tramsansat = 0, vertrasat=0;
	float[] vitesse3d = new float[1];
	float[] timepos = new float[1];
	double[] longitude = new double[1];
	double[] latitude = new double[1];
	double[] direction = new double[1];
	Vector3[] position= new Vector3[1];
	Vector3[] positionrelative= new Vector3[1];
	double[] disbetlonglat = new double[1];
	double[] distentreptsxy = new double[1];
	Vector3[] path= new[] {new Vector3(0.0f,0.0f,0.0f)};
	double pi =3.14159265358979;
	int r=6371000 ;// Le rayon de la Terre en m
	// Use this for initialization
	
	//*********************Initialisation*************************
	void Start () 
	{
		downloadfile ();
		ReadAndProcessLargeFile( lecturetext ,  positiondebutlecture );
		pathcreation ();
		traitement();
	}
	
	//*************Update is called once per frame****************
	void Update () 
	{
		
	}
	
	//***********************Methodes*************************
	public void downloadfile () {
		using (WebClient ftpClient = new WebClient())
		{
			//ftpClient.Credentials = new System.Net.NetworkCredential("Anis", "anis");
			//ftpClient.DownloadFile("ftp://10.180.89.108/database/rawdata/surledash.txt", @"C:\Users\anis\Documents\New Unity Project\surledash.txt");
		}
	}
	
	public void ReadAndProcessLargeFile(string readtxt, int whereToStartReading = 0)
	{
		//****************Declaration des variables locales*********************
		int i,j=0,c=0,satellites;
		string state = "";
		byte[] v=new byte[4];
		byte[] lon=new byte[4];
		byte[] lat=new byte[4];
		byte[] dir=new byte[4];
		byte[] tp=new byte[4];
		double longi, lati,lonrad,latrad;
		float xt,yt=0,zt,lon0=0f,tempos;
		
		//***********************Traitement*************************************
		FileStream fileStram = new FileStream(readtxt,FileMode.Open,FileAccess.Read);
		BinaryReader br = new BinaryReader(fileStram);
		
		for(i= 0x0; i<fileStram.Length;i++)//0x4abb55e    (taille-c)
		{
			
			br.BaseStream.Position=i+44;//j=0x2d;//taille d'une trame sans des données satelites
			state = br.ReadByte().ToString("X2");
			if(state=="04")
			{
				br.BaseStream.Position=i+168;//j=0x2d;//taille d'une trame sans des données satelites
				satellites =  (int) br.ReadByte();
				
				for(j= 0x0; j<4;j++)
				{
					br.BaseStream.Position=i+142+j;//pointer sur l@ de la valeur de la vitesse
					v[j]=br.ReadByte();
					br.BaseStream.Position=i+102+j;//pointer sur l@ de la valeur de la longitude
					lon[j]=br.ReadByte();
					br.BaseStream.Position=i+106+j;//pointer sur l@ de la valeur de la latitude
					lat[j]=br.ReadByte();
					br.BaseStream.Position=i+150+j;//pointer sur l@ de la valeur de la latitude
					dir[j]=br.ReadByte();
					br.BaseStream.Position=i+98+j;//pointer sur l@ de la valeur de la latitude
					tp[j]=br.ReadByte();
					
					
				}
				// modifier les tailles des tableaux vitesse3d, altitude et longitude et les remplir
				tempos=(float)((tp[3]<<24)+(tp[2]<<16)+(tp[1]<<8)+tp[0]);
				longi=(double)((lon[3]<<24)+(lon[2]<<16)+(lon[1]<<8)+lon[0])*System.Math.Pow(10,-7);
				lati=(double)((lat[3]<<24)+(lat[2]<<16)+(lat[1]<<8)+lat[0])*System.Math.Pow(10,-7);
				lonrad=longi*(pi/180);
				latrad=lati*(pi/180);
				
				xt=(float)(longi* 20037508.342789244f / 180);
				zt=(float)(System.Math.Log(System.Math.Tan((90+lati)*System.Math.PI / 360))/(System.Math.PI / 180));
				zt = zt * 20037508.342789244f / 180;
				
				//xt = (float)(6378137.0f *(lonrad-lon0));
				//zt = (float)(6378137.0f * System.Math.Log((System.Math.Sin(latrad)+1)/System.Math.Cos(latrad)));
				
				//xt=(float)(r*System.Math.Cos(latrad)*System.Math.Cos(lonrad));
				//yt=(float)(r*System.Math.Cos(latrad)*System.Math.Sin(lonrad));
				//zt=(float)(r*System.Math.Sin(latrad));
				
				
				
				if (c+1==vitesse3d.Length)
				{
					timepos[c]= tempos;		
					vitesse3d[c]= (float)((v[3]<<24)+(v[2]<<16)+(v[1]<<8)+v[0])*0.036f;
					direction[c]=(double)((dir[3]<<24)+(dir[2]<<16)+(dir[1]<<8)+dir[0])*System.Math.Pow(10,-5);
					longitude[c]= longi;	
					latitude[c]= lati;	
					position[c]=  new Vector3(0,0,0);
					c++;
				}
				else
				{
					System.Array.Resize(ref timepos, c+1);
					System.Array.Resize(ref vitesse3d, c+1);
					System.Array.Resize(ref direction, c+1);
					System.Array.Resize(ref longitude, c+1);
					System.Array.Resize(ref latitude, c+1);
					System.Array.Resize(ref position, c+1);
					
					timepos[c]=tempos;
					vitesse3d[c]= (float)((v[3]<<24)+(v[2]<<16)+(v[1]<<8)+v[0])*0.036f;
					direction[c]=(double)((dir[3]<<24)+(dir[2]<<16)+(dir[1]<<8)+dir[0])*System.Math.Pow(10,-5);
					longitude[c]= longi;
					latitude[c]= lati;
					position[c]=  new Vector3(xt/1.4f,yt,zt/1.4f);//position[c]=  new Vector3(xt-(position[c-1].x),yt-(position[c-1].y),zt-(position[c-1].z));
					c++;
					
				}
				
				tramavecsat++;
				i=i+45+156+satellites*24;				
				//if(satellites!=0)
				//	print("test");
				vertrasat=vertrasat+(46+156+satellites*24);
				
			}
			else
			{
				
				tramsansat++;
				i=i+45;
			}
		}
		//Changement de repere 
		System.Array.Resize(ref disbetlonglat, c);
		System.Array.Resize(ref distentreptsxy, c);
		for(i= 0x0; i<c-1;i++)
		{
			disbetlonglat[i]= DistanceBetweenPlaces(longitude[i],latitude[i],longitude[i+1],latitude[i+1]);
			distentreptsxy[i]= (System.Math.Sqrt(System.Math.Pow((position[i+1].x-position[i].x),2)+System.Math.Pow((position[i+1].z-position[i].z),2)));
		}

	}
	public static double Radians(double x)
	{
		return x * System.Math.PI / 180;
	}


	public  double DistanceBetweenPlaces(	double lon1, double lat1, double lon2, double lat2)
	{
		double dlon = Radians(lon2 - lon1);
		double dlat = Radians(lat2 - lat1);
		
		double a = (System.Math.Sin(dlat / 2) * System.Math.Sin(dlat / 2)) + System.Math.Cos(Radians(lat1)) * System.Math.Cos(Radians(lat2)) * (System.Math.Sin(dlon / 2) * System.Math.Sin(dlon / 2));
		double angle = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));
		return angle * r;
	}

	public void pathcreation()
	{
		int m = 0;
		int n = 0;
		int j = startpoint (m,n);

		System.Array.Resize(ref path, 100);
		for(int i= 0; i<99;i++)
		{
			path[i+1].x=path[i].x + position[j+i+1].x-position[j+i].x;
			path[i+1].y=0.0f;
			path[i+1].z=path[i].z + position[j+i+1].z-position[j+i].z;
		}




	}


	public  int startpoint(int j,int comptdiff)
	{

		for(int i= 0x0; i<position.Length-1;i++)
		{
			float diffx = position[i+1].x-position[i].x;
			float diffz = position[i+1].z-position[i].z;
			if(diffx < 50  && diffz < 50 && diffz != 0 && diffx != 0 )//50 m/s(ou 600ms)
			{
				if(comptdiff > 100 )// ***************************on peut ajuster  la durer de lecture minimale de l'enregistrement 
				{
					j=i-100;
					print("position de depart : "+j);
					return j;
				}
				else
				{
					comptdiff++;					
				}
			}
			
		}
		return j;

	}

	public  void traitement()
	{
		
		print ("Il y a "+tramsansat+" trame(s) sans des données satelites, "+tramavecsat+" trame(s) avec des données satelites");
		int Verificationtaille=(tramsansat*46)+vertrasat;
		print("Verification du nombre de tram par rapport a la taille du fichier(78361950):"+Verificationtaille);
	}
	








}

