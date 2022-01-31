using System.Diagnostics;

namespace OnButtonStartApp 
{
    public partial class Form1 : Form
    {
        private static string softWarePath = null;
        private static bool isBackup = true,isBackuped = false,isFaild = false;
        public Form1() 
        {
            InitializeComponent();
            //���ھ���
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// �ڴ��ڳ���ǰ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + "\\Backup";
            //�û�֮ǰ�Ѿ����������Ѿ�����
            if (Directory.Exists(path))
            {
                label3.Text = "�����������⣬��Ҫ��ԭ��";
                button3.Text = "��ԭ";
                button1.Enabled = checkBox1.Enabled = !(button3.Enabled = isBackuped = true);
            }
        }

        /// <summary>
        /// ���Github Desktop�Ƿ���ȷ�İ�װ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e) 
        {
            //��ʾ�û����ڲ���Github Desktop·��
            //����Github Desktop·��
            if ((softWarePath = GetSoftWare()) == null)
            {
                label3.Text = "�޷��ҵ�Github Desktop�������Ƿ���ȷ��װ";
                label3.ForeColor = Color.Red;
                linkLabel1.Visible = isFaild = true;
                return;
            }
            else
            {
                //��ʾ�û����Ժ���
                label3.Text = "����Ժ���Github Desktop";
                label3.ForeColor = Color.Green;
                //���Ժ���
                button3.Enabled = true;
                button1.Enabled = false;
            }
        }

        /// <summary>
        /// �˳�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// �����ļ��ͻ�ԭ�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isBackuped">�Ƿ񱸷�</param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //�û��Ѿ�������ͬ�⻹ԭ
            if(isBackuped)
            {
                //��ʾ�û����ڲ���Github Desktop·��
                //����Github Desktop·��
                if ((softWarePath = GetSoftWare()) == null)
                {
                    label3.Text = "�޷��ҵ�Github Desktop�������Ƿ���ȷ��װ";
                    label3.ForeColor = Color.Red;
                    linkLabel1.Visible = isFaild = true;
                    return;
                }
                //��ȡ����·��
                string path_ = Directory.GetCurrentDirectory(), destPath_, rePath_;
                rePath_ = path_ + "\\Backup"; //Դ�ļ���
                destPath_ = softWarePath + "\\"; //Ŀ���ļ���                        
                //��ȡapp-XXX�ļ�
                DirectoryInfo directoryInfo_ = new DirectoryInfo(softWarePath);
                //���������ļ���
                foreach (DirectoryInfo dir in directoryInfo_.GetDirectories())
                {
                    //����ļ������ִ���"app-"�ַ���
                    if (dir.Name.StartsWith("app-"))
                    {
                        //����destPath_��
                        destPath_ = destPath_ + dir.Name + "\\resources\\app";
                        break;
                    }
                }
                //û���ҵ��ļ�
                if (destPath_ == softWarePath + "\\")
                {
                    //��ʾ�û�
                    label3.Text = "���ȱʧ�ļ���";
                    label3.ForeColor = Color.Red;
                    label3.Enabled = !(linkLabel1.Visible = isFaild = true);
                    return;
                }
                //��ȡԴ�ļ����µ������ļ�
                string[] files_ = Directory.GetFiles(rePath_);
                //�����ļ�
                foreach (string file in files_)
                {
                    string name = rePath_ + "\\" + Path.GetFileName(file); //Դ�ļ�
                    string pFilePath = destPath_ + "\\" + Path.GetFileName(file); //Ŀ���ļ�
                    File.Copy(name, pFilePath, true); //�����ļ�
                    File.Delete(name); //ͬʱɾ���ļ�
                }
                Directory.Delete(rePath_,true); //ɾ���ļ���
                //ɾ���ļ��м�������ļ�
                //���Ƴɹ�����ʾ�û�
                label3.Text = "��ԭ�ɹ��������Լ��������������Ǳ�������";
                //LinkLabel
                linkLabel1.Visible = true;
                linkLabel1.Text = "��������";
                button3.Text = "����"; button1.Enabled = button2.Enabled = !(isBackuped = false);
                return;
            } 

