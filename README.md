# GoofyLittleLinkChanger
This is a lightly-tested sample app for changings link types in Azure Boards en masse from one type to another. This can be useful for augmenting a large import of hierarchical work items.

## Setup
You effectively set the following values in the app code (besides the ADO org, project, and personal access token):

- sourceWorkitemType: What work item type to look for as the source linked item
- targetWorkitemType: What work item type to look for as the target linked item
- sourceLinkType: What link type you don’t want
- targetLinkType: What link type you do want

Yes, they’re hard-coded, but I’m keeping this super-simple, remember?

For example, if you specify:

```
const string sourceWorkItemType = "User Story";
const string targetWorkItemType = "Test Case"; 
const string sourceLinkType = "System.LinkTypes.Hierarchy-Forward"; 
const string targetLinkType = "Microsoft.VSTS.Common.TestedBy-Forward"
```
This means you want the app to find all user stories that have parent-child links to test cases and change them to have tests/tested by links.

If you know me or have seen [my LinkedIn profile](https://www.linkedin.com/in/stevenlange/), you know I’m not a professional developer – so you don’t get to make fun of my code. But it works on my machine!
