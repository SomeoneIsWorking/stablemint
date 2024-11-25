## For development
- Run frontend
    - `cd` into frontend
    - `npm i`
    - `npm run dev`
    - Keep it running
- Run dotnet project
    - `cd` into the project directory
    - Make sure environment variable is set correctly `ASPNETCORE_ENVIRONMENT=development`
    - Example: `ASPNETCORE_ENVIRONMENT=development dotnet run`

## For production
- Build frontend
    - `cd` into frontend
    - `npm i`
    - `npm run build`
    - Wait for build to finish
- Run dotnet project
    - `cd` into the project directory
    - Make sure environment variable is set correctly `ASPNETCORE_ENVIRONMENT=production`
    - Example: `ASPNETCORE_ENVIRONMENT=production dotnet run`