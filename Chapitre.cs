using System.Text.Json.Serialization;

namespace isgasoir
{
    public class Chapitre
    {
        long id;
        string title;
        string content;
        [JsonIgnore]
        Module? module;
        double duree;
        List<Activity>? activities;
        public double Duree { get => duree; set => duree = value; }
        public long Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        [JsonIgnore]
        public Module? Module { get => module; set => module = value; }
        public List<Activity>? Activities { get => activities; set => activities = value; }
    }
}
