﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace SKGL
{
    /// <summary>
    /// The methods that are being used under the hood.
    /// </summary>
    public class HelperMethods
    {

        public static T SendRequestToWebAPI3<T>(object inputParameters, 
                                                string typeOfAction,
                                                string token,
                                                WebProxy proxy = null,
                                                int version = 1 )                                                    
        {

            // converting the input
            Dictionary<string, string> inputParams = (from x in inputParameters.GetType().GetProperties() select x)
                                                          .ToDictionary(x => x.Name, x => (x.GetGetMethod()
                                                          .Invoke(inputParameters, null) == null ? "" : x.GetGetMethod()
                                                          .Invoke(inputParameters, null).ToString()));


            using (WebClient client = new WebClient())
            {
                NameValueCollection reqparm = new NameValueCollection();

                foreach (var input in inputParams)
                {
                    reqparm.Add(input.Key, input.Value);
                }

                reqparm.Add("token", token);

                // version 1 is default so no need to send it twice.
                if (version > 1)
                    reqparm.Add("v", version.ToString());

                // in case we have a proxy server. if not, we set it to null to avoid unnecessary time delays.
                // based on http://stackoverflow.com/a/4420429/1275924 and http://stackoverflow.com/a/6990291/1275924. 
                client.Proxy = proxy;

                try
                {
                    byte[] responsebytes = client.UploadValues("https://serialkeymanager.com//api/" + typeOfAction, "POST", reqparm);
                    string responsebody = Encoding.UTF8.GetString(responsebytes);

                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responsebody);
                }
                catch(Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("An error occurred when we tried to contact SKM. The following error was received: " + ex.Message);
#endif
                    return default(T);
                }

            }
        }

    }
}
