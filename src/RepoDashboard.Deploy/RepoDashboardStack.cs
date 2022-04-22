using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SecretsManager;
using Constructs;

namespace RepoDashboard.Deploy;

public class RepoDashboardStack : Stack
{
    public RepoDashboardStack(Construct scope, string id, IStackProps props)
        : base(scope, id, props)
    {
        var environmentName = System.Environment.GetEnvironmentVariable("TARGET_ENVIRONMENT") ?? "dev";
        var environmentVariables = EnvironmentVariablesFactory.AddEnvironmentVariables(this, environmentName);

        var function =
            new DockerImageFunction(this,
                                    "RepoDashboardFunctions",
                                    new DockerImageFunctionProps
                                    {
                                        Code =
                                            DockerImageCode.FromImageAsset(@"src/RepoDashboard"),
                                        Description = "Razor Pagyes app to view github repository data for a team",
                                        Environment = environmentVariables,
                                        Timeout = Duration.Minutes(5)
                                    });

        var policyStatement = new PolicyStatement(new PolicyStatementProps
                                                  {
                                                      Actions = new[] { "secretsmanager:ListSecrets" },
                                                      Resources = new []{"*"}
                                                  });

        function.AddToRolePolicy(policyStatement);
        
        var newSecret = new Secret(this,
                                   "GitHubAccessToken",
                                   new SecretProps
                                   {
                                       SecretName = $"{environmentName}/RepoDashboard/Authentication__AccessToken"
                                   });

        newSecret.GrantRead(function);


        var api = new LambdaRestApi(this,
                                    "RepoDashboardApiGateway",
                                    new LambdaRestApiProps
                                    {
                                        Handler = function,
                                        Proxy = true,
                                        DefaultMethodOptions = new MethodOptions { ApiKeyRequired = false }
                                    });
    }
}