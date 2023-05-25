using Eob_Web.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public class Job_Service : IJob_Service
    {
        private readonly IEmailSender _email_Sender;
        private readonly IBulkEmailSender _bulk_Email_Sender;
        private readonly IEob_Service _eob_Service;
        private readonly ISubscription_Service _subscription_Service;
        private readonly IUser_Service _user_Service;

        public Job_Service(
            IEmailSender email_Sender,
            IBulkEmailSender bulk_Email_Sender,
            IEob_Service eob_Service,
            ISubscription_Service subscription_Service,
            IUser_Service user_Service)
        {
            _email_Sender = email_Sender;
            _bulk_Email_Sender = bulk_Email_Sender;
            _eob_Service = eob_Service;
            _user_Service = user_Service;
            _subscription_Service = subscription_Service;
        }

        public async Task Email_Company_Admin_Joined(string company_Admin_Email, List<string> admin_Emails, string message)
        {
            await _email_Sender.SendEmailAsync(
                company_Admin_Email,
                "Thank you",
                "Thank you for your company submission. We'll respond back as soon as posssible!");

            await _bulk_Email_Sender.SendBulkEmailAsync(
                admin_Emails,
                "New user registerd a company",
                message);
        }

        public async Task Email_Employee_Joined(string company_Admin_Email, string message)
        {
            await _email_Sender.SendEmailAsync(
                company_Admin_Email,
                "New employee has joined your company",
                message);
        }

        public async Task Email_Contact(List<string> admin_Emails, string message)
        {
            await _bulk_Email_Sender.SendBulkEmailAsync(
                admin_Emails,
                "new EOB contact message",
                message);
        }

        /*************************************************
        *
        *               Recurring jobs
        *
        *************************************************/

        public async Task Email_Subscription_Expiration_Remider()
        {
            var eobs = await _eob_Service.Get_All();
            var emails = await _user_Service.Get_All_By_Recieve_Email();
            string message = @$"The following EOB('s) will be deactivated after a month:</br>";
            bool send = false;

            foreach (var eob in eobs)
            {
                if (!eob.Subscription.Expiration_Date.HasValue)
                    continue;

                if ((eob.Subscription.Expiration_Date - DateTime.Today).Value.Days == 30 && eob.Subscription.Active)
                {
                    message += @$"</br>Name: <b>{eob.Node_Name}</b>, company: <b>{eob.Company.Name}</b>";
                    send = true;
                }
            }

            if (!send)
                return;

            await _bulk_Email_Sender.SendBulkEmailAsync(
                emails,
                "EOB('s) expiration remider",
                message);
        }

        public async Task Email_Subscription_Expired()
        {
            var eobs = await _eob_Service.Get_All();
            var emails = await _user_Service.Get_All_By_Recieve_Email();
            string message = @$"The following EOB('s) went past the expiration date and thus been deactivated:</br>";
            bool send = false;

            foreach (var eob in eobs)
            {
                if (!eob.Subscription.Expiration_Date.HasValue)
                    continue;

                if ((eob.Subscription.Expiration_Date - DateTime.Today).Value.Days <= 0 && eob.Subscription.Active)
                {
                    message += @$"</br>Name: <b>{eob.Node_Name}</b>, company: <b>{eob.Company.Name}</b>";
                    eob.Subscription.Active = false;
                    await _subscription_Service.Update(eob.Subscription);
                    send = true;
                }
            }

            if (!send)
                return;

            await _bulk_Email_Sender.SendBulkEmailAsync(
                emails,
                "Expired EOB('s)",
                message);
        }
    }
}
