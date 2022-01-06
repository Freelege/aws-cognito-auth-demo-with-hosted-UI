using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CognitoDemoApi.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public class Customer
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        [HttpPost]
        [Route("api/token")]
        public async Task<ActionResult<string>> Token(Customer user)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Constants.AWSAccessKeyId, Constants.AWSAccessSecretKey, Constants.AWSRegion);

            CognitoUserPool userPool = new CognitoUserPool(Constants.CognitoUserPoolId, Constants.CognitoAppClientId, provider);
            CognitoUser cognitoUser = new CognitoUser(user.Username, Constants.CognitoAppClientId, userPool, provider);
            var authRequest = new InitiateAdminNoSrpAuthRequest()
            {
                Password = user.Password
            };

            AuthFlowResponse authResponse = await cognitoUser.StartWithAdminNoSrpAuthAsync(authRequest).ConfigureAwait(false);
            var token = authResponse.AuthenticationResult.IdToken;

            return token;
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<ActionResult<string>> Register(Customer user)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(Constants.AWSAccessKeyId, Constants.AWSAccessSecretKey, Constants.AWSRegion);

            var request = new SignUpRequest
            {
                ClientId = Constants.CognitoAppClientId,
                //If "Generate client secret" is checked when adding app client for user pool, a secret hash must be computed and provided for SignupRequest
                // SecretHash = , //https://aws.amazon.com/premiumsupport/knowledge-center/cognito-unable-to-verify-secret-hash/
                Password = user.Password,
                Username = user.Username
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = user.Email
            };
            request.UserAttributes.Add(emailAttribute);

            await cognito.SignUpAsync(request);

            return Ok();
        }

        [HttpPost]
        [Route("api/signin")]
        public async Task<ActionResult<string>> SignIn(Customer user)
        {
            var cognito = new AmazonCognitoIdentityProviderClient(Constants.AWSAccessKeyId, Constants.AWSAccessSecretKey, Constants.AWSRegion);

            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = Constants.CognitoUserPoolId,
                ClientId = Constants.CognitoAppClientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add("USERNAME", user.Username);
            request.AuthParameters.Add("PASSWORD", user.Password);

            var response = await cognito.AdminInitiateAuthAsync(request);

            return Ok(response.AuthenticationResult.IdToken);
        }
    }
}
