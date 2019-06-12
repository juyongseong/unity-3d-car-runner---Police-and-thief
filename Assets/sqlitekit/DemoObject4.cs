
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using SQLiteExtension;
using System.Linq;

public class DemoObject4 : MonoBehaviour {



	private SQLiteDB db = null;
	private bool inprogress = false;

	private string log;

	void Start () {

	}
	
	void Update () {
	
	}
	
	void OnGUI()
	{
		if( db == null && inprogress == false )
		{
			if ( GUI.Button(new Rect (10,10,150,50), "Run Performance Test") ) 
			{
				StartCoroutine(PerformanceTestCoroutine());
			}

			if ( GUI.Button(new Rect (10,70,150,70), "Run Async SQLite Test") ) 
			{
				inprogress = true;
				StartCoroutine(AsyncPerformanceTestCoroutine());
			}
						
		}
		else
		{
			if ( GUI.Button(new Rect (10,10,150,50), "Back") ) 
			{
				db = null;
				inprogress = false;
			}
			GUI.Label (new Rect (10,70,600,600), log);
		}
	}
	
	IEnumerator AsyncPerformanceTestCoroutine()
	{
		Stopwatch stopwatch = new Stopwatch();

		log = "Coroutine Api Test";

		// Copy Database
		yield return StartCoroutine( 
			CopyFileFromStreamingAssetsToPersistanceFolder("db.sqlite") );


		SQLiteExt.Handle handle = new SQLiteExt.Handle();

		// Open Database
		stopwatch.Start();

		yield return StartCoroutine( 
			this.SQLiteOpenDatabase(Application.persistentDataPath + "/db.sqlite", handle) );

		stopwatch.Stop();
		
		log += "\n Open Database: " + stopwatch.ElapsedMilliseconds + " msec";




		// Select
		stopwatch.Start();
		
		yield return StartCoroutine( 
		    this.SQLiteQuery("SELECT * FROM en WHERE word like 'a%' LIMIT 1", null, handle) );
		
		stopwatch.Stop();
		
		log += "\n Create Query: " + stopwatch.ElapsedMilliseconds + " msec";


		// Step
		stopwatch.Start();
		
		yield return StartCoroutine( 
            this.SQLiteStep(null, handle) );
		
		stopwatch.Stop();
		
		log += "\n Step: " + stopwatch.ElapsedMilliseconds + " msec";


		if(handle.Success)
		{

			UnityEngine.Debug.Log(handle.Query.GetString("word"));

		}


		// release query
		stopwatch.Start();
		
		yield return StartCoroutine( 
			this.SQLiteRelease(handle) );
		
		stopwatch.Stop();
		log += "\n Relese query: " + stopwatch.ElapsedMilliseconds + " msec";




		// Close database
		stopwatch.Start();

		yield return StartCoroutine( 
		    this.SQLiteCloseDatabase(handle) );

		stopwatch.Stop();
		log += "\n Close Database: " + stopwatch.ElapsedMilliseconds + " msec";

		log += "\ndone.";
	}

	IEnumerator CopyFileFromStreamingAssetsToPersistanceFolder(string dbfilename)
	{
		// a product persistant database path.
		string filename = Application.persistentDataPath + "/" + dbfilename;

		yield return 0;

		// check if database already exists.
		
		if(!File.Exists(filename))
		{
			
			// ok , this is first time application start!
			// so lets copy prebuild dtabase from StreamingAssets and load store to persistancePath with Test2

			byte[] bytes = null;				
			
			
			#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
			string dbpath = "file://" + Application.streamingAssetsPath + "/" + dbfilename; log += "asset path is: " + dbpath;
			WWW www = new WWW(dbpath);
			yield return www;
			bytes = www.bytes;
			#elif UNITY_WEBPLAYER
			string dbpath = "StreamingAssets/" + dbfilename;								log += "asset path is: " + dbpath;
			WWW www = new WWW(dbpath);
			yield return www;
			bytes = www.bytes;
			#elif UNITY_IPHONE
			string dbpath = Application.dataPath + "/Raw/" + dbfilename;					log += "asset path is: " + dbpath;					
			try{	
				using ( FileStream fs = new FileStream(dbpath, FileMode.Open, FileAccess.Read, FileShare.Read) ){
					bytes = new byte[fs.Length];
					fs.Read(bytes,0,(int)fs.Length);
				}			
			} catch (Exception e){
				log += 	"\nTest Fail with Exception " + e.ToString();
				log += 	"\n";
			}
			#elif UNITY_ANDROID
			string dbpath = Application.streamingAssetsPath + "/" + dbfilename;	            log += "asset path is: " + dbpath;
			WWW www = new WWW(dbpath);
			yield return www;
			bytes = www.bytes;
			#endif
			if ( bytes != null )
			{
				try{	
					
					//
					//
					// copy database to real file into cache folder
					using( FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write) )
					{
                        fs.Write(bytes,0,bytes.Length);             log += "\nCopy database from streaminAssets to persistentDataPath: " + filename;
                    }
                    
                } catch (Exception e){
                    log += 	"\nTest Fail with Exception " + e.ToString();
                    log += 	"\n\n Did you copy test.db into StreamingAssets ?\n";
                }
			}
		}
	}
        
	IEnumerator PerformanceTestCoroutine()
	{
		db = new SQLiteDB();
				
		log = "";

		yield return StartCoroutine( 
			CopyFileFromStreamingAssetsToPersistanceFolder("db.sqlite"));

		// a product persistant database path.
		string filename = Application.persistentDataPath + "/db.sqlite";
	
		// it mean we already download prebuild data base and store into persistantPath
		// lest update, I will call Test
		
		try{
			//
			// initialize database
			//
			db.Open(filename);                               log += "Database created! filename:"+filename;
			
			PerformanceTest(db, ref log);
			
		} catch (Exception e){
			log += 	"\nTest Fail with Exception " + e.ToString();
		}
		

	}

	
	void PerformanceTest( SQLiteDB db, ref string log )
	{
		SQLiteQuery qr;

		Stopwatch stopwatch = new Stopwatch();

		stopwatch.Start();

		qr = new SQLiteQuery(db, "SELECT * FROM en WHERE word like 'a%' LIMIT 1");
		qr.Step();												

		UnityEngine.Debug.Log(qr.GetString("word"));

		qr.Release();                                        log += "\nSelect.";

		stopwatch.Stop();

		log += stopwatch.ElapsedMilliseconds + " msec";
		
		//
		// if we reach that point it's mean we pass the test!
		db.Close();                                           log += "\nDatabase closed!\nTest succeeded!";

	}

	
	
	
	
	
}
