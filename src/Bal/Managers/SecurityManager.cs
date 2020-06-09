using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using GranadaCoder.IdentityDemo.Bal.Managers.Interfaces;

namespace GranadaCoder.IdentityDemo.Bal.Managers
{
    public class SecurityManager : ISecurityManager
    {
        private readonly IPrincipal principal;

        public SecurityManager(IPrincipal principal)
        {
            this.principal = principal;
        }

        public bool IsAdminAkaRoot()
        {
            bool returnValue =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                new WindowsPrincipal(WindowsIdentity.GetCurrent())
                    .IsInRole(WindowsBuiltInRole.Administrator) :
                Mono.Unix.Native.Syscall.geteuid() == 0;

            return returnValue;
        }

        public string GetEnvironmentUserName()
        {
            string domain = Environment.UserDomainName;
            return domain + Environment.UserName;
        }

        public string GetClaimsPrincipalCurrent()
        {
            string returnValue;
            try
            {
                returnValue = System.Security.Claims.ClaimsPrincipal.Current.Identity.Name;
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }

            return returnValue;
        }

        public string GetWindowsIdentityGetCurrent()
        {
            string returnValue;
            try
            {
                returnValue = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }

            return returnValue;
        }

        public string GetThreadCurrentPrincipalIdenityName()
        {
            string returnValue;
            try
            {
                var currentPrincipal = System.Threading.Thread.CurrentPrincipal;
                var ident = currentPrincipal?.Identity;
                returnValue = ident.Name;
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }

            return returnValue;
        }

        public string GetInjectedPrincipalName()
        {
            string returnValue;
            try
            {
                returnValue = this.principal.Identity.Name + ":::isauth?=" + Convert.ToString(this.principal.Identity.IsAuthenticated);
            }
            catch (Exception ex)
            {
                returnValue = ex.Message;
            }

            return returnValue;
        }
    }
}
