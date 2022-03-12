# Slateplan

Slateplan is a schematic and budgeting tool for home automation technology.

- **[Pipelines](https://dev.azure.com/iticentral/Slateplan/_build)**
- **[Jira](https://itidev.atlassian.net/jira/software/c/projects/SLP/boards/33)**
- **[SharePoint (Documentation)](https://iticentral.sharepoint.com/sites/Client-System7/Shared%20Documents/Forms/AllItems.aspx)**
- **Production Support document:** `ITI Network Drive/Clients/System 7/Slateplan/Production Support.md`

## Prerequisites

- Visual Studio 2022 or newer
- SQL Server
- Node (latest LTS)
- Yarn

## Running the Backend

1. Open the solution.
2. Build the solution.
3. Run `TestDataLoader` from the command line or through Visual Studio.
4. Run `FunctionApp`.

## Running the UI

1. (Optional) Open `UI/slateplan.code-workspace` in VS Code.
2. `cd UI/react-app`
3. `yarn`
4. `yarn start`

### Dev Users

You can log in as one of the following users while developing. The password is
`LetMeIn98` for all users.

| Email                     | Role              |
| ------------------------- | ----------------- |
| System7Admin@example2.com | SystemAdmin       |
| SBeer@example2.com        | OrganizationAdmin |
| CWeimann@example2.com     | BasicUser         |
