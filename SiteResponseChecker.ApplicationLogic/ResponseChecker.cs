using System;
using SiteResponseChecker.ApplicationLogic.Utils;
using SiteResponseChecker.Domain;
using SiteResponseChecker.NhRepository;
using SiteResponseChecker.Domain.Exceptions;

namespace SiteResponseChecker.ApplicationLogic
{
    public class ResponseChecker
    {
        private readonly SiteRepository _siteRepository;
        private readonly SiteResponseRepository _siteResponseRepository;
        private readonly SiteErrorRepository _siteErrorRepository;
        private readonly NotificationsRepository _notificationsRepository;

        private readonly ILogger _logger;

        public ResponseChecker(ILogger logger, SiteRepository siteRepository, SiteResponseRepository siteResponseRepository, SiteErrorRepository siteErrorRepository, NotificationsRepository notificationsRepository)
        {
            _logger = logger;
            _siteRepository = siteRepository;
            _siteResponseRepository = siteResponseRepository;
            _siteErrorRepository = siteErrorRepository;
            _notificationsRepository = notificationsRepository;
        }

        public void CheckResponse(Site site, SiteResponse lastResponse)
        {
            try
            {
                if (lastResponse == null)
                    _logger.LogInfo(site.SiteName + " - Checking changes (first time).");
                else
                    _logger.LogInfo(site.SiteName + " - Checking changes.");

                string html = SiteHtmlUtil.GetSiteHtml(site);
                SiteResponse currentResponse;
                string html_to_text = SiteHtmlUtil.StripHTMLAdvanced(html);
                //string currentResponseHash = HashUtil.CalculateMD5Hash(html);
                string currentResponseHash = HashUtil.CalculateMD5Hash(html_to_text);

                //if (lastResponse == null || !lastResponse.Contents.Equals(html))
                if (lastResponse == null || !lastResponse.Hash.Equals(currentResponseHash))
                {
                    NotificationHelper notificationHelper = new NotificationHelper();
                    currentResponse = new SiteResponse();
                    currentResponse.Site = site;
                    currentResponse.CheckDate = DateTime.Now;
                    currentResponse.StatusCode = "200";
                    currentResponse.Contents = html;
                    //currentResponse.HtmlContents = html;
                    //currentResponse.Contents = current_response_html_to_text;
                    currentResponse.Hash = currentResponseHash;
                    currentResponse.Diff = notificationHelper.GetDiff(currentResponse, lastResponse);
                    site.SiteResponses.Add(currentResponse);
                    _siteResponseRepository.Save(currentResponse);
                    _siteRepository.SaveOrUpdate(site);

                    if (lastResponse != null)
                    {
                        string message = site.SiteName + " has changes - Sending notification email.";
                        _logger.LogInfo(message);
                        Notification notification = notificationHelper.CreateNotification(site, currentResponse, lastResponse);
                        _notificationsRepository.SaveOrUpdate(notification);
                        _logger.LogSiteChanged(notification, site, message, currentResponse.Diff);
                    }
                }
                else
                {
                    _logger.LogInfo(site.SiteName + " doesn't have changes.");
                    currentResponse = lastResponse;
                    currentResponse.CheckDate = DateTime.Now;
                    currentResponse.StatusCode = "200";
                    _siteResponseRepository.SaveOrUpdate(currentResponse);
                }
            }
            catch(Exception ex)
            {
                LogErrorGettingSiteResponse(site, ex);
            }
        }

        public void LogErrorGettingSiteResponse(Site site, Exception ex)
        {
            _siteErrorRepository.DeleteOld(site);

            //TODO: pegar o ultimo erro nas 24 horas passadas, caso exista atualizar um contador de erros ++
            SiteError siteError = new SiteError();
            siteError.Site = site;
            siteError.Date = DateTime.Now;
            siteError.ErrorDetails = ex.Message;
            if (ex is RequestException)
            {
                _logger.LogInfo("Request exception - Error getting site's contents - " + site.SiteName);
                siteError.ErrorType = ErrorTypes.RequestError.ToString();
                _logger.LogError(ex);
            }
            else if (ex is SpecificElementException)
            {
                _logger.LogInfo("Error getting specific element from site's contents -" + site.SiteName);
                siteError.ErrorType = ErrorTypes.SpecificElementError.ToString();
                _logger.LogError(ex);
            }
            else
            {
                _logger.LogInfo("Generic error getting site's contents -" + site.SiteName);
                siteError.ErrorType = ErrorTypes.GenericError.ToString();
                _logger.LogError(ex);
            }

            site.SiteErrors.Add(siteError);
            _siteErrorRepository.Save(siteError);
            _siteRepository.SaveOrUpdate(site);
        }
    }
}
