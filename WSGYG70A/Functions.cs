using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Functions
    {
        readonly char separador = '';
        readonly string RutaLog = System.AppDomain.CurrentDomain.BaseDirectory;
        public void EscribeLog(string metodo, string exception, string rutaI)
        {
            bool error = false;
            StreamWriter sw = null;
            do
            {
                try
                {
                    string nomarchi = rutaI + @"DOCUMENTOS/LOGS/SFTP_DAVIENDA" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    sw = new StreamWriter(nomarchi, true, Encoding.Unicode);
                    DateTime horaf = DateTime.Now;
                    int horaff = (horaf.Minute * 60) + horaf.Second;
                    sw.WriteLine("************************************************************************************************************************");
                    sw.WriteLine(metodo + " " + DateTime.Now);
                    sw.WriteLine(exception);
                    sw.Close();
                    error = false;
                }
                catch
                {
                    error = true;
                }
            } while (error == true);
        }
    }
}
