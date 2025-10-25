namespace isgasoir
{
    public class Filiere
    {
        long id;
        string name = string.Empty;
        string description = string.Empty;
        List<Semestre> semestres = new();

        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public List<Semestre> Semestres { get => semestres; set => semestres = value; }
    }
}
