// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.18.1

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using MongoDB.Driver;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;

namespace DemoGPTBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private IMongoCollection<Log> _log;
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var replyText = await GetAnswer(turnContext.Activity.Text);
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        async Task<string> GetAnswer(string question)
        {
            OpenAIAPI api = new OpenAIAPI("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");
            GetMongoCollection("Log");
            var log = PopulateLog(question,"REQ");
            //Write Log
            await _log.InsertOneAsync(log);
            var result = await api.Completions.CreateCompletionAsync(
                new CompletionRequest(question, model: Model.DavinciText, temperature: 0.1, max_tokens: 100));
            var resp = "";
            foreach (var token in result.Completions)
            {
                resp += token.Text + " ";
            }
            log = PopulateLog(resp,"RESP");
            await _log.InsertOneAsync(log);
            return resp;

        }

        private void GetMongoCollection(string Name)
        {
            var client = new MongoClient("mongodb+srv://solutionsforai:D7UcFi1A6DxqAPdh@solutionsforaicluster.cmkm7hd.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("Logs");
            _log = database.GetCollection<Log>(Name);
        }

        private Log PopulateLog(string LogEntry,string type)
        {
            Log log = new Log();
            log.Type=type;
            log.Message=LogEntry;
            log.actionDate=DateTime.UtcNow;
            return log;
        }


        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
