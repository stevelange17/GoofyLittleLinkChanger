# GoofyLittleLinkChanger
This is a lightly-tested sample app for changings link types in Azure Boards en masse from one type to another. This can be useful for augmenting a large import of hierarchical work items.

## Setup
You effectively set the following values in the config.json file (to be passed as the only argument to the console application):

```
{
  "orgName": "<org_name>",
  "projectName": "<project name>",
  "sourceWorkItemType": "<source work item type>",
  "targetWorkItemType": "<target work item type>",
  "sourceLinkType": "<link type to look for>",
  "targetLinkType": "<link type to replace with>",
  "personalAccessToken": "<pat>"
}
```

For example, if you specify:
```
{
  "orgName": "MyKillerOrg",
  "projectName": "My Killer Project",
  "sourceWorkItemType": "User Story",
  "targetWorkItemType": "Test Case",
  "sourceLinkType": "System.LinkTypes.Hierarchy-Forward",
  "targetLinkType": "Microsoft.VSTS.Common.TestedBy-Forward",
  "personalAccessToken": "<your pat here>"
}
```

This means you want the app to find all user stories that have parent-child links to test cases and change them to have tests/tested by links.

If you know me or have seen [my LinkedIn profile](https://www.linkedin.com/in/stevenlange/), you know I’m not a professional developer – so you don’t get to make fun of my code. But it works on my machine!
