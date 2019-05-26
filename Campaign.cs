using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentWorkerApp
{
    public class SMSSender:ICampaignSender
    {
        public int Send(CampaignRecipientDetail recipient) {
            int result = 0;
            return result;
        }
        public int UpdateStatus(CampaignRecipientDetail recipient)
        {
            int result = 0;
            return result;
        }
    }
    public class EmailSender : ICampaignSender
    {
        public int Send(CampaignRecipientDetail recipient)
        {
            int result = 0;

            return result;
        }
        public int UpdateStatus(CampaignRecipientDetail recipient)
        {
            int result = 0;
            return result;
        }
    }
    public class NotificationSender : ICampaignSender
    {
        public int Send(CampaignRecipientDetail recipient)
        {
            int result = 0;
            return result;
        }
        public int UpdateStatus(CampaignRecipientDetail recipient)
        {
            int result = 0;
            return result;
        }
    }
    public interface ICampaignSender
    {
        int Send(CampaignRecipientDetail recipient);
        int UpdateStatus(CampaignRecipientDetail recipient);
    }
    public class CampaignRecipientDetail {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public int EventId { get; set; }
        public string RecipientAddress { get; set; }
        public int Categoryid { get; set; }

    }
}
