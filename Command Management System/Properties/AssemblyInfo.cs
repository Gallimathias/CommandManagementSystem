using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Command Management System-alpha")]
[assembly: AssemblyDescription("A system for executable and extensible commands as events. This is an alpha version which is still in development. Please report error on GitHub.")]
#region AssemblyConfiguration
#if V47
[assembly: AssemblyConfiguration(".NET 4.7")]
#elif V462
[assembly: AssemblyConfiguration(".NET 4.6.2")]
#elif V461
[assembly: AssemblyConfiguration(".NET 4.6.1")]
#elif V46
[assembly: AssemblyConfiguration(".NET 4.6")]
#elif V452
[assembly: AssemblyConfiguration(".NET 4.5.2")]
#elif V451
[assembly: AssemblyConfiguration(".NET 4.5.1")]
#elif V45
[assembly: AssemblyConfiguration(".NET 4.5")]
#endif
#endregion AssemblyConfiguration
[assembly: AssemblyCompany("www.gallimathias.de")]
[assembly: AssemblyProduct("Command Management System-alpha")]
[assembly: AssemblyCopyright("Copyright © All rights reserved 2017")]

[assembly: ComVisible(false)]

[assembly: Guid("d20367e9-5af9-4024-b22f-3ac6acd8dd36")]

#region version information
/* version information:
*
*     Major release:      Product main version
*     Minor version
*     and revision:       Patch and hotfix level or count. The number is two digits. 
*                         The first digit corresponds to a patch the second digit of a hotfix.
*                         For two or multi-digit patches and hotfix versions, both versions are separated by a null.
*     Build number:       The build number is a consecutive number representing the realeses respectively tags.
*     Additional version: Additional information on the product which are separated by zeros. The first digit is reserved,
*                         from the second digit to the first separating null follows the framework version.    
*
*    Pattern: Major.MinorAndRevision.Build.AdditionalVersion
*    Example: 2.00.1496512783.047
*/
#endregion version information

#pragma warning disable CS7035 // The specified version string is: Major.MinorAndRevision.Build.AdditionalVersion
#region AssemblyVersion
#if V47
[assembly: AssemblyVersion("2.00.7.047")]
[assembly: AssemblyFileVersion("2.00.7.047")]
#elif V462
[assembly: AssemblyVersion("2.00.7.0462")]
[assembly: AssemblyFileVersion("2.00.7.0462")]
#elif V461
[assembly: AssemblyVersion("2.00.7.0461")]
[assembly: AssemblyFileVersion("2.00.7.0461")]
#elif V46
[assembly: AssemblyVersion("2.00.7.046")]
[assembly: AssemblyFileVersion("2.00.7.046")]
#elif V452
[assembly: AssemblyVersion("2.00.7.0452")]
[assembly: AssemblyFileVersion("2.00.7.0452")]
#elif V451
[assembly: AssemblyVersion("2.00.7.0451")]
[assembly: AssemblyFileVersion("2.00.7.0451")]
#elif V45
[assembly: AssemblyVersion("2.00.7.045")]
[assembly: AssemblyFileVersion("2.00.7.045")]
#endif
#endregion AssemblyVersion
[assembly: AssemblyInformationalVersion("Napoleon I. 2.0 - alpha 3")]
#pragma warning restore CS7035 // The specified version string is: Major.MinorAndRevision.Build.AdditionalVersion