            //��ʼ������ȡ�������˳���ť
            button1.Enabled = button2.Enabled = false;
            checkBox1.Enabled = false;
            //����û�ͬ�ⱸ����ʼ����
            if (isBackup) Backup();
            else
            {
                int backUp = (int)MessageBox.Show("���ǽ������ں���֮ǰ�ȱ���ԭ���ļ��Է�ֹһЩ���⣬��ȷ����������","ȷ����������",MessageBoxButtons.OKCancel);
                if (backUp == 0) Backup();
            }
            //��ֹ�˳�
            button2.Enabled = false;
            //��ʼ�����ļ�
            //��ȡ��ǰ�ļ�·��
            string path = Directory.GetCurrentDirectory(), destPath, rePath;
            rePath = path + "\\Release"; //Դ�ļ���
            destPath = softWarePath + "\\"; //Ŀ���ļ���
            //��ȡapp-XXX�ļ�
            DirectoryInfo directoryInfo = new DirectoryInfo(softWarePath);
            //���������ļ���
            foreach(DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                //����ļ������ִ���"app-"�ַ���
                if(dir.Name.StartsWith("app-"))
                {
                    //����destPath��
                    destPath = destPath + dir.Name + "\\resources\\app";
                    break;
                }
            }
            //û���ҵ��ļ�
            if(destPath == softWarePath + "\\")
            {
                //��ʾ�û�
                label3.Text = "���ȱʧ�ļ���";
                label3.ForeColor = Color.Red;
                linkLabel1.Visible = isFaild = true;
                return;
            }
            //��ȡRelease�ļ����µ������ļ�
            string[] files = Directory.GetFiles(rePath);
            //�����ļ�
            foreach (string file in files)
            {
                string name = rePath + "\\" + Path.GetFileName(file); //Դ�ļ�
                string pFilePath = destPath + "\\" + Path.GetFileName(file); //Ŀ���ļ�
                File.Copy(name, pFilePath, true);
            }
            //���Ƴɹ�����ʾ�û�
            label3.Text = "��ϲ���Ѿ������ɹ���";
            button3.Text = "��ԭ";
        }

        /// <summary>
        /// �û��Ƿ񱸷�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isBackup = checkBox1.Checked;
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        private void Backup()
        {
            //��ǰ�ļ�·��
            string path = Directory.GetCurrentDirectory(), destPath, rePath;
            destPath = path + "\\Backup"; //Ŀ���ļ���
            rePath = softWarePath + "\\"; //Դ�ļ���
            //�����ļ���
            Directory.CreateDirectory(destPath);
            //��ȡԴ�ļ����µ������ļ���
            DirectoryInfo directoryInfo = new DirectoryInfo(softWarePath);
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                //����ļ������ִ���"app-"�ַ���
                if (dir.Name.StartsWith("app-"))
                {
                    //����rePath��
                    rePath = rePath + dir.Name + "\\resources\\app";
                    break;
                }
            }
            //�����ļ�
            if(!Directory.Exists(rePath))
            {
                label3.Text = "��������ʧ�ܣ�Դ�ļ���ʧ�������������ļ�";
                label3.ForeColor = Color.Red;
                button2.Enabled = true; button3.Enabled = false;
                button2.Text = "�˳�";
                Directory.CreateDirectory(rePath);
                return;
            }
            //�õ�Դ�ļ����µ������ļ�
            string[] files = Directory.GetFiles(rePath);
            foreach (string file in files)
            {
                string name = rePath + "\\" + Path.GetFileName(file);
                string pFilePath = destPath + "\\" + Path.GetFileName(file);
                if (File.Exists(pFilePath)) continue;
                File.Copy(name, pFilePath,true);
            }
            isBackuped = true;
        }

        /// <summary>
        /// ��������װ·��
        /// </summary>
        /// <param name="SoftWarePath"></param>
        /// <returns>true or false</returns>
        public string GetSoftWare()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\GitHubDesktop";
            if (Directory.Exists(path)) 
            {
                return path;
            } 
            else return null;
        }

        /// <summary>
        /// ����ҳ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            if (isFaild) Process.Start("explorer.exe", "https://desktop.github.com/");
            else if (!isBackuped) Process.Start("explorer.exe", "https://github.com/BeginTu/GithubDesktop_zh/issues");
        }
    }
}