// See https://aka.ms/new-console-template for more information
using ADOBulkChangeLinkTypes;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System.ComponentModel;

LinkWorker worker = new LinkWorker("config.json");

Console.WriteLine("\n------------------------------------------------");
Console.WriteLine("Goofy Little Link Changer");
Console.WriteLine("------------------------------------------------");
Console.WriteLine("This little bit of code will look through the '{0}' project for '{1}' work items with '{2}' links to '{3}' work items, and replace those links (remove/add) with '{4}' links.",
    worker.projectName,
    worker.sourceWIT,
    worker.sourceLinkType,
    worker.targetWIT,
    worker.targetLinkType);
Console.WriteLine("Press ENTER when ready. (Progress will be shown in the console.)");
Console.ReadLine();
Console.WriteLine("------------------------------------------------\n");

worker.ProcessWorkItems();

Console.WriteLine("\n------------------------------------------------");
Console.WriteLine("Done!");

Console.WriteLine("Press ENTER to quit.");
Console.ReadLine(); // Quit the console app when ENTER is pressed
// Done
