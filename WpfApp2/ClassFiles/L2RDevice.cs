using System;
using System.Collections.Generic;
using Managed.Adb;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using log4net;
using System.Xml;
using System.Threading;

namespace L2RBot
{
    public class L2RDevice
    {
        //properties
        FileMonitor Monitor { get; set; }

        ShellOutputReceiver Receiver { get; set; }

        public Device ADBDevice { get; set; }

        public String CharacterName { get; set; }

        public string LocalPath { get; set; }

        //constructors
        public L2RDevice(Device Device)
        {
            ADBDevice = Device;

            Receiver = new ShellOutputReceiver();

            //Monitor.
            Monitor = new FileMonitor();
            if (ADBDevice.IsOnline)
            {
                GetCharacterName();
            }
        }

        public void GetCharacterName()
        {
            //List of remote files to be downloaded.
            List<FileEntry> Files = new List<FileEntry>();

            try
            {
                //First remote file.
                FileEntry File = FileEntry.Find(ADBDevice, "/data/data/com.netmarble.lin2ws/shared_prefs/NetmarbleNeo_LineageS.xml");

                //Add remote file to list of remote files.
                Files.Add(File);
            }
            catch (Exception e)
            {
                MainWindow.main.UpdateLog = "Unable to locate player data";
            }

            //Local filename string split character.
            string Axe = ":";

            //Local filename.
            string[] FileName = Regex.Split(ADBDevice.SerialNumber, Axe);

            LocalPath = "C:\\temp\\";

            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }

            //Local path + filename.
            string LocalURI = LocalPath + FileName[FileName.Length - 1] + ".xml";


            if (!ADBDevice.CanSU())
            {
                MainWindow.main.UpdateLog = "Android must be Rooted.";
            }

            try
            {
                //Pull file from remote emulator to local folder.
                ADBDevice.SyncService.Pull(Files, LocalPath, Monitor);
            }
            catch (Exception e)
            {
                MainWindow.main.UpdateLog = "Unable to locate player data";
            }

            //Rename file to adb port number.
            System.IO.File.Delete(LocalURI);
            try
            {
                System.IO.File.Move(LocalPath + "NetmarbleNeo_LineageS.xml", LocalURI);
                //sample XML input code:
                //<string name="LOCAL_PUSH_TARGET">CharacterName</string>
                CharacterName = Xml.FindInnerTextByTagAttribute(LocalURI, "string", "name", "LOCAL_PUSH_TARGET");
            }
            catch (Exception e)
            {
                MainWindow.main.UpdateLog = "Unable to rename file, file read/write error.";
            }
            if (CharacterName == null)
            {
                CharacterName = FileName[FileName.Length - 1];
            }

            ////testing shell commands
            //ShellOutPRec receiver = new ShellOutPRec( );
            //item.ExecuteShellCommand("ps", receiver);

            ////testing pull command
            //FileMonitor monitor = new FileMonitor();
            //List<FileEntry> files = new List<FileEntry>();
            //FileEntry file = FileEntry.Find(item, "/data/data/com.netmarble.lin2ws/shared_prefs/");
            //files.Add(file);
            //item.SyncService.Pull(files, "shared_prefs", monitor);

            ////testing screencap
            //PixelFormat format = PixelFormat.Format32bppArgb;
            //Image image = item.Screenshot.ToImage(format);


            ////Local storage path
            //LocalPath = "C:\\test\\";

            ////Monitor.
            //Monitor = new FileMonitor();

            //MainWindow.main.UpdateLog = ADBDevice.SerialNumber;
            ////Root Check
            //if (!ADBDevice.CanSU())
            //{
            //    MainWindow.main.UpdateLog = "Root Android.";
            //}

            ////List of remote files to be downloaded.
            //List<FileEntry> files = new List<FileEntry>();

            ////First remote file.
            //FileEntry file = FileEntry.Find(ADBDevice, "/data/data/com.netmarble.lin2ws/shared_prefs/NetmarbleNeo_LineageS.xml");

            ////Device 'IP:Port'.
            //string DeviceIPPort = ADBDevice.SerialNumber;

            ////String split character.
            //string Axe = ":";

            ////[0]IP
            ////[1]Port
            //string[] FileData = Regex.Split(DeviceIPPort, Axe);

            ////the last index in FileData containing Port.
            //string Port = FileData[FileData.Length - 1];

            ////Local storage path + Port.xml.
            //string LocalFilename = "C:\\test\\" + Port + ".xml";

            ////Delete the old file, if present.
            ////File.Delete(LocalFilename);

            ////Pull file from remote emulator to LocalPath.
            //ADBDevice.SyncService.Pull(files, "C:\\test\\", Monitor);

            //MainWindow.main.UpdateLog = Receiver.ToString();

            //Thread.Sleep(1000);

            ////Rename file to adb port number.
            //File.Move(LocalPath + "NetmarbleNeo_LineageS.xml", LocalFilename);

            ////sample XML to understand usage:
            ////<string name="LOCAL_PUSH_TARGET">CharacterName</string>
            ////<return>CharacterName</return>
            //CharacterName = Xml.FindInnerTextByTagAttribute(LocalFilename, "string", "name", "LOCAL_PUSH_TARGET");

            ////Show CharacterName
            //MainWindow.main.UpdateLog = CharacterName;
        }

        //logic

        public void Click(Point GamePoint)
        {
            ADBDevice.ExecuteShellCommand("input mouse tap " + GamePoint.X + " " + GamePoint.Y, Receiver);
        }

        public Image ScreenCap()
        {
            //testing screencap
            PixelFormat format = PixelFormat.Format32bppArgb;

            Image image = ADBDevice.Screenshot.ToImage(format);

            return image;
        }
    }


}
