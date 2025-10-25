namespace isgasoir
{
    public class Activity
    {
        long id;
        string title = string.Empty;
        string instructions = string.Empty;
        long chapitreId = 0;

        [System.Text.Json.Serialization.JsonIgnore]
        public Chapitre? Chapitre { get; set; }

        public long Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Instructions { get => instructions; set => instructions = value; }
        public long ChapitreId { get => chapitreId; set => chapitreId = value; }
    }
}
