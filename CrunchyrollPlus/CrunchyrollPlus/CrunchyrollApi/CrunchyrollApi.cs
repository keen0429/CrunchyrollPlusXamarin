using System;
using System.Net.Http;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
namespace CrunchyrollPlus
{
    class CrunchyrollApi
    {
        public HttpClient crunchyClient;
        public string sessionId;
        private CrunchyrollApi()
        {
            crunchyClient = new HttpClient();
            crunchyClient.BaseAddress = new Uri("https://api.crunchyroll.com");
        }
        private static readonly CrunchyrollApi singleton = new CrunchyrollApi();
        public static CrunchyrollApi GetSingleton()
        {
            return singleton;
        }

        public struct SessionResponse
        {
            public bool success;
            public bool authed;
            public SessionResponse(bool success, bool authed)
            {
                this.success = success;
                this.authed=authed;
            }
        }  


        /// <summary>
        /// Starts a session with the crunchyroll api
        /// </summary>
        /// <returns>If request is successful </returns>
        public Task<SessionResponse> StartSession()
        {
            
            return Task.Run(async () => {
                const string deviceType = "com.crunchyroll.windows.desktop";
                string url = $"/start_session.0.json?access_token=LNDJgOit5yaRIWN&device_id={Guid.NewGuid()}&device_type={deviceType}";
                if (Application.Current.Properties.ContainsKey("auth"))
                {
                    url += "&auth=" + Application.Current.Properties["auth"].ToString();

                }
                HttpResponseMessage res = await crunchyClient.PostAsync(url, null);
                if (res.IsSuccessStatusCode)
                {
                    string resData = await res.Content.ReadAsStringAsync();
                    JObject o = JObject.Parse(resData);
                    if ((bool)o["error"])
                    {

                    }
                    else
                    {
                        JObject data = (JObject)o["data"];
                        if (data["auth"] == null)
                        {
                            return new SessionResponse(true, false);
                        }
                        else
                        {
                            return new SessionResponse(true, true);

                        }
                    }
                }
                return new SessionResponse(false, false);


            });
            
        }
        
        public struct LoginResponse
        {
            bool success;
            string message;
            public LoginResponse(bool succes, string message)
            {
                this.success = succes;
                this.message = message;
            }
        }
        public Task<LoginResponse> Login(string username, string password)
        {
            return Task.Run(async () => {

                HttpResponseMessage httpResponseMessage = await crunchyClient.PostAsync(GetPath("login", $"&account={username}&password={password}"), null);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string res = await httpResponseMessage.Content.ReadAsStringAsync();
                    JObject o = JObject.Parse(res);
                    if ((bool)o["error"])
                    {
                        return new LoginResponse(false, (string)o["message"]);
                    }
                    else
                    {
                        JObject data = (JObject)o["data"];
                        JObject user = (JObject)data["user"];
                        Application.Current.Properties["auth"] = data["auth"];
                        return new LoginResponse(true, "");

                    }
                }
                return new LoginResponse(true, "HE");
            });
        }
        
        
        public struct QueueEntry
        {
            int playhead;
            Media mostLikely;
            Series series;
        }

        public string GetPath(string req, string data)
        {
            return $"/{req}.0.json?session_id={sessionId}{data}";
        }

        
    }
}
