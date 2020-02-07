using MySql.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FT_Retail.Models
{
    public class FT_RetailContext
    {
        public string ConnectionString { get; set; }

        public FT_RetailContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public List<Artigo> ObterArtigos()
        {
            List<Artigo> list = new List<Artigo>();


            using (Database conn = ConnectionString)
            {

                using (var result = conn.Query("SELECT * from dat_articulo"))
                {
                    while (result.Read())
                    {
                        list.Add(new Artigo()
                        {
                            idArtigo = result["IdArticulo"],
                            Nome = result["Descripcion"],
                            Preco = result["PrecioConIva"]
                        });
                    }
                }
                return list;
            }
        }
    }
}
