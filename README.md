# Opti Blog

Optimizely CMS 13 (PaaS) blog built for the developer training test.

## Requirements

- .NET SDK 10
- SQL Server LocalDB (`(localdb)\MSSQLLocalDB`)
- An Optimizely Graph instance (app key, secret, single key)

## Setup

1. Create `src/OptiBlog/appsettings.Local.json` (git-ignored) with your Graph keys:

   ```json
   {
     "Optimizely": {
       "ContentGraph": {
         "AppKey": "<app key>",
         "Secret": "<secret>",
         "SingleKey": "<single key>"
       }
     }
   }
   ```

2. Run the site:

   ```bash
   cd src/OptiBlog
   dotnet run --urls http://localhost:5000
   ```

   The `OptiBlog` database is created in LocalDB on first start.

3. Open http://localhost:5000/optimizely/cms and register the first admin user.

The site is live at http://localhost:5000/en/.
