# Text Life RPG

**Text Life RPG** is an open source .NET-based text life simulation game designed with Clean Architecture principles. It blends EF Core for static game data and JSON for save data, offering a flexible and testable game engine. Unit-tested and modular, this is your foundation for a rich, character-driven RPG.

## Architecture Overview

This project follows Clean Architecture principles, splitting responsibilities into layers:

- **Domain**: Core game rules and entities
- **Application**: Use cases and service interfaces
- **Infrastructure**:
  - **EF Core** for static data
  - **JSON** for save/load
- **Presentation**: Blazor (wrapped in Electron for offline native desktop)

## Dependencies

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Node.js + NPM](https://nodejs.org/)
- Electron (installed via `npm install`)

## How to publish

Go inside the dist/electron folder and run this in a terminal :
```
npm install
```

Then install electron packager :
```
npm install -g @electron/packager
```

Then run
```
dotnet publish ../../src/TextLifeRpg.Blazor -c Release -r win-x64 --self-contained true -o ./server
npx @electron/packager . TextLifeRpg --platform=win32 --arch=x64 --overwrite
```

Your game will be available in the ``TextLifeRpg-win32-x64`` folder.
