using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class SftpGyG
    {
        public string SftpHost { get; set; }
        public int SftpPort { get; set; }
        public string SftpUserName { get; set; }
        public string SftpPassword { get; set; }
        public string SftpRemotePath { get; set; }
        public string NombreArchivo { get; set; }
        public string SftpRutaDispersion { get; set; }
        public string SftpPendientes { get; set; }
        public string SftpProcesados { get; set; }
        public string SftpRespuesta { get; set; }
        public string PGPPublicKey { get; set; }
    }
    public class SftpDavivienda
    {
        public string SftpCliente { get; set; }
        public string SftpClienteRuta { get; set; }
        public int SftpPortClient { get; set; }
        public string SftpClienteUser { get; set; }
        public string SftpClientePass { get; set; }
    }
    public class Parametros
    {
        public string RutaI { get; set; }
        public string RutaDoc { get; set; }
    }
    public class Config
    {
        public SftpGyG GyG { get; set; }
        public SftpDavivienda Davivienda { get; set; }
        public Parametros param { get; set; }

    }

}
