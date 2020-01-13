using System;
using System.Net.Http;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Linq;
namespace CrunchyrollPlus
{
    public class CrunchyrollApi
    {
        public HttpClient crunchyClient;
        public string sessionId="";
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
        #region Session
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
                        sessionId = (string)data["session_id"];
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
        #endregion
        #region Login
        public struct LoginResponse
        {
            public bool success;
            public string message;
            public LoginResponse(bool success, string message)
            {
                this.success = success;
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
        #endregion
        #region Queue
        public struct QueueEntry
        {
            public Media mostLikely;
            public Series series;

            /// <summary>
            /// Successful request
            /// </summary>
            /// <param name="success"></param>
            /// <param name="playhead"></param>
            /// <param name="mostLikely"></param>
            /// <param name="series"></param>
            public QueueEntry( Media mostLikely, Series series)
            {
                this.mostLikely = mostLikely;
                this.series = series;
            }
            
        }
        public struct GetQueueResponse
        {
            public QueueEntry[] entry;
            public bool success;
            public string message;

            /// <summary>
            /// Unsucceful
            /// </summary>
            /// <param name="success"></param>
            /// <param name="message"></param>
            public GetQueueResponse( bool success, string message)
            {
                entry = new QueueEntry[0];
                this.success = success;
                this.message = message;
            }
            public GetQueueResponse(QueueEntry[] queueEntries)
            {
                success = true;
                message = "";
                this.entry = queueEntries;
            }
        }
        public Task<GetQueueResponse> GetQueue()
        {
            return Task.Run(async () =>
            {
                HttpResponseMessage res = await crunchyClient.PostAsync(GetPath("queue", ""), null);
                if (res.IsSuccessStatusCode)
                {
                    string jsonString = await res.Content.ReadAsStringAsync();
                    JObject o = JObject.Parse(jsonString);
                    if ((bool)o["error"])
                    {
                        return new GetQueueResponse(false, (string)o["message"]);
                    }
                    else
                    {
                        JArray data = (JArray)o["data"];
                        QueueEntry[] a = data.Select(i => {
                            JObject mostLike = (JObject)i["most_likely_media"];
                            //Media mostLikely = new Media(mostLike["media_id"].ToString(), (string)mostLike["name"], (string)mostLike["description"],
                            //    (string)mostLike["screenshot_image"]["large_url"], (bool)mostLike["free_available"], (bool)mostLike["premium_available"], (int)i["playhead"]);
                            Media mostLikely = new Media(mostLike);
                            mostLikely.playhead = (int)i["playhead"];
                            JObject s = (JObject)i["series"];
                            Series series = new Series(s);
                            return new QueueEntry(mostLikely, series);
                        }).ToArray();
                        return new GetQueueResponse(a);
                    }

                }
                return new GetQueueResponse(false, "Unknown error");
                

            });
        }
        #endregion
        #region StreamData
        struct StreamDataResponse
        {
            bool success;
            string message;
            string url;
            int playhead;

            /// <summary>
            /// Unsuccessful response
            /// </summary>
            /// <param name="message"></param>
            public StreamDataResponse(string message)
            {
                this.message = message;
                this.success = false;
                url = "";
                playhead = 0;
            }
            /// <summary>
            /// Successful response
            /// </summary>
            /// <param name="url"></param>
            public StreamDataResponse(string url, int playhead)
            {
                this.url = url;
                success = true;
                message = "";
                this.playhead = playhead;
            }

            
        }

        Task<StreamDataResponse> GetStreamData(string mediaId)
        {
            return Task.Run(async () =>
            {
                HttpResponseMessage res = await crunchyClient.PostAsync(GetPath("info", $"&media_id={mediaId}&fields=media.stream_data,media.playhead"), null);
                if (res.IsSuccessStatusCode)
                {
                    string jsonString = await res.Content.ReadAsStringAsync();
                    JObject o = (JObject)JObject.Parse(jsonString);
                    if ((bool)o["error"])
                    {
                        return new StreamDataResponse((string)o["message"]);
                    }
                    else
                    {
                        return new StreamDataResponse((string)o["data"]["stream_data"]["streams"][0]["url"], (int)o["data"]["playhead"]);
                    }
                }

                return new StreamDataResponse("Unknown Error");
            });
        }
        #endregion
        #region GetCollection
        struct GetCollectionsResponse
        {
            Collection[] collections;
            bool success;
            string message;

            public GetCollectionsResponse(Collection[] collections)
            {
                this.collections = collections;
                success = true;
                message = "";
            }
            public GetCollectionsResponse(string message)
            {
                this.message = message;
                collections = new Collection[0];
                success = false;
            }
        }
        Task<GetCollectionsResponse> GetCollections(string id)
        {
            return Task.Run(async () =>
            {
            HttpResponseMessage res = await crunchyClient.PostAsync(GetPath("list_collections", $"series_id={id}"), null);
                if (res.IsSuccessStatusCode)
                {
                    JObject o = JObject.Parse(await res.Content.ReadAsStringAsync());
                    if ((bool)o["error"])
                    {
                        return new GetCollectionsResponse((string)o["message"]);
                    }
                    else
                    {
                        JArray a = (JArray)o["data"];
                        Collection[] collections = (Collection[])a.Select(i => new Collection((string)i["name"], (string)i["collection_id"]));

                        return new GetCollectionsResponse(collections);

                    }

                    
                }
                return new GetCollectionsResponse("Unknown error");

            });
        }
        #endregion
        #region Autocomplete
        public struct AutocompleteResponse
        {
            Series[] series;
            bool success;
            string message;
            public AutocompleteResponse(Series[] series)
            {
                this.series = series;
                success = true;
                message = "";
            }
            public AutocompleteResponse(string message)
            {
                this.message = message;
                success = false;
                series = null;
            }
            

        }
        public Task<AutocompleteResponse> Autocomplete(string query)
        {
            return Task.Run(async () =>
            {
                HttpResponseMessage res = await crunchyClient.PostAsync(GetPath("autocomplete", $"media_types=anime&q={query}"), null);
                if (res.IsSuccessStatusCode)
                {
                    JObject o = JObject.Parse(await res.Content.ReadAsStringAsync());
                    if ((bool)o["error"])
                    {
                        return new AutocompleteResponse((string)o["message"]);

                    }
                    else
                    {
                        JArray a = (JArray) o["data"];
                        Series[] series = a.Select(i => new Series((JObject)i)).ToArray();
                        return new AutocompleteResponse(series);
                    }
                }
                else
                {
                    return new AutocompleteResponse("Network problem");
                }
                
            });
        }
        #endregion
        public string GetPath(string req, string data)
        {
            return $"/{req}.0.json?session_id={sessionId}{data}";
        }

        
    }
}
