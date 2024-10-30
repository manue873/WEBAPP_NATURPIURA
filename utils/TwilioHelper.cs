using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace WEBAPP_NATURPIURA.utils
{
    public class TwilioHelper
    {
        //debes crear la entidad con tokens 
        //private SMSSetings smsSetings = new SMSSetings();
        //public string sendSMSMEssage(string toMobileNumber, string messagetosend)
        //{
        //    TwilioClient.Init(
        //        smsSetings.Twilio_Account_SID,
        //        smsSetings.Twilio_Auth_TOKEN
        //        );
        //    var message = MessageResource.Create(
        //        from: new PhoneNumber(smsSetings.Twilio_Phone_Number),
        //        to: new PhoneNumber("+51" + toMobileNumber),
        //        body: messagetosend
        //        );
        //    return message.Sid;
        //}
    }
}
