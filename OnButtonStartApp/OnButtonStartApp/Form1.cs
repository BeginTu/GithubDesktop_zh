using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OnButtonStartApp
{
    public partial class Form1 : Form
    {
        private static string softWarePath = null;
        //是否备份
        private static bool isBackup = true;
        //是否已经汉化
        private static bool isHanhua = false;
        //汉化时是否失败
        private static bool isFaild = false;
        //是否已经备份
        private static bool isBackuped = false;
        //用户是否同意退出时启动GitHub Desktop
        private static bool isStart = true;

        public Form1()
        {
            InitializeComponent();
            //窗口居中
            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 在窗口出现前加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + "\\Backup";
            //用户之前已经汉化并且已经备份
            if (Directory.Exists(path))
            {
                label3.Text = "汉化出现问题，需要还原？";
                button3.Text = "还原";
                button1.Enabled = checkBox1.Enabled = !(button3.Enabled = isBackuped = true);
            }
        }

        /// <summary>
        /// 检查Github Desktop是否被正确的安装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //提示用户正在查找Github Desktop路径
            //查找Github Desktop路径
            linkLabel1.Visible = false;
            if ((softWarePath = GetSoftWare()) == null)
            {
                label3.Text = "无法找到Github Desktop，请检查是否正确安装";
                label3.ForeColor = Color.Red;
                linkLabel1.Visible = isFaild = true;
                return;
            }
            else
            {
                //提示用户可以汉化
                label3.Text = "你可以汉化Github Desktop";
                label3.ForeColor = Color.Green;
                //可以汉化
                button3.Enabled = true;
                button1.Enabled = false;
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 汉化文件和还原文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isBackuped">是否备份</param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //用户已经备份且同意还原
            if (isBackuped)
            {
                //提示用户正在查找Github Desktop路径
                //查找Github Desktop路径
                if ((softWarePath = GetSoftWare()) == null)
                {
                    label3.Text = "无法找到Github Desktop，请检查是否正确安装";
                    label3.ForeColor = Color.Red;
                    linkLabel1.Visible = isFaild = true;
                    return;
                }
                //获取现在路径
                string path_ = Directory.GetCurrentDirectory(), destPath_, rePath_;
                rePath_ = path_ + "\\Backup"; //源文件夹
                destPath_ = softWarePath + "\\"; //目标文件夹                        
                //获取app-XXX文件
                DirectoryInfo directoryInfo_ = new DirectoryInfo(softWarePath);
                //遍历所有文件夹
                foreach (DirectoryInfo dir in directoryInfo_.GetDirectories())
                {
                    //如果文件夹名字带有"app-"字符串
                    if (dir.Name.StartsWith("app-"))
                    {
                        //存入destPath_中
                        destPath_ = destPath_ + dir.Name + "\\resources\\app";
                        break;
                    }
                }
                //没有找到文件
                if (destPath_ == softWarePath + "\\")
                {
                    //提示用户
                    label3.Text = "软件缺失文件！";
                    label3.ForeColor = Color.Red;
                    label3.Enabled = !(linkLabel1.Visible = isFaild = true);
                    return;
                }
                //获取源文件夹下的所有文件
                string[] files_ = Directory.GetFiles(rePath_);
                //遍历文件
                foreach (string file in files_)
                {
                    string name = rePath_ + "\\" + Path.GetFileName(file); //源文件
                    string pFilePath = destPath_ + "\\" + Path.GetFileName(file); //目标文件
                    File.Copy(name, pFilePath, true); //复制文件
                    File.Delete(name); //同时删除文件
                }
                Directory.Delete(rePath_, true); //删除文件夹
                //删除文件夹及下面的文件
                //复制成功，提示用户
                label3.Text = "还原成功！您可以继续汉化或向我们报告问题";
                label3.ForeColor = Color.Green;
                //LinkLabel
                linkLabel1.Visible = true;
                linkLabel1.Text = "报告问题";
                button3.Text = "汉化";
                button1.Enabled = button2.Enabled = checkBox1.Enabled = !(isBackuped = button3.Enabled = false);
                Process.Start(softWarePath + "\\GitHubDesktop.exe");
                return;
            }

            //开始汉化，取消检查和退出按钮
            button1.Enabled = button2.Enabled = false;
            checkBox1.Enabled = false;
            //如果用户同意备份则开始备份
            if (isBackup) Backup();
            else
            {
                int backUp = (int)MessageBox.Show("我们建议您在汉化之前先备份原先文件以防止一些意外，您确定不备份吗", "确定不备份吗？", MessageBoxButtons.OKCancel);
                if (backUp == 0) Backup();
            }
            //禁止退出
            button2.Enabled = false;
            //开始复制文件
            //获取当前文件路径
            string path = Directory.GetCurrentDirectory(), destPath, rePath;
            rePath = path + "\\Release"; //源文件夹
            destPath = softWarePath + "\\"; //目标文件夹
            //获取app-XXX文件
            DirectoryInfo directoryInfo = new DirectoryInfo(softWarePath);
            //遍历所有文件夹
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                //如果文件夹名字带有"app-"字符串
                if (dir.Name.StartsWith("app-"))
                {
                    //存入destPath中
                    destPath = destPath + dir.Name + "\\resources\\app";
                    break;
                }
            }
            //没有找到文件
            if (destPath == softWarePath + "\\")
            {
                //提示用户
                label3.Text = "软件缺失文件！";
                label3.ForeColor = Color.Red;
                linkLabel1.Visible = isFaild = true;
                return;
            }
            //获取Release文件夹下的所有文件
            string[] files = Directory.GetFiles(rePath);
            //遍历文件
            foreach (string file in files)
            {
                string name = rePath + "\\" + Path.GetFileName(file); //源文件
                string pFilePath = destPath + "\\" + Path.GetFileName(file); //目标文件
                File.Copy(name, pFilePath, true);
            }
            //复制成功，提示用户
            label3.Text = "恭喜！已经汉化成功！";
            isHanhua = checkBox1.Enabled = button2.Enabled = true;
            button3.Text = "还原";
        }

        /// <summary>
        /// 用户是否备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isBackup = checkBox1.Checked;
        }

        /// <summary>
        /// 备份文件
        /// </summary>
        private void Backup()
        {
            //当前文件路径
            string path = Directory.GetCurrentDirectory(), destPath, rePath;
            destPath = path + "\\Backup"; //目标文件夹
            rePath = softWarePath + "\\"; //源文件夹
            //创建文件夹
            Directory.CreateDirectory(destPath);
            //获取源文件夹下的所有文件夹
            DirectoryInfo directoryInfo = new DirectoryInfo(softWarePath);
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                //如果文件夹名字带有"app-"字符串
                if (dir.Name.StartsWith("app-"))
                {
                    //存入rePath中
                    rePath = rePath + dir.Name + "\\resources\\app";
                    break;
                }
            }
            //复制文件
            if (!Directory.Exists(rePath))
            {
                label3.Text = "汉化程序失败：源文件丢失。请下载完整文件";
                label3.ForeColor = Color.Red;
                button2.Enabled = true; button3.Enabled = false;
                button2.Text = "退出";
                Directory.CreateDirectory(rePath);
                return;
            }
            //得到源文件夹下的所有文件
            string[] files = Directory.GetFiles(rePath);
            foreach (string file in files)
            {
                string name = rePath + "\\" + Path.GetFileName(file);
                string pFilePath = destPath + "\\" + Path.GetFileName(file);
                if (File.Exists(pFilePath)) continue;
                File.Copy(name, pFilePath, true);
            }
            isBackuped = true;
        }

        /// <summary>
        /// 获得软件安装路径
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
        /// 打开网页
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
