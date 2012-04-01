using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Web;

namespace Atom {
    class AtomCore {

        #region "Class constants"
        private int port = 5226;
        private TcpListener tcpListener;
        private Thread listenThread;
        delegate void SetTextCallback(string text);
        #endregion
        public AtomCore() {}
        #region "Server Methods"

        static public string encodeData(string toEncode) {
            return HttpUtility.UrlEncode(toEncode);
        }

        static public string decodeData(string encodedData) {
            // Şu an için Base64 ile iletişim sağlıyoruz.
            return HttpUtility.UrlDecode(encodedData);
        }

        public void runServer() {
            this.tcpListener = new TcpListener(IPAddress.Any, port);
            this.listenThread = new Thread(new ThreadStart(atomListen));
            this.listenThread.Start();
        }
        private void atomListen() {
            this.tcpListener.Start();
            while (true) {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(atomHandle));
                clientThread.Start(client);
            }
        }
        public void atomHandle(object client) {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            byte[] message = new byte[4096];
            int bytesRead;
            while (true) {
                bytesRead = 0;
                try { bytesRead = clientStream.Read(message, 0, 4096); } catch { break; }
                if (bytesRead == 0) { break; }
                ASCIIEncoding encoder = new ASCIIEncoding();
                string getMessage = encoder.GetString(message, 0, bytesRead);
                putMessage(getMessage);
            }

            tcpClient.Close();
        }

        private void putMessage(string msg = "") {
            if (msg != "") {
                msg = decodeData(msg);
                var Atom = Form.ActiveForm as AtomMain;
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (Atom.incomingData.InvokeRequired) {                    
                    SetTextCallback d = new SetTextCallback(putMessage);
                    Atom.incomingData.Invoke(d, new object[] { msg });
                } else {
                    Atom.incomingData.Text = msg;
                }
            }
        }

        public void testServer(string msg = "") {
            TcpClient client = new TcpClient();
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            client.Connect(serverEndPoint);
            NetworkStream clientStream = client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(msg);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }
        #endregion

        #region "File system methods"

        public Dictionary<int, Dictionary<string, string>> DriveList() {
            Dictionary<int, Dictionary<string, string>> driveData = new Dictionary<int, Dictionary<string, string>>();
            DriveInfo[] driveList = DriveInfo.GetDrives();
            int i = 0;
            foreach (DriveInfo Drive in driveList) {
                Dictionary<string, string> driveInfoDetail = new Dictionary<string, string>();
                string DriveFormat, DriveType, TotalSize, TotalFreeSpace, VolumeLabel, isReady;
                if (Drive.IsReady == true) {
                    DriveFormat = Drive.DriveFormat.ToString();
                    DriveType = Drive.DriveType.ToString();
                    TotalSize = Drive.TotalSize.ToString();
                    TotalFreeSpace = Drive.TotalFreeSpace.ToString();
                    VolumeLabel = Drive.VolumeLabel.ToString();
                    isReady = "true";
                } else {
                    DriveFormat = "{n/a}"; DriveType = "{n/a}"; TotalSize = "{n/a}"; TotalFreeSpace = "{n/a}"; VolumeLabel = "{n/a}";
                    isReady = "false";
                }
                driveInfoDetail.Add("name", Drive.ToString());
                driveInfoDetail.Add("fileSystem", DriveFormat);
                driveInfoDetail.Add("driveType", DriveType);
                driveInfoDetail.Add("totalSpace", TotalSize);
                driveInfoDetail.Add("freeSpace", TotalFreeSpace);
                driveInfoDetail.Add("label", VolumeLabel);
                driveInfoDetail.Add("ready", isReady);
                driveData[i] = driveInfoDetail;
                i++;
            }
            return driveData;
        }

        public bool isFolder(string path) {
            try {
                return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
            } catch {
                return false;
            }
        }

        public bool isDriveReady(string driveName = "", Dictionary<int, Dictionary<string, string>> driveList = null) {
            if (driveName != "" || driveList != null) {
                for (int i = 0; i < driveList.Count(); i++) {
                    Dictionary<string, string> driveData = driveList[i];
                    if (driveName == driveData["name"]) {
                        if (driveData["ready"] == "true") {
                            return true;
                        } else {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public string[] getDir(string dir = "") {
            if (dir != "") {
                if (isFolder(dir)) {
                    string[] dirList = Directory.GetDirectories(dir);
                    string[] fileList = Directory.GetFiles(dir);
                    string[] dirItems = new string[dirList.Length + fileList.Length];
                    dirList.CopyTo(dirItems, 0);
                    fileList.CopyTo(dirItems, dirList.Length);
                    return dirItems;
                }
            }
            return null;
        }

        #endregion

        #region "GUI Interaction"

        public void refreshList(ListBox listBox) {
            listBox.Items.Clear();
        }

        public void driveList(ListBox listBox, Dictionary<int, Dictionary<string, string>> driveList) {
            refreshList(listBox);
            for(int i=0;i<driveList.Count();i++) {
                Dictionary<string,string> driveData = driveList[i];
                listBox.Items.Add(driveData["name"]);
            }
        }

        public void dirList(ListBox listBox) {
            string dir = listBox.SelectedItem.ToString();
            string[] itemList = getDir(dir);

            if (itemList != null) { 
                refreshList(listBox);
                listBox.Items.Add("..");
                foreach(string item in itemList) {
                    listBox.Items.Add(item.ToString());
                }
            }
        }

        #endregion

    }
}