using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Reddit_Image_Viewer
{
    public partial class Form1 : Form
    {
        public dynamic GetJSON()
        {
            WebClient client = new WebClient();
            dynamic JSONResult;

            try
            {
                JSONResult = JsonConvert.DeserializeObject(client.DownloadString("http://reddit.com/.json"));

                return JSONResult;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);

                return (string) e.Message;
            }
        }
        public Dictionary<string, string> ParseJSON(dynamic _jsoncontent)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();

            List<string> AcceptedDomains = new List<string>();
            AcceptedDomains.Add("imgur.com");
            AcceptedDomains.Add("i.imgur.com");

            foreach (dynamic _child in _jsoncontent.data.children) 
            {
                string _url, _title;

                try
                {
                    _url = (string)_child.data.url;
                    _title = (string)_child.data.title;
                }
                catch (Exception e)
                {
                    continue;
                }

                if (AcceptedDomains.Contains((string)_child.data.domain) && (_url.Contains(".jpg") || _url.Contains(".png")))
                {
                    Result.Add(_url, _title);
                }
            }

            foreach (var _keyval in Result) 
            {
                Console.WriteLine("{0} ({1})", _keyval.Value, _keyval.Key);
            }

            return Result;
        }
        public Form1()
        {
            InitializeComponent();
            dynamic JSON = GetJSON();
            Dictionary<string, string> JSONResult = ParseJSON(JSON);

            Console.Write("");

            // pictureBox1.Load();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)  // Prev image
        {

        }

        private void button2_Click(object sender, EventArgs e)  // Next image
        {

        }
    }
}
