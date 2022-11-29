// See https://aka.ms/new-console-template for more information
using ADOBulkChangeLinkTypes;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System.ComponentModel;

const string orgName = "<your org name>";   // https://dev.azure.com/<orgName>
const string projectName = "<your project name>";    // Name of the project to query
const string sourceWorkItemType = "User Story"; // Name of the work item type to search for
const string targetWorkItemType = "Test Case";  // Name of the work item type to find on the other end of the link
const string sourceLinkType = "System.LinkTypes.Hierarchy-Forward"; // Name of the link type to change from
const string targetLinkType = "Microsoft.VSTS.Common.TestedBy-Forward"; // Name of the link type to change to
const string personalAccessToken = "<your PAT>";  // PAT for Azure DevOps


Console.WriteLine("\n------------------------------------------------");
Console.WriteLine("Goofy Little Link Changer");
Console.WriteLine("------------------------------------------------");
Console.WriteLine("This little bit of code will look through the '{0}' project for '{1}' work items with '{2}' links to '{3}' work items, and replace those links (remove/add) with '{4}' links.",
    projectName,
    sourceWorkItemType,
    sourceLinkType,
    targetWorkItemType,
    targetLinkType);
Console.WriteLine("Press ENTER when ready. (Progress will be shown in the console.)");
Console.ReadLine();
Console.WriteLine("------------------------------------------------\n");

// Initialize and get to work
LinkWorker worker = new LinkWorker(
    orgName, 
    personalAccessToken, 
    projectName,
    sourceWorkItemType,
    targetWorkItemType,
    sourceLinkType,
    targetLinkType);
worker.ProcessWorkItems();

Console.WriteLine("\n------------------------------------------------");
Console.WriteLine("Done!");

Console.WriteLine("Press ENTER to quit.");
Console.ReadLine(); // Quit the console app when ENTER is pressed
// Done
