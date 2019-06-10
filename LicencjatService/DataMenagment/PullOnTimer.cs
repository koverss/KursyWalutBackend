using KursyWalutService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Globalization;

namespace KursyWalutService.DataMenagment
{
    public static class PullOnTimer
    {
        private static string url_tableA = "http://www.nbp.pl/kursy/xml/LastA.xml";
        private static string url_tableB = "http://www.nbp.pl/kursy/xml/LastB.xml";
        private static bool isSent;    

        private static List<Currencies> tab;//wszystkie elementy tabeli w liscie

        public static async void PeriodicWork()
        {
            DateTime presentDate = DateTime.UtcNow;
            try
            {
                presentDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(presentDate, "Central European Standard Time");
            }
            catch(Exception e)
            {
                var t = e.Message;
            }

            DateTime docDate;
            
            try
            {
                docDate = PrepareData.GetDocumentDate(url_tableA);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var licContext = new LicencjatContext();
            tab = licContext.AllCurrencies.ToList();

            var timeWindow = new DateTime(presentDate.Year, presentDate.Month, presentDate.Day, 12,30, 00);//12, 30, 00
            var noon = new DateTime(presentDate.Year, presentDate.Month, presentDate.Day, 12, 00, 00);//12, 00, 00
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            if (presentDate.CompareTo(noon) < 0)
            {
                isSent = false;
            }

            if (docDate.Day == presentDate.Day
                && presentDate.CompareTo(timeWindow) < 0 
                && presentDate.CompareTo(noon) >= 0
                && isSent==false)
            {
                if (tab.Count != 0)
                    await SelectUsersAndSendPush();
                isSent = true;
            }
        }

        private static async Task SelectUsersAndSendPush() //static void
        {
            foreach (var user in tab)
            {
                var xmlDocA = PrepareData.FormatXMLtoUTF8(url_tableA);
                var xmlDocB = PrepareData.FormatXMLtoUTF8(url_tableB);

                var listUserTags = new List<string>();

                if (!xmlDocA.HasChildNodes || !xmlDocB.HasChildNodes)
                {
                    throw new Exception("Dokument nie posiada węzłów");
                }

                var listUserCurrencies = new List<string>
                {
                    user.FirstCurrency,
                    user.SecondCurrency,
                    user.ThirdCurrency,
                    user.FourthCurrency,
                    user.FifthCurrency
                };

                for (var i = 0; i < listUserCurrencies.Count; i++)
                {
                    if (listUserCurrencies[i] != null)
                    {
                        if (xmlDocA.
                            SelectSingleNode("/tabela_kursow/pozycja/nazwa_waluty[text()='" + listUserCurrencies[i] + "']") == null)
                        {
                            var innerNode = xmlDocB.SelectSingleNode(
                                "/tabela_kursow/pozycja/nazwa_waluty[text()='"
                                + listUserCurrencies[i]
                                + "']"
                                );
                            var pNode = innerNode.ParentNode;
                            var rate = pNode.LastChild.InnerText;
                            var tag = SelectWhatToPush(user.UserID, rate, listUserCurrencies[i]);

                            if (tag != string.Empty)
                                listUserTags.Add(tag);

                            await SendPushAsync("gcm", "Kurswaluty " + listUserCurrencies[i] + " uległ zmianie", tag);
                        }
                        else
                        {
                            var innerNode = xmlDocA.SelectSingleNode(
                                "/tabela_kursow/pozycja/nazwa_waluty[text()='"
                                + listUserCurrencies[i]
                                + "']"
                                );
                            var pNode = innerNode.ParentNode;
                            var rate = pNode.LastChild.InnerText;
                            var tag = SelectWhatToPush(user.UserID, rate, listUserCurrencies[i]);

                            if (tag != string.Empty)
                            {
                                var mess = "Kurs waluty " + listUserCurrencies[i] + " uległ zmianie";
                                await SendPushAsync("gcm", mess, tag);
                            }
                        }
                    }
                }
            }
        }

        private static string SelectWhatToPush(string userID, string currentRate, string currencyName)
        {
            string _tag = "";
            var clientRate =
                tab.Single(t => t.UserID == userID);
                //&& (t.FirstCurrency == currencyName
                //|| t.SecondCurrency == currencyName
                //|| t.ThirdCurrency == currencyName
                //|| t.FirstCurrency == currencyName
                //|| t.FifthCurrency == currencyName));

            if (currencyName == clientRate.FirstCurrency)
            {
                var push = WantPush(clientRate.FirstCurrencyValue, currentRate, clientRate.FirstCurrencyOver);
                if (push)
                {
                    _tag = CreateTag(currencyName, clientRate.FirstCurrencyValue, clientRate.FirstCurrencyOver);
                }
            }
            else if (currencyName == clientRate.SecondCurrency)
            {
                var push = WantPush(clientRate.SecondCurrencyValue, currentRate, clientRate.SecondCurrencyOver);
                if (push)
                {
                    _tag = CreateTag(currencyName, clientRate.SecondCurrencyValue, clientRate.SecondCurrencyOver);
                }
            }
            else if (currencyName == clientRate.ThirdCurrency)
            {
                var push = WantPush(clientRate.ThirdCurrencyValue, currentRate, clientRate.ThirdCurrencyOver);
                if (push)
                {
                    _tag = CreateTag(currencyName, clientRate.ThirdCurrencyValue, clientRate.ThirdCurrencyOver);
                }
            }
            else if (currencyName == clientRate.FourthCurrency)
            {
                var push = WantPush(clientRate.FourthCurrencyValue, currentRate, clientRate.FourthCurrencyOver);
                if (push)
                {
                    _tag = CreateTag(currencyName, clientRate.FourthCurrencyValue, clientRate.FourthCurrencyOver);
                }
            }
            else if (currencyName == clientRate.FifthCurrency)
            {
                var push = WantPush(clientRate.FifthCurrencyValue, currentRate, clientRate.FifthCurrencyOver);
                if (push)
                {
                    _tag = CreateTag(currencyName, clientRate.FifthCurrencyValue, clientRate.FifthCurrencyOver);
                }
            }
            return _tag;
        }

        private static bool WantPush(string clientRate, string serverRate, bool over)
        {
            serverRate = serverRate.Replace(",", ".");
            if (double.Parse(serverRate) > double.Parse(clientRate) && over)
                return true;
            else if (double.Parse(serverRate) < double.Parse(clientRate) && !over)
                return true;
            else
                return false;
        }

        private static string CreateTag(string currencyName, string value, bool over)
        {
            Regex reg = new Regex(@"[^\u0020-\u007E]");//usuwam polskie znaki
            var str = currencyName.Replace(" ", "")
                .Replace("\t", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Replace("(","")
                .Replace(")","");

            str = reg.Replace(str, "");
                
            string tail;

            if (over)
            {
                tail = "o";
            }
            else
            {
                tail = "u";
            }

            var tag = str + value + tail;

            tag = tag.Replace(",", "COMA")
                .Replace(".", "COMA");

            return tag;
        }

        private async static Task<HttpResponseMessage> SendPushAsync(string pns, [FromBody]string message, string to_tag)
        {
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            var notif = "{ \"data\" : {\"message\":\"" + message + "\"}}";

            outcome = await Notifications.Instance.Hub.SendGcmNativeNotificationAsync(notif, to_tag);

            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }
            return new HttpResponseMessage(ret);
        }
    }
}