using System;
using System.Collections.Generic;
using Flow.Launcher.Plugin;

namespace Flow.Launcher.Plugin.ZAi
{
    public class ZAi : IPlugin
    {
        private PluginInitContext _context;

        private const string ZAiUrl = "https://chat.z.ai/?q=";
        private const string TemporaryArg = "&temporary-chat=true";
        private const string QueryIconPath = "Icons/zai.png";

        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        public List<Result> Query(Query query)
        {
            if (string.IsNullOrWhiteSpace(query.Search))
            {
                return new List<Result>
                {
                    new()
                    {
                        Title = "Ask Z.ai...",
                        SubTitle = "Type your prompt and press Enter",
                        IcoPath = QueryIconPath
                    }
                };
            }

            return new List<Result>
            {
                CreateNewResult(query.Search, QueryIconPath, CreateZAiSearchUrl(ZAiUrl, query)),
                CreateNewResult($"{query.Search} (Temporary Chat)", QueryIconPath, CreateZAiSearchUrl(ZAiUrl, query, TemporaryArg))
            };
        }

        private Func<ActionContext, bool> CreateZAiSearchUrl(string url, Query query, string args = "")
        {
            return _ =>
            {
                // Fallback: se o Z.ai não preencher o campo a partir de ?q=, o prompt já está na área de transferência (Ctrl+V).
                _context.API.CopyToClipboard(query.Search, showDefaultNotification: false);
                _context.API.ShowMsg("Z.ai", "Prompt copiado. Se não aparecer no chat, cole com Ctrl+V.");
                _context.API.OpenUrl($"{url}{Uri.EscapeDataString(query.Search)}{args}");
                return true;
            };
        }

        private Result CreateNewResult(string title, string iconPath, Func<ActionContext, bool> action)
        {
            return new Result
            {
                Title = title,
                IcoPath = iconPath,
                Action = action
            };
        }
    }
}
