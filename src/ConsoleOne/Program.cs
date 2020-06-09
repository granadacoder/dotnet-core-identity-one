namespace GranadaCoder.IdentityDemo.ConsoleOne
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using GranadaCoder.IdentityDemo.Bal.Managers;
    using GranadaCoder.IdentityDemo.Bal.Managers.Interfaces;
    using GranadaCoder.IdentityDemo.Dal;
    using GranadaCoder.IdentityDemo.Dal.Interfaces;
    using GranadaCoder.IdentityDemo.Domain;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Extensions.Logging;

    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Logger lgr = LogManager.GetCurrentClassLogger();
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                await Task.Delay(0);

                IServiceProvider servicesProvider = BuildDi(config);
                using (servicesProvider as IDisposable)
                {                   
                    /*
                    IEmployeeManager empMan = servicesProvider.GetRequiredService<IEmployeeManager>();
                     empMan.DoSomething();

                    Employee emp = await empMan.GetByID(int.MaxValue);
                    ShowEmployee(lgr, "GetByID", emp);
                    ICollection<Employee> emps = await empMan.GetByDateOfBirth(DateTime.MaxValue);
                    ShowEmployeeCollection(lgr, "GetByDateOfBirth", emps);
                    */

                    ISecurityManager secMan = servicesProvider.GetRequiredService<ISecurityManager>();
                    string value = secMan.GetEnvironmentUserName();
                    Console.WriteLine("GetEnvironmentUserName='{0}'", value);

                    value = secMan.GetClaimsPrincipalCurrent();
                    Console.WriteLine("GetClaimsPrincipalCurrent='{0}'", value);

                    value = secMan.GetWindowsIdentityGetCurrent();
                    Console.WriteLine("GetWindowsIdentityGetCurrent='{0}'", value);

                    value = secMan.GetThreadCurrentPrincipalIdenityName();
                    Console.WriteLine("GetThreadCurrentPrincipalIdenityName='{0}'", value);

                    value = secMan.GetInjectedPrincipalName();
                    Console.WriteLine("GetInjectedPrincipalName='{0}'", value);

                    value = Convert.ToString(secMan.IsAdminAkaRoot());
                    Console.WriteLine("IsAdminAkaRoot='{0}'", value);

                    Console.WriteLine("Press ANY key to exit");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                lgr.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }

            Console.WriteLine("Returning 0 and exiting.");

            return 0;
        }

        private static void ShowEmployee(Logger lgr, string label, Employee emp)
        {
            ICollection<Employee> emps = new List<Employee> { emp };
            ShowEmployeeCollection(lgr, label, emps);
        }

        private static void ShowEmployeeCollection(Logger lgr, string label, ICollection<Employee> emps)
        {
            if (null != emps)
            {
                foreach (Employee emp in emps)
                {
                    string msg = string.Format("{0} ::: {1},{2},{3},{4}", label, emp.ID, emp.FirstName, emp.LastName, emp.DateOfBirth);
                    lgr.Info(msg);
                }
            }
        }

        private static IServiceProvider BuildDi(IConfiguration config)
        {
            string[] roles = { "User", "Admin" };
            GenericPrincipal gp = new GenericPrincipal(
        new GenericIdentity("Fred"), roles);

            const string Issuer = "https://someIssuer.com";

            var claims = new List<Claim> 
            {
        new Claim(ClaimTypes.Name, "MyAddTransientIPrincipal", ClaimValueTypes.String, Issuer),
        new Claim(ClaimTypes.Surname, "Lock", ClaimValueTypes.String, Issuer),
        new Claim(ClaimTypes.Country, "UK", ClaimValueTypes.String, Issuer),
        new Claim("ChildhoodHero", "Ronnie James Dio", ClaimValueTypes.String)
    };

            var userIdentity = new ClaimsIdentity(claims, string.Empty);

            var userPrincipal = new ClaimsPrincipal(userIdentity);
            var identity = new GenericIdentity(Environment.UserDomainName + "\\" + Environment.UserName, string.Empty);

            IPrincipal myprinc = new GenericPrincipal(identity, null);

            //AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal);
            AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.UnauthenticatedPrincipal);
            var currentPrincipal = System.Threading.Thread.CurrentPrincipal;
            var ident = currentPrincipal?.Identity;

            return new ServiceCollection()
                .AddSingleton<IEmployeeManager, EmployeeManager>()
                .AddTransient<IEmployeeDataLayer, EmployeeDataLayer>()

                .AddTransient<IPrincipal>(x => userPrincipal)

                .AddSingleton<ISecurityManager, SecurityManager>()

                .AddLogging(loggingBuilder =>
                {
                    // configure Logging with NLog
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
                })

                .AddSingleton<IConfiguration>(config)
                .BuildServiceProvider();
        }
    }
}