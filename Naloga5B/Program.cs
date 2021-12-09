using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Naloga5
{
    class Program
    {
        private static bool valid = true;

        //private static List<Dobavitelj> dobaviteljs = new List<Dobavitelj>() {
        //    new Dobavitelj(1, "Zadruga","Kmecka 3", "123123123", "0310103102", "Prodajajo krompir"),
        //    new Dobavitelj(2, "FERI",   "Postna 3", "761235671", "1051343241", "Prodajajo racunalnicarje"),
        //    new Dobavitelj(3, "Telekom","Vsepovsod 3", "123123123432", "646451213", "Prodajajo telefone")
        //};

        //private static List<Artikel> artikli = new List<Artikel>() {
        //     new Artikel("Krompir",     dobaviteljs.ElementAt(0),  2,    400),
        //     new Artikel("Računalnik",  dobaviteljs.ElementAt(1),     2000, 150),
        //     new Artikel("Telefon",     dobaviteljs.ElementAt(1),     4000, 190),
        //     new Artikel("Prenosnnik",  dobaviteljs.ElementAt(1),     2500, 180),
        //     new Artikel("Jabka",       dobaviteljs.ElementAt(1),     2600, 170),
        //     new Artikel("neki",        dobaviteljs.ElementAt(1),     2700, 160),
        //     new Artikel("iPhone",      dobaviteljs.ElementAt(2),       23, 120)
        //};

        // TODO: Neki je treba nardit


        private static List<Dobavitelj> dobaviteljs = new List<Dobavitelj>();
        private static List<Artikel> artikli = new List<Artikel>();
        private static List<Artikel> artikliVAkciji = new List<Artikel>();

        static void Main(string[] args) {
            //writeListToFile(artikli);
            //writeListToFile(dobaviteljs, "Dobavitelji.xml");

            //writeToFile(new Dobavitelj(1, "Zadruga", "Kmecka 3", "123123123", "0310103102", "Prodajajo krompir"), "Zadruga.xml");
            //writeToFile(new Artikel(1, "Riž", dobaviteljs.ElementAt(0), 1.11m, 122222), "Riz.xml");

            //bool b = ValidateXmlArtikel("Riz.xml");
            //b = ValidateXmlDobavitelj("Zadruga.xml");
            //IzpisiVsaImenaArtiklovKaterihCenaJeVecjaOd();
            Init();
            int selection = 1;

            while (selection != 0) {
                selection = UI();
            }

            writeListToFile(artikli);
            writeListToFile(dobaviteljs, "Dobavitelji.xml");

        }

        private static int UI() {
            Console.WriteLine("0. Izhod");
            Console.WriteLine("1. Branje vseh dokumentov");
            Console.WriteLine("2. Iskanje po dobavitelju in številu artiklov");
            Console.WriteLine("3. Znižaj ceno");
            Console.WriteLine("4. Artikli z znižano ceno");
            Console.WriteLine("5. Dodaj artikel");
            Console.WriteLine("6. Dodaj dobavitelja");
            Console.WriteLine("7. Shrani v xml");
            Console.WriteLine("8. XPath");
            Console.WriteLine("9. Transformacija artiklov");
            Console.WriteLine("10. Transformacija dobaviteljev");

            string? s = Console.ReadLine();
            int selection = int.Parse(s == null ? "0" : s);


            switch (selection) {
                case 1:
                    IzpisiSeznam(artikli);
                    break;
                case 2:
                    Console.WriteLine("Vpiši naziv dobavitelja");
                    string dobavitelj = Console.ReadLine();
                    Console.WriteLine("Vpišite maksimalno število artiklov na zalogi");
                    int zaloga = int.Parse(Console.ReadLine());
                    IEnumerable<Artikel> arts = artikli.Where(x => x.Dobavitelj.Naziv.ToLower() == dobavitelj.ToLower() && x.Zaloga < zaloga);

                    IzpisiSeznam(arts);
                    writeListToFile(arts, $"Artikli_{dobavitelj}");
                    break;
                case 3:
                    Console.WriteLine("Vpišite naziv artikla katerm želite nižati ceno");
                    string naziv = Console.ReadLine().ToLower();
                    Artikel artikel = artikli.Find(x => x.Ime.ToLower() == naziv);
                    Artikel? artikelVAkciji;

                    Console.WriteLine("Za koliko odstotkov želite znižati ceno");
                    int odstotki = int.Parse(Console.ReadLine());

                    if (artikliVAkciji.Where(x => x.Ime == artikel.Ime).Count() < 1) {
                        artikel = znizajCeno(artikel, odstotki);
                        artikliVAkciji.Add(artikel);
                    }
                    else {
                        artikelVAkciji = artikliVAkciji.Find(x => x.Ime == artikel.Ime);
                        artikelVAkciji = znizajCeno(artikel, odstotki);
                        artikliVAkciji.Find(x => x.Ime == artikelVAkciji.Ime).Cena = artikelVAkciji.Cena;
                    }

                    writeListToFile(artikli);
                    writeListToFile(artikliVAkciji, "artikliVAkciji.xml");


                    break;
                case 4:
                    IzpisiSeznam(artikliVAkciji);
                    break;
                case 5:
                    DodajArtikel();
                    break;
                case 6:
                    DodajDobavitelja();
                    break;
                case 7:
                    writeListToFile(artikli);
                    writeListToFile(dobaviteljs, "Dobavitelji.xml");
                    break;
                case 8:
                    XUI();
                    break;
                case 9:
                    TransformArtikli();
                    break;
                case 10:
                    TransformDobavitelji();
                    break;
                //case 11:
                //    TransformArtikliXPATH();
                //    break;
                //case 12:
                //    TransformDobaviteljiXPATH();
                //    break;

            }
            Console.WriteLine();
            return selection;
        }

        //private static void TransformArtikliXPATH() {
        //    try {
        //        XPathDocument myXPathDoc = new XPathDocument(@"Temp.xml");
        //        XslCompiledTransform myXslTrans = new XslCompiledTransform();
        //        myXslTrans.Load(@"template.xslt");
        //        XmlTextWriter myWriter = new XmlTextWriter(@"rez.html", null);
        //        myXslTrans.Transform(myXPathDoc, null, myWriter);
        //        myWriter.Close();

        //        // odpremo dokument
        //        //Process.Start(@"rez.html");
        //    }
        //    catch (Exception e) {
        //        Console.WriteLine("Napaka!" + e.ToString());
        //    }
        //}

        //private static void TransformDobaviteljiXPATH() {
        //    try {
        //        XPathDocument myXPathDoc = new XPathDocument(@"Temp.xml");
        //        XslCompiledTransform myXslTrans = new XslCompiledTransform();
        //        myXslTrans.Load(@"templateDobavitelji.xslt");
        //        XmlTextWriter myWriter = new XmlTextWriter(@"rezDob.html", null);
        //        myXslTrans.Transform(myXPathDoc, null, myWriter);
        //        myWriter.Close();

        //        // odpremo dokument
        //        //Process.Start(@"rez.html");
        //    }
        //    catch (Exception e) {
        //        Console.WriteLine("Napaka!" + e.ToString());
        //    }
        //}


        //private static void 

        private static void TransformDobavitelji() {
            try {
                XPathDocument myXPathDoc = new XPathDocument(@"Dobavitelji.xml");
                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                myXslTrans.Load(@"templateDobavitelji.xslt");
                XmlTextWriter myWriter = new XmlTextWriter(@"rezDob.html", null);
                myXslTrans.Transform(myXPathDoc, null, myWriter);
                myWriter.Close();

                // odpremo dokument
                //Process.Start(@"rez.html");
            }
            catch (Exception e) {
                Console.WriteLine("Napaka!" + e.ToString());
            }
        }

        private static void TransformArtikli() {
            try {
                XPathDocument myXPathDoc = new XPathDocument(@"artikli.xml");
                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                myXslTrans.Load(@"template.xslt");
                XmlTextWriter myWriter = new XmlTextWriter(@"rez.html", null);
                myXslTrans.Transform(myXPathDoc, null, myWriter);
                myWriter.Close();

                // odpremo dokument
                //Process.Start(@"rez.html");
            }
            catch (Exception e) {
                Console.WriteLine("Napaka!" + e.ToString());
            }
        }


        private static string ExecuteAnyXPath(string xpath, string filename = "artikli.xml") {
            //XPathDocument doc = new XPathDocument(filename);
            XPathDocument doc = new XPathDocument(filename);
            XPathNavigator nav = doc.CreateNavigator();
            string result;
            try {

                result = ExecuteXPathString(xpath, filename);
                return result;
            }
            catch (XPathException) {
                result = ExecuteXPathEvalString(xpath, filename);
                return result;
            }
            catch (Exception) {

                throw;
            }
        }


        private static void XUI() {
            Console.Clear();
            Console.WriteLine("1. Izpisi vsa imena");
            Console.WriteLine("2. Izpši vsa imena artiklov katerih cena je večja od: ");
            Console.WriteLine("3. Izpisi vsoto cen vseh artiklov");
            Console.WriteLine("4. Izpisi povprečje cen vseh artiklov");
            Console.WriteLine("5. Izpisi vse artikle določenega dobavitelja");
            Console.WriteLine("6. Izpisi najdražji artikel");
            Console.WriteLine("7. Izpisi najcenejši artikel");
            Console.WriteLine("8. Izpisi artikel z največjo zalogo");
            Console.WriteLine("9. Količina vseh artiklov");
            //Console.WriteLine("9. Izpisi vsa imena");
            //Console.WriteLine("10. Izpisi vsa imena");

            int input = int.Parse(Console.ReadLine());
            Console.Clear();
            switch (input) {
                case 1:
                    IzpisiVsaImenaArtiklov();
                    break;
                case 2:
                    Console.WriteLine("Vnesi ceno");
                    int cena = int.Parse(Console.ReadLine());
                    IzpisiVsaImenaArtiklovKaterihCenaJeVecjaOd(cena);
                    break;
                case 3:
                    IzpisiVsotoCenVsehArtiklov();
                    break;
                case 4:
                    IzpisiPovprecjeCenVsehArtiklov();
                    break;
                case 5:
                    Console.WriteLine("Vnesi naziv dobavitelja");
                    string dobavitelj = Console.ReadLine();
                    IzpisiVseArtikleDolocenegaDobavitelja(dobavitelj);
                    break;
                case 6:
                    IzpisiNajdrazjiArtikel();
                    break;
                case 7:
                    IzpisiNajcenejsiArtikel();
                    break;
                case 8:
                    ArtilekZNajvecjoZalogo();
                    break;
                case 9:
                    SteviloVsehArtiklov();
                    break;

            }
        }

        private static void ExecuteXPath(string xpath, string filename = "artikli.xml") {
            //XPathDocument doc = new XPathDocument(filename);
            XPathDocument doc = new XPathDocument(filename);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNodeIterator iterator = nav.Select(xpath);
            while (iterator.MoveNext()) {
                if (iterator.Current != null) {
                    Console.WriteLine(iterator.Current.Value);
                }
            }
        }
        
        private static string ExecuteXPathString(string xpath, string filename = "artikli.xml") {
            //XPathDocument doc = new XPathDocument(filename);
            XPathDocument doc = new XPathDocument(filename);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNodeIterator iterator = nav.Select(xpath);
            string outString = "";
            while (iterator.MoveNext()) {
                if (iterator.Current != null) {
                    //Console.WriteLine(iterator.Current.Value);
                    outString += iterator.Current.Value;
                }
            }

            return outString;
        }
        private static void ExecuteXPathEval(string xpath, string filename = "artikli.xml") {
            XPathDocument doc = new XPathDocument(filename);
            XPathNavigator nav = doc.CreateNavigator();


            string output = nav.Evaluate(xpath).ToString();
            Console.WriteLine(output);
        }
        private static string ExecuteXPathEvalString(string xpath, string filename = "artikli.xml") {
            XPathDocument doc = new XPathDocument(filename);
            XPathNavigator nav = doc.CreateNavigator();


            string output = nav.Evaluate(xpath).ToString();
            //Console.WriteLine(output);
            return output;
        }

        private static void IzpisiVsaImenaArtiklov() {
            ExecuteXPath("//Ime");
        }
        private static void IzpisiVsaImenaArtiklovKaterihCenaJeVecjaOd(int cena = 10) {
            ExecuteXPath($"//Artikel[Cena>{cena}]/Ime");
        }

        private static void IzpisiVsotoCenVsehArtiklov() {
            ExecuteXPathEval("sum(//Artikel/Cena)");
        }
        private static void IzpisiPovprecjeCenVsehArtiklov() {
            ExecuteXPathEval("avg(//Artikel/Cena)");
        }
        private static void IzpisiVseArtikleDolocenegaDobavitelja(string dobavitelj = "FERI") {
            ExecuteXPath($"//Artikel[Dobavitelj/Naziv='{dobavitelj}']/Ime");
        }
        private static void IzpisiNajdrazjiArtikel() {
            ExecuteXPath($"//Artikel[Cena=max(//Cena)]/Ime");
        }

        private static void IzpisiNajcenejsiArtikel() {
            ExecuteXPath($"//Artikel[Cena=min(//Cena)]/Ime");
        }
        private static void ArtilekZNajvecjoZalogo() {
            ExecuteXPath($"//Artikel[Zaloga=max(//Zaloga)]");
        }

        private static void SteviloVsehArtiklov() {
            ExecuteXPathEval("sum(//Artikel/zaloga)");
        }




        private static void DodajArtikel() {
            int id = artikli.Max(x => x.Id) + 1;
            Console.WriteLine("Vpišite naziv artikla");
            string? naziv = Console.ReadLine();
            Console.WriteLine("Vpišite naziv dobavitelja");

            Dobavitelj? dobavitelj;
            string? dobaviteljNaziv;
            while (true) {
                dobaviteljNaziv = Console.ReadLine();
                try {
                    if (dobaviteljNaziv != null) {
                        dobavitelj = dobaviteljs.Find(x => x.Naziv.ToLower() == dobaviteljNaziv.ToLower());

                        if (dobavitelj != null) {
                            break;
                        }
                    }
                }
                catch {
                    Console.WriteLine("Vpisan dobavitelj ne obstaja");
                }
            }

            Console.WriteLine("Vnesite ceno");
            decimal cena = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Vnesite zalogo");
            int zaloga = int.Parse(Console.ReadLine());

            Artikel artikel = new Artikel(id, naziv, dobavitelj, cena, zaloga);
            string fileName = $"{id}_{artikel.Ime}.xml";
            writeToFile(artikel, fileName);

            bool _valid = ValidateXmlArtikel(fileName);

            if (_valid) {
                artikli.Add(artikel);
                Console.WriteLine("Artikel uspešno dodan");
            }
        }

        private static void DodajDobavitelja() {
            int Id = dobaviteljs.Max(x => x.Id) + 1;

            Console.WriteLine("Vnesite naziv");
            string naziv = Console.ReadLine();

            Console.WriteLine("Vnesite naslov");
            string naslov = Console.ReadLine();

            Console.WriteLine("Vnesite davcno");
            string davcna = Console.ReadLine();

            Console.WriteLine("Vnesite kontakt");
            string kontakt = Console.ReadLine();

            Console.WriteLine("Vnesite opis");
            string opis = Console.ReadLine();

            Dobavitelj dobavitelj = new Dobavitelj(Id, naziv, naslov, davcna, kontakt, opis);

            string fileName = $"{dobavitelj.Id}_{dobavitelj.Naziv}.xml";
            writeToFile(dobavitelj, fileName);
            bool _valid = ValidateXmlDobavitelj(fileName);
            if (_valid) {
                dobaviteljs.Add(dobavitelj);
                Console.WriteLine("Dobavitelj uspešno dodan");
            }

        }

        private static bool ValidateXmlArtikel(string filename) {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = new XmlUrlResolver();
            settings.Schemas.Add("Artikel", "Validator.xsd");
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            settings.IgnoreWhitespace = true;
            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(filename, settings);

            // Parse the file.
            while (reader.Read()) ;



            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("Artikel", "Validator.xsd");

            XDocument xDocument = XDocument.Load(filename);
            xDocument.Validate(schemas, (o, e) => { Console.WriteLine(e.Message); valid = false; });

            bool _valid = valid;
            valid = true;
            return _valid;
        }
        private static bool ValidateXmlDobavitelj(string filename) {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.XmlResolver = new XmlUrlResolver();
            settings.Schemas.Add("Dobavitelj", "Validator.xsd");
            settings.ValidationType = ValidationType.Schema;
            //settings.ValidationFlags
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            // Create the XmlReader object.
            XmlReader reader = XmlReader.Create(filename, settings);

            // Parse the file.
            while (reader.Read()) ;


            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("Dobavitelj", "Validator.xsd");

            XDocument xDocument = XDocument.Load(filename);
            xDocument.Validate(schemas, (o, e) => { Console.WriteLine(e.Message); });

            bool _valid = valid;
            valid = true;
            return _valid;
        }



        // Display any validation errors.
        private static void ValidationCallBack(object sender, ValidationEventArgs e) {
            Console.WriteLine("Validation Error: {0}", e.Message);
            valid = false;
        }

        private static void Init() {
            if (File.Exists("artikli.xml")) {
                artikli = readArtikliFromFile();
            }
            if (File.Exists("artikliVAkciji.xml")) {
                artikliVAkciji = readArtikliFromFile("artikliVAkciji.xml");
            }
            if (File.Exists("Dobavitelji.xml"))
                dobaviteljs = readDobaviteljiFromFile("Dobavitelji.xml");
        }



        private static void IzpisiSeznam(IEnumerable<Artikel> artikli) {
            foreach (var art in artikli) {
                Console.WriteLine(art.ToString());
            }
        }

        private static List<Artikel> iskanjePoDobavitelju(string dobavitelj, int zaloga) {
            return artikli.Where(x => x.Dobavitelj.Naziv == dobavitelj && x.Zaloga < zaloga).ToList();
        }

        private static Artikel znizajCeno(Artikel artikel, int procenti) {
            Artikel copy = new Artikel(artikel);
            decimal procentiPo = (100 - procenti) / 100.0m;
            copy.Cena = (decimal)copy.Cena * procentiPo;

            return copy;
        }


        private static void writeListToFile<T>(IEnumerable<T> seznam, string fileName = "artikli.xml") {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));


            using (StreamWriter sw = new StreamWriter(fileName)) {
                serializer.Serialize(sw, seznam);
            }

        }
        private static void writeToFile<T>(T obj, string fileName) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            string type = typeof(T).Name;

            //Da ni xsi in xsd kr zjebeta pol pri preverjanju
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (XmlWriter xw = XmlWriter.Create(fileName)) {
                //xw.WriteDocType(type, null, $"{type}.dtd", null);
                serializer.Serialize(xw, obj, ns);
            }
        }

        private static void dodaj(Artikel artikel) {
            artikli.Add(artikel);
        }

        private static List<Artikel> readArtikliFromFile(string fileName = "artikli.xml") {
            List<Artikel> artikli = new List<Artikel>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Artikel>));

            using (StreamReader sr = new StreamReader(fileName)) {
                artikli = (List<Artikel>)serializer.Deserialize(sr);
            }
            return artikli;
        }

        private static List<Dobavitelj> readDobaviteljiFromFile(string fileName) {
            List<Dobavitelj> dobavitelji = new List<Dobavitelj>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Dobavitelj>));

            using (StreamReader sr = new StreamReader(fileName)) {
                dobavitelji = (List<Dobavitelj>)serializer.Deserialize(sr);
            }
            return dobavitelji;
        }

        //private static Artikel split(string line)
        //{
        //    Artikel artikel = new Artikel();
        //    string lin = line.Substring(0, line.Length - 2);
        //    string[] values = line.Split(',');

        //    artikel.Ime = values[0].Trim();
        //    artikel.Cena = decimal.Parse(values[1].Trim());
        //    artikel.Zaloga = int.Parse(values[2].Trim());
        //    artikel.Dobavitelj = values[3].Trim().Replace(";", ""); ;

        //    return artikel;
        //}
    }
}
