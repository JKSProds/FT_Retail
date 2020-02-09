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
            List<Promocao> listPromocao = new List<Promocao>();
            List <Seccao> listSeccao = new List<Seccao>();

            using Database conn2 = ConnectionString;

            using (var result = conn2.Query("select idseccion, nombreseccion from dat_seccion"))
            {
                while (result.Read())
                {
                    listSeccao.Add(new Seccao()
                    {
                        IdSeccao = result["idseccion"],
                        NomeSeccao = result["nombreseccion"]
                    });
                }
                conn2.Connection.Close();
            }

            conn2.Connection.Open();
            using (var result = conn2.Query("SELECT FechaInicio, FechaFin, idarticulo FROM dat_tarifa where idarticulo not like '' or idarticulo not like null"))
            {
                while (result.Read())
                {
                    DateTime.TryParse(result[0], out DateTime dataInicio);
                    DateTime.TryParse(result[1], out DateTime dataFim);
                    listPromocao.Add(new Promocao(dataInicio, dataFim, result["idarticulo"]));
                }
                conn2.Connection.Close();  
            }

            using Database conn = ConnectionString;
            List<Artigo> list = new List<Artigo>();

            using (var result = conn.Query(sql))
            {
                while (result.Read())
                {
                    DateTime dataInicio = new DateTime();
                    DateTime dataFim = new DateTime();
                    Promocao promocao = new Promocao(dataInicio, dataFim, result["idarticulo"]);

                    if (listPromocao.Where(i => i.IdArtigo == result["idarticulo"]).Count() > 0)
                    {
                       promocao = listPromocao.Where(i => i.IdArtigo == result["idarticulo"]).First();
                    }

                    list.Add(ObterArtigo(result, promocao, listSeccao.Where(i => i.IdSeccao == result["idseccion"]).First()));
                }
            }
            conn.Connection.Close();
            return list;
        }

        public Promocao ObterPromocao(int idArtigo)
        {
            using Database conn2 = ConnectionString;

            DateTime dataInicio = new DateTime();
            DateTime dataFim = new DateTime();

            using (var result = conn2.Query("SELECT FechaInicio, FechaFin, idarticulo FROM dat_tarifa where idarticulo='"+idArtigo+"'"))
            {
                while (result.Read())
                {
                    DateTime.TryParse(result[0], out  dataInicio);
                    DateTime.TryParse(result[1], out  dataFim);
                }
                conn2.Connection.Close();
            }

            Promocao promocao = (new Promocao(dataInicio, dataFim, idArtigo));

            return promocao;
        }

        public Seccao ObterSeccao(int idseccion) {
            using Database conn2 = ConnectionString;
            Seccao seccao = new Seccao();

            using (var result = conn2.Query("select idseccion, nombreseccion from dat_seccion where idseccion='" + idseccion + "'")) 
            {
                while (result.Read())
                {
                    seccao = new Seccao()
                    {
                        IdSeccao = result["idseccion"],
                        NomeSeccao = result["nombreseccion"]
                    };
                }
                conn2.Connection.Close();
            }
            return seccao;
        }

        public Artigo ObterArtigo(int idartigo)
        {
            using Database conn = ConnectionString;
            Artigo artigo = new Artigo();

            using (var result = conn.Query("select * from dat_articulo where idarticulo='"+idartigo+"'"))
            {
                while (result.Read())
                {
                    
                    artigo = ObterArtigo(result, ObterPromocao(idartigo), ObterSeccao(result["idseccion"]));
                
                };
            }
            conn.Connection.Close();

            return artigo;
        }

        public Artigo ObterArtigo(QueryResult result, Promocao promocao, Seccao seccao)
            {
                Artigo artigo = new Artigo();
                Double.TryParse(result["PrecioConIva"], out double pvp);
                Double.TryParse(result["PrecioOferta"], out double pvpPromocao);
                int.TryParse(result["DiasCaducidad"], out int diasValidade);


                artigo = new Artigo()
                {
                    IdArtigo = result["IdArticulo"],
                    NomeArtigo = String.Concat(result["Descripcion"], result["Descripcion1"]),
                    Preco = pvp,
                    PrecoPromocao = pvpPromocao,
                    DataInicioPromocao = promocao.DataInicio,
                    DataFimPromocao = promocao.DataFim,
                    NomeSeccao = seccao.NomeSeccao,
                    DiasValidade = diasValidade,
                    TxtConservacao = String.Concat(result["Texto7"], result["Texto8"], result["Texto9"], result["Texto10"], result["Texto11"], result["Texto12"], result["Texto13"]),
                    TxtInfoUtilizacao = String.Concat(result["Texto14"], result["Texto15"], result["Texto16"], result["Texto17"], result["Texto18"], result["Texto19"], result["Texto20"]),
                    TxtIngredientes = result["TextoLibre"],
                    TxtAlergenos = result["TextoAlergenos"],
                    TxtInfoNutricional = result["TextoNutricionales"]
                };
            return artigo;
            }

        public List<Artigo> ObterTodosArtigos()
        {
                return ObterListaArtigos("SELECT * from dat_articulo where descripcion not like ''");
        }

        public List<Artigo> ObterArtigosWhere(string PLU, string Nome)
        {
            return ObterListaArtigos("SELECT * from dat_articulo where (descripcion not like '' AND IdArticulo like '%" + PLU + "%') AND (CONCAT(descripcion,descripcion1) like '%" + Nome + "%')");
        }

        public List<Artigo> ObterArtigosOrderBy(string orderBy, bool asc)
        {
  
            string query = ("SELECT* from dat_articulo where descripcion not like ''");

            switch (orderBy)
            {
                case "plu":
                    if (asc)
                    {
                        query += "order by idarticulo ASC";
                    }
                    else
                    {
                        query += "order by idarticulo DESC";
                    }
                    break;
                case "nome":
                    if (asc)
                    {
                        query += "order by descripcion ASC";
                    }
                    else
                    {
                        query += "order by descripcion DESC";
                    }
                    break;
                default:
                    break;
            }

            return ObterListaArtigos(query);
        }

        public void atualizarArtigo(Artigo artigo)
        {
            using Database conn = ConnectionString;
            string[] txtConservacao = new string[7];
            string[] txtUtilizacao = new string[7];
            artigo.TxtConservacao = artigo.TxtConservacao.PadRight(168);
            artigo.TxtInfoUtilizacao = artigo.TxtInfoUtilizacao.PadRight(168);

            for (int i = 0; i < 7; i++)
            {
                txtConservacao[i] = artigo.TxtConservacao.Substring(i * 24, 24);
            }

            for (int i = 0; i < 7; i++)
            {
                txtUtilizacao[i] = artigo.TxtInfoUtilizacao.Substring(i * 24, 24);
            }

            conn.Execute("update dat_articulo set Texto7='"+txtConservacao[0]+ "', Texto8='" + txtConservacao[1] + "', Texto9='" + txtConservacao[2] + "', Texto10='" + txtConservacao[3] + "', Texto11='" + txtConservacao[4] + "', Texto12='" + txtConservacao[5] + "', Texto13='" + txtConservacao[6] + "', Texto14='"+txtUtilizacao[0]+ "', Texto15='" + txtUtilizacao[1] + "', Texto16='" + txtUtilizacao[2] + "', Texto17='" + txtUtilizacao[3] + "', Texto18='" + txtUtilizacao[4] + "', Texto19='" + txtUtilizacao[5] + "', Texto20='" + txtUtilizacao[6] + "', DiasCaducidad='" + artigo.DiasValidade+ "', TextoLibre='" + artigo.TxtIngredientes+ "', TextoNutricionales='" + artigo.TxtInfoNutricional+ "', TextoAlergenos='" + artigo.TxtAlergenos+ "', Modificado=1, ModificadoTextos=1, ModificadoTextoG=1, ModificadoTextoNutricionales=1, ModificadoTextoAlergenos=1 where idarticulo=" + artigo.IdArtigo + "");
       
        }
    }
}


