using System;
using System.IO;
using System.Windows.Forms;

namespace SimpleRenamer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // イベント登録
            btnAddFiles.Click += BtnAddFiles_Click;
            btnAddFolder.Click += BtnAddFolder_Click;
            btnPreview.Click += BtnPreview_Click;
            btnRename.Click += BtnRename_Click;
            btnRemoveSelected.Click += BtnRemoveSelected_Click;
            btnClearAll.Click += BtnClearAll_Click;
        }

        private void BtnAddFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in ofd.FileNames)
                        listBoxFiles.Items.Add(file);
                }
            }
        }

        private void BtnAddFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in Directory.GetFiles(fbd.SelectedPath))
                        listBoxFiles.Items.Add(file);
                }
            }
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            listBoxPreview.Items.Clear();
            int startNum = (int)numericStart.Value;

            for (int i = 0; i < listBoxFiles.Items.Count; i++)
            {
                string oldPath = listBoxFiles.Items[i].ToString();
                string dir = Path.GetDirectoryName(oldPath);
                string ext = Path.GetExtension(oldPath);
                string newName = $"{txtPrefix.Text}{startNum + i}{txtSuffix.Text}{ext}";
                listBoxPreview.Items.Add(Path.Combine(dir, newName));
            }
        }

        private void BtnRename_Click(object sender, EventArgs e)
        {
            int startNum = (int)numericStart.Value;

            for (int i = 0; i < listBoxFiles.Items.Count; i++)
            {
                string oldPath = listBoxFiles.Items[i].ToString();
                string dir = Path.GetDirectoryName(oldPath);
                string ext = Path.GetExtension(oldPath);
                string newPath = Path.Combine(dir, $"{txtPrefix.Text}{startNum + i}{txtSuffix.Text}{ext}");

                try
                {
                    File.Move(oldPath, newPath);
                    listBoxFiles.Items[i] = newPath; // リスト更新
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"リネーム失敗: {oldPath}\n{ex.Message}");
                }
            }

            MessageBox.Show("リネーム完了！");
            BtnPreview_Click(null, null); // プレビュー更新
        }

        private void BtnRemoveSelected_Click(object sender, EventArgs e)
        {
            while (listBoxFiles.SelectedItems.Count > 0)
                listBoxFiles.Items.Remove(listBoxFiles.SelectedItems[0]);
        }

        private void BtnClearAll_Click(object sender, EventArgs e)
        {
            listBoxFiles.Items.Clear();
            listBoxPreview.Items.Clear();
        }

        /*[STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }*/
    }
}
