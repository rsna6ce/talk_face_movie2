using System;
using System.IO;
using System.Windows;

// This sentence has been quoted from 
// https://www.peliphilo.net/archives/1545

namespace WaveReadSample
{
    /// <summary>WAVE ヘッダ情報構造体</summary>
    public struct WaveHeaderArgs
    {
        /// <summary>RIFF ヘッダ</summary>
        public string RiffHeader;

        /// <summary>ファイルサイズ</summary>
        public int FileSize;

        /// <summary>WAVE ヘッダ</summary>
        public string WaveHeader;

        /// <summary>フォーマットチャンク</summary>
        public string FormatChunk;

        /// <summary>フォーマットチャンクサイズ</summary>
        public int FormatChunkSize;

        /// <summary>フォーマット ID</summary>
        public short FormatID;

        /// <summary>チャンネル数</summary>
        public short Channel;

        /// <summary>サンプリングレート</summary>
        public int SampleRate;

        /// <summary>1秒あたりのデータ数</summary>
        public int BytePerSec;

        /// <summary>ブロックサイズ</summary>
        public short BlockSize;

        /// <summary>1サンプルあたりのビット数</summary>
        public short BitPerSample;

        /// <summary>Data チャンク</summary>
        public string DataChunk;

        /// <summary>波形データのバイト数</summary>
        public int DataChunkSize;

        /// <summary>再生時間(msec)</summary>
        public int PlayTimeMsec;
    }

    /// <summary>
    /// WAVE 読み込みクラス
    /// </summary>
    public class WaveReadSample
    {
        /// <summary>WAVE ヘッダ情報</summary>
        private WaveHeaderArgs _waveHeaderArgs = new WaveHeaderArgs();

        public WaveHeaderArgs GetHeader()
        {
            return _waveHeaderArgs;
        }

        /// <summary>WAVE データ配列</summary>
        public Int16[] _waveData { get; private set; }

        /// <summary>WAVE 読み込み</summary>
        /// <param name="waveFilePath">Wave ファイルへのパス</param>
        /// <returns>読み込み結果</returns>
        /// <remarks>fmt チャンクおよび data チャンク以外は読み飛ばします</remarks>
        public bool ReadWave(string waveFilePath)
        {
            // ファイルの存在を確認する
            if (File.Exists(waveFilePath) == false)
            {
                return false;
            }

            using (FileStream fs = new FileStream(waveFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    var br = new BinaryReader(fs);
                    _waveHeaderArgs.RiffHeader = System.Text.Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));
                    _waveHeaderArgs.FileSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
                    _waveHeaderArgs.WaveHeader = System.Text.Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));

                    var readFmtChunk = false;
                    var readDataChunk = false;
                    while (readFmtChunk == false || readDataChunk == false)
                    {
                        // ChunkIDを取得する
                        var chunk = System.Text.Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));
                        if (chunk.ToLower().CompareTo("fmt ") == 0)
                        {
                            // fmtチャンクの読み込み
                            _waveHeaderArgs.FormatChunk = chunk;
                            _waveHeaderArgs.FormatChunkSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
                            _waveHeaderArgs.FormatID = BitConverter.ToInt16(br.ReadBytes(2), 0);
                            _waveHeaderArgs.Channel = BitConverter.ToInt16(br.ReadBytes(2), 0);
                            _waveHeaderArgs.SampleRate = BitConverter.ToInt32(br.ReadBytes(4), 0);
                            _waveHeaderArgs.BytePerSec = BitConverter.ToInt32(br.ReadBytes(4), 0);
                            _waveHeaderArgs.BlockSize = BitConverter.ToInt16(br.ReadBytes(2), 0);
                            _waveHeaderArgs.BitPerSample = BitConverter.ToInt16(br.ReadBytes(2), 0);
                            readFmtChunk = true;
                        }
                        else if (chunk.ToLower().CompareTo("data") == 0)
                        {
                            // dataチャンクの読み込み
                            _waveHeaderArgs.DataChunk = chunk;
                            _waveHeaderArgs.DataChunkSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
                            byte[] b = br.ReadBytes(_waveHeaderArgs.DataChunkSize);

                            // バッファに読み込み
                            // Note: L/Rに分けたい場合にはこの辺で分割する
                            _waveData = new Int16[_waveHeaderArgs.DataChunkSize / 2];
                            var insertIndex = 0;
                            for (int i = 0; i < b.Length; i += 2)
                            {
                                _waveData[insertIndex] = BitConverter.ToInt16(b, i);
                                ++insertIndex;
                            }

                            // 再生時間を算出する
                            // Note: 使うことが多いのでついでに算出しておく
                            var bytesPerSec = _waveHeaderArgs.SampleRate * _waveHeaderArgs.Channel * _waveHeaderArgs.BlockSize;
                            _waveHeaderArgs.PlayTimeMsec = (int)(((double)_waveHeaderArgs.DataChunkSize / (double)bytesPerSec) * 1000);
                            readDataChunk = true;
                        }
                        else
                        {
                            // 不要なチャンクの読み捨て
                            Int32 size = BitConverter.ToInt32(br.ReadBytes(4), 0);
                            if (0 < size)
                            {
                                br.ReadBytes(size);
                            }
                        }
                    }
                }
                catch
                {
                    fs.Close();
                    return false;
                }
            }

            return true;
        }
    }
}