using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Muzicki_festival.Mapiranje;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISession = NHibernate.ISession;

namespace Muzicki_festival
{
    //sluzi za kreiranje svega neophodnog za bazu
    public class DataLayer
    {
        private static ISessionFactory factory = null;
        private static object objLock = new object();
        //trebamo da kreiramo dva interfejsa
        //Isesstion - sesija - ono sto kreiramo u toj niti ostaje u toj niti, ne moze u drugim nitima, za krairanje sesija
        //ISessionFactory - kreira se samo 1

        //funkcija na zahtev otvara sesiju
        public static ISession GetSession()
        {
            //singlator obrazac - ukoliko ne postoji objekat  kreirati ga
            //ukoliko session facotry nije kreiran 
            if (factory == null)
            {
                lock (objLock)
                {
                    if (factory == null)
                    {
                        factory = CreateSessionFactory();
                    }
                }
            }
            return factory.OpenSession();
        }
        //funkcija za kreiranje session factory
        private static ISessionFactory CreateSessionFactory()
        {
            try
            {
                #region KonfiguracijaStanimirovic

                //var cfg = OracleManagedDataClientConfiguration.Oracle10.ConnectionString
                //    (c => c.Is("DATA SOURCE = gislab-oracle.elfak.ni.ac.rs:1521/SBP_PDB;PERSIST SECURITY INFO = True; User ID=S19184; Password = StasaKostic1#"));
                //return Fluently.Configure()
                //    .Database(cfg.ShowSql())
                //    //imamo samo jedno mapiranje za sve entitete, ukoliko menjamo da sve menjamo
                //    //kada kazemo jedno mapiranje sve se zameni
                //    //nema potrebe da pisemo vise mapiranja, jer jedno mapiranje
                //    //sa addfromassemblyof kaze da trazi sva mapiranja u toj biblioteci
                //    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Mapiranje.DogadjajMapiranje>()).BuildSessionFactory();

                #endregion

                var cfg = MsSqlConfiguration.MsSql2012
                     .ConnectionString(c => c.Is(@"Data Source=(localdb)\MuzickiFestival; Initial Catalog=MojaLokalnaBaza; Integrated Security=True;"))
                     .Driver<NHibernate.Driver.MicrosoftDataSqlClientDriver>()  
                     .ShowSql();  

                return Fluently.Configure()
                    .Database(cfg)
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Mapiranje.DogadjajMapiranje>())
                    .ExposeConfiguration(conf =>
                    {
                        new SchemaUpdate(conf).Execute(false, true);
                    })
                    .BuildSessionFactory();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri kreiranju SessionFactory:\n\n" + ex.ToString());
                throw;
            }
        }
    }
}
