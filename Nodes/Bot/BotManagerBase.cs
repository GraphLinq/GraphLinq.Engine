using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Timers;

namespace NodeBlock.Engine.Nodes.Dextools
{
    public abstract class BotManagerBase
    {
        protected static readonly HttpClient _httpClient = new HttpClient();
        protected readonly string _baseUrl;
        protected readonly double _intervalInSeconds;

        protected readonly List<string> _activeBots;
        protected readonly ConcurrentDictionary<string, Timer> _keepAliveTimers;

        protected BotManagerBase(string baseUrlEnvVar)
        {
            _baseUrl = Environment.GetEnvironmentVariable(baseUrlEnvVar) ?? throw new InvalidOperationException("Base URL not set.");
            _activeBots = new List<string>();
            _keepAliveTimers = new ConcurrentDictionary<string, Timer>();
            double.TryParse(Environment.GetEnvironmentVariable("keep_alive_interval"), out _intervalInSeconds);
        }

        public string StartBot(object payload, BlockGraph graph)
        {
            try
            {
                var requestUrl = $"{_baseUrl}/start-bot";
                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync(requestUrl, content);
                var jsonObject = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);

                if (jsonObject.ContainsKey("id"))
                {
                    var botId = jsonObject["id"]?.ToString();
                    _activeBots.Add(botId);
                    graph.AppendLog("info", "Bot started");
                    return botId;
                }

                throw new Exception(jsonObject.ToString());
            }
            catch (Exception ex)
            {
                graph.AppendLog("error", $"Bot error : {ex.Message}");
                return null;
            }
        }

        public void StartKeepAlive(string botId, BlockGraph graph)
        {
            if (!_activeBots.Contains(botId))
            {
                graph.AppendLog("error", $"Cannot start KeepAlive for inactive bot ID: {botId}");
                return;
            }

            var timer = new Timer(_intervalInSeconds);
            timer.Elapsed += (sender, args) => KeepAlive(botId, graph);
            timer.AutoReset = true;
            timer.Start();

            _keepAliveTimers.TryAdd(botId, timer);
        }

        protected void KeepAlive(string botId, BlockGraph graph)
        {
            try
            {
                var requestUrl = $"{_baseUrl}/keep-alive/{botId}";
                var response = _httpClient.PostAsync(requestUrl, null);
                var jsonObject = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);
                graph.AppendLog("info", $"Bot info : {jsonObject}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in KeepAlive for bot ID {botId}: {ex.Message}");
            }
        }

        public void StopBot(string botId, BlockGraph graph)
        {
            try
            {
                if (_activeBots.Contains(botId))
                {
                    var requestUrl = $"{_baseUrl}/stop-bot/{botId}";
                    _httpClient.DeleteAsync(requestUrl).Wait();

                    if (_keepAliveTimers.TryRemove(botId, out var timer))
                    {
                        timer.Stop();
                        timer.Dispose();
                    }

                    _activeBots.Remove(botId);
                }
                else
                {
                    graph.AppendLog("error", $"Bot ID {botId} is not active or does not exist.");
                }
            }
            catch (Exception ex)
            {
                graph.AppendLog("error", $"Exception while stopping the bot: {ex.Message}");
            }
        }
    }
}
