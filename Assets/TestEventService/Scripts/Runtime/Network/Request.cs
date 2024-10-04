using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace TestEventService.Network
{
    public sealed class Request
    {
        private readonly string _uri;

        private HttpRequestMethod _method;
        private IDictionary<string, string> _parameters;
        private Dictionary<string, string> _form;
        private string _data;
        private bool _json;

        public Request(string uri)
        {
            _uri = uri;
        }

        public Request SetMethod(HttpRequestMethod method)
        {
            _method = method;

            return this;
        }

        public Request SetParameters(IDictionary<string, string> parameters)
        {
            _parameters = parameters;

            return this;
        }

        public Request SetForm(Dictionary<string, string> form)
        {
            _form = form;

            return this;
        }

        public Request SetData(string data)
        {
            _data = data;
            _json = false;

            return this;
        }

        public Request SetJson(string json)
        {
            _data = json;
            _json = true;

            return this;
        }

        private Uri CreateUri()
        {
            var uriBuilder = new UriBuilder(_uri);

            if (_parameters == null)
            {
                return uriBuilder.Uri;
            }

            var query = string.Join('&', _parameters.Select(item => $"{item.Key}={item.Value}"));

            uriBuilder.Query += string.IsNullOrWhiteSpace(uriBuilder.Query)
                ? query
                : $"&{query}";

            return uriBuilder.Uri;
        }

        private UnityWebRequest CreateRequest()
        {
            var uri = CreateUri();
            var request = new UnityWebRequest
            {
                uri = uri,
                method = _method switch
                {
                    HttpRequestMethod.Get => "GET",
                    HttpRequestMethod.Post => "POST",
                    _ => throw new ArgumentOutOfRangeException(nameof(_method), _method, null)
                },
                downloadHandler = new DownloadHandlerBuffer()
            };

            if (_form is { Count: > 0 })
            {
                request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                request.uploadHandler = new UploadHandlerRaw(UnityWebRequest.SerializeSimpleForm(_form));
            }
            else if (!string.IsNullOrWhiteSpace(_data))
            {
                var contentType = _json ? "application/json" : "text/plain";
                request.SetRequestHeader("Content-Type", contentType);
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(_data));
            }
            else
            {
                request.SetRequestHeader("Content-Type", "text/plain");
            }

            return request;
        }

        public async UniTask<Response> ExecuteAsync(CancellationToken cancellationToken)
        {
            using var request = CreateRequest();

            await request.SendWebRequest()
                         .ToUniTask(cancellationToken: cancellationToken);

            var downloadHandler = request.downloadHandler;

            return new Response(downloadHandler.data);
        }
    }
}