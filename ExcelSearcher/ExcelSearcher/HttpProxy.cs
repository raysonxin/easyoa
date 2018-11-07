using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExcelSearcher
{
    public class HttpProxy
    {
        private string baseUri = string.Empty;

        //private static DataService instance;
        //public static DataService Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new DataService(baseUrl);
        //        }
        //        return instance;
        //    }
        //}

        /// <summary>
        /// 基础地址
        /// </summary>
        public string BaseURI
        {
            get
            {
                return baseUri;
            }
        }

        /// <summary>
        /// 身份认证token
        /// </summary>
        public string AccessToken { get; set; }

        public HttpProxy(string uri, string token = null)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("Arguement uri can not be null.");
            }

            this.baseUri = uri.EndsWith("/") ? uri : uri + "/";
            AccessToken = token;
        }

        /// <summary>
        /// 更新接口调用
        /// </summary>
        /// <param name="url">接口名称</param>
        /// <param name="message">更新对象</param>
        /// <returns></returns>
        public async Task<HttpResult<T>> PutMessage<T>(string url, Dictionary<string, string> message, int timeout = 10)
        {
            Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func = async (client) =>
            {
                HttpContent requestContent = null;
                requestContent = new FormUrlEncodedContent((Dictionary<string, string>)message);
                return await client.PutAsync(CheckUrl(url), requestContent);
            };
            return await Request<T>(func, timeout);
        }

        /// <summary>
        /// 更新接口调用,参数会以json形式发送
        /// </summary>
        /// <param name="url">接口名称</param>
        /// <param name="message">更新对象</param>
        /// <returns></returns>
        public async Task<HttpResult<T>> PutMessage<T>(string url, object message, int timeout = 10)
        {
            Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func = async (client) =>
            {
                HttpContent requestContent = null;
                requestContent = new StringContent(JsonConvert.SerializeObject(message));
                return await client.PutAsync(CheckUrl(url), requestContent);
            };

            return await Request<T>(func, timeout);
        }

        /// <summary>
        /// 删除接口
        /// </summary>
        /// <param name="url">接口名称</param>
        /// <param name="message">删除条件</param>
        /// <returns></returns>
        public async Task<HttpResult<T>> RemoveMessage<T>(string url, object message, int timeout = 10)
        {
            Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func = async (client) =>
            {
                if (message != null)
                {
                    var messageString = "?";
                    var type = message.GetType();
                    var properties = type.GetProperties();
                    foreach (var item in properties)
                    {
                        messageString += item.Name.ToLower() + "=" + item.GetValue(message) + "&";
                    }
                    url += messageString;
                }
                return await client.DeleteAsync(CheckUrl(url));
            };
            return await Request<T>(func, timeout);
        }

        /// <summary>
        /// 新增数据接口调用
        /// </summary>
        /// <param name="url">接口名称</param>
        /// <param name="message">新增对象</param>
        /// <returns></returns>
        public async Task<HttpResult<T>> PostMessage<T>(string url, object message, int timeout = 10)
        {
            Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func = async (client) =>
            {
                var sendContent = new StringContent(JsonConvert.SerializeObject(message));
                return await client.PostAsync(CheckUrl(url), sendContent);
            };
            return await Request<T>(func, timeout);
        }

        /// <summary>
        /// 新增对象,使用自定义的HttpContent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResult<T>> PostMessage<T>(string url, HttpContent message, int timeout = 10)
        {
            Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func = async (client) =>
            {
                return await client.PostAsync(CheckUrl(url), message);
            };
            return await Request<T>(func, timeout);
        }

        public async Task<HttpResult<T>> PostFileMessage<T>(string url, string localfile, string savefile, object message, int timeout = 10)
        {
            Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func = async (client) =>
            {
                var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                content.Add(new StreamContent(File.Open(localfile, FileMode.Open, FileAccess.Read, FileShare.Read)), "filename", savefile);
                content.Add(new StringContent(JsonConvert.SerializeObject(message)), "data");
                return await client.PostAsync(CheckUrl(url), content);
            };
            return await Request<T>(func, timeout);
        }

        /// <summary>
        /// 查询接口调用
        /// </summary>
        /// <param name="url">接口名称</param>
        /// <param name="message">查询条件，默认为空</param>
        /// <returns></returns>
        public async Task<HttpResult<T>> GetMessage<T>(string url, object message = null, int timeout = 10)
        {
            Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func = async (client) =>
            {
                if (message != null)
                {
                    var messageString = "?";
                    var type = message.GetType();
                    var properties = type.GetProperties();
                    foreach (var item in properties)
                    {

                        messageString += item.Name.ToLower() + "=" + item.GetValue(message) + "&";
                    }
                    url += messageString;
                }
                return await client.GetAsync(CheckUrl(url));
            };
            return await Request<T>(func, timeout);
        }

        private string CheckUrl(string url)
        {
            if (url[0] == '/')
            {
                return url.Substring(1);
            }
            return url.ToLower();
        }

        private async Task<HttpResult<T>> Request<T>(Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func, int timeout = 10)
        {
            HttpResult<T> result = new HttpResult<T>();
            result.HttpStatusCode = System.Net.HttpStatusCode.OK;
            try
            {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.Timeout = new TimeSpan(0, 0, timeout);
                    client.BaseAddress = new Uri(baseUri, UriKind.Absolute);
                    if (!string.IsNullOrWhiteSpace(AccessToken))
                    {
                        client.DefaultRequestHeaders.Add("QSC-Token", AccessToken);
                    }

                    var response = await func.Invoke(client);
                    var datas = await response.Content.ReadAsByteArrayAsync();
                    result.HttpStatusCode = response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            if (datas.Length == 0)
                            {
                                result.Content = (T)Convert.ChangeType(true, typeof(T));
                            }
                            else
                            {
                                result.Content = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(datas), LocalJsonSerializerSetting);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Error = string.Format("Deserialize Result Object Error,Exception:{0},Origin-Data:{1}", ex.Message, System.Text.Encoding.UTF8.GetString(datas));
                            result.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        result.Error = JsonConvert.DeserializeObject<ErrorData>(Encoding.UTF8.GetString(datas)).Error;
                    }
                }
            }
            catch (HttpRequestException)
            {
                result.Error = "Server Not Avaliable";
                result.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
            }
            return result;
        }


        private static JsonSerializerSettings localJsonSerializerSetting;
        /// <summary>
        /// 本地json序列化库，主要用于时区问题
        /// </summary>
        public static JsonSerializerSettings LocalJsonSerializerSetting
        {
            get
            {
                if (localJsonSerializerSetting == null)
                {
                    localJsonSerializerSetting = new JsonSerializerSettings();
                    localJsonSerializerSetting.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    localJsonSerializerSetting.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
                    localJsonSerializerSetting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                }
                return localJsonSerializerSetting;
            }
        }
    }
}
