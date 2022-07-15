using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace dischook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        WebClient webClient = new WebClient();

        private void sendMessage(string url, string message, string username)
        {
            using (webClient)
            {
                webClient.UploadValues(url, new NameValueCollection()
                {
                    {
                        "username",
                        username
                    },
                    {
                        "content",
                        message
                    }
                });
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            sendMessage(urlTextBox.Text, messageTextBox.Text, wuTextBox.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            
            BackgroundWorker.RunWorkerAsync();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            BackgroundWorker.CancelAsync();
            BackgroundWorker.Dispose();
            webClient.Dispose();
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                if (!BackgroundWorker.CancellationPending)
                {
                    if (!string.IsNullOrWhiteSpace(wuTextBox.Text) && !string.IsNullOrWhiteSpace(urlTextBox.Text) && !string.IsNullOrWhiteSpace(messageTextBox.Text))
                    {
                        if (!Uri.IsWellFormedUriString(urlTextBox.Text, UriKind.Absolute))
                        {
                            sendButton.Enabled = false;
                        }
                        else
                        {
                            if (urlTextBox.Text.Contains("discord") && urlTextBox.Text.Contains("api") && urlTextBox.Text.Contains("webhooks") == true)
                            {
                                sendButton.Enabled = true;
                            }
                            else
                            {
                                sendButton.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        sendButton.Enabled = false;
                    }
                    Thread.Sleep(1);
                }
            }
        }
    }
}
