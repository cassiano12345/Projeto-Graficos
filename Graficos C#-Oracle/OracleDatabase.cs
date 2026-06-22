using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;

namespace Graficos
{

    class Ponto
    {
        public double valor1 { get; set; }    // Y 1
        public double valor2 { get; set; }    // Y 2
        public string legenda { get; set; } // X
    }


    class OracleDatabase
    {
        public static String ConnectionString = Properties.Settings.Default.ConStr;
 
        public static List<Ponto> executarQueryGrafico(string query)
        {
            List<Ponto> pontos = new List<Ponto>();

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(query, conn);
                conn.Open();
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        double auxf2 = 0;
                        try
                        {
                            auxf2 = dr.GetDouble(2);
                        }
                        catch (IndexOutOfRangeException)
                        {

                        }
                        double auxf1 = dr.GetDouble(1);
                        string auxstr = dr.GetString(0);

                        Ponto p = new Ponto
                        {
                            valor2 = auxf2,
                            valor1 = auxf1,
                            legenda = auxstr
                        };
                        pontos.Add(p);
                    }
                }
            }
            return pontos;


        }
 




    }
}
