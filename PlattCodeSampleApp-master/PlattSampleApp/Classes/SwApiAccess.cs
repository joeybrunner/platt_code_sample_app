using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PlattSampleApp.Classes
{
    public class SwApiAccess
    {
        public string apiGetRequest(string apiEndpoint)
        {
            WebRequest request = WebRequest.Create(apiEndpoint);
            WebResponse response = request.GetResponse();

            string responseFromServer = "";

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
            }

            response.Close();

            return responseFromServer;
        }
    }
}