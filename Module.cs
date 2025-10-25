using System.Text.Json.Serialization;

namespace isgasoir
{
    public class Module
    {
        long id;
        string name = string.Empty;
        double coiff;

        [JsonIgnore]
        Semestre? sem;

        List<Chapitre>? chapitres = new();

        List<Studant>? studants = new();

        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public double Coiff { get => coiff; set => coiff = value; }
        [JsonIgnore]
        public Semestre Sem { get => sem; set => sem = value; }
        public List<Chapitre>? Chapitres { get => chapitres; set => chapitres = value; }
        public List<Studant>? Studants { get => studants; set => studants = value; }
    }
}
