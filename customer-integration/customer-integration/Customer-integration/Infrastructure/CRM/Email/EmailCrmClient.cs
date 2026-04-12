using customer_integration.Application.Models;
using customer_integration.Infrastructure.CRM.Email;
using customer_integration.Infrastructure.CRM.Models;
using customer_integration.Infrastructure.CRM.SystemUser;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using EGC = customer_integration.Infrastructure.CRM.Models;

namespace customer_integration.Infrastructure.CRM.Email
{
    public class EmailCrmClient : IEmailCrmClient
    {
        private readonly ServiceClient _serviceClient;
        private readonly ISystemUserClient _systemUserClient;


        public EmailCrmClient(ServiceClient serviceClient, ISystemUserClient systemUserClient)
        {
            _serviceClient = serviceClient;
            _systemUserClient = systemUserClient;
        }


        public async Task<Guid> CreateEmailAsync(CreateEmailModel createEmailModel, CancellationToken cancellationToken)
        {
            var customerContactAc = new EGC.ActivityParty
            {
                PartyId = new EntityReference(EGC.Contact.EntityLogicalName, createEmailModel.CustomerId)
            };

            var ownerContactAc = new EGC.ActivityParty
            {
                PartyId = new EntityReference(EGC.Contact.EntityLogicalName, createEmailModel.OwnerId)
            };

            EGC.SystemUser? technicalUser = await _systemUserClient.GetTechnicalUserAsync(cancellationToken);
            if (technicalUser == null) return Guid.Empty;

            var systemUserAc = new EGC.ActivityParty
            {
                PartyId = new EntityReference(EGC.SystemUser.EntityLogicalName, technicalUser.Id)
            };

            var email = new EGC.Email
            {
                Subject = createEmailModel.Title,
                Description = createEmailModel.Body,
                To = new[] { customerContactAc, ownerContactAc },
                From = new[] { systemUserAc }
            };
            
            return await _serviceClient.CreateAsync(email, cancellationToken);
        }


        public async Task<Template?> GetTemplateAsync(string templateName, CancellationToken cancellationToken)
            => (await _serviceClient.RetrieveMultipleAsync(new QueryExpression
            {
                EntityName = EGC.Template.EntityLogicalName,
                ColumnSet = new ColumnSet(EGC.Template.Fields.TemplateId,
                    EGC.Template.Fields.Title,
                    EGC.Template.Fields.Subject,
                    EGC.Template.Fields.Body),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(EGC.Template.Fields.Title, ConditionOperator.Equal, templateName)
                    }
                }
            }, cancellationToken))?.Entities?.Select(x => x?.ToEntity<EGC.Template>())?.FirstOrDefault();
    }
}
