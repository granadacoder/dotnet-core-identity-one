using System;

namespace GranadaCoder.IdentityDemo.Bal.Managers.Interfaces
{
    public interface ISecurityManager
    {
        string GetEnvironmentUserName();

        string GetClaimsPrincipalCurrent();

        string GetWindowsIdentityGetCurrent();

        string GetThreadCurrentPrincipalIdenityName();

        string GetInjectedPrincipalName();

        bool IsAdminAkaRoot();
    }
}
