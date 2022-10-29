using MinuteTaker.Models;
using System.Collections.Generic;

namespace MinuteTaker
{
    public class MinuteTakerLibary
    {
        // ************************************************
        //                  P E O P L E
        // ************************************************
        public static List<PersonModel> RefreshPeople()
        {
            List<PersonModel> output = new();

            output = SQLiteDataAccess.LoadPerson();

            // Sort the List By LastName then FirstName
            output.Sort((x, y) => x.LastName.CompareTo(y.LastName));
            output.Sort((x, y) => x.FirstName.CompareTo(y.FirstName));

            return output;
        }

        public static void SavePerson(string first, string last, string email, string phone)
        {
            PersonModel p = new PersonModel();
            p.FirstName = first;
            p.LastName = last;
            p.EmailAddress = email;
            p.PhoneNbr = phone;

            SQLiteDataAccess.SavePerson(p);
        }

        public static void DeletePerson(int id)
        {
            SQLiteDataAccess.DeletePerson(id);
        }

        public static void UpdatePerson(PersonModel p)
        {
            SQLiteDataAccess.UpdatePerson(p);
        }

        // ************************************************
        //                  G A N G S
        // ************************************************
        public static List<GangModel> RefreshGangs()
        {
            List<GangModel> output = new();

            output = SQLiteDataAccess.LoadGangs();

            // Sort the List
            output.Sort((x, y) => x.Name.CompareTo(y.Name));

            // For each gang get there members
            foreach (var g in output)
            {
                List<GangMembersModel> members = new();
                members = SQLiteDataAccess.LoadMembers(g.Id);

                // For each Gang Member Id get the Person
                foreach (var m in members)
                {
                    List<PersonModel> p = new();
                    p = SQLiteDataAccess.GetPerson(m.PersonId);
                    g.Members.Add(p[0]);
                }
            }

            return output;
        }

        public static GangModel GetGang(int id)
        {
            List<GangModel> list = new();
            GangModel output = new();

            list = SQLiteDataAccess.GetGang(id);

            output = list[0];

            // Get the members of the Gang
            List<GangMembersModel> members = new();
            members = SQLiteDataAccess.LoadMembers(output.Id);

            // For each Gang Member Id get the Person
            foreach (var m in members)
            {
                List<PersonModel> p = new();
                p = SQLiteDataAccess.GetPerson(m.PersonId);
                output.Members.Add(p[0]);
            }

            return output;
        }

        public static void SaveGang(string name)
        {
            GangModel g = new();
            g.Name = name;

            SQLiteDataAccess.SaveGang(g);
        }

        public static void DeleteGang(int id)
        {
            SQLiteDataAccess.DeleteGang(id);
        }

        // ************************************************
        //            G A N G  M E M B E R S
        // ************************************************
        public static void SaveGangMember(int g, int p)
        {
            GangMembersModel gm = new();
            gm.GangId = g;
            gm.PersonId = p;

            SQLiteDataAccess.SaveGangMember(gm);
        }

        public static void DeleteGangMember(int gid, int pid)
        {
            SQLiteDataAccess.DeleteGangMember(gid, pid);
        }

        // ************************************************
        //                  A G E N D A S
        // ************************************************
        public static List<AgendaModel> RefreshAgendas(int archive)
        {
            List<AgendaModel> output = new();

            output = SQLiteDataAccess.LoadAgendas(archive);

            foreach (var a in output)
            {
                // Load Topics
                a.Topics = SQLiteDataAccess.LoadTopics(a.Id);
                // Sort the Topic List
                a.Topics.Sort((x, y) => x.ItemNbr.CompareTo(y.ItemNbr));

                // Load the Agenda Members based of Attend status
                List<AgendaMembersModel> am = new();
                am = RefreshAgendaMembers(a.Id);

                List<PersonModel> p = new();

                foreach (var m in am)
                {
                    p = SQLiteDataAccess.GetPerson(m.PersonId);

                    if (m.Attend == 1)
                    {
                        // Load the Attendees
                        a.Attendees.Add(p[0]);
                    }
                    if (m.Attend == 0)
                    {
                        // Load the NonAttendees
                        a.NonAttendees.Add(p[0]);
                    }
                    if (m.Attend == 2)
                    {
                        // Load the Apologies
                        a.Apologies.Add(p[0]);
                    }
                }
            }

            // Sort the List
            output.Sort((x, y) => y.RealDate.CompareTo(x.RealDate));

            return output;
        }

