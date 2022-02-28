using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class Dao
{

    private IDbConnection dbConnection;
    private IDbCommand dbCommand;
    private IDataReader dbReader;
    private string dbpath;

    // create a DAO object
    public Dao(string path){

        dbpath = path;

    }

    // this function should first be called to open the database file
    public void OpenDB(){

        // throw exception if path is null or empty
        if(string.IsNullOrEmpty(dbpath)) throw new IOException("No path for database.");

        // create and open database connection at path indicated
        dbConnection = new SqliteConnection(dbpath);
        dbConnection.Open();

        // get command object from database connection
        dbCommand = dbConnection.CreateCommand();

    }

    // close database after use
    public void CloseDB(){

        dbConnection.Close();

    }

    // submit a command to the database
    // do NOT use if you expect output!!
    public void command(string command){

        // throw IO exception if database connection does not exist
        if(dbConnection == null) throw new IOException("Database connection not opened.");

        // set command text and execute as non-query
        dbCommand.CommandText = command;
        dbCommand.ExecuteNonQuery();

    }

    // submit a query to the database and return a data reader
    public IDataReader query(string query){

        // throw IO exception if database connection does not exist
        if(dbConnection == null) throw new IOException("Database connection not opened.");

        // set command text and execute as a readable query
        dbCommand.CommandText = query;
        return dbCommand.ExecuteReader();

    }

}
