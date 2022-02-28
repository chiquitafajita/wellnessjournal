using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DBController : MonoBehaviour
{

    private Dao dao;
    private IDataReader reader;

    // open database at start of program
    void Start()
    {

        // open database at path
        string dbPath = "URI=file:" + Application.persistentDataPath + "/wjdb.sqlite";
        Debug.Log("Opened database at: " + dbPath);
        dao = new Dao(dbPath);  // create data access object
        dao.OpenDB();           // open database

        CreateTables();         // create tables if necessary

    }

    // close database upon program close
    private void OnDestroy() {
        dao.CloseDB();
        Debug.Log("Closed database");
    }

    // create tables if DNE: doses, daily records, logs
    private void CreateTables(){

        string command;

        command = "CREATE TABLE IF NOT EXISTS doses (id INT, name VARCHAR(255), time BIGINT, sun BOOL, mon BOOL, tue BOOL, wed BOOL, thu BOOL, fri BOOL, sat BOOL, active BOOL, stars INT, color INT, shape INT, PRIMARY KEY (id));";
        reader = dao.query(command);
        while(reader.Read()){
            for(int i = 0; i < reader.FieldCount; i++)
                Debug.Log(reader[i]);
        }
        reader.Close();

    }

    // insert medication into database; returns ID
    public int InsertMedication(Medication medication){
        
        string command = "INSERT INTO doses " + medication.GetSqlColumns() + " VALUES " + medication.GetSqlValues() + ";";
        Debug.Log(command);
        reader = dao.query(command);
        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
                Debug.Log(reader[i]);
        reader.Close();

        reader = dao.query("last_insert_rowid();");
        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
                Debug.Log(reader[i]);
        reader.Close();
        
        return 0;

    }

}