        public static void SaveAgenda(AgendaModel name)
        {
            SQLiteDataAccess.SaveAgenda(name);
        }

        public static void UpdateAgenda(AgendaModel name)
        {
            SQLiteDataAccess.UpdateAgenda(name);
        }

        public static void DeleteAgenda(int id)
        {
            SQLiteDataAccess.DeleteAgenda(id);
        }

        // ************************************************
        //          A G E N D A   M E M B E R S
        // ************************************************
        public static List<AgendaMembersModel> RefreshAgendaMembers(int id)
        {
            List<AgendaMembersModel> output = new();

            output = SQLiteDataAccess.LoadAgMbrs(id);

            return output;
        }

        public static void SaveAgendaMembers(AgendaMembersModel am)
        {
            SQLiteDataAccess.SaveAgendaMember(am);
        }

        public static void UpdateAgendaMembers(AgendaMembersModel am)
        {
            SQLiteDataAccess.UpdateAgendaMember(am);
        }

        public static void DeleteAgendaMembers(int AId, int PId)
        {
            SQLiteDataAccess.DeleteAgendaMember(AId, PId);
        }

        // ************************************************
        //                  T O P I C S
        // ************************************************
        public static List<TopicModel> RefreshTopics(int id)
        {
            List<TopicModel> output = new();

            output = SQLiteDataAccess.LoadTopics(id);

            // Sort the List
            output.Sort((x, y) => x.ItemNbr.CompareTo(y.ItemNbr));

            return output;
        }

        public static void SaveTopic(TopicModel name)
        {
            SQLiteDataAccess.SaveTopic(name);
        }

        public static void UpdateTopic(TopicModel topic)
        {
            SQLiteDataAccess.UpdateTopic(topic);
        }

        public static void UpdateTopics(List<TopicModel> topics)
        {
            foreach (var t in topics)
            {
                SQLiteDataAccess.UpdateTopic(t);
            }
        }

        public static void DeleteTopic(int id)
        {
            SQLiteDataAccess.DeleteTopic(id);
        }

        // ************************************************
        //                  S M T P s
        // ************************************************
        public static List<SmtpsModel> LoadSmtps()
        {
            List<SmtpsModel> output = new();

            output = SQLiteDataAccess.LoadSmtps();

            // Sort the List
            output.Sort((x, y) => x.Name.CompareTo(y.Name));

            return output;
        }

        // ************************************************
        //                  G E N E R A L
        // ************************************************
        public static List<PersonModel> RemoveSelected(List<PersonModel> selected, PersonModel p)
        {
            List<PersonModel> output = new();

            // Remove the person from the selected list
            foreach (var s in selected)
            {
                if (s.Id != p.Id)
                {
                    output.Add(s);
                }
            }

            return output;
        }

        public static string? AppKeyLookup(string key)
        {
            ConfigModel cm = new();
            cm = SQLiteDataAccess.GetKeyValue(key);

            if (cm.MyValue != "")
            {
                // Decrypt Passwords
                if (key == "senderPassword")
                {
                    cm.MyValue = SecretKeeper.DecyptText(cm.MyValue);
                }
            }

            return cm.MyValue;
        }

        public static void UpdateKeyLookup(string key, string value)
        {
            // Encrypt Passwords
            if (key == "senderPassword")
            {
                value = SecretKeeper.EncyptText(value);
            }

            ConfigModel cm = new()
            {
                MyKey = key,
                MyValue = value
            };

            SQLiteDataAccess.UpdateKeyValue(cm);
        }
    }
}