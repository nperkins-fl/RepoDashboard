// See https://aka.ms/new-console-template for more information

using Amazon.CDK;
using RepoDashboard.Deploy;

var app = new App();

new RepoDashboardStack(app, 
                       "RepoDashboardStack", 
                       new StackProps());

app.Synth();