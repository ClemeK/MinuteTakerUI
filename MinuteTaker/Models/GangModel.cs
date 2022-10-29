using System.Collections.Generic;

namespace MinuteTaker
{
    public class GangModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<PersonModel> Members = new();

    }
}
