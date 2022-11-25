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
namespace UDP
{
    /// <summary>
    /// / برنامج مراسلة peer to peer with udp
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        ///  اول  ما احتاجه  هو تعريف  كائن من  نوع
        ///  socket 
        /// </summary>
        Socket Sc;
        /// ip and port
        IPEndPoint iep;

        /// remot    
        /// Endpoit  يحصل علي
        /// الخاص بالمرسل  
        EndPoint Ep;

        // thread   تعريف  كائن من نوع 
        // فانه   يفصلها  عن البرنامج حتى لا توثر  عليه  loop الغرض  منه  هو     تعدد المهام بمعنى  لو كان معنا 
        Thread thd;
        public Form1()
        {
            InitializeComponent();
            ///  
            Sc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            iep = new IPEndPoint(IPAddress.Any, 8888);
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
                    // ReceiveFrom(data, مرجعية )

                    int res = Sc.ReceiveFrom(data, ref Ep);
                    //    سوف  تواجهك مشكلة عند التعامل  مع اي اداة في  الفور 
                    //  thread  في  الدالة التى يوشر عليها  
                    //  اخر  thread   لن الادوادات  مبنية في 
                    //  لحل المشكلة استخدم الدالة التالية 
                    this.Invoke((MethodInvoker)delegate
                    {
                        /// تمكنك الدالة التالية من التعامل  مع الادوات   
                        /// TextBox  - label ---etc ..... المقصود بالادوات  مثل   
                        string str = Encoding.ASCII.GetString(data, 0, res);
                        listBox1.Items.Add("AAA>>>" + str);
                    });
                        
                    
                }
                catch { 
                
                }
            
            }

        }
    
        private void Form1_Load(object sender, EventArgs e)
        {
            /// عملية بداء الاتصال   
            //// ربط  ip and port  with socket
            Sc.Bind(iep);

            /// تشغيل الثريد 
            thd.Start();

            button1.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /// في  حدث  ععند الاغلاق  
            /// هذه  الدالة تغلق  الثريد بشكل  نهائي 
            Thread.CurrentThread.Abort();
            Application.ExitThread();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte []data=Encoding.ASCII.GetBytes(textBox1.Text);
            Sc.SendTo(data, SocketFlags.None, Ep);
            listBox1.Items.Add("my>>>" + textBox1.Text);
            textBox1.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled=(textBox1.Text.Trim()!="")?true:false;
        }
    }
}
