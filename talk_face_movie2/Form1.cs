using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace talk_face_movie2
{
    public partial class Form1 : Form
    {
        private const string param_json_name = @"param.json";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxLog.Text = "";

            if (File.Exists(textBoxInputfile.Text) == false)
            {
                MessageBox.Show("Not found input file.\n\n" + textBoxInputfile.Text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string exe_dir = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
            string image_dir = textBoxImagedir.Text;
            if (image_dir.IndexOf(":") < 0)
            {
                image_dir = exe_dir + @"\" + image_dir;
            }
            if (Directory.Exists(image_dir) == false)
            {
                MessageBox.Show("Not found image dir.\n\n" + textBoxImagedir.Text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 1; i <= 4; i++)
            {
                string filename = image_dir + @"\" + "face_" + i.ToString() + ".png";
                if (File.Exists(filename) == false)
                {
                    MessageBox.Show("Not found image file.\n\n" + filename, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (File.Exists(textBoxFfmpeg.Text) == false)
            {
                if (ExistFileWithPathEnv(textBoxFfmpeg.Text) == false)
                {
                    MessageBox.Show("Not found ffmpeg.\n\n" + textBoxFfmpeg.Text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }


            WaveReadSample.WaveReadSample wrs = new WaveReadSample.WaveReadSample();

            // clean up temp dir
            string temp_dir = Path.Combine(exe_dir, "temp");
            foreach (string filename in Directory.GetFiles(temp_dir, "*.png"))
            {
                File.Delete(filename);
            }

            string temp_mp4 = Path.Combine(exe_dir, "temp.mp4");
            string filename_close = Path.Combine(image_dir, "face_1.png");
            string filename_small = Path.Combine(image_dir, "face_2.png");
            string filename_large = Path.Combine(image_dir, "face_3.png");
            string filename_blink = Path.Combine(image_dir, "face_4.png");

            wrs.ReadWave(textBoxInputfile.Text);
            WaveReadSample.WaveHeaderArgs wha = wrs.GetHeader();

            int data_length = wrs._waveData.Length;
            int sample_rate = wha.SampleRate;
            int sample_interval = sample_rate / (int)numericUpDownFramerate.Value;
            //int sample_count = data_length / sample_interval;
            int sample_count = 0;
            double signal_peak = 0;
            int latest_blink = 0;
            int frame_number = 0;
            string prev_style = "";
            int blink_interval = (int)numericUpDownBlinkInterval.Value;

            double small_threshold = (double)numericUpDownSmallThreshold.Value / 100.0;
            double large_threshold = (double)numericUpDownLargeThreshold.Value / 100.0;

            for (int i = 0; i < data_length; i++)
            {
                int value = (int)wrs._waveData[i];
                double value_d = (double)value / (double)((1 << 15) - 1);
                signal_peak = Math.Max(signal_peak, Math.Abs(value_d));
                sample_count++;
                if (sample_count >= sample_interval)
                {
                    string frame_number_with_zero = frame_number.ToString("D8");
                    double div = (double)frame_number / (double)numericUpDownFramerate.Value;
                    double secd = Math.Floor(div);
                    double msec = div - secd;
                    int min = (int)secd / 60;
                    int sec = (int)secd % 60;
                    string msec_with_zero = ((int)(msec * 1000)).ToString("D3");
                    string sec_with_zero = sec.ToString("D2");
                    string min_with_zero = min.ToString("D3");
                    //print_textbox($"{frame_number_with_zero} ({min_with_zero}:{sec_with_zero}:{msec_with_zero}) {signal_peak:F3}  ", false);
                    print_textbox(string.Format("{0} ({1}:{2}:{3}) {4:F3}  ", 
                        frame_number_with_zero, min_with_zero, sec_with_zero, msec_with_zero, signal_peak), false);
                    string temp_filename = temp_dir + @"\" + frame_number_with_zero + ".png";
                    frame_number++;


                    if (signal_peak < small_threshold)
                    {
                        if (((i - latest_blink) * 1000 / sample_rate) > blink_interval)
                        {
                            latest_blink = i;
                            // blink
                            File.Copy(filename_blink, temp_filename);
                            prev_style = "blink";
                            print_textbox("blink");
                        }
                        else
                        {
                            //close face
                            File.Copy(filename_close, temp_filename);
                            prev_style = "close";
                            print_textbox("close");
                        }
                    }
                    else if (signal_peak < large_threshold)
                    {
                        // small mouth
                        if (prev_style == "small")
                        {
                            // numnum
                            File.Copy(filename_close, temp_filename);
                            prev_style = "close";
                            print_textbox("close(small)");
                        }
                        else
                        {
                            File.Copy(filename_small, temp_filename);
                            prev_style = "small";
                            print_textbox("small");
                        }
                    }
                    else
                    {
                        // large mouth
                        if (prev_style == "large")
                        {
                            // numnum
                            File.Copy(filename_small, temp_filename);
                            prev_style = "small";
                            print_textbox("small(large)");
                        }
                        else
                        {
                            File.Copy(filename_large, temp_filename);
                            prev_style = "large";
                            print_textbox("large");
                        }
                    }
                    sample_count = 0;
                    signal_peak = 0;
                }
            }

            // encode with ffmpeg
            print_textbox("encoding with ffmpeg...");
            int frame_rate = (int)numericUpDownFramerate.Value;
            Bitmap bmp = new Bitmap(filename_close);
            int width = bmp.Width;
            int height = bmp.Height;
            bmp.Dispose();
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = textBoxFfmpeg.Text,
                    Arguments = string.Format("-loglevel warning " +
                                             "-y " +
                                             "-framerate {0} " +
                                             "-i \"{1}/%8d.png\" " +
                                             "-vframes {2} " +
                                             "-vf \"scale={3}:{4},format=yuv420p\" " +
                                             "-vcodec libx264 " +
                                             "-r {0} " +
                                             "\"{5}\"",
                                             frame_rate, temp_dir, frame_number, width, height, temp_mp4),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using (Process process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();
                    process.WaitForExit();

                    // 必要に応じて出力やエラーを取得
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(output))
                        print_textbox(output);
                    if (!string.IsNullOrEmpty(error))
                        print_textbox(error);
                }
            }


            // mux sound with ffmpeg
            print_textbox("muxing sound with ffmpeg...");
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = textBoxFfmpeg.Text,
                    Arguments = string.Format("-loglevel warning " +
                                             "-y " +
                                             "-i \"{0}\" " +
                                             "-i \"{1}\" " +
                                             "-c:v copy " +
                                             "-c:a aac " +
                                             "-map 0:v:0 " +
                                             "-map 1:a:0 " +
                                             "\"{2}\" ",
                                             temp_mp4, textBoxInputfile.Text, textBoxOutputfile.Text),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using (Process process = new Process())
                {
                    process.StartInfo = processInfo;
                    process.Start();
                    process.WaitForExit();

                    // 必要に応じて出力やエラーを取得
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(output))
                        print_textbox(output);
                    if (!string.IsNullOrEmpty(error))
                        print_textbox(error);
                }
            }

            // cleanup temp
            foreach (string filename in Directory.GetFiles(temp_dir, "*.png"))
            {
                File.Delete(filename);
            }
            if (File.Exists(temp_mp4))
            {
                File.Delete(temp_mp4);
            }
            print_textbox("OUTPUT: " + textBoxOutputfile.Text);
            print_textbox("finished !!");
            MessageBox.Show("Finished.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void print_textbox(string text, bool newline = true)
        {
            string crlf = newline ? Environment.NewLine : "";
            textBoxLog.AppendText(text + crlf);
            textBoxLog.SelectionStart = textBoxLog.Text.Length;
            textBoxLog.ScrollToCaret();
        }

        private bool ExistFileWithPathEnv(string filename)
        {
            string pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrEmpty(pathEnv))
            {
                return false;
            }

            string[] directories = pathEnv.Split(';');
            foreach (string dir in directories)
            {
                if (string.IsNullOrWhiteSpace(dir)) continue;
                try
                {
                    string fullPath = Path.Combine(dir, filename);
                    if (File.Exists(fullPath))
                    {
                        return true;
                    }
                }
                catch 
                {
                    // error 
                    return false;
                }
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(param_json_name))
            {
                using (var ms = new FileStream(param_json_name, FileMode.Open))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Params));
                    var p = (Params)serializer.ReadObject(ms);
                    textBoxImagedir.Text = p.image_dir;
                    textBoxInputfile.Text = p.input_filename;
                    textBoxOutputfile.Text = p.output_filename;
                    textBoxFfmpeg.Text = p.ffmpeg;
                    numericUpDownFramerate.Value = p.frame_rate;
                    numericUpDownSmallThreshold.Value = p.small_threshold;
                    numericUpDownLargeThreshold.Value = p.large_threshold;
                    numericUpDownBlinkInterval.Value = p.blink_interval;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Params p = new Params();
            p.image_dir = textBoxImagedir.Text;
            p.input_filename = textBoxInputfile.Text;
            p.output_filename = textBoxOutputfile.Text;
            p.ffmpeg = textBoxFfmpeg.Text;
            p.frame_rate = (int)numericUpDownFramerate.Value;
            p.small_threshold = (int)numericUpDownSmallThreshold.Value;
            p.large_threshold = (int)numericUpDownLargeThreshold.Value;
            p.blink_interval = (int)numericUpDownBlinkInterval.Value;

            using (var stream = new MemoryStream())
            using (var fs = new FileStream(param_json_name, FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                var serializer = new DataContractJsonSerializer(typeof(Params));
                serializer.WriteObject(stream, p);
                var str2write = Encoding.UTF8.GetString(stream.ToArray());
                sw.Write(str2write);
            }
        }

        private void buttonInputfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.Filter = "Wav file(*.wav)|*.wav";
            ofd.FilterIndex = 1;
            ofd.Title = "Select input wav filename.";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxInputfile.Text = ofd.FileName;
                string new_filename = Path.GetDirectoryName(ofd.FileName) + @"\" + Path.GetFileNameWithoutExtension(ofd.FileName) + ".mp4";
                if (MessageBox.Show("Set output filename automatic?\n" + new_filename, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    textBoxOutputfile.Text = new_filename;
                }
            }
        }

        private void buttonOutputfile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "";
            sfd.Filter = "MP4 file(*.mp4)|*.mp4";
            sfd.FilterIndex = 1;
            sfd.Title = "Set output filename";
            sfd.RestoreDirectory = true;
            sfd.OverwritePrompt = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                textBoxOutputfile.Text = sfd.FileName;
            }
        }

        private void buttonFfmpeg_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "ffmpeg.exe";
            ofd.Filter = "ffmpeg.exe|ffmpeg.exe";
            ofd.FilterIndex = 1;
            ofd.Title = "Select installed ffmpeg.exe";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxFfmpeg.Text = ofd.FileName;
            }
        }

        private void buttonImageDir_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.Filter = "face_1.png|face_1.png";
            ofd.FilterIndex = 1;
            ofd.Title = "Select face_1.png in image folder.";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxImagedir.Text = Path.GetDirectoryName(ofd.FileName);
            }
        }

        [DataContract]
        public class Params
        {
            [DataMember]
            public string image_dir { get; set; }

            [DataMember]
            public string input_filename { get; set; }
            [DataMember]
            public string output_filename { get; set; }
            [DataMember]
            public string ffmpeg { get; set; }

            [DataMember]
            public int frame_rate { get; set; }

            [DataMember]
            public int small_threshold { get; set; }

            [DataMember]
            public int large_threshold { get; set; }

            [DataMember]
            public int blink_interval { get; set; }

        }
    }
}