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

        public Artigo ObterArtigo(int IdArtigo)
        {
            using Database conn = ConnectionString;
            QueryResult result = conn.Query("select * from dat_articulo where idarticulo='" + IdArtigo + "'");

            result.Read();

            Double.TryParse(result["PrecioConIva"], out double pvp);
            int.TryParse(result["DiasCaducidad"], out int diasValidade);
            Promocao promocao = ObterPromocao(IdArtigo);
            Seccao seccao = ObterSeccao(result["idseccion"]);

            var artigo = new Artigo()
            {
                IdArtigo = result["IdArticulo"],
                NomeArtigo = String.Concat(result["Descripcion"], result["Descripcion1"]),
                Preco = pvp,
                Promocao = promocao,
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
        public Promocao ObterPromocao(int idArtigo)
        {
            using Database conn = ConnectionString;
            QueryResult result = conn.Query("SELECT FechaInicio, FechaFin, dat_tarifa.idarticulo, dat_articulo.PrecioOferta FROM dat_tarifa INNER JOIN dat_articulo ON dat_tarifa.idarticulo = dat_articulo.idarticulo where dat_tarifa.idarticulo='" + idArtigo + "'");
            Promocao promocao = new Promocao();

            if (result.reader.HasRows)
            {

                DateTime dataInicio = new DateTime();
                DateTime dataFim = new DateTime();

                result.Read();

                DateTime.TryParse(result[0], out dataInicio);
                DateTime.TryParse(result[1], out dataFim);
                Double.TryParse(result[3], out double pvpPromocao);

                promocao = (new Promocao(dataInicio, dataFim, idArtigo, pvpPromocao));

            }
            return promocao;
        }
        public Seccao ObterSeccao(int idseccion)
        {
            using Database conn = ConnectionString;
            QueryResult result = conn.Query("select idseccion, nombreseccion from dat_seccion where idseccion='" + idseccion + "'");

            result.Read();

            Seccao seccao = new Seccao()
            {
                IdSeccao = result["idseccion"],
                NomeSeccao = result["nombreseccion"]
            };

            return seccao;
        }

        public List<Artigo> ObterListaArtigos(String sql)
        {
            List<Promocao> lstPromocao = ObterListaPromocao();
            List<Seccao> lstSeccao = ObterListaSeccao();
            List<Artigo> lstArtigo = new List<Artigo>();

            using Database conn = ConnectionString;
            QueryResult result = conn.Query(sql);


            while (result.Read())
            {
                Double.TryParse(result["PrecioConIva"], out double pvp);

                int idArtigo = result["IdArticulo"];

                Promocao promocao = lstPromocao.Find(p => p.IdArtigo == idArtigo);
                if (promocao != null && promocao.PromocaoAtiva)
                {
                    Double.TryParse(result["PrecioOferta"], out double pvpPromocao);
                    if (pvpPromocao < pvp)
                    {
                        pvp = pvpPromocao;
                    }
                }

                lstArtigo.Add(new Artigo()
                {
                    IdArtigo = result["IdArticulo"],
                    NomeArtigo = String.Concat(result["Descripcion"], result["Descripcion1"]),
                    Preco = pvp
                });

            }
            return lstArtigo;
        }
        public List<Promocao> ObterListaPromocao()
        {
            using Database conn = ConnectionString;
            QueryResult result = conn.Query("SELECT FechaInicio, FechaFin, dat_tarifa.idarticulo, dat_articulo.PrecioOferta as PrecioOferta FROM dat_tarifa INNER JOIN dat_articulo ON dat_tarifa.idarticulo = dat_articulo.idarticulo");
 
            List<Promocao> lstPromocao = new List<Promocao>();
            DateTime dataInicio = new DateTime();
            DateTime dataFim = new DateTime();

            while (result.Read())
            {
                DateTime.TryParse(result[0], out dataInicio);
                DateTime.TryParse(result[1], out dataFim);
                Double.TryParse(result[3], out double pvpPromocao);

                lstPromocao.Add(new Promocao(dataInicio, dataFim, result["idarticulo"], pvpPromocao));
            }

            return lstPromocao;
        }
        public List<Seccao> ObterListaSeccao()
        {
            using Database conn = ConnectionString;
            QueryResult result = conn.Query("select idseccion, nombreseccion from dat_seccion");

            List<Seccao> lstSeccao = new List<Seccao>();
 
                while (result.Read())
                {
                    lstSeccao.Add(new Seccao()
                    {
                        IdSeccao = result["idseccion"],
                        NomeSeccao = result["nombreseccion"]
                    });
                }

            return lstSeccao;
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
            string[] txtConservacao = obterTextos24Caracteres(artigo.TxtConservacao);
            string[] txtUtilizacao = obterTextos24Caracteres(artigo.TxtInfoUtilizacao);
            
            using Database conn = ConnectionString;
            conn.Execute("update dat_articulo set Texto7='" + txtConservacao[0] + "', Texto8='" + txtConservacao[1] + "', Texto9='" + txtConservacao[2] + "', Texto10='" + txtConservacao[3] + "', Texto11='" + txtConservacao[4] + "', Texto12='" + txtConservacao[5] + "', Texto13='" + txtConservacao[6] + "', Texto14='" + txtUtilizacao[0] + "', Texto15='" + txtUtilizacao[1] + "', Texto16='" + txtUtilizacao[2] + "', Texto17='" + txtUtilizacao[3] + "', Texto18='" + txtUtilizacao[4] + "', Texto19='" + txtUtilizacao[5] + "', Texto20='" + txtUtilizacao[6] + "', DiasCaducidad='" + artigo.DiasValidade + "', TextoLibre='" + artigo.TxtIngredientes + "', TextoNutricionales='" + artigo.TxtInfoNutricional + "', TextoAlergenos='" + artigo.TxtAlergenos + "', Modificado=1, ModificadoTextos=1, ModificadoTextoG=1, ModificadoTextoNutricionales=1, ModificadoTextoAlergenos=1 where idarticulo=" + artigo.IdArtigo + "");    
        }

        public string[] obterTextos24Caracteres(string TxtCompleto)
        {
            string[] txt = new string[7];

            if (TxtCompleto == null)
            {
                TxtCompleto = "";
            }

            TxtCompleto = TxtCompleto.PadRight(168);

            for (int i = 0; i < 7; i++)
            {
                txt[i] = TxtCompleto.Substring(i * 24, 24);
            }

            return txt;
        }
    }
}


