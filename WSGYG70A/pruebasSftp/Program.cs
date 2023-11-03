using System;
using WinSCP;
using static System.Collections.Specialized.BitVector32;
using ConsoleApp3;
using Newtonsoft.Json;
using Functions = ConsoleApp3.Functions;
using System.IO;
using System.Reflection;

internal class Example
{
    public static void Main()
    {
        Functions functions = new Functions();
        string RutaLog = AppDomain.CurrentDomain.BaseDirectory;
        string configFilePath = RutaLog + "config.json";
        string json = File.ReadAllText(configFilePath);
        Config config = JsonConvert.DeserializeObject<Config>(json);
        string metodo = "";
        string fileName = "";
        string remoteFilePath = config.GyG.SftpPendientes;
        string localFilePath = RutaLog + "/Floating_repo/";
        bool keepIndexing = true;
        string host = config.GyG.SftpHost;
        int port = config.GyG.SftpPort;
        string user = config.GyG.SftpUserName;
        string password = config.GyG.SftpPassword;
        string fingerPrintHost = config.GyG.SftpFingerPrintHost;
        string hostClient = config.Davivienda.SftpCliente;
        int portClient = 19629;
        string userClient = config.Davivienda.SftpClienteUser;
        string passwordClient = config.Davivienda.SftpClientePass;
        string fingerPrintClient = config.Davivienda.SftpFingerPrint;
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
            while (true)
            {
                using (Session sessionHost = new Session())
                {
                    sessionHost.Open(sessionHostOptions);
                    if (!sessionHost.Opened)
                    {
                        metodo = " -- Hay un problema con el host interno -- ";
                        functions.EscribeLog(host, metodo, config.param.RutaDoc);
                        sessionHost.Open(sessionHostOptions);
                    }
                    else
                    {
                        metodo = " -- Se establece conexión con el host interno -- ";
                        functions.EscribeLog(host, metodo, config.param.RutaDoc);
                    }
                    RemoteDirectoryInfo files2 = sessionHost.ListDirectory(remoteFilePath);
                    int currentFileCount = files2.Files.Count();
                    while (keepIndexing)
                    {
                        files2 = sessionHost.ListDirectory(remoteFilePath);
                        int newFileCount = files2.Files.Count();
                        foreach (RemoteFileInfo file2 in files2.Files)
                        {
                            if (!file2.IsDirectory)
                            {
                                string localTempPath = Path.Combine(localFilePath, file2.Name);
                                TransferOperationResult transferResult = sessionHost.GetFiles(RemotePath.EscapeFileMask(remoteFilePath + "/" + file2.Name), localTempPath);
                                if (transferResult.IsSuccess)
                                {
                                    sessionHost.RemoveFiles(remoteFilePath + "/" + file2.Name);
                                }
                                else
                                {
                                    Console.WriteLine("Failed to download file: " + file2.Name);
                                }
                            }
                        }
                        Console.WriteLine("Descargando archivos de la ruta " + remoteFilePath + "...");
                        keepIndexing = false;
                        sessionHost.Close();
                    }
                    sessionHost.Dispose();
                }
                using (Session sessionCliente = new Session())
                {
                    sessionCliente.ReconnectTime = TimeSpan.FromSeconds(10.0);
                    sessionCliente.Open(sessionClientOptions);
                    if (!sessionCliente.Opened)
                    {
                        metodo = " -- Hay un problema con el cliente Davivienda -- ";
                        functions.EscribeLog(hostClient, metodo, config.param.RutaDoc);
                        sessionCliente.Open(sessionClientOptions);
                    }
                    else
                    {
                        metodo = " -- Se establece conexión con el servidor de Davivienda -- ";
                        functions.EscribeLog(hostClient, metodo, config.param.RutaDoc);
                    }
                    Console.WriteLine("Subiendo elementos a la ruta " + config.Davivienda.SftpClienteRuta + "...");
                    string[] files3 = Directory.GetFiles(localFilePath);
                    foreach (string file in files3)
                    {
                        TransferOperationResult transferUpload = sessionCliente.PutFiles(file, config.Davivienda.SftpClienteRuta);
                        File.Delete(file);
                    }
                    sessionCliente.Close();
                }
                Thread.Sleep(5000);
            }
        }
        catch (Exception ex)
        {
            metodo = " -- Algo sucedió durante la transacción --";
            functions.EscribeLog(metodo, ex.ToString(), config.param.RutaDoc);
        }
    }
}
