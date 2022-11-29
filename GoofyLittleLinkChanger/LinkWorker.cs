using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;

namespace ADOBulkChangeLinkTypes
{
    /// <summary>
    /// **SAMPLE** code for finding certain work item relationships/links, removing them, and replacing with new links of preferred type.
    /// </summary>
    public class LinkWorker
    {
        #region Properties
        private readonly Uri uri;
        private readonly string personalAccessToken;
        private readonly string projectName;
        private readonly string sourceWIT;
        private readonly string targetWIT;
        private readonly string sourceLinkType;
        private readonly string targetLinkType;
        private readonly VssBasicCredential credentials;
        private readonly WorkItemTrackingHttpClient witClient;
        #endregion

        public LinkWorker(string orgName, string personalAccessToken, string projectName, string sourceWIT, string destinationWIT, string sourceLinkType, string destinationLinkType)
        {
            uri = new Uri("https://dev.azure.com/" + orgName);
            this.personalAccessToken = personalAccessToken;
            this.projectName = projectName;
            this.sourceWIT = sourceWIT;
            targetWIT = destinationWIT;
            this.sourceLinkType = sourceLinkType;
            targetLinkType = destinationLinkType;

            credentials = new VssBasicCredential(string.Empty, this.personalAccessToken);
            witClient = new WorkItemTrackingHttpClient(uri, credentials);
        }

        /// <summary>
        /// Orchestrates the link-changing process using the specified values in the class' properties.
        /// </summary>
        public void ProcessWorkItems()
        {
            var workItems = QueryWorkItems();

            Console.WriteLine("Query Results: {0} items found", workItems.Count);

            // We need to track both work item Ids and the relationship (link) indexes in order to properly run batch updates
            List<int> targetRelationIndexes = new List<int>();
            List<int> targetWorkItemIds = new List<int>();

            // loop though work items
            foreach (var wi in workItems)
            {
                Console.WriteLine("{0}: {1}", wi.Id, wi.Fields["System.Title"]);
                targetRelationIndexes = new();
                targetWorkItemIds = new();

                if (wi.Relations != null) 
                {
                    WorkItemRelation rel;
                    for (int i = 0; i < wi.Relations.Count; i++)
                    {
                        rel = wi.Relations[i];
                        if (rel.Rel == sourceLinkType) 
                        {
                            var linkedItem = witClient.GetWorkItemAsync(GetWorkItemIdFromUrl(rel.Url)).Result;
                            if (linkedItem.Fields["System.WorkItemType"].ToString() == targetWIT)
                            {
                                targetRelationIndexes.Add(i);
                                targetWorkItemIds.Add(Convert.ToInt32(linkedItem.Id));
                            }
                        }
                    }
                }
                if (targetRelationIndexes.Count > 0)
                {
                    Console.WriteLine("\tFound {0} links to update.", targetRelationIndexes.Count);
                    // Remove current links
                    BulkRemoveSourceRelationships(Convert.ToInt32(wi.Id), targetRelationIndexes);
                    // Add new links
                    BulkAddTargetLinks(Convert.ToInt32(wi.Id), targetWorkItemIds);
                }
                else
                {
                    Console.WriteLine("\tNo links found to update.");
                }
            }
        }

        /// <summary>
        /// Adds links/relationshops from the specified work item Ids to the provided work item Ids
        /// </summary>
        /// <param name="sourceId">The source work item Id</param>
        /// <param name="targetIds">The list of target work item Ids to which to create links/relationships.</param>
        /// <returns></returns>
        WorkItem BulkAddTargetLinks(int sourceId, List<int> targetIds)
        {
            WorkItem targetItem;
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            foreach (int id in targetIds)
            {
                targetItem = witClient.GetWorkItemAsync(id).Result;
                patchDocument.Add(
                   new JsonPatchOperation()
                   {
                       Operation = Operation.Add,
                       Path = "/relations/-",
                       Value = new
                       {
                           rel = targetLinkType,
                           url = targetItem.Url,
                           attributes = new
                           {
                               comment = "Making a new link for tested/tested by"
                           }
                       }
                   }
                );
            }
            return witClient.UpdateWorkItemAsync(patchDocument, sourceId).Result;
        }

        /// <summary>
        /// Removes links/relationships from the source work item and specified link indices.
        /// </summary>
        /// <param name="sourceId">The source work item Id</param>
        /// <param name="indexes">List of relations index values to remove</param>
        /// <returns></returns>
        WorkItem BulkRemoveSourceRelationships(int sourceId, List<int> indexes)
        {
            JsonPatchDocument patchDocument = new JsonPatchDocument();
            foreach (int index in indexes)
            {
                patchDocument.Add(
                    new JsonPatchOperation()
                    {
                        Operation = Operation.Remove,
                        Path = string.Format("/relations/{0}", index)
                    }
                );
            }
            return witClient.UpdateWorkItemAsync(patchDocument, sourceId).Result;
        }

        /// <summary>
        /// Queries the specified ADo project for work items of the specified type.
        /// </summary>
        /// <returns>List of work items meeting query criteria</returns>
        public List<WorkItem> QueryWorkItems()
        {
            // create a wiql object and build our query
            var wiql = new Wiql()
            {
                Query = "Select [Id] " +
                        "From WorkItems " +
                        "Where [Work Item Type] = '" + sourceWIT + "' " +
                        "And [System.TeamProject] = '" + projectName + "' " +
                        "Order By [Id] Asc",
            };
            // execute the query to get the list of work items in the results
            var result = witClient.QueryByWiqlAsync(wiql).Result;
            var ids = result.WorkItems.Select(item => item.Id).ToArray();

            // get work items for the ids found in query
            return witClient.GetWorkItemsAsync(ids, expand: WorkItemExpand.Relations).Result;
        }


        static int GetWorkItemIdFromUrl(string Url)
        {
            int id = -1;

            string splitStr = "_apis/wit/workItems/";

            if (Url.Contains(splitStr))
            {
                string[] strarr = Url.Split(new string[] { splitStr }, StringSplitOptions.RemoveEmptyEntries);

                if (strarr.Length == 2 && int.TryParse(strarr[1], out id))
                    return id;
            }

            return id;
        }
    }
}
