namespace isgasoir
{
    public class Semestre
    {
        long id;
        string name = string.Empty;
        List<Module>? modules = new();
        // foreign key to Filiere
        public long? FiliereId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Filiere? Filiere { get; set; }
       

        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        
        public List<Module>? Modules { get => modules; set => modules = value; }
    }
}
