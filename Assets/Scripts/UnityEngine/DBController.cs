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
                for(prev = prev.AddDays(1); (today - prev).Days >= 0; prev = prev.AddDays(1)){

                    string code = prev.ToString("yyyy-MM-dd");
                    Debug.Log("Creating record for " + code);

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

    public void LogMedications(DateTime date){

        string code = TimeKeeper.dayCodes[(int)date.DayOfWeek];

        List<int> ids = new List<int>();

        reader = dao.query("SELECT id FROM doses WHERE " + code + "=1 AND active=1;");
        while(reader.Read()){
            ids.Add(int.Parse(reader[0].ToString()));
        }
        reader.Close();

        foreach(int id in ids){
            LogMedication(id, date);
        }

    }

    public void LogMedication(int id, DateTime date){

        string dateCode = date.ToString("yyyy-MM-dd");
        reader = dao.query("INSERT OR IGNORE INTO logs (id, date, status) VALUES (" + id + ",'" + dateCode + "',0);");
        reader.Close();

    }

    // insert medication into database; returns ID
    public int InsertMedication(Medication medication){
        
        string command = "INSERT INTO doses " + medication.GetSqlColumns() + " VALUES " + medication.GetSqlValues() + ";";
        reader = dao.query(command);
        reader.Close();

        reader = dao.query("SELECT MAX(id) FROM doses");
        int id = int.Parse(reader[0].ToString());
        reader.Close();
        
        LogMedications(TimeKeeper.GetDate());

        return id;

    }

    public List<int> GetDoseIDs(DateTime date){

        List<int> ids = new List<int>();
        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT id FROM logs WHERE date='" + d + "';");
        while(reader.Read()){
            ids.Add(int.Parse(reader[0].ToString()));
        }
        reader.Close();
        return ids;

    }

    public List<Medication> GetMedications(DateTime date){

        List<Medication> medications = new List<Medication>();
        string d = date.ToString("yyyy-MM-dd");
        string subquery = "SELECT id FROM logs WHERE date='" + d + "'";
        reader = dao.query("SELECT * FROM doses WHERE id in (" + subquery + ") ORDER BY time;");
        while(reader.Read()){
            medications.Add(new Medication(reader));
        }
        reader.Close();
        return medications;

    }

    public Medication GetMedication(int id){

        reader = dao.query("SELECT * FROM doses WHERE id=" + id + ";");
        Medication medication = new Medication(reader);
        reader.Close();
        return medication;

    }

    // 0 == untaken
    // 1 == taken
    // 2 == taken late
    public void ChangeLogStatus(int id, DateTime date, int status){

        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("UPDATE logs SET status=" + status + " WHERE id=" + id + " AND date='" + d + "';");
        reader.Close();

    }

    public int GetLogStatus(int id, DateTime date){

        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT status FROM logs WHERE id=" + id + " AND date='" + d + "';");
        int status = int.Parse(reader[0].ToString());
        reader.Close();
        return status;

    }

    public void UpdateMedication(Medication medication){



    }

}
