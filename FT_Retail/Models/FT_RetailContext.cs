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

                using (var result = conn.Query("SELECT * from dat_articulo where descripcion not like ''"))
                {
                    while (result.Read())
                    {
                        double preco = 0.00;
                        Double.TryParse(result["PrecioConIva"], out preco);

                        list.Add(new Artigo()
                        {
                            idArtigo = result["IdArticulo"],
                            Nome = result["Descripcion"],
                            Preco = preco
                        });
                    }
                }
                return list;
            }
        }

        public List<Artigo> ObterArtigosWherePLU(string search)
        {
            List<Artigo> list = new List<Artigo>();


            using (Database conn = ConnectionString)
            {

                using (var result = conn.Query("SELECT * from dat_articulo where descripcion not like '' AND IdArticulo like '%" + search + "%'"))
                {
                    while (result.Read())
                    {
                        double preco = 0.00;
                        Double.TryParse(result["PrecioConIva"], out preco);

                        list.Add(new Artigo()
                        {
                            idArtigo = result["IdArticulo"],
                            Nome = result["Descripcion"],
                            Preco = preco
                        });
                    }
                }
                return list;
            }
        }

        public List<Artigo> ObterArtigosOrderBy(string orderBy, bool asc)
        {
            List<Artigo> list = new List<Artigo>();

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

            using (Database conn = ConnectionString)
            {

                using (var result = conn.Query("SELECT * from dat_articulo where descripcion not like ''"))
                {
                    while (result.Read())
                    {
                        double preco = 0.00;
                        Double.TryParse(result["PrecioConIva"], out preco);

                        list.Add(new Artigo()
                        {
                            idArtigo = result["IdArticulo"],
                            Nome = result["Descripcion"],
                            Preco = preco
                        });
                    }
                }
                return list;
            }
        }
    }
}
