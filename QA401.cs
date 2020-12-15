using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace QA401H_TestApp
{
    static class QA401
    {
        static HttpClient Client = new HttpClient();
        static string RootUrl;

        static bool[] GenEnabled = new bool[2] { false, false };
        static double[] GenFreq = new double[2] { 1000, 1000 };
        static double[] GenAmp = new double[2] { -99, -99 };

        static int LastBufferSize = 0;

        static QA401()
        {
            //SetRootUrl("http://localhost:9401");
        }

        static public void Restart()
        {
            SetRootUrl(RootUrl);
        }

        static public void SetRootUrl(string rootUrl)
        {
            RootUrl = rootUrl;
            Client = new HttpClient
            {
                BaseAddress = new Uri(RootUrl)
            };
        }

        static public double Version
        {
            get
            {
                return Convert.ToDouble(GetSync("/Status/Version", "Value"));
            }
        }

        static public bool IsConnected
        {
            get
            {
                return Convert.ToBoolean(GetSync("/Status/Connection", "Value"));
            }
        }

        static public void SetDefaults()
        {
            PutSync("/Settings/Default");
        }

        static public int SampleRate
        {
            set
            {
                PutSync(string.Format("/Settings/SampleRate/{0}", value));
            }
        }

        static public bool RoundFrequencies
        {
            set
            {
                PutSync(string.Format("/Settings/RoundFrequencies/{0}", value ? "On" : "Off"));
            }
        }

        static public string WindowType
        {
            set
            {
                PutSync(string.Format("/Settings/Window/{0}", value));
            }
        }

        static public double Gen1Freq
        {
            set
            {
                GenFreq[0] = value;
                PutSync(string.Format("/Settings/AudioGen/Gen1/{0}/{1}/{2}", GenEnabled[0] ? "On" : "Off", GenFreq[0], GenAmp[0]));
            }
        }

        static public double Gen1Amp
        {
            set
            {
                GenAmp[0] = value;
                PutSync(string.Format("/Settings/AudioGen/Gen1/{0}/{1}/{2}", GenEnabled[0] ? "On" : "Off", GenFreq[0], GenAmp[0]));
            }
        }

        static public bool Gen1Enable
        {
            set
            {
                GenEnabled[0] = value;
                PutSync(string.Format("/Settings/AudioGen/Gen1/{0}/{1}/{2}", GenEnabled[0] ? "On" : "Off", GenFreq[0], GenAmp[0]));
            }
        }

        static public void SetGen1(double freq, double amp, bool enabled)
        {
            GenAmp[0] = amp; GenFreq[0] = freq; GenEnabled[0] = enabled;
            PutSync(string.Format("/Settings/AudioGen/Gen1/{0}/{1}/{2}", GenEnabled[0] ? "On" : "Off", GenFreq[0], GenAmp[0]));
        }

        static public double Gen2Freq
        {
            set
            {
                GenFreq[1] = value;
                PutSync(string.Format("/Settings/AudioGen/Gen2/{0}/{1}/{2}", GenEnabled[1]? "On" : "Off", GenFreq[1], GenAmp[1]));
            }
        }

        static public double Gen2Amp
        {

            set
            {
                GenAmp[1] = value;
                PutSync(string.Format("/Settings/AudioGen/Gen2/{0}/{1}/{2}", GenEnabled[1]?"On" :"Off", GenFreq[1], GenAmp[1]));
            }
        }

        static public bool Gen2Enable
        {
            set
            {
                GenEnabled[1] = value;
                PutSync(string.Format("/Settings/AudioGen/Gen2/{0}/{1}/{2}", GenEnabled[1]?"On":"Off", GenFreq[1], GenAmp[1]));
            }
        }

        static public void SetGen2(double freq, double amp, bool enabled)
        {
            GenAmp[1] = amp; GenFreq[1] = freq; GenEnabled[1] = enabled;
            PutSync(string.Format("/Settings/AudioGen/Gen2/{0}/{1}/{2}", GenEnabled[1] ? "On" : "Off", GenFreq[1], GenAmp[1]));
        }

        static public int BufferSize
        {
            set
            {
                LastBufferSize = value;
                PutSync(string.Format("/Settings/BufferSize/{0}", LastBufferSize));
            }

            get 
            {
                return LastBufferSize;
            }
        }

        static public double MaxInput
        {
            set
            {
                PutSync(string.Format("/Settings/Input/Max/{0}", value));
            }
        }


        static public void GetSnrDb(double fundFreq, double minFreq, double maxFreq, out double left, out double right)
        {
            Dictionary<string, object> d = GetSync(string.Format("/SnrDb/{0}/{1}/{2}", fundFreq, minFreq, maxFreq));
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetThdDb(double fundFreq, double maxFreq, out double left, out double right)
        {
            Dictionary<string, object> d = GetSync(string.Format("/ThdDb/{0}/{1}", fundFreq, maxFreq));
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetThdPct(double fundFreq, double maxFreq, out double left, out double right)
        {
            Dictionary<string, object> d = GetSync(string.Format("/ThdPct/{0}/{1}", fundFreq, maxFreq));
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetThdnDb(double fundFreq, double minFreq, double maxFreq, out double left, out double right)
        {
            Dictionary<string, object> d = GetSync(string.Format("/ThdnDb/{0}/{1}/{2}", fundFreq, minFreq, maxFreq));
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetThdnPct(double fundFreq, double minFreq, double maxFreq, out double left, out double right)
        {
            Dictionary<string, object> d = GetSync(string.Format("/ThdnPct/{0}/{1}/{2}", fundFreq, minFreq, maxFreq));
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetRmsDbv(double startFreq, double endFreq, out double left, out double right, bool aWeighting = false)
        {
            Dictionary<string, object> d = GetSync(string.Format("/RmsDbv/{0}{1}/{2}", aWeighting ? "AWeighting/" : "",startFreq, endFreq));
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetPeakDbv(double startFreq, double endFreq, out double left, out double right)
        {
            Dictionary<string, object> d = GetSync(string.Format("/PeakDbv/{0}/{1}", startFreq, endFreq));
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetPhaseDegrees(out double left, out double right)
        {
            Dictionary<string, object> d = GetSync("/Phase/Seconds");
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetPhaseSeconds(out double left, out double right)
        {
            Dictionary<string, object> d = GetSync("/Phase/Seconds");
            left = Convert.ToDouble(d["Left"]);
            right = Convert.ToDouble(d["Right"]);
        }

        static public void GetFrequencyData(out double[] left, out double[] right, out double dx)
        {
            Dictionary<string, object> d = GetSync("/Data/Freq");
            string sl = Convert.ToString(d["Left"]);
            string sr = Convert.ToString(d["Right"]);
            dx = Convert.ToDouble(d["Dx"]);

            left = GetDoubles(Convert.FromBase64String(sl));
            right = GetDoubles(Convert.FromBase64String(sr));
        }

        static public void GetTimeData(out double[] left, out double[] right, out double dx)
        {
            Dictionary<string, object> d = GetSync("/Data/Time");
            string sl = Convert.ToString(d["Left"]);
            string sr = Convert.ToString(d["Right"]);
            dx = Convert.ToDouble(d["Dx"]);

            left = GetDoubles(Convert.FromBase64String(sl));
            right = GetDoubles(Convert.FromBase64String(sr));
        }

        static double[] GetDoubles(byte[] vals)
        {
            double[] result = new double[vals.Length / 4];
            Buffer.BlockCopy(vals, 0, result, 0, result.Length);
            return result;
        }

 
        static public void DoAcquisition()
        {
            PostSync("/Acquisition");
        }

        static public void DoAcquisition(double[] left, double[] right)
        {
            string l = Convert.ToBase64String(GetBytes(left));
            string r = Convert.ToBase64String(GetBytes(right));

            string s = $"{{ \"Left\":\"{l}\", \"Right\":\"{r}\" }}";


            PostSync("/Acquisition", s);
        }

        static byte[] GetBytes(double[] vals)
        {
            var byteArray = new byte[vals.Length * sizeof(double)];
            Buffer.BlockCopy(vals, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        static public Bitmap GetGraph()
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(RootUrl + "/Graph/Frequency/In/0");
            Bitmap bitmap = new Bitmap(stream);
            stream.Flush();
            stream.Close();

            return bitmap;
        }

        /*******************************************************************/
        /*********************** HELPERS for REST **************************/
        /*******************************************************************/

        static private void PutSync(string url)
        {
            PutSync(url, "", 0);
        }

        /// <summary>
        /// Synchronous PUT. This will throw an exception of the PUT fails for some reason
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="value"></param>
        static private void PutSync(string url, string token, int value)
        {
            string json;

            if (token != "")
                json = string.Format("{{\"{0}\":{1}}}", token, value);
            else
                json = "{{}}";

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // We make the PutAsync synchronous via the .Result
            var response = Client.PutAsync(url, content).Result;

            // Throw an exception if not successful
            response.EnsureSuccessStatusCode();
            response.Dispose();
        }

        static private void PostSync(string url)
        {
            PostSync(url, "");
        }

        static private void PostSync(string url, string value)
        {
            StringContent content = new StringContent(value, Encoding.UTF8, "application/json");

            // We make the PutAsync synchronous via the .Result
            var response = Client.PostAsync(url, content).Result;

            // Throw an exception if not successful
            response.EnsureSuccessStatusCode();
            response.Dispose();
        }

        static private Dictionary<string, object> GetSync(string url)
        {
            string content;

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = Client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
            content = response.Content.ReadAsStringAsync().Result;
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            var result = jsSerializer.DeserializeObject(content);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict = (Dictionary<string, object>)result;
            response.Dispose();

            return dict;
        }

        static private string GetSync(string url, string token)
        { 
            Dictionary<string, object> dict = GetSync(url);
            return dict[token].ToString();
        }
    }
}
