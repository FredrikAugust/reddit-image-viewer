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
        private int position;

        private Dictionary<string, string> JSONResult;

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

        public int GetPos(Dictionary<string, string> _dict)
        {
            int _count = 0;

            foreach (var _keyval in _dict)
            {
                if (_keyval.Key == pictureBox1.ImageLocation)
                {
                    break;
                }
                else
                {
                    _count++;
                }
            }

            return _count;
        }

        public int GetTotal(Dictionary<string, string> _dict)
        {
            int _total = 0;

            foreach (var _keyval in _dict) 
            {
                _total++;
            }

            return _total;
        }

        private void pictureBox1_Click(object sender, EventArgs e) {  }

        private void label1_Click(object sender, EventArgs e) {  }

        public void PrevImage(Dictionary<string, string> _pics)
        {
            uint _count = 0;
            int _current = GetPos(_pics);

            foreach (var _keyval in _pics)
            {
                if (_count == _current - 1 && _count >= 0)
                {
                    pictureBox1.Load(_keyval.Key);
                    label1.Text = _keyval.Value;
                    break;
                }
                else if (_count != _current - 1 && _count >= 0)
                {
                    _count++;
                    continue;
                }
                else
                {
                    continue;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)  // Prev image
        {
            PrevImage(JSONResult);
        }

        public void NextImage(Dictionary<string, string> _pics)
        {
            uint _count = 0;
            int _current = GetPos(_pics);
            int _total = GetTotal(_pics);

            foreach (var _keyval in _pics) 
            {
                if (_count == _current + 1  && _count <= _total) 
                {
                    pictureBox1.Load(_keyval.Key);
                    label1.Text = _keyval.Value;
                    break;
                }
                else if (_count != _current + 1 && _count <= _total)
                {
                    _count++;
                    continue;
                }
                else
                {
                    continue;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)  // Next image
        {
            NextImage(JSONResult);
        }

        public Form1()
        {
            InitializeComponent();
            dynamic JSON = GetJSON();
            JSONResult = ParseJSON(JSON);

            pictureBox1.Load(JSONResult.First().Key);
            label1.Text = JSONResult.First().Value;

            position = 0;
        }
    }
}
