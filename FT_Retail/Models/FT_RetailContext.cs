using MySql.Simple;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FT_Retail.Models
{
    public class FT_RetailContext
    {
        public string ConnectionString { get; set; }
        public string SW1100Folder { get; set; }

        public FT_RetailContext(string connectionString, string sw1100Folder)
        {
            this.ConnectionString = connectionString;
            this.SW1100Folder = sw1100Folder;
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
            conn.Connection.Close();

            if (artigo.Promocao.PromocaoAtiva) {
                conn.Connection.Open();
                conn.Execute("update dat_tarifa set modificado=1 where idarticulo="+artigo.IdArtigo+"");
                conn.Connection.Close();
            }
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

        public Loja ObterLoja(int IdLoja)
        {
            using Database conn = ConnectionString;
            QueryResult result = conn.Query("SELECT dat_empresa.NombreEmpresa, dat_tienda.Nombre, dat_tienda.Direccion, dat_tienda.Dir_IP from dat_tienda INNER JOIN dat_empresa ON dat_tienda.idempresa = dat_empresa.idempresa where idtienda='" + IdLoja + "'");

            result.Read();

            Loja loja = new Loja()
            {
                IdLoja = IdLoja,
                NomeEmpresa = result["NombreEmpresa"],
                NomeLoja = result["Nombre"],
                MoradaLoja = result["Direccion"],
                IPLoja = result["Dir_IP"],
                NBalancas = obterNBalancasLoja(IdLoja)
            };

            return loja;
        }
        private int obterNBalancasLoja(int IdLoja)
        {
            int res = 0;
            using Database conn = ConnectionString;
            QueryResult result = conn.Query("Select COUNT(*) as NBalancas From dat_balanza where idtienda = '" + IdLoja + "' and ActivedScale = 1;");

            result.Read();

            int.TryParse(result["NBalancas"], out res);

            return res;


        }
        private int obterCount(string SQL)
        {
            using Database conn = ConnectionString;
            QueryResult result = conn.Query(SQL);

            result.Read();

            return result[0];
        }

        public List<Balanca> ObterListaBalancas()
        {
            List<Balanca> LstBalancas = new List<Balanca>();

            using Database conn = ConnectionString;
            QueryResult result = conn.Query("SELECT IdBalanza FROM dat_balanza Where ActivedScale=1 Order by DireccionLogica");

            while (result.Read())
            {
                LstBalancas.Add(ObterBalanca(result["IdBalanza"]));
            }

            return LstBalancas;
        }

        public Balanca ObterBalanca(int IDBalanca)
        {
            Balanca res = new Balanca();

            using Database conn = ConnectionString;
            QueryResult result = conn.Query("SELECT IdBalanza, Nombre, Dir_IP, BalanzaTradicional, DireccionLogica, PuertoEnvio_Tx FROM dat_balanza Where IdBalanza=" + IDBalanca + " Order by DireccionLogica");

result.Read();

            res = new Balanca()
            {
                IdBalanca = result["IdBalanza"],
                NomeBalanca = result["Nombre"],
                Dir_IP = result["Dir_IP"],
                DirecaoLogica = result["DireccionLogica"],
                PortaTX = result["PuertoEnvio_Tx"],
                IDTipoBalanca = result["BalanzaTradicional"]

            };
            conn.Connection.Close();

            res.DefinirTipo();

            DateTime UltimaAtualizacaoSucesso = new DateTime();
            conn.Connection.Open();
            result = conn.Query("SELECT Fecha FROM sys_transacciones.dat_transacciones Where Dir_IPDestino='" + res.Dir_IP + "' AND Enviado = 1 ORDER BY Fecha DESC LIMIT 1;");
            result.Read();
            if (result.reader.HasRows) { DateTime.TryParse(result["Fecha"], out UltimaAtualizacaoSucesso); }
            conn.Connection.Close();

            res.TransacoesPendentesArtigos = obterCount("SELECT COUNT(*) FROM sys_transacciones.dat_transacciones where Dir_IPDestino='" + res.Dir_IP + "' AND Enviado = 0 AND SentenciaInsert like '" + res.DirecaoLogica + res.RegistoArtigo + "%';");
            res.TransacoesPendentesOfertas = obterCount("SELECT COUNT(*) FROM sys_transacciones.dat_transacciones where Dir_IPDestino='" + res.Dir_IP + "' AND Enviado = 0 AND SentenciaInsert like '" + res.DirecaoLogica + res.RegistoOferta + "%';");
            res.TransacoesPendentes = obterCount("SELECT COUNT(*) FROM sys_transacciones.dat_transacciones where Dir_IPDestino='" + res.Dir_IP + "' AND Enviado = 0;");
            res.TransacoesErro = obterCount("SELECT COUNT(*) FROM sys_transacciones.dat_transacciones where Dir_IPDestino='" + res.Dir_IP + "' AND NIntentos > 0 AND Enviado=0;");
            res.UltimaAtualizacaoSucesso = UltimaAtualizacaoSucesso;
            res.DefinirEstado();

            return res;
        }

        public List<Artigo> ObterListaArtigosBalanca(int IdBalanca, string Nome, string PLU)
        {
            List<Artigo> LstArtigos = new List<Artigo>();

            Balanca balanca = ObterBalanca(IdBalanca);

            if (Directory.Exists(SW1100Folder))
            {
                DirectoryInfo info = new DirectoryInfo(SW1100Folder);
                FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();

                foreach (FileInfo file in files)
                {
                    if (!file.Name.Contains("commL"))
                    {
                        string[] linhas = File.ReadAllLines(file.FullName);
                        bool lerLinha = false;
                        foreach (var linha in linhas)
                        {
                            if (lerLinha && linha.Length == 177 && (linha.ToString().Substring(47, 4) == balanca.DirecaoLogica + balanca.RegistoArtigo) && (linha.ToString().Substring(53, 1) != "B"))
                            {
                                int.TryParse(linha.ToString().Substring(54, 6), out int IdArtigo);
                                double.TryParse(linha.ToString().Substring(135, 6) + "," + linha.ToString().Substring(141, 2), out double preco);
                                double.TryParse(linha.ToString().Substring(143, 6) + "," + linha.ToString().Substring(149, 2), out double precoPromocao);
                                
                                string Data = linha.Substring(0, 10) + " " + linha.Substring(12, 8);
                                DateTime.TryParse(Data, out DateTime dataAtualizacao);

                                Artigo artigo = new Artigo();

                                artigo.IdArtigo = IdArtigo;
                                artigo.NomeArtigo = linha.Substring(63, 48);
                                artigo.UltimaAtualizacao = dataAtualizacao;
                                if (precoPromocao == 0)
                                {
                                    artigo.Preco = preco;
                                }
                                else
                                {
                                    artigo.Preco = precoPromocao;
                                }

                                if (IdArtigo.ToString().Contains(PLU) && artigo.NomeArtigo.Contains(Nome, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    int index = LstArtigos.IndexOf(LstArtigos.Where(a => a.IdArtigo == IdArtigo).FirstOrDefault());

                                    if (index >= 0)
                                    {
                                        LstArtigos[index] = artigo;
                                    }
                                    else
                                    {
                                        LstArtigos.Add(artigo);
                                    }
                                }

                            }

                            if (linha.ToString().Contains("Socket cliente abierto correctamente. IP Balanza:"))
                            {
                                lerLinha = (obterIPString(linha.ToString()) == balanca.Dir_IP);
                            }

                        }
                    }

                }

            }

            return LstArtigos.OrderByDescending(o => o.UltimaAtualizacao).ToList();
        }

        private string obterIPString(string linha)
        {
            int i = 78;
            char c = linha[i];
            string res = "";

            while (c != ' ')
            {
                res += c;
                i += 1;
                c = linha[i];
            }

            return res;
        }

        public List<Rastreabilidade> ObterListaRastreabilidades() {
            List<Rastreabilidade> res = new List<Rastreabilidade>();

            using Database conn = ConnectionString;
            QueryResult result = conn.Query("SELECT IdElemAsociado FROM `sys_datos`.`dat_elem_asociado` Order by IdElemAsociado");

            while (result.Read())
            {
                res.Add(ObterRastreabilidade(result[0]));
            }

            return res;
        }

        public Rastreabilidade ObterRastreabilidade(int ID) {
            Rastreabilidade res = new Rastreabilidade();

            using Database conn = ConnectionString;
            QueryResult result = conn.Query("SELECT IdElemAsociado, NombreElemAsociado FROM `sys_datos`.`dat_elem_asociado` Where IdElemAsociado=" + ID + ";");

            result.Read();

            res = new Rastreabilidade()
            {
                IDRast = result[0],
                NomeRastreabilidade = result[1]
            };

            return res;
        }

        public List<LinhaRastreabilidade> ObterListaLinhaRastreabilidades(int ID) {
            List<LinhaRastreabilidade> LstLinhas = new List<LinhaRastreabilidade>();

             using Database conn = ConnectionString;
            QueryResult result = conn.Query("SELECT IdParametro, Parametro, Valor FROM `sys_datos`.`dat_detalle_elem_asociado` Where IdElemAsociado=" + ID + ";");

            while (result.Read()) {
                LstLinhas.Add(new LinhaRastreabilidade() {
                    IDLinha = result[0],
                    TextoLinha = result[1],
                    ValorLinha = result[2]
                });
            }

            return LstLinhas;
        }

        public void atualizarRastreabilidade (List<LinhaRastreabilidade> rastreabilidades, int IDRast) {
                 using Database conn = ConnectionString;

            foreach (var linha in rastreabilidades) {
                conn.Execute("update dat_detalle_elem_asociado set Valor='"+linha.ValorLinha+"', Modificado=1 where IdParametro="+linha.IDLinha+" AND IdElemAsociado="+IDRast+";");
            }
                
            conn.Connection.Close();
        }
    }
}


