using System;
using System.IO;
using WinSCP;

namespace ComercialColindres.Servicios.Clases
{
    public class StorageService : IStorageService
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;
        private readonly int _portNumber;

        public StorageService()
        {
            _hostName = AppSettings.Instance.FtpHostname;
            _userName = AppSettings.Instance.FtpUser;
            _password = AppSettings.Instance.FtpPass;
            _portNumber = AppSettings.Instance.FtpPortNumber;
        }

        public FileSaveResult SaveFile(string boletaStorageRoot, byte[] fileBytes, string fileName)
        {
            FileSaveResult fileSaveResult = new FileSaveResult();

            try
            {
                string pathToNewFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ColindresImagenes");
                DirectoryInfo directory = Directory.CreateDirectory(pathToNewFolder); //Create virtual directory
                string filePath = $"{directory.FullName}\\{fileName+".jpg"}";
                //string directory = System.IO.Path.GetTempPath() + "/ColindresImagenes/";
                FileStream fileStream = File.Create(filePath);
                fileStream.Write(fileBytes, 0, fileBytes.Length);
                fileStream.Close();

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password,
                    PortNumber = _portNumber
                };

                using (var session = new Session())
                {
                    session.Open(sessionOptions);

                    var transferOptions = new TransferOptions
                    {
                        TransferMode = TransferMode.Binary,
                        OverwriteMode = OverwriteMode.Overwrite,
                        PreserveTimestamp = false
                    };

                    //Create new directory
                    string newDirectory = $"{boletaStorageRoot + "2021/Gildan/Abril/"}";

                    if (!session.FileExists(newDirectory))
                    {
                        session.CreateDirectory(newDirectory);
                    }

                    // Retrieving all files in the local directory uploading them to the remote ftp directory
                    //var transferResult = session.PutFileToDirectory(directory.FullName, boletaStorageRoot, true, transferOptions);
                    var transferResult = session.PutFileToDirectory(filePath, boletaStorageRoot, true, transferOptions);                    

                    fileSaveResult.DestinationPath = boletaStorageRoot + fileName;
                    fileSaveResult.WasSuccessful = true;
                }
            }
            catch (Exception e)
            {
                fileSaveResult.WasSuccessful = false;
            }

            return fileSaveResult;
        }
    }
}
