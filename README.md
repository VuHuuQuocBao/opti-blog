# Opti Blog

Optimizely CMS 13 (PaaS) blog built for the developer training test.

## Run (Windows)

1. Create `src/OptiBlog/appsettings.Local.json` (git-ignored) with your Optimizely Graph keys:

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

2. Open **PowerShell as Administrator** in the repo root and run:

   ```powershell
   Set-ExecutionPolicy -Scope Process Bypass
   .\setup.ps1
   ```

   It installs the .NET 10 SDK and SQL Server LocalDB if missing, starts LocalDB, and runs the site. The `OptiBlog` database is created on first start.

3. Open http://localhost:5000/optimizely/cms and register the first admin user. The site is at http://localhost:5000/en/.

Stop the site with **Ctrl+C**.
