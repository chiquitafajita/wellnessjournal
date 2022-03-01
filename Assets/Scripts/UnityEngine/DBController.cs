using UnityEngine;
using System.Collections.Generic;
using System.Data;
using System;

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
        CheckDate();            // check date and create new if necessary

    }

    // close database upon program close
    private void OnDestroy() {
        dao.CloseDB();
        Debug.Log("Closed database");
    }

    // create tables if DNE: doses, daily records, logs
    private void CreateTables(){

        string command;

        command = "CREATE TABLE IF NOT EXISTS doses (id INTEGER PRIMARY KEY, name VARCHAR(255), time BIGINT, sun BOOL, mon BOOL, tue BOOL, wed BOOL, thu BOOL, fri BOOL, sat BOOL, active BOOL, stars INT, color INT, shape INT);";
        dao.query(command).Close();

        command = "CREATE TABLE IF NOT EXISTS dates (date DATE PRIMARY KEY, rating INTEGER, tags VARCHAR(512));";
        dao.query(command).Close();

        command = "CREATE TABLE IF NOT EXISTS logs (date DATE, id INTEGER, status INTEGER, PRIMARY KEY(date, id));";
        dao.query(command).Close();

    }

    private void CheckDate(){



        DateTime today = TimeKeeper.GetDate();
        

        // find previous date recorded
        DateTime prev = new DateTime(0);
        reader = dao.query("SELECT MAX(date) FROM dates;");
        while(reader.Read()){

            // if output is null or empty, there is no prev date
            if(String.IsNullOrEmpty(reader[0].ToString())) break;

            // otherwise, there is a prev date so we'll access it
            prev = DateTime.ParseExact(reader[0].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            Debug.Log("Previous day opened: " + prev);
        }
        reader.Close();

        // if a previous date exists, compare to today's date and fill in calendar gaps
        if(prev.Ticks > 0){

            // get days since app was last opened
            int daysSince = (today - prev).Days;
            Debug.Log(daysSince + " days have passed since app was last opened.");

            // if it has been a couple of days
            if(daysSince > 0){

                // for each day from previous recorded to today
                for(prev.AddDays(1); (today - prev).Days >= 0; prev.AddDays(1)){

                    string code = prev.ToString("yyyy-MM-dd");

                    // insert missing day into database and log medications
                    dao.query("INSERT INTO dates (date, rating, tags) VALUES ('" + code + "',0,'');").Close();
                    LogMedications(prev);

                }

            }


        }

        // else, create the first date and log its medications if any exist
        else{
            
            string code = today.ToString("yyyy-MM-dd");
            dao.query("INSERT INTO dates (date, rating, tags) VALUES ('" + code + "',0,'');").Close();
        }

        
        LogMedications(today);
        

    }

    public List<Medication> LogMedications(DateTime date){

        string code = TimeKeeper.dayCodes[(int)date.DayOfWeek];

        List<Medication> medications = new List<Medication>();

        reader = dao.query("SELECT * FROM doses WHERE " + code + "=1 AND active=1;");
        while(reader.Read()){
            Debug.Log(new Medication(reader).GetSqlValues());
        }

        reader.Close();

        return medications;

    }

    // insert medication into database; returns ID
    public int InsertMedication(Medication medication){
        
        string command = "INSERT INTO doses " + medication.GetSqlColumns() + " VALUES " + medication.GetSqlValues() + ";";
        reader = dao.query(command);
        reader.Close();
        
        return 0;

    }

    public void UpdateMedication(Medication medication){



    }

}
