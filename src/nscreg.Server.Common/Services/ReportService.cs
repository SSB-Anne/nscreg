using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nscreg.Data;
using nscreg.Data.Entities;
using nscreg.Utilities.Configuration;
using Newtonsoft.Json;

namespace nscreg.Server.Common.Services
{
    public class ReportService
    {
        private readonly NSCRegDbContext _ctx;
        private readonly ReportingSettings _settings;

        public ReportService(NSCRegDbContext context, ReportingSettings settings)
        {
            _ctx = context;
            _settings = settings;
        }

        public async Task<List<ReportTree>> GetReportsTree(string userName)
        {
            var role =
                from u in _ctx.Users
                join ur in _ctx.UserRoles on u.Id equals ur.UserId
                join r in _ctx.Roles on ur.RoleId equals r.Id
                where u.UserName == userName
                select r;

            var sqlWalletUser = role.FirstOrDefault()?.SqlWalletUser;

            if (string.IsNullOrEmpty(sqlWalletUser))
                throw  new Exception("Please specify sqlWalletUser in Administrator or Employee roles");

            List<ReportTree> queryResult;
            try
            {
                queryResult = await _ctx.ReportTree.FromSql("GetReportsTree @p0", sqlWalletUser).ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured while trying get data of reports from database. Message: {e.Message}");
            }
            
            var resultNodes = new List<ReportTree>(queryResult);
            RemoveEmptyFolders(queryResult, resultNodes);

            var result = GetAccessToken(_settings, sqlWalletUser);

            if (string.IsNullOrEmpty(result))
                throw new Exception("Can not get access token from SqlWallet.");

            var hostName = !string.IsNullOrEmpty(_settings.ExternalHostName)
                ? _settings.ExternalHostName
                : _settings.HostName;

            foreach (var node in resultNodes)
            {
                if (node.Type == "Report")
                    node.ReportUrl = $"http://{hostName}/embed?access_token={result}#{node.ReportId}";
            }

            return resultNodes;
        }

        private static void RemoveEmptyFolders(ICollection<ReportTree> nodes, ICollection<ReportTree> resultNodes)
        {
            if (nodes == null || nodes.Count == 0)
                return;
            foreach (var reportTreeNode in nodes)
            {
                var childNodes = resultNodes.Where(x => x.ParentNodeId == reportTreeNode.Id).Select(x => x).ToList();
                RemoveEmptyFolders(childNodes, resultNodes);
                if (resultNodes.All(x => x.ParentNodeId != reportTreeNode.Id) && (reportTreeNode.ReportId == null && reportTreeNode.ParentNodeId != null))
                    resultNodes.Remove(reportTreeNode);
            }
        }

        private string GetAccessToken(ReportingSettings settings, string sqlWalletUserName)
        {
            var authResponse = new SqlWalletResponse();

            var client = new HttpClient();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"http://{settings.HostName}/connect/token")
                {
                    Content = new StringContent(
                        $"client_secret={settings.SecretKey}&grant_type=client_credentials&client_id=sqlwallet&scope=sqlwallet",
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded")
                };

                request.Headers.ExpectContinue = true;

                client.SendAsync(request).ContinueWith(responseTask =>
                {
                    var content = responseTask.Result.Content.ReadAsStringAsync().Result;

                    authResponse = JsonConvert.DeserializeObject<SqlWalletResponse>(content);
                }).Wait();


                var userRequest =
                    new HttpRequestMessage(HttpMethod.Post,
                        $"http://{settings.HostName}/auth/accesstoken/{sqlWalletUserName}")
                    {
                        Content = new StringContent("", Encoding.UTF8, "application/json")
                    };

                userRequest.Headers.Authorization =
                    new AuthenticationHeaderValue(authResponse.Token_Type, authResponse.Access_Token);
                userRequest.Headers.CacheControl = new CacheControlHeaderValue {NoCache = true};
                userRequest.Headers.Host = settings.HostName;

                var accessToken = "";
                client.SendAsync(userRequest).ContinueWith(respTask =>
                {
                    accessToken = respTask.Result.Content.ReadAsStringAsync().Result;
                }).Wait();

                return accessToken;
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured while trying getting access token. Message: {e.Message}");
            }
            finally
            {
                client.CancelPendingRequests();
            }
            

        }

        internal class SqlWalletResponse
        {
            public string Access_Token { get; set; }
            public string Expires_In { get; set; }
            public string Token_Type { get; set; }

        }
    }
}
