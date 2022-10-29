using System;
using System.Collections.Generic;

namespace MinuteTaker
{
    public class AgendaModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public int GangId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Archive { get; set; }

        public List<TopicModel> Topics = new();

        public List<PersonModel> Attendees = new();
        public List<PersonModel> NonAttendees = new();
        public List<PersonModel> Apologies = new();

        public DateTime RealDate
        {
            get
            {
                return new DateTime(Year, Month, Day, Hour, Minute, 0);
            }
            set
            {
                Year = value.Year;
                Month = value.Month;
                Day = value.Day;
                Hour = value.Hour;
                Minute = value.Minute;
            }
        }

        public string Descrption()
        {
            return Day.ToString() + "/" + Month.ToString() + "/" + Year.ToString() + " " + Title.Trim();
        }

        public void CleanAgenda()
        {
            Title = Title.Trim();
            Location = Location.Trim();
        }

        public void AddDefaultTopics(int AId)
        {
            TopicModel t = new()
            {
                AgendaId = AId,
                ItemNbr = 1,
                Heading = "Apologies"
            };

            Topics.Add(t);
            MinuteTakerLibary.SaveTopic(t);

            t = new()
            {
                AgendaId = AId,
                ItemNbr = 99,
                Heading = "Any Other Business"
            };
            Topics.Add(t);
            MinuteTakerLibary.SaveTopic(t);
        }
    }
}