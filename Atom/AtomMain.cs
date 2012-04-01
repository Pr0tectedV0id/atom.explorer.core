using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace Atom {
    public partial class AtomMain : Form {
        AtomCore Core = new AtomCore();
        Dictionary<int, Dictionary<string, string>> driveList = new AtomCore().DriveList();

        public AtomMain() {
            InitializeComponent();
        }

        private void AtomMain_Load(object sender, EventArgs e) {
            Core.runServer();
            Core.driveList(listForm, driveList);
        }

        private void fileList_DoubleClick(object sender, EventArgs e) {
            /* if (Core.isDriveReady(listForm.SelectedItem.ToString(), driveList)) { */
            Core.dirList(listForm);
            /* } else {
                MessageBox.Show(String.Format("{0} sürücüsü hazır değil.", listForm.SelectedItem.ToString()));
            }  */
        }

        private void incomingData_TextChanged(object sender, EventArgs e) {
            
        }
    }
}
