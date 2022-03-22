using UnityEngine;
using System.Collections.Generic;
using System.Data;
using System;

public class DBController : MonoBehaviour
{

    private Dao dao;
    private IDataReader reader;

    // open database at start of program
    void Awake()
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

        dao.command("CREATE TABLE IF NOT EXISTS doses (id INTEGER PRIMARY KEY, name VARCHAR(255), time BIGINT, sun BOOL, mon BOOL, tue BOOL, wed BOOL, thu BOOL, fri BOOL, sat BOOL, active BOOL, stars INT, color INT, shape INT);");

        dao.command("CREATE TABLE IF NOT EXISTS dates (date DATE PRIMARY KEY, rating INTEGER, tags VARCHAR(512));");

        dao.command("CREATE TABLE IF NOT EXISTS logs (date DATE, id INTEGER, status INTEGER, PRIMARY KEY(date, id));");

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
                    dao.command("INSERT INTO dates (date, rating, tags) VALUES ('" + code + "',3,'');");
                    LogMedications(prev);

                }

            }


        }

        // else, create the first date and log its medications if any exist
        else{
            
            string code = today.ToString("yyyy-MM-dd");
            dao.command("INSERT INTO dates (date, rating, tags) VALUES ('" + code + "',0,'');");
        }

        
        LogMedications(today);
        

    }

    public void LogMedications(DateTime date){

        // get today's code
        string code = TimeKeeper.dayCodes[(int)date.DayOfWeek];
        
        // create list of ids
        List<int> ids = new List<int>();

        // select all IDs of doses scheduled for weekday
        reader = dao.query("SELECT id FROM doses WHERE " + code + "=1 AND active=1;");
        while(reader.Read()){
            ids.Add(int.Parse(reader[0].ToString()));
        }
        reader.Close();

        // log each scheduled dose
        foreach(int id in ids){
            LogMedication(id, date);
        }

    }

    // log a dose for a given date
    public void LogMedication(int id, DateTime date){

        string dateCode = date.ToString("yyyy-MM-dd");
        dao.command("INSERT OR IGNORE INTO logs (id, date, status) VALUES (" + id + ",'" + dateCode + "',0);");

    }

    // insert medication into database; returns ID
    public int InsertMedication(Medication medication){
        
        dao.command("INSERT INTO doses " + medication.GetSqlColumns() + " VALUES " + medication.GetSqlValues() + ";");

        reader = dao.query("SELECT MAX(id) FROM doses");
        int id = int.Parse(reader[0].ToString());
        reader.Close();
        
        LogMedications(TimeKeeper.GetDate());

        return id;

    }

    // get all medications scheduled for a day, according to logs
    public List<Medication> GetDosesForDay(DateTime date, bool showTakenDoses){

        List<Medication> medications = new List<Medication>();
        string d = date.ToString("yyyy-MM-dd");
        string subquery = "SELECT id FROM logs WHERE date='" + d + "'";
        if(!showTakenDoses) subquery += " AND status=0";
        reader = dao.query("SELECT * FROM doses WHERE id in (" + subquery + ") ORDER BY time;");
        while(reader.Read()){
            medications.Add(new Medication(reader));
        }
        reader.Close();
        return medications;

    }

    // get all dose IDs
    public List<Medication> GetAllMedications(){

        List<Medication> medications = new List<Medication>();
        reader = dao.query("SELECT * FROM doses ORDER BY time;");
        while(reader.Read()){
            medications.Add(new Medication(reader));
        }
        reader.Close();
        return medications;

    }

    // get dose by ID
    public Medication GetMedication(int id){

        reader = dao.query("SELECT * FROM doses WHERE id=" + id + ";");
        Medication medication = new Medication(reader);
        reader.Close();
        return medication;

    }

    // 0 == untaken
    // 1 == taken late
    // 2 == taken on time
    public void ChangeLogStatus(int id, DateTime date, int status){

        string d = date.ToString("yyyy-MM-dd");
        dao.command("UPDATE logs SET status=" + status + " WHERE id=" + id + " AND date='" + d + "';");

    }

    // get status of dose on a given day
    public int GetLogStatus(int id, DateTime date){

        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT status FROM logs WHERE id=" + id + " AND date='" + d + "';");
        int status = int.Parse(reader[0].ToString());
        reader.Close();
        return status;

    }

    // get letter grade of a day based on average of log scores
    public char GetDayGrade(DateTime date){

        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT AVG(status) FROM logs WHERE date='" + d + "';");
        float average = String.IsNullOrEmpty(reader[0].ToString()) ? -1 : float.Parse(reader[0].ToString()) / 2;
        reader.Close();

        char grade = 'F';

        if(average >= 0.9F)
            grade = 'A';
        else if(average >= 0.8F)
            grade = 'B';
        else if(average >= 0.7F)
            grade = 'C';
        else if(average >= 0.6F)
            grade = 'D';
        else if(average == -1)
            grade = '-';

        return grade;

    }

    // get average grade of past 7 days
    public char GetWeekGrade(DateTime date){

        date = date.AddDays(-7);
        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT AVG(status) FROM logs WHERE date>'" + d + "';");
        float average = String.IsNullOrEmpty(reader[0].ToString()) ? -1 : float.Parse(reader[0].ToString()) / 2;
        reader.Close();

        char grade = 'F';

        if(average >= 0.9F)
            grade = 'A';
        else if(average >= 0.8F)
            grade = 'B';
        else if(average >= 0.7F)
            grade = 'C';
        else if(average >= 0.6F)
            grade = 'D';
        else if(average == -1)
            grade = '-';

        return grade;

    }

    // get average grade of past 28 days
    public char GetMonthGrade(DateTime date){

        date = date.AddDays(-28);
        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT AVG(status) FROM logs WHERE date>'" + d + "';");
        float average = String.IsNullOrEmpty(reader[0].ToString()) ? -1 : float.Parse(reader[0].ToString()) / 2;
        reader.Close();

        char grade = 'F';

        if(average >= 0.9F)
            grade = 'A';
        else if(average >= 0.8F)
            grade = 'B';
        else if(average >= 0.7F)
            grade = 'C';
        else if(average >= 0.6F)
            grade = 'D';
        else if(average == -1)
            grade = '-';

        return grade;

    }

    // update medication in database
    public void UpdateMedication(Medication medication){

        // get existing record
        Medication oldRecord = GetMedication(medication.ID);

        // update record
        dao.command("UPDATE doses SET " + medication.GetUpdate() + " WHERE id=" + medication.ID + ";");

        // get current day of week
        int dayOfWeek = TimeKeeper.GetDayOfWeek();

        // if scheduled times differ between old record and update
        if(oldRecord.Weekdays[dayOfWeek] != medication.Weekdays[dayOfWeek]){
            
            string d = TimeKeeper.GetDate().ToString("yyyy-MM-dd");

            // if now the dose is scheduled for that day and it is today, create log
            if(medication.Weekdays[dayOfWeek]){
                LogMedication(medication.ID, TimeKeeper.GetDate());
            }

            // otherwise, delete the log scheduled for today
            else{
                dao.command("DELETE FROM logs WHERE id=" + medication.ID + " AND date='" + d + "';");
            }

        }

    }

    // delete medication from database and remove all associated logs
    public void DeleteMedication(Medication medication){

        dao.command("DELETE FROM doses WHERE id=" + medication.ID + ";");

        dao.command("DELETE FROM logs WHERE id=" + medication.ID + ";");

    }

    // check if day is in database
    // (we have recorded all days after the earliest)
    public bool IsDayRecorded(DateTime date){

        return date >= GetEarliestDate() && date <= TimeKeeper.GetDate();

    }

    // get earliest date in database
    public DateTime GetEarliestDate(){

        reader = dao.query("SELECT min(date) FROM dates;");
        DateTime oldest = DateTime.ParseExact(reader[0].ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        reader.Close();

        return oldest;

    }

    // update day rating with number 1-5 (default is 3)
    public void UpdateDayRating(DateTime date, int rating){

        string d = date.ToString("yyyy-MM-dd");
        dao.command("UPDATE dates SET rating=" + rating + " WHERE date='" + d + "';");
        
    }

    // get day rating in database
    public int GetDayRating(DateTime date){

        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT rating FROM dates WHERE date='" + d + "';");
        int rating = 0;
        try{
            rating = int.Parse(reader[0].ToString());
        }
        catch(FormatException e){
            Debug.LogWarning("Wrong Format: " + reader[0].ToString() + "\n" + e);
        }
        reader.Close();
        return rating;

    }

    // get week rating in database as average of last 7 days
    public float GetWeekRating(DateTime date){

        date = date.AddDays(-7);
        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT AVG(rating) FROM dates WHERE date>'" + d + "';");
        float rating = 0;
        try{
            rating = float.Parse(reader[0].ToString());
        }
        catch(FormatException e){
            Debug.LogWarning("Wrong Format: " + reader[0].ToString() + "\n" + e);
        }
        reader.Close();
        return rating;

    }

    // get month rating in database as average of last 28 days
    public float GetMonthRating(DateTime date){

        date = date.AddDays(-28);
        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT AVG(rating) FROM dates WHERE date>'" + d + "';");
        float rating = 0;
        try{
            rating = float.Parse(reader[0].ToString());
        }
        catch(FormatException e){
            Debug.LogWarning("Wrong Format: " + reader[0].ToString() + "\n" + e);
        }
        reader.Close();
        return rating;

    }

    // replace day's microjournal
    public void UpdateDayTags(DateTime date, string tags){

        string d = date.ToString("yyyy-MM-dd");
        dao.command("UPDATE dates SET tags='" + tags + "' WHERE date='" + d + "';");

    }

    // get day's microjournal
    public string GetDayTags(DateTime date){

        string d = date.ToString("yyyy-MM-dd");
        reader = dao.query("SELECT tags FROM dates WHERE date='" + d + "';");
        string tags = reader[0].ToString();
        reader.Close();
        return tags;

    }

}
