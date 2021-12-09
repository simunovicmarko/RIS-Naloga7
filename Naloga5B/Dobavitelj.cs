namespace Naloga5
{
    public class Dobavitelj
    {
        int id;
        string naziv;
        string naslov;
        string davcna;
        string kontakt;
        string opis;

        public Dobavitelj() { }

        public Dobavitelj(string naziv, string naslov, string davcna, string kontakt, string opis) {
            this.naziv = naziv;
            this.naslov = naslov;
            this.davcna = davcna;
            this.kontakt = kontakt;
            this.opis = opis;
        }

        public Dobavitelj(int id, string naziv, string naslov, string davcna, string kontakt, string opis) {
            this.id = id;
            this.naziv = naziv;
            this.naslov = naslov;
            this.davcna = davcna;
            this.kontakt = kontakt;
            this.opis = opis;
        }

        public int Id { get => id; set => id = value; }
        public string Naziv { get => naziv; set => naziv = value; }
        public string Naslov { get => naslov; set => naslov = value; }
        public string Davcna { get => davcna; set => davcna = value; }
        public string Kontakt { get => kontakt; set => kontakt = value; }
        public string Opis { get => opis; set => opis = value; }
    }
}
