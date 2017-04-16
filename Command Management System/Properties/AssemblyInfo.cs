using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über die folgenden 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.
[assembly: AssemblyTitle("Command Management System")]
[assembly: AssemblyDescription("A system for executable and extensible commands as events")]
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
[assembly: AssemblyProduct("Command Management System")]
[assembly: AssemblyCopyright("Copyright © All rights reserved 2017")]

// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten.  Wenn Sie auf einen Typ in dieser Assembly von 
// COM aus zugreifen müssen, sollten Sie das ComVisible-Attribut für diesen Typ auf "True" festlegen.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("d20367e9-5af9-4024-b22f-3ac6acd8dd36")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Build- und Revisionsnummern 
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.0.*")]
[assembly: AssemblyFileVersion("2.0-alpha")] //2.0.1514b02.06.01.2017
[assembly: AssemblyInformationalVersion("Napoleon I. 2.0 - alpha")]
