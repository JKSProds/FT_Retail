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

        public List<Artigo> ObterListaArtigos(String sql)
        {


            using (Database conn = ConnectionString)
            {
                List<Artigo> list = new List<Artigo>();

                using (var result = conn.Query(sql))
                {
                    while (result.Read())
                    {
                        double preco = 0.00;
                        Double.TryParse(result["PrecioConIva"], out preco);

                        list.Add(new Artigo()
                        {
                            idArtigo = result["IdArticulo"],
                            Nome = String.Concat(result["Descripcion"], result["Descripcion1"]),
                            Preco = preco
                        });
                    }
                }
                return list;
            }
        }

        public List<Artigo> ObterTodosArtigos()
        {
                return ObterListaArtigos("SELECT * from dat_articulo where descripcion not like ''");
        }

        public List<Artigo> ObterArtigosWhere(string PLU, string Nome)
        {
            return ObterListaArtigos("SELECT * from dat_articulo where (descripcion not like '' AND IdArticulo like '%" + PLU + "%') AND (descripcion like '%"+ Nome + "%' OR descripcion1 like '%" + Nome + "%')");
        }

        public List<Artigo> ObterArtigosOrderBy(string orderBy, bool asc)
        {
  
            string query = ("SELECT* from dat_articulo where descripcion not like ''");

            switch (orderBy)
            {
                case "plu":
                    if (asc)
                    {
                        query = query + "order by idarticulo ASC";
                    }
                    else
                    {
                        query = query + "order by idarticulo DESC";
                    }
                    break;
                case "nome":
                    if (asc)
                    {
                        query = query + "order by descripcion ASC";
                    }
                    else
                    {
                        query = query + "order by descripcion DESC";
                    }
                    break;
                default:
                    break;
            }

            return ObterListaArtigos(query);
        }
    }
}
