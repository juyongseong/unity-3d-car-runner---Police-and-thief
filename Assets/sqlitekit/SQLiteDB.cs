using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if SQLITE_NATIVE
//using Sqlite3Native;
using sqlite3 = System.IntPtr;
using vdbe = System.IntPtr;
using Sqlite3 = Sqlite3Native.Sqlite3;
#else
using Sqlite3 = Community.CsharpSqlite.Sqlite3;
using sqlite3 = Community.CsharpSqlite.Sqlite3.sqlite3;
using vdbe = Community.CsharpSqlite.Sqlite3.Vdbe;
#endif

public class SQLiteDB 
{
	
    
	private sqlite3        db;
	#if !SQLITE_NATIVE
	private Sqlite3.sqlite3_stream stream;
	#endif
	
#region Query Registration
	private List<SQLiteQuery>      queries = new List<SQLiteQuery>();
	
    public void RegisterQuery(SQLiteQuery qr)
    {
        queries.Add(qr);
    }

    public void UnregisterQuery(SQLiteQuery qr)
    {
        queries.Remove(qr);
    }

    public void ReleaseAllQueries()
    {
        SQLiteQuery[] qrs = queries.ToArray();
        foreach (SQLiteQuery q in qrs)
        {
            q.Release();
        }
        queries.Clear();
    }
#endregion
	
	public SQLiteDB()
	{
		
		#if !SQLITE_NATIVE
		db = null;
		stream = null;
		#else
		db = IntPtr.Zero;
		#endif
	}
	
	public void Open(string filename)
	{
		#if !SQLITE_NATIVE
		if( db != null )
		#else
		if( db != IntPtr.Zero )
		#endif
		{
			throw new Exception( "Error database already open!" );
		}
		
		if ( Sqlite3.sqlite3_open( filename, out db ) != Sqlite3.SQLITE_OK )
		{
			#if !SQLITE_NATIVE
			db = null;
			#else
			db = IntPtr.Zero;
			#endif
			throw new IOException( "Error with opening database " + filename + " !" );
		}
	}

	public void OpenInMemory() 
	{
		#if !SQLITE_NATIVE
		if( db != null )
		#else
		if( db != IntPtr.Zero )
		#endif
		{
			throw new Exception( "Error database already open!" );
		}
		
		if ( Sqlite3.sqlite3_open( ":memory:", out db ) != Sqlite3.SQLITE_OK )
		{
			#if !SQLITE_NATIVE
			db = null;
			#else
			db = IntPtr.Zero;
			#endif
			throw new IOException( "Error with opening database :memory:!" );
		}
	}

	#if !SQLITE_NATIVE
	public void OpenStream(string name, Stream io) 
	{
		if( db != null )
		{
			throw new Exception( "Error database already open!" );
		}
		
		stream = Sqlite3.sqlite3_stream_create(name, io);

        if ( Sqlite3.sqlite3_stream_register(stream) != Sqlite3.SQLITE_OK )
        {
            throw new IOException("Error with opening database with stream " + name + "!");
        }

        if (Sqlite3.sqlite3_open_v2(name, out db, Sqlite3.SQLITE_OPEN_READWRITE, "stream") != Sqlite3.SQLITE_OK)
		{
			db = null;
			throw new IOException( "Error with opening database with stream " + name + "!" );
		}
	}

	public void Key(string hexkey)
	{
		Sqlite3.sqlite3_key(db,hexkey,hexkey.Length);
	}

	public void Rekey(string hexkey)
	{
		Sqlite3.sqlite3_rekey(db,hexkey,hexkey.Length);
	}
	#endif

	public sqlite3 Connection()
	{
		return db;
	}
	
	public long LastInsertRowId()
	{
		#if !SQLITE_NATIVE
		if( db == null )
		#else
		if( db == IntPtr.Zero )
		#endif
		{
			throw new Exception( "Error database not ready!" );
		}
		
		return Sqlite3.sqlite3_last_insert_rowid(db);
	}
	
	public void Close()
	{
		
		ReleaseAllQueries();
		

		#if !SQLITE_NATIVE
		if( db != null )
		{
			Sqlite3.sqlite3_close( db );
			db = null;
		}

		if( stream != null )
		{
			Sqlite3.sqlite3_stream_unregister(stream);
		}
		#else
		if( db != IntPtr.Zero )
		{
			Sqlite3.sqlite3_close( db );
			db = IntPtr.Zero;
		}
		#endif
	}
	
}
/*

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using Community.CsharpSqlite;
//using SQLite4Unity3d;
//using Sqlite3 = SQLite4Unity3d.SQLite3;
using Sqlite3Native;

public class SQLiteDB 
{

	private System.IntPtr  db;

	#region Query Registration
	private List<SQLiteQuery>      queries = new List<SQLiteQuery>();

	public void RegisterQuery(SQLiteQuery qr)
	{
		queries.Add(qr);
	}

	public void UnregisterQuery(SQLiteQuery qr)
	{
		queries.Remove(qr);
	}

	public void ReleaseAllQueries()
	{
		SQLiteQuery[] qrs = queries.ToArray();
		foreach (SQLiteQuery q in qrs)
		{
			q.Release();
		}
		queries.Clear();
	}
	#endregion

	public SQLiteDB()
	{
		db = System.IntPtr.Zero;
	}

	public void Open(string filename)
	{
		if( db != System.IntPtr.Zero )
		{
			throw new Exception( "Error database already open!" );
		}

		if ( Sqlite3.sqlite3_open( filename, out db ) != Sqlite3.SQLITE_OK )
		{
			db = System.IntPtr.Zero;
			throw new IOException( "Error with opening database " + filename + " !" );
		}
	}

	public void OpenInMemory() 
	{
		if( db != System.IntPtr.Zero )
		{
			throw new Exception( "Error database already open!" );
		}

		if ( Sqlite3.sqlite3_open( ":memory:", out db ) != Sqlite3.SQLITE_OK )
		{
			db = System.IntPtr.Zero;
			throw new IOException( "Error with opening database :memory:!" );
		}
	}



	public System.IntPtr Connection()
	{
		return db;
	}

	public long LastInsertRowId()
	{
		if( db == System.IntPtr.Zero )
		{
			throw new Exception( "Error database not ready!" );
		}

		return Sqlite3.sqlite3_last_insert_rowid(db);
	}

	public void Close()
	{

		ReleaseAllQueries();

		if( db != System.IntPtr.Zero )
		{
			Sqlite3.sqlite3_close( db );
			db = System.IntPtr.Zero;
		}

	}

}
*/