using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace RepoDashboard.Deploy;

public class RepoDashboardStack : Stack
{
    public RepoDashboardStack(Construct scope, string id, IStackProps props)
        : base(scope, id, props)
    {
        var environmentVariables = EnvironmentVariablesFactory.AddEnvironmentVariables(this);

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