using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.CognitoEvents;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CognitoPreSignup
{
    public class PreSignupFunction
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public JsonElement LambdaHandler(JsonElement cognitoEvent, ILambdaContext context)
        {
            context.Logger.Log(cognitoEvent.ToString());

            //pre-signup trigger lambda is mainly for the validation of new cognito user's attributes

            return cognitoEvent;
        }
    }
}
