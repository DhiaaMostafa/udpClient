using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;/// فضاء الاسماء الخاص بالشبكة 
using System.Net.Sockets;///   فضاء الاسماء الخاص  ب مقبض الشبكة 
using System.Threading;  // فضاء الاسماء الخاص  بthread
namespace Clint_Upd_peer_to_peer
{
    /// <summary>
    /// /برنامج العميل    لا يحتاج الى عملية اتصال او ربط اتصال  ارسال  مباشرة 
    /// </summary>
    public partial class Form1 : Form
    {
        Socket Sc;
        /// ip and port
        IPEndPoint iep;
        /// remot    
        /// Endpoit  يحصل علي
        /// الخاص بالمرسل  
        EndPoint Ep;
        Thread thd;
        public Form1()
        {
            InitializeComponent();
            Sc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            Ep = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            /// thd=new therad( اسم الدلة التي  تريد ان يوشر  عليها الثريد )
            thd = new Thread(Recive_is);
     
        }
        void Recive_is()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024];
                


                    int res = Sc.ReceiveFrom(data, ref Ep);
                
                    this.Invoke((MethodInvoker)delegate
                    {  
                       string str= Encoding.ASCII.GetString(data, 0, res);
                        listBox1.Items.Add("AAA>>>" + str);
                    });


                }
                catch
                {
                   
                }

            }

        }
    

        private void Form1_Load(object sender, EventArgs e)
        {
            Sc.SendTo(Encoding.ASCII.GetBytes("welcam"),iep);
            /// تشغيل الثريد 
            thd.Start();
            button1.Enabled = false;
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes(textBox1.Text);
            Sc.SendTo(data, SocketFlags.None,iep);
            listBox1.Items.Add("my>>>" + textBox1.Text);
            textBox1.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = (textBox1.Text.Trim() != "") ? true : false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Thread.CurrentThread.Abort();
            Application.ExitThread();
        }
    }
}
