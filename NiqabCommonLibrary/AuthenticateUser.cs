using Microsoft.Azure.CognitiveServices.Vision.Face;
using NiqabCommonLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiqabCommonLibrary
{
    public class AuthenticateUser
    {
        public async Task<Dictionary<Guid,UserInfo>> Authenticate (string testImageFile)
        {
            IFaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials("<Redacted>"), new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = "<Redacted>"
            };
            string personGroupId = "employee";
            // one time activity only
            //await faceClient.PersonGroup.CreateAsync(personGroupId, "My Friends");
            var userMapping = new Dictionary<Guid, UserInfo>();
            using (Stream s = File.OpenRead(testImageFile))
            {
                var faces = await faceClient.Face.DetectWithStreamAsync(s);
                var faceIds = faces.Select(face => face.FaceId.Value).ToArray();
                var results = await faceClient.Face.IdentifyAsync(faceIds, personGroupId);
               
                foreach (var identifyResult in results)
                {
                    if (identifyResult.Candidates.Count == 0)
                    {
                        userMapping.Add(identifyResult.FaceId, new UserInfo() { UserFound = false });
                    }
                    else
                    {
                        // Get top 1 among all candidates returned
                        var candidateId = identifyResult.Candidates[0].PersonId;
                        var person = await faceClient.PersonGroupPerson.GetAsync(personGroupId, candidateId);
                        userMapping.Add(identifyResult.FaceId, new UserInfo() { UserFound= true, Alias = person.Name});
                    }
                }
            }
            return userMapping;
        }
    }
}
