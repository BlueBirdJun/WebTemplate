using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WebTemplate.Common.Enums;
using WebTemplate.Common.Helpers;
using WebTemplate.Common.Models;

namespace WebTemplate.Common.Services
{
    public class WebProxyService<T> : IWebProxyService<T>
    {

        private readonly TimeSpan _timeout;
        private HttpClient _httpClient;
        private HttpClientHandler _httpClientHandler;
        private const string ClientUserAgent = "fxgear-client-v0.93";
        public string MediaTypeXml = "application/xml";

        public string SendValue { get; set; }
        public string CallPath { get; set; }
        public HttpMethods HttpMethodValue { get; set; }
        public bool XmlYN { get; set; }
        public Dictionary<string, string> headervalue = new Dictionary<string, string>();

        #region constuctor
        public WebProxyService()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _timeout = new TimeSpan(0, 0, 30);
        }
        #endregion
        private Uri GetUrl()
        {
            Uri sendurl = new Uri(this.CallPath);
            return sendurl;
        }


        public async Task<ApiEntityModel<T>> AsyncCallData()
        {
            ApiEntityModel<T> _data = new ApiEntityModel<T>();
            var endcoingCode = 51949;
            System.Text.Encoding euckr = System.Text.Encoding.GetEncoding(endcoingCode);
            try
            {
                EnsureHttpClientCreated();
                switch (this.HttpMethodValue)
                {
                    case HttpMethods.GET:
                        using (var response = await _httpClient.GetAsync(GetUrl()))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                byte[] data = await response.Content.ReadAsByteArrayAsync();

                                string strrsp = euckr.GetString(data);
                                if (this.XmlYN)
                                {
                                    var stringReader = new System.IO.StringReader(strrsp);
                                    var serializer = new XmlSerializer(typeof(T));
                                    var atr = (T)serializer.Deserialize(stringReader);// as T;                                     
                                    _data.Success = true;
                                    _data.OutValue = atr;
                                }
                                else
                                {
                                    using (TextReader sr = new StringReader(strrsp))
                                    {
                                        var xmlserial = new System.Xml.Serialization.XmlSerializer(typeof(T));
                                        T rtt = (T)xmlserial.Deserialize(sr);
                                        _data.Success = true;
                                        _data.OutValue = rtt;
                                    }
                                }
                            }
                            else
                            {
                                _data.Success = false;
                                _data.Message = $"{response.StatusCode}/{response.ReasonPhrase}";
                            }
                        }
                        break;
                    case HttpMethods.POST:
                        using (var response = await _httpClient.SendAsync(GetRequestMessage()))
                        {
                            _data.HttpCode = response.StatusCode.ToString();
                            if (response.IsSuccessStatusCode)
                            {
                                _data.Success = true;
                                var rt = await response.Content.ReadAsStringAsync();
                                if (XmlYN)
                                    _data.StringOutValue = rt.ToString();
                                else
                                    _data.OutValue = JsonConvert.DeserializeObject<T>(rt);
                            }
                            else if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                            {
                                _data.Success = false;
                                var rt = await response.Content.ReadAsStringAsync();
                                _data.Message = $"{response.RequestMessage.RequestUri}/{response.StatusCode}/{rt}";

                            }
                            else
                            {
                                _data.Success = false;
                                _data.Success = false;
                                _data.Message = $"{response.RequestMessage.RequestUri}/{response.StatusCode}/{response.ReasonPhrase}";
                            }
                        }
                        break;
                    case HttpMethods.PUT:
                        using (var requestContent = new StringContent(SendValue, Encoding.UTF8, MediaTypeXml))
                        {
                            using (var response = await _httpClient.PutAsync(GetUrl(), requestContent))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    byte[] data = await response.Content.ReadAsByteArrayAsync();

                                    string strrsp = euckr.GetString(data);
                                    if (this.XmlYN)
                                    {
                                        var stringReader = new System.IO.StringReader(strrsp);
                                        var serializer = new XmlSerializer(typeof(T));
                                        var atr = (T)serializer.Deserialize(stringReader);// as T;                                     
                                        _data.Success = true;
                                        _data.OutValue = atr;
                                    }
                                    else
                                    {
                                        using (TextReader sr = new StringReader(strrsp))
                                        {
                                            var xmlserial = new System.Xml.Serialization.XmlSerializer(typeof(T));
                                            T rtt = (T)xmlserial.Deserialize(sr);
                                            _data.Success = true;
                                            _data.OutValue = rtt;
                                        }
                                    }

                                }
                                else
                                {
                                    _data.Success = false;
                                    _data.Message = $"{response.StatusCode}/{response.ReasonPhrase}";
                                }
                            }
                        }
                        break;
                    case HttpMethods.DELETE:

                        break;
                    case HttpMethods.DEBUG:

                        break;
                }
            }
            catch (WebException wxc)
            {
                _data.HasError = true;
                _data.Success = false;
                _data.Message = wxc.Message;
            }
            catch (Exception exc)
            {
                _data.HasError = true;
                _data.Success = false;
                if (exc.Message == "The operation was canceled.")
                    _data.Message = "Time Out";
                else
                    _data.Message = exc.Message;
            }
            finally
            {

            }
            return _data;
        }

        public void Dispose()
        {
            if (_httpClientHandler != null)
                _httpClientHandler.Dispose();
            if (_httpClient != null)
                _httpClient.Dispose();
        }

        private static string ConvertToJsonString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private HttpRequestMessage GetRequestMessage()
        {
            string content = string.Empty;
            if (!this.SendValue.IsNullOrEmpty())
                content = SendValue;
            Uri sendurl = new Uri(this.CallPath);
            var uriBuilder = new UriBuilder(sendurl);
            Uri finalUrl = uriBuilder.Uri;
            var request = new HttpRequestMessage()
            {
                RequestUri = finalUrl,
                Method = new HttpMethod(this.HttpMethodValue.ToString())
            };

            if (content.Length > 0)
            {
                request.Content = new StringContent(content,
                                    Encoding.UTF8,
                                    "application/json");
            }
            return request;
        }

        private void EnsureHttpClientCreated()
        {
            if (_httpClient == null)
                CreateHttpClient();
        }
        private void CreateHttpClient()
        {
            _httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };
            _httpClient = new HttpClient(_httpClientHandler, false)
            {
                Timeout = _timeout
            };
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(ClientUserAgent);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            //_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeXml));
            if (headervalue.Count > 0)
            {
                foreach (var h in headervalue)
                {
                    _httpClient.DefaultRequestHeaders.Add(h.Key, h.Value);
                }
            }

        }
    }
}
