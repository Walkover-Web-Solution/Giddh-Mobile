using Giddh_Cross_Portable.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Giddh_Cross_Portable.Services
{
    class postSubmitter
    {
        //const string domainName = "http://192.168.1.57:9292/giddh-api/";
        //const string domainName = "http://apitest.giddh.com/giddh-api/";//"http://54.21.254.1/giddh-api/";
        const string domainName = "http://api.giddh.com/v1/";

        public static async Task<Response> SendRequestGETResponse(string url, List<KeyValuePair<string, string>> values,KeyValuePair<string,string> header)
        {
            Response RJson = new Response();
            string finalurl = domainName + url;
            if (values.Count > 0)
            {
                foreach (KeyValuePair<string, string> value in values)
                {
                    if (finalurl.Contains("?"))
                    {
                        finalurl = finalurl + "&" + value.Key + "=" + value.Value;
                    }
                    else
                    {
                        finalurl = finalurl + "?" + value.Key + "=" + value.Value;
                    }
                }
            }
            var httpHandler = new HttpClientHandler();
            foreach (Cookie cky in httpHandler.CookieContainer.GetCookies(new Uri(finalurl)))
            {
                cky.Discard = true;
                cky.Expired = true;
            }
            httpHandler.UseCookies = false;           
            var httpClient = new HttpClient(httpHandler);            
            var content = new FormUrlEncodedContent(values);
            //content.Headers.Add(header.Key, header.Value);            
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            
            HttpResponseMessage response = await httpClient.GetAsync(finalurl);
            //HttpResponseMessage response = await httpClient.PostAsync(domainName + url, content);
            Debug.WriteLine(domainName + url);
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(url + "--" + responseString);
            RJson = JsonConvert.DeserializeObject<Response>(responseString.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });            
            return RJson;
        }

        public static async Task<Response> SendRequestGETResponse(string url, List<KeyValuePair<string, string>> values, List<KeyValuePair<string, string>> header)
        {
            Response RJson = new Response();
            var httpClient = new HttpClient(new HttpClientHandler());
            var content = new FormUrlEncodedContent(values);
            //content.Headers.Add(header.Key, header.Value);
            foreach (KeyValuePair<string, string> head in header)
            {
                httpClient.DefaultRequestHeaders.Add(head.Key, head.Value);
            }
            string finalurl = domainName + url;
            if (values.Count > 0)
            {
                foreach (KeyValuePair<string, string> value in values)
                {
                    if (finalurl.Contains("?"))
                    {
                        finalurl = finalurl + "&" + value.Key + "=" + value.Value;
                    }
                    else
                    {
                        finalurl = finalurl + "?" + value.Key + "=" + value.Value;
                    }
                }
            }
            HttpResponseMessage response = await httpClient.GetAsync(finalurl);
            //HttpResponseMessage response = await httpClient.PostAsync(domainName + url, content);
            Debug.WriteLine(domainName + url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(url + "--" + responseString);
            RJson = JsonConvert.DeserializeObject<Response>(responseString.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return RJson;
        }

        public static async Task<Response> SendRequestPOSTResponse(string url, List<KeyValuePair<string, string>> values, KeyValuePair<string, string> header)
        {
            Response RJson = new Response();
            var httpClient = new HttpClient(new HttpClientHandler());
            var content = new FormUrlEncodedContent(values);
            content.Headers.Add(header.Key, header.Value);
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            HttpResponseMessage response = await httpClient.PostAsync(domainName + url, content);
            //HttpResponseMessage response = await httpClient.PostAsync(domainName + url, content);
            Debug.WriteLine(domainName + url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(url + "--" + responseString);
            RJson = JsonConvert.DeserializeObject<Response>(responseString.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return RJson;
        }


        public static async Task<ResponseA> SendRequestGETResponseA(string url, List<KeyValuePair<string, string>> values, KeyValuePair<string, string> header)
        {
            ResponseA RJson = new ResponseA();
            var httpClient = new HttpClient(new HttpClientHandler());
            var content = new FormUrlEncodedContent(values);
            content.Headers.Add(header.Key, header.Value);
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            string finalurl = domainName + url;
            if (values.Count > 0)
            {
                foreach (KeyValuePair<string, string> value in values)
                {
                    if (finalurl.Contains("?"))
                    {
                        finalurl = finalurl + "&" + value.Key + "=" + value.Value;
                    }
                    else
                    {
                        finalurl = finalurl + "?" + value.Key + "=" + value.Value;
                    }
                }
            }
            HttpResponseMessage response = await httpClient.GetAsync(domainName + url);
            //HttpResponseMessage response = await httpClient.PostAsync(domainName + url, content);
            Debug.WriteLine(domainName + url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(url + "--" + responseString);
            RJson = JsonConvert.DeserializeObject<ResponseA>(responseString.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return RJson;
        }

        public static async Task<ResponseA> SendRequestPOSTResponseA(string url, List<KeyValuePair<string, string>> values, KeyValuePair<string, string> header)
        {
            ResponseA RJson = new ResponseA();
            var httpClient = new HttpClient(new HttpClientHandler());
            var content = new FormUrlEncodedContent(values);
            content.Headers.Add(header.Key, header.Value);
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            HttpResponseMessage response = await httpClient.PostAsync(domainName + url, content);
            //HttpResponseMessage response = await httpClient.PostAsync(domainName + url, content);
            Debug.WriteLine(domainName + url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(url + "--" + responseString);
            RJson = JsonConvert.DeserializeObject<ResponseA>(responseString.ToString(), new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return RJson;
        }
    }
}
