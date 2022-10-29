namespace MinuteTaker
{
    public class TopicModel
    {
        public int Id { get; set; }
        public int AgendaId { get; set; }
        public int ItemNbr { get; set; }
        public string Heading { get; set; }
        public string Detail { get; set; }

        public string Descrption()
        {
            return ItemNbr.ToString() + ") " + Heading.Trim();
        }
    }
}
