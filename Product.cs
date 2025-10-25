namespace isgasoir
{
    public class Product
    {
        int id;
        string desg = string.Empty;
        double qte;
        int pu;

        public int Id { get => id; set => id = value; }
        public string Desg { get => desg; set => desg = value; }
        public double Qte { get => qte; set => qte = value; }
        public int Pu { get => pu; set => pu = value; }
    }
}
