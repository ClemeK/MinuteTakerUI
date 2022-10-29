using Dapper;
using MinuteTaker.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace MinuteTaker
{
    internal class SQLiteDataAccess
    {
        /// <summary>
        /// Retrieves the connection string from the Configure Manager
        /// </summary>
        /// <param name="id"></param>
        /// <returns>string = connection name</returns>
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        // ************************************************
        //                  C O N F I G
        // ************************************************
        /// <summary>
        /// Get a Config Value
        /// </summary>
        /// <param name="key">for the key provided</param>
        /// <returns>ConfigModel</returns>
        public static ConfigModel GetKeyValue(string key)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var queryResult = cnn.Query<ConfigModel>("select * from MyConfig where MyKey = \"" + key.Trim() + "\"", new DynamicParameters());

                List<ConfigModel> output = new();
                output = queryResult.ToList();

                return output[0];
            }
        }
        /// <summary>
        /// Update a Config value
        /// </summary>
        /// <param name="c">for a given ConfigModel</param>
        public static void UpdateKeyValue(ConfigModel c)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update MyConfig set" +
                    " MyValue = \"" + c.MyValue + "\"" +
                    " where MyKey = \"" + c.MyKey + "\"";

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        //                  P E O P L E
        // ************************************************
        /// <summary>
        /// Load Person
        /// </summary>
        /// <returns>List of People</returns>
        public static List<PersonModel> LoadPerson()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PersonModel>("select * from Person", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Updates a Person in the DB
        /// </summary>
        /// <param name="person">Person to update</param>
        public static void UpdatePerson(PersonModel person)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update Person set" +
                    " FirstName = \"" + person.FirstName + "\" " +
                    ", LastName = \"" + person.LastName + "\" " +
                    ", EmailAddress = \"" + person.EmailAddress + "\" " +
                    ", PhoneNbr = \"" + person.PhoneNbr + "\" " +
                    "where Id = " + person.Id.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        /// <summary>
        /// Saves a Person to the DB
        /// </summary>
        /// <param name="person">Person to save</param>
        public static void SavePerson(PersonModel person)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Person (FirstName, LastName, EmailAddress, PhoneNbr) " +
                    "values (@FirstName, @LastName, @EmailAddress, @PhoneNbr)", person);
            }
        }
        // ************************************************
        /// <summary>
        /// Fetch's a Person from the DB
        /// </summary>
        /// <param name="id">Person ID to Fetch</param>
        /// <returns>Person as a List</returns>
        public static List<PersonModel> GetPerson(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PersonModel>("select * from Person where Id = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Deletes a Person from the DB
        /// </summary>
        /// <param name="id">Person ID to delete</param>
        public static void DeletePerson(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<PersonModel>("delete from Person where Id = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        //                  G A N G S
        // ************************************************
        /// <summary>
        /// Load Gangs
        /// </summary>
        /// <returns>List of Gang</returns>
        public static List<GangModel> LoadGangs()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<GangModel>("select * from Gang", new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Saves a Gang to the DB
        /// </summary>
        /// <param name="Gang">Group to save</param>
        public static void SaveGang(GangModel gang)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Gang (Name) " +
                    "values (@Name)", gang);
            }
        }
        // ************************************************
        /// <summary>
        /// Fetch's a Gang from the DB
        /// </summary>
        /// <param name="id">Gang ID to Fetch</param>
        /// <returns>Gang as a List</returns>
        public static List<GangModel> GetGang(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<GangModel>("select * from Gang where Id = " + id.ToString(), new DynamicParameters());

                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Deletes a Gang from the DB
        /// </summary>
        /// <param name="id">Group ID to delete</param>
        public static void DeleteGang(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<GangModel>("delete from Gang where Id = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        //            G A N G  M E M B E R S
        // ************************************************
        /// <summary>
        /// Load member of a gang
        /// </summary>
        /// <param name="Id">Gang Id</param>
        /// <returns>List of members</returns>
        public static List<GangMembersModel> LoadMembers(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<GangMembersModel>("select * from GangMembers where GangId = " + id.ToString(),
                                new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Save a Gang Member to db
        /// </summary>
        /// <param name="gm">Gang Member to save</param>
        public static void SaveGangMember(GangMembersModel gm)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into GangMembers (GangId, PersonId) " +
                    "values (@GangId, @PersonId)", gm);
            }
        }
        // ************************************************
        /// <summary>
        /// Deletes a GangMember from the DB
        /// </summary>
        /// <param name="id">Group ID to delete</param>
        public static void DeleteGangMember(int Gid, int Pid)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<GangModel>("delete from GangMembers where GangId = " + Gid.ToString() + " and PersonId = " + Pid.ToString()
                    , new DynamicParameters());
            }
        }
        // ************************************************
        //                  A G E N D A S
        // ************************************************
        /// <summary>
        /// Load Agendas
        /// </summary>
        /// <returns>List of Agendas</returns>
        public static List<AgendaModel> LoadAgendas(int archive)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string q = "select * from Agenda where Archive = " + archive.ToString();

                var output = cnn.Query<AgendaModel>(q, new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Saves a Agenda to the DB
        /// </summary>
        /// <param name="Agenda">Agenda to save</param>
        public static void SaveAgenda(AgendaModel agenda)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Agenda (Title, Location, GangId, Year, Month, Day, Hour, Minute, Archive) " +
                    "values (@Title, @Location, @GangId, @Year, @Month, @Day, @Hour, @Minute, @Archive)", agenda);
            }
        }
        // ************************************************
        /// <summary>
        /// Update a Agenda to the DB
        /// </summary>
        /// <param name="agenda">Agenda Id</param>
        public static void UpdateAgenda(AgendaModel agenda)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update Agenda set" +
                    " Title = \"" + agenda.Title + "\" " +
                    ", Location = \"" + agenda.Location + "\" " +
                    ", GangId = " + agenda.GangId.ToString() + " " +
                    ", Year = " + agenda.Year.ToString() + " " +
                    ", Month = " + agenda.Month.ToString() + " " +
                    ", Day = " + agenda.Day.ToString() + " " +
                    ", Hour = " + agenda.Hour.ToString() + " " +
                    ", Minute = " + agenda.Minute.ToString() + " " +
                    ", Archive = " + agenda.Archive + " " +
                    "where Id = " + agenda.Id.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        /// <summary>
        /// Deletes a Agenda from the DB
        /// </summary>
        /// <param name="id">Agenda ID to delete</param>
        public static void DeleteAgenda(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<GangModel>("delete from Agenda where Id = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        //                  T O P I C S
        // ************************************************
        /// <summary>
        /// Load Specific Topics
        /// </summary>
        /// <param name="Agenda id"></param>
        /// <returns>List of Topics</returns>
        public static List<TopicModel> LoadTopics(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TopicModel>("select * from Topic where AgendaId = " + id.ToString(), new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Saves a Topic to the DB
        /// </summary>
        /// <param name="Topics">Topic to save</param>
        public static void SaveTopic(TopicModel topic)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Topic (AgendaId, ItemNbr, Heading) " +
                    "values (@AgendaId, @ItemNbr, @Heading)", topic);
            }
        }
        /// <summary>
        /// Update a Topic
        /// </summary>
        /// <param name="topic">Topic Model to Update</param>
        public static void UpdateTopic(TopicModel topic)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update Topic set" +
                    " Detail = \"" + topic.Detail + "\"" +
                    " where Id = " + topic.Id.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        /// <summary>
        /// Deletes a Topic from the DB
        /// </summary>
        /// <param name="id">Topic ID to delete</param>
        public static void DeleteTopic(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<TopicModel>("delete from Topic where Id = " + id.ToString(), new DynamicParameters());
            }
        }
        // ************************************************
        //            A G E N D A  M E M B E R S
        // ************************************************
        /// <summary>
        /// Load member of an Agenda
        /// </summary>
        /// <param name="Id">Agenda Id</param>
        /// <returns>List of members</returns>
        public static List<AgendaMembersModel> LoadAgMbrs(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<AgendaMembersModel>("select * from AgMembers where AgendaId = " + id.ToString(),
                                new DynamicParameters());
                return output.ToList();
            }
        }
        // ************************************************
        /// <summary>
        /// Save a Agenda Member to db
        /// </summary>
        /// <param name="gm">Agenda Member to save</param>
        public static void SaveAgendaMember(AgendaMembersModel am)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into AgMembers (AgendaId, PersonId, Attend) " +
                    "values (@AgendaId, @PersonId, @Attend)", am);
            }
        }
        /// <summary>
        /// Update Agenda Member
        /// </summary>
        /// <param name="am">Member to Update</param>
        public static void UpdateAgendaMember(AgendaMembersModel am)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string UpdateText = "update AgMembers set" +
                    // " AgendaId = " + am.AgendaId.ToString() +
                    // ", PersonId = " + am.PersonId.ToString() +
                    // ", Attend = " + am.Attend.ToString() +
                    " Attend = " + am.Attend.ToString() +
                    " where AgendaId = " + am.AgendaId.ToString() +
                    " and PersonId = " + am.PersonId.ToString();

                cnn.Execute(UpdateText, new DynamicParameters());
            }
        }
        // ************************************************
        /// <summary>
        /// Deletes AgendaMember from the DB
        /// </summary>
        /// <param name="Aid">Agenda Id</param>
        /// <param name="Pid">Person Id</param>
        public static void DeleteAgendaMember(int Aid, int Pid)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<AgendaMembersModel>("delete from AgMembers where AgendaId = " + Aid.ToString() + " and PersonId = " + Pid.ToString()
                    , new DynamicParameters());
            }
        }
        // ************************************************
        /// <summary>
        /// Delete *ALL AgendaMembers
        /// </summary>
        /// <param name="Aid">Agenda Id</param>
        public static void DeleteAllAgendaMember(int Aid)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<AgendaMembersModel>("delete from AgMembers where AgendaId = " + Aid.ToString()
                    , new DynamicParameters());
            }
        }
        // ************************************************
        //                  S M T P s
        // ************************************************
        /// <summary>
        /// Load SMTPs
        /// </summary>
        /// <returns>List of SMTPs</returns>
        public static List<SmtpsModel> LoadSmtps()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<SmtpsModel>("select * from Smtps", new DynamicParameters());
                return output.ToList();
            }
        }
    }
}