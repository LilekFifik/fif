using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace WindowsFormsApplication16
{
    public partial class Form1 : Form
    {
        delegate void AppendTextToDebugWindowHandler(TreeNode node, TreeNode node2);
        delegate void AppendFilePathToDebugWindowHandler(string filepath);
        delegate void ToLabelHandler(string Text);
        delegate void NoNodesHandler(string NoNodesText);
        delegate void butt_eventHandler(bool b);
        private delegate void UpdateLabelDelegate();
        DateTime start;
   
       public Form1()
       {
           InitializeComponent();
           this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
           string savedata=Application.StartupPath + @"\" + "savedata.txt";
           FileInfo MyFile = new FileInfo(savedata);

           if (!File.Exists(savedata))
           {
               if (MyFile.Exists == false)
               {
                   FileStream fs = MyFile.Create();
                   fs.Close();
               }


           }
           else
           {
               StreamReader sr2 = new StreamReader(savedata);
               string[] lines = System.IO.File.ReadAllLines(savedata);

                  using (var sr = new StreamReader(savedata, Encoding.GetEncoding(1251)))
                       {

                           label9.Text = lines[0].ToString();
                           comboBox2.Text = lines[1].ToString();
                           richTextBox1.Text += lines[2].ToString();
                       }
                       sr2.Close();
                   
               
           }
          
           start = new DateTime();
           timer1.Interval = 100;
           timer1.Enabled = true;
           button1.Text = "Start/pause";
           button2.Text = "Stop";

           backgroundWorker1.WorkerReportsProgress = true;
           backgroundWorker1.WorkerSupportsCancellation = true;
        
       }


        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
       {
         
            progressBar1.Value = e.ProgressPercentage;

            this.Text = e.ProgressPercentage.ToString();
        }
     private string openFileName = "", folderName = "", rich = "", cb = "";
        private int count = 0;
     
      
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.Items.Add("*.txt");
        

            comboBox2.SelectedIndex = 0;
           
     }

        private void NoNodes(string Text)
        {
            if (treeView1.InvokeRequired)
            {
                this.Invoke(new ToLabelHandler(this.NoNodes), new object[] { Text });
                return;
            }

            treeView1.Nodes.Add(Text);

        }
        private void butt_event(bool b=true)
        {
            if (button1.InvokeRequired)
            {
                this.Invoke(new butt_eventHandler(this.butt_event), new object[] { b });
                return;
            }

            button1.Enabled = b;

        }

      private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            duration = 0;
            if (flag == true)
            {
                
                progressBar1.Value = 0;
                string str="";
                string NoNodesText = "Совпадения отсуствуют";
                int n = 1;
                string[] patharray = new string[n];
                count = 0;
                string[] patharray_after = new string[1];
                string openfilename = "";
                Array.Resize(ref patharray_after, patharray_after.Length);
                openfilename = Path.GetFileNameWithoutExtension(openFileName);
                
                int i = 0; int z = 0;
                TreeNode Parent = new TreeNode();
                TreeNode Child = new TreeNode();
                TreeNode[] array = new TreeNode[0];

                Parent.Text = folderName;
                TreeNode treeNode = new TreeNode();
                bool b = true;
                int i1 = 0; patharray = new string[n];
                int count_files = Directory.GetFiles(folderName, cb, SearchOption.TopDirectoryOnly).Length;

             
                foreach (string fileName in Directory.GetFiles(folderName, cb, SearchOption.TopDirectoryOnly))
                {
                   
        
                    if (backgroundWorker1.CancellationPending == false)
                    {

                        b = false;
                        string[] lines = System.IO.File.ReadAllLines(fileName);

                        AppendFilePathToDebugWindow(fileName); Thread.Sleep(1000);
                        i1++;
                        ToLabel(i1.ToString());
                        foreach (string line in lines)
                        {
                           
                            backgroundWorker1.ReportProgress(i1 * 100 / count_files);

                            System.Threading.Thread.Sleep(1000);
                            using (var sr = new StreamReader(fileName, Encoding.GetEncoding(1251)))
                            {

                                string read = null;
                                while ((read = sr.ReadLine()) != null)
                                {
                                    string[] split = rich.Split(new Char[] { ' ' });
                                    if (split[split.Length - 1] == "")
                                    {
                                        Array.Resize(ref split, split.Length - 1);

                                    }
                                    for (int g = 0; g < split.Length; g++)
                                    {
                                        str = split[g] + " ";
                                    }
                                    if (read.ToLower().Contains(str.ToLower()))
                                    {

                                        Array.Resize(ref patharray, n);
                                        patharray[n - 1] = fileName;

                                        n++;
                                        b = true;
                                    }
                                }


                            }

                            if (b == true)
                            {
                                string fileName2 = Path.GetFileName(fileName);
                                Child = new TreeNode(fileName2);

                                Array.Resize(ref array, array.Length + 1);
                                array[i] = Child;
                                AppendTextToDebugWindow(array[i], Parent);
                                Thread.Sleep(1000);

                                break;
                            }
                            else
                            {
                                break;
                            }

                        }

                    }
                  

                    }


                
                flag = false;

                if (array.Length == 0)
                {
                   
                    NoNodes(NoNodesText);
                  

                }

                butt_event(b = true);
                timer1.Stop();
                duration = 0;
            }
            }


        int duration = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
         
            
         
          label1.Text = string.Format("{0:HH:mm:ss}", start.AddMilliseconds(timer1.Interval * duration));
          duration++;
      
        }
        private void AppendFilePathToDebugWindow(string fileName)
        {

            if (label8.InvokeRequired)
            {
                this.Invoke(new AppendFilePathToDebugWindowHandler(this.AppendFilePathToDebugWindow), new object[] { fileName });
                return;
            }

            label8.Text = fileName;
         
           
        }
       
        private void ToLabel(string Text)
        { 
            if (label7.InvokeRequired)
            {
                this.Invoke(new ToLabelHandler(this.ToLabel), new object[] { Text });
                return;
            }

            label7.Text = Text;
          
        }
        private void AppendTextToDebugWindow(TreeNode child, TreeNode parent)
        {
            
            if (treeView1.InvokeRequired)
            {
                this.Invoke(new AppendTextToDebugWindowHandler(this.AppendTextToDebugWindow), new object[] { child,parent });
                return;
            }

                parent.Nodes.Add(child);
                if (count == 0)
                {
                    treeView1.Nodes.Add(parent);
                }
            count++;
        }


        bool flag = false;
        private void button1_Click_1(object sender, EventArgs e)
        {
            flag = true;
            Control.CheckForIllegalCrossThreadCalls = false;
            folderName = label9.Text;
            string[] contents= {label9.Text,comboBox2.Text,richTextBox1.Text};
          
                File.WriteAllLines(Application.StartupPath +@"/"+ "savedata.txt", contents);
            
            button1.Enabled = false;
           
          rich = richTextBox1.Text;
            cb = comboBox2.Text;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
          
            if (!backgroundWorker1.IsBusy)
            {
                timer1.Start(); 
                backgroundWorker1.RunWorkerAsync();
               
                return;
            }

           
            
  }
     private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            bool fileOpened = false;
            if (!fileOpened)
            {
                openFileDialog1.InitialDirectory = folderBrowserDialog1.SelectedPath;
                openFileDialog1.FileName = null;
            }
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Выберите папку,где имеются файлы с расширением "+comboBox2.Text;
                dialog.ShowNewFolderButton = false;
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                try
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        folderName = dialog.SelectedPath;
                        label9.Text = folderName;
                    }
                }
                   
                catch (Exception exc)
                {
                    MessageBox.Show("Import failed because " + exc.Message + " , please try again later.");

                }



            }
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
           timer1.Stop();
            backgroundWorker1.CancelAsync();
            progressBar1.Style = ProgressBarStyle.Continuous;
        }

     

     

       
    }
}
