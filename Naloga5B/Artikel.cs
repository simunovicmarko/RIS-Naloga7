namespace Naloga5
{
    public class Artikel
    {
        private int id;
        private string ime;
        private Dobavitelj dobavitelj;
        private decimal cena;
        private int zaloga;

        public int Id { get => id; set => id = value; }
        public string Ime { get => ime; set => ime = value; }
        public Dobavitelj Dobavitelj { get => dobavitelj; set => dobavitelj = value; }
        public decimal Cena { get => cena; set => cena = value; }
        public int Zaloga { get => zaloga; set => zaloga = value; }

        public Artikel(string ime, Dobavitelj dobavitelj, decimal cena, int zaloga) {
            this.Ime = ime;
            this.Dobavitelj = dobavitelj;
            this.Cena = cena;
            this.Zaloga = zaloga;
        }

        public Artikel() { }
        public Artikel(Artikel artikel) {
            this.ime = artikel.ime;
            this.dobavitelj = artikel.dobavitelj;
            this.cena = artikel.cena;
            this.zaloga = artikel.zaloga;
        }

        public Artikel(int id, string ime, Dobavitelj dobavitelj, decimal cena, int zaloga) {
            this.id = id;
            this.ime = ime;
            this.dobavitelj = dobavitelj;
            this.cena = cena;
            this.zaloga = zaloga;
        }

        public override string ToString() {
            return $"{Ime}, {Cena.ToString().Replace(',', '.')}, {Zaloga}, {Dobavitelj.Naziv};";
        }
    }
}
