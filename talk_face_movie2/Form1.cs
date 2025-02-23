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

using WaveRead;

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
            // 入力ファイルチェック
            if (File.Exists(textBoxInputfile.Text) == false)
            {
                MessageBox.Show("入力ファイルが見つかりません。\n\n" + textBoxInputfile.Text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 出力ファイルチェック
            if (textBoxOutputfile.Text == "" )
            {
                MessageBox.Show("出力ファイルを設定してください。", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //出力フォルダを作っておく
            string output_dir = Path.GetDirectoryName(textBoxOutputfile.Text);
            Directory.CreateDirectory(output_dir);
            // 画像フォルダチェック
            string exe_dir = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
            string image_dir = textBoxImagedir.Text;
            if (image_dir.IndexOf(":") < 0)
            {
                image_dir = exe_dir + @"\" + image_dir;
            }
            if (Directory.Exists(image_dir) == false)
            {
                MessageBox.Show("顔画像フォルダが見つかりません。\n\n" + textBoxImagedir.Text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 1; i <= 4; i++)
            {
                string filename = image_dir + @"\" + "face_" + i.ToString() + ".png";
                if (File.Exists(filename) == false)
                {
                    MessageBox.Show("顔画像ファイルが見つかりません。\n\n" + filename, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // ffmpegチェック
            if (File.Exists(textBoxFfmpeg.Text) == false)
            {
                if (ExistFileWithPathEnv(textBoxFfmpeg.Text) == false)
                {
                    MessageBox.Show("ffmpegが見つかりません。\n\n" + textBoxFfmpeg.Text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //入力ファイルがmp3の場合、中間wavファイルに変換する
            string filename_input = textBoxInputfile.Text;
            string filename_temp_wav = "";
            if (Path.GetExtension(filename_input).ToString().ToLower() == ".mp3")
            {
                // ffmpeg -i "input.mp3" -vn -ac 2 -ar 44100 -acodec pcm_s16le -f wav "output.wav"
                print_textbox("converting from mp3 to wav ...");
                filename_temp_wav = Path.Combine(exe_dir, "temp_mp3_to_wav.wav");
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = textBoxFfmpeg.Text,
                    Arguments = string.Format("-loglevel warning " +
                                             "-y " +
                                             "-i \"{0}\" " +
                                             "-vn -ac 1 -ar 24000 " +
                                             "-vcodec libx264 " +
                                             "-acodec pcm_s16le " +
                                             "-f wav " +
                                             "\"{1}\"",
                                             filename_input, filename_temp_wav),
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

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(output))
                        print_textbox(output);
                    if (!string.IsNullOrEmpty(error))
                        print_textbox(error);
                }
                // input file replace from mp3 to wav
                filename_input = filename_temp_wav;
            }
            else if (Path.GetExtension(filename_input).ToString().ToLower() != ".wav")
            {
                MessageBox.Show("入力されたファイル形式はサポートしていません。\n\n" + textBoxInputfile.Text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // clean up temp dir
            string temp_dir = Path.Combine(exe_dir, "temp");
            Directory.CreateDirectory(temp_dir); // うっかりtempフォルダ消してしまった時のリカバリ
            foreach (string filename in Directory.GetFiles(temp_dir, "*.png"))
            {
                File.Delete(filename);
            }

            string temp_mp4 = Path.Combine(exe_dir, "_temp_without_sound.mp4");
            string filename_close = Path.Combine(image_dir, "face_1.png");
            string filename_small = Path.Combine(image_dir, "face_2.png");
            string filename_large = Path.Combine(image_dir, "face_3.png");
            string filename_blink = Path.Combine(image_dir, "face_4.png");

            WavFileReader wavReader = new WavFileReader();
            try
            {
                wavReader.ReadWavFile(filename_input);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wavファイル読み込みに失敗しました。", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                print_textbox(ex.Message);
                return;
            }

            int data_length = wavReader.WaveformData.Length;
            int sample_rate = (int)wavReader.Header.SampleRate;
            int sample_interval = sample_rate / (int)numericUpDownFramerate.Value;
            int sample_count = 0;
            double signal_peak = 0;
            int latest_blink = 0;
            int frame_number = 0;
            string prev_style = "";
            int blink_interval = (int)numericUpDownBlinkInterval.Value;
            int progress_percent = 0;

            double small_threshold = (double)numericUpDownSmallThreshold.Value / 100.0;
            double large_threshold = (double)numericUpDownLargeThreshold.Value / 100.0;

            for (int i = 0; i < data_length; i++)
            {
                progress_percent = (int)(i * 80.0 / data_length);
                SetProgressbar(progress_percent);

                int value = (int)wavReader.WaveformData[i];
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

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(output))
                        print_textbox(output);
                    if (!string.IsNullOrEmpty(error))
                        print_textbox(error);
                }
            }
            SetProgressbar(90);

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
                                             temp_mp4, filename_input, textBoxOutputfile.Text),
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

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(output))
                        print_textbox(output);
                    if (!string.IsNullOrEmpty(error))
                        print_textbox(error);
                }
            }
            SetProgressbar(95);

            // cleanup temp
            foreach (string filename in Directory.GetFiles(temp_dir, "*.png"))
            {
                File.Delete(filename);
            }
            if (File.Exists(temp_mp4))
            {
                File.Delete(temp_mp4);
            }
            if (filename_temp_wav != "" && File.Exists(filename_temp_wav))
            {
                File.Delete(filename_temp_wav);
            }
            print_textbox("OUTPUT: " + textBoxOutputfile.Text);
            print_textbox("\nfinished !!");
            SetProgressbar(100);
            var ret = MessageBox.Show("変換完了！！\n\n出力ファイルのフォルダを開きますか？", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (ret == DialogResult.Yes)
            {
                Process.Start("explorer.exe", output_dir);
            }
            SetProgressbar(0);
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

        private void SetProgressbar(int percent)
        {
            int width = Math.Min(buttonRun.Width, buttonRun.Width * percent / 100);
            if (labelProgressbar.Width != width)
            {
                labelProgressbar.Width = width;
                labelProgressbar.Refresh();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SetProgressbar(0);
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
            string exe_dir = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.InitialDirectory = exe_dir;
            if (textBoxInputfile.Text != "" && File.Exists(textBoxInputfile.Text))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(textBoxInputfile.Text);
            }
            ofd.Filter = "Sound file(*.wav;*.mp3)|*.wav;*.mp3";
            ofd.FilterIndex = 1;
            ofd.Title = "入力ファイル(wav/mp3)を選択してください。";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxInputfile.Text = ofd.FileName;
                string new_filename = Path.GetDirectoryName(ofd.FileName) + @"\" + Path.GetFileNameWithoutExtension(ofd.FileName) + ".mp4";
                if (MessageBox.Show("出力ファイル名を自動的に設定しますか?\n" + new_filename, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    textBoxOutputfile.Text = new_filename;
                }
            }
        }

        private void buttonOutputfile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "";
            if (textBoxOutputfile.Text != "" && File.Exists(textBoxOutputfile.Text))
            {
                sfd.InitialDirectory = Path.GetDirectoryName(textBoxInputfile.Text);
            }
            sfd.Filter = "MP4 file(*.mp4)|*.mp4";
            sfd.FilterIndex = 1;
            sfd.Title = "出力ファイル名(mp4)を設定してください。";
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
            ofd.Title = "インストールされたffmpeg.exeを設定してください。";
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
            if (textBoxImagedir.Text == "image\\")
            {
                string exe_dir = Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString();
                ofd.InitialDirectory = Path.Combine(exe_dir, "image");
            }
            else if (textBoxImagedir.Text != "" && Directory.Exists(textBoxImagedir.Text))
            {
                ofd.InitialDirectory = Path.GetDirectoryName(textBoxImagedir.Text);
            }
            ofd.FileName = "";
            ofd.Filter = "face_1.png|face_1.png";
            ofd.FilterIndex = 1;
            ofd.Title = "顔画像フォルダのface_1.pngを指定してください。";
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