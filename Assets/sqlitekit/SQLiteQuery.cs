using System;
using System.Collections;


#if SQLITE_NATIVE
using Sqlite3Native;
using sqlite3 = System.IntPtr;
using vdbe = System.IntPtr;
using Sqlite3 = Sqlite3Native.Sqlite3;
#else
using Sqlite3 = Community.CsharpSqlite.Sqlite3;
using sqlite3 = Community.CsharpSqlite.Sqlite3.sqlite3;
using vdbe = Community.CsharpSqlite.Sqlite3.Vdbe;
#endif

public class SQLiteQuery {
	
	readonly public SQLiteDB SqlDb;
	private sqlite3  		 db;
	private vdbe     		 vm;
	private string[]         columnNames;
	private int[]            columnTypes;
	private int              bindIndex;
	
	public string[] Names { get {return columnNames;} }

	public SQLiteQuery( SQLiteDB sqliteDb, string query )
	{
		SqlDb = sqliteDb;
		bindIndex = 1;
		db = sqliteDb.Connection();
		if( Sqlite3.sqlite3_prepare16_v2( db, query, -1, ref vm, 
			#if !SQLITE_NATIVE
			0
			#else
			IntPtr.Zero
			#endif
			) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "Error with prepare query! error:" + Sqlite3.sqlite3_errmsg(db) ); 
		};
		SqlDb.RegisterQuery(this);
	}
	
	public void Reset()
	{
		bindIndex = 1;
		
		if( Sqlite3.sqlite3_reset( vm ) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "Error with sqlite3_reset!" ); 
		};
	}
	
	public void Release()
	{
		SqlDb.UnregisterQuery(this);
		
		if( Sqlite3.sqlite3_reset( vm ) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "Error with sqlite3_reset!" ); 
		};
		
		if( Sqlite3.sqlite3_finalize( vm ) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "Error with sqlite3_finalize!" ); 
		};
	}
	
	public void Bind( string str ) {BindAt(str,-1);}
	public void BindAt( string str, int bindAt )
	{
		if( bindAt == -1 )
		{
			bindAt = bindIndex++;
		}
		if( Sqlite3.sqlite3_bind_text16( vm, bindAt, str, -1, 
			#if !SQLITE_NATIVE
			null
			#else
			IntPtr.Zero
			#endif
		) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "SQLite fail to bind string with error: " + Sqlite3.sqlite3_errmsg(db) ); 
		};
	}
	
	public void Bind( int integer ) {BindAt(integer,-1);}
	public void BindAt( int integer, int bindAt )
	{
		if( bindAt == -1 )
		{
			bindAt = bindIndex++;
		}
		if( Sqlite3.sqlite3_bind_int( vm, bindAt, integer ) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "SQLite fail to bind integer with error: " + Sqlite3.sqlite3_errmsg(db) ); 
		};
	}
	
	public void Bind( long integer ) {BindAt(integer,-1);}
	public void BindAt( long integer, int bindAt )
	{
		if( bindAt == -1 )
		{
			bindAt = bindIndex++;
		}
		if( Sqlite3.sqlite3_bind_int64( vm, bindAt, integer ) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "SQLite fail to bind long with error: " + Sqlite3.sqlite3_errmsg(db) ); 
		};
	}
	
	public void Bind( double real ) {BindAt(real,-1);}
	public void BindAt( double real, int bindAt )
	{
		if( bindAt == -1 )
		{
			bindAt = bindIndex++;
		}
		if( Sqlite3.sqlite3_bind_double( vm, bindAt, real ) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "SQLite fail to bind double with error: " + Sqlite3.sqlite3_errmsg(db) ); 
		};
	}
	
	public void Bind( DateTime time ) {BindAt(time,-1);}
	public void BindAt( DateTime time, int bindAt )
	{
		if( bindAt == -1 )
		{
			bindAt = bindIndex++;
		}
		if( Sqlite3.sqlite3_bind_int64( vm, bindAt, time.ToBinary() ) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "SQLite fail to bind double with error: " + Sqlite3.sqlite3_errmsg(db) ); 
		};
	}
	
	public void Bind( byte[] blob ) {BindAt(blob,-1);}
	public void BindAt( byte[] blob, int bindAt )
	{
		if( bindAt == -1 )
		{
			bindAt = bindIndex++;
		}
		if( Sqlite3.sqlite3_bind_blob( vm, bindAt, blob, blob.Length, 
			#if !SQLITE_NATIVE
			null
			#else
			IntPtr.Zero
			#endif
		) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "SQLite fail to bind blob with error: " + Sqlite3.sqlite3_errmsg(db) ); 
		};
	}
	
	public void BindNull() {BindNullAt(-1);}
	public void BindNullAt( int bindAt )
	{
		if( bindAt == -1 )
		{
			bindAt = bindIndex++;
		}
		if( Sqlite3.sqlite3_bind_null( vm, bindAt ) != Sqlite3.SQLITE_OK ) 
		{ 
			throw new Exception( "SQLite fail to bind null error: " + Sqlite3.sqlite3_errmsg(db) ); 
		};
	}

	
	public bool Step()
	{
		switch( Sqlite3.sqlite3_step( vm ))
		{
		case Sqlite3.SQLITE_DONE: return false;
		case Sqlite3.SQLITE_ROW: 
		{
			
			int columnCount = Sqlite3.sqlite3_column_count( vm );
			columnNames = new string[columnCount];
			columnTypes = new int[columnCount];

			try
			{
				// reads columns one by one
				for ( int i = 0; i < columnCount; i++ )
				{
					columnNames[i] = Sqlite3.sqlite3_column_name( vm, i );
					columnTypes[i] = Sqlite3.sqlite3_column_type( vm, i );
				}
			}
			catch
			{
				throw new Exception( "SQLite fail to read column's names and types! error: " + Sqlite3.sqlite3_errmsg(db));
			}
			
			return true; 
		}
		}	
		throw new Exception( "SQLite step fail! error: " + Sqlite3.sqlite3_errmsg(db));
	}

	
	public bool IsNULL( string field )
	{
		int i = GetFieldIndex( field );
		return Sqlite3.SQLITE_NULL == columnTypes[i];
	}
	
	public int GetFieldType( string field )
	{
		for( int i = 0; i < columnNames.Length; i++ )
		{
			if( columnNames[i] == field )
				return columnTypes[i];
		}
		throw new Exception( "SQLite unknown field name: " + field);
	}	
	
	private int GetFieldIndex(string field)
	{
		for( int i = 0; i < columnNames.Length; i++ )
		{
			if( columnNames[i] == field )
				return i;
		}
		throw new Exception( "SQLite unknown field name: " + field);
	}
	
	public string GetString( string field )
	{
		int i = GetFieldIndex( field );
		if (Sqlite3.SQLITE_TEXT == columnTypes [i]) {
			return Sqlite3.sqlite3_column_text16 (vm, i);
		} else if (Sqlite3.SQLITE_NULL == columnTypes [i]) {
			return null;
		}
		
		throw new Exception( "SQLite wrong field type (expecting String) : " + field);
	}
	
	public int GetInteger( string field )
	{
		int i = GetFieldIndex( field );
		if( Sqlite3.SQLITE_INTEGER == columnTypes[i])
		{
			return Sqlite3.sqlite3_column_int( vm, i );
		}
		
		throw new Exception( "SQLite wrong field type (expecting Integer) : " + field);
	}
	
	public long GetLong( string field )
	{
		int i = GetFieldIndex( field );
		if( Sqlite3.SQLITE_INTEGER == columnTypes[i])
		{
			return Sqlite3.sqlite3_column_int64( vm, i );
		}
		
		throw new Exception( "SQLite wrong field type (expecting Integer) : " + field);
	}

	public DateTime GetDateTime( string field )
	{
		int i = GetFieldIndex( field );
		if( Sqlite3.SQLITE_INTEGER == columnTypes[i])
		{
			return DateTime.FromBinary(Sqlite3.sqlite3_column_int64( vm, i ));
		}
		
		throw new Exception( "SQLite wrong field type (expecting Integer) : " + field);
	}

	
	public double GetDouble( string field )
	{
		int i = GetFieldIndex( field );
		if( Sqlite3.SQLITE_FLOAT == columnTypes[i])
		{
			return Sqlite3.sqlite3_column_double( vm, i );
		}
		
		throw new Exception( "SQLite wrong field type (expecting Double) : " + field);
	}

	public float GetFloat( string field )
	{
		int i = GetFieldIndex( field );
		if( Sqlite3.SQLITE_FLOAT == columnTypes[i])
		{
			return (float) Sqlite3.sqlite3_column_double( vm, i );
		}

		throw new Exception( "SQLite wrong field type (expecting Double) : " + field);
	}


	public byte[] GetBlob( string field )
	{
		int i = GetFieldIndex( field );
		if( Sqlite3.SQLITE_BLOB == columnTypes[i])
		{
			return Sqlite3.sqlite3_column_blob( vm, i );
		}
		
		throw new Exception( "SQLite wrong field type (expecting byte[]) : " + field);
	}

}
