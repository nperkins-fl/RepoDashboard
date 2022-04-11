using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RepoDashboard.Services;

namespace RepoDashboard;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddOptions()
            .Configure<TeamSettings>(Configuration.GetSection("Team"))
            .Configure<AuthenticationSettings>(Configuration.GetSection("Authentication"))
            .AddSingleton(HttpClientFactory)
            .AddScoped<GithubProxy>()
            .AddRazorPages();
    }

    private static HttpClient HttpClientFactory(IServiceProvider serviceProvider)
    {
        var authenticationSettings = serviceProvider.GetService<IOptions<AuthenticationSettings>>().Value;

        var client = new HttpClient
                     {
                         BaseAddress = new Uri("https://api.github.com")
                     };

        client.DefaultRequestHeaders.UserAgent.ParseAdd(authenticationSettings.UserAgent);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("token", authenticationSettings.AccessToken);

        return client;
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
    }
}