using System;
using WinSCP;
using static System.Collections.Specialized.BitVector32;
using ConsoleApp3;
using Newtonsoft.Json;
using Functions = ConsoleApp3.Functions;
using System.IO;
using System.Reflection;

class Example
{
    public static void Main()
    {
        #region
        Functions functions = new Functions();
        string RutaLog = System.AppDomain.CurrentDomain.BaseDirectory;
        string configFilePath = RutaLog + "config.json";
        string json = File.ReadAllText(configFilePath);
        var config = JsonConvert.DeserializeObject<Config>(json);
        string metodo = "";
        // Archivo a descargar para encriptar
        string fileName = "";
        string remoteFilePath = config.GyG.SftpPendientes;

        // Archivo para encriptar
        string localFilePath = RutaLog + @"/Floating_repo/";
        bool keepIndexing = true;

        // Cargar la configuración desde el archivo
        var host = config.Davivienda.SftpCliente;
        var port = config.Davivienda.SftpPortClient;
        var user = config.Davivienda.SftpClienteUser;
        var password = config.Davivienda.SftpClientePass;
        var fingerPrintHost = config.Davivienda.SftpFingerPrint;

        var hostClient = config.GyG.SftpHost;
        var portClient = config.GyG.SftpPort;
        var userClient = config.GyG.SftpUserName;
        var passwordClient = config.GyG.SftpPassword;
        var fingerPrintClient = config.GyG.SftpFingerPrintHost;
        #endregion
        SessionOptions sessionHostOptions = new SessionOptions
        {
            Protocol = Protocol.Sftp,
            HostName = host,
            UserName = user,
            Password = password,
            PortNumber = port,
            SshHostKeyFingerprint = fingerPrintHost
        };

        SessionOptions sessionClientOptions = new SessionOptions
        {
            Protocol = Protocol.Sftp,
            HostName = hostClient,
            UserName = userClient,
            Password = passwordClient,
            PortNumber = portClient,
            SshHostKeyFingerprint = fingerPrintClient
        };

        try
        {
                using (Session sessionHost = new Session())
                {
                    sessionHost.Open(sessionHostOptions);
                    if (!sessionHost.Opened)
                    {
                        metodo = $" -- Hay un problema con el host interno -- ";
                        functions.EscribeLog(host, metodo, config.param.RutaDoc);
                        sessionHost.Open(sessionHostOptions);
                    }
                    else
                    {
                        metodo = $" -- Se establece conexión con el host interno -- ";
                        functions.EscribeLog(host, metodo, config.param.RutaDoc);
                    }
                    RemoteDirectoryInfo files = sessionHost.ListDirectory(remoteFilePath);
                    while (keepIndexing)
                    {
                        foreach (RemoteFileInfo file in files.Files)
                        {
                            if (!file.IsDirectory)
                            {
                                string localTempPath = Path.Combine(config.param.RutaDoc, file.Name);
                                TransferOperationResult transferResult = sessionHost.GetFiles(
                                    RemotePath.EscapeFileMask(remoteFilePath + "/" + file.Name), localTempPath);
                                if (transferResult.IsSuccess)
                                {
                                    sessionHost.RemoveFiles(remoteFilePath + "/" + file.Name);
                                }
                                else
                                {
                                    Console.WriteLine("Failed to download file: " + file.Name);
                                }
                            }
                        }
                        Console.WriteLine($"Descargando archivos de la ruta {remoteFilePath}...");
                        keepIndexing = false;
                        sessionHost.Close();
                    }
                    sessionHost.Dispose();
                }
        }
        catch (Exception ex)
        {
            metodo = " -- Algo sucedió durante la transacción --";
            functions.EscribeLog(metodo, ex.ToString(), config.param.RutaDoc);
        }
    }
}