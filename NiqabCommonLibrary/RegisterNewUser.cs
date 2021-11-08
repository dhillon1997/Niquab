using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace NiqabCommonLibrary
{
    public class RegisterNewUser
    {
        public async Task<string> Register(string userName, string imagePath)
        {
            IFaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials("<Redacted>"), new System.Net.Http.DelegatingHandler[] { })
            {
                Endpoint = "<Redacted>"
            };
            string personGroupId = "employee";
            // one time activity only
           // await faceClient.PersonGroup.CreateAsync(personGroupId, "My Friends");
            
            // Define Anna
            Person friend1 = await faceClient.PersonGroupPerson.CreateAsync(personGroupId, userName);

            // Directory contains image files of Anna
            using (Stream s = File.OpenRead(imagePath))
            {
                // Detect faces in the image and add to Anna
                await faceClient.PersonGroupPerson.AddFaceFromStreamAsync(
                    personGroupId, friend1.PersonId, s);
            }
            
            //Train data 
            await faceClient.PersonGroup.TrainAsync(personGroupId);

            //Wait for training to finish 
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await faceClient.PersonGroup.GetTrainingStatusAsync(personGroupId);

                if (trainingStatus.Status != TrainingStatusType.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
            return "User registered successfully";
        }
    }
}
