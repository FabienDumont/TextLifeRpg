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

Create a dist/electron folder at the solution level
In this folder create a package.json file like this :
```
{
  "name": "text-rpg",
  "version": "1.0.0",
  "main": "main.js",
  "scripts": {
    "start": "electron ."
  },
  "devDependencies": {
    "electron": "^29.0.0"
  }
}
```

Then create a main.js file like this :
```
const { app, BrowserWindow, screen } = require('electron');
const path = require('path');
const { spawn } = require('child_process');
const http = require('http');

let serverProcess;

function createWindow() {
  const { width, height } = screen.getPrimaryDisplay().workAreaSize;
  const win = new BrowserWindow({
    width,
    height,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true
    }
  });

  // Keep pinging localhost:5000 until it's ready
  const checkServer = () => {
    http.get('http://localhost:5000', (res) => {
      if (res.statusCode === 200) {
        win.loadURL('http://localhost:5000');
      } else {
        setTimeout(checkServer, 500);
      }
    }).on('error', () => {
      setTimeout(checkServer, 500);
    });
  };

  checkServer();
}

app.whenReady().then(() => {
  const serverExePath = path.join(__dirname, 'server', 'TextLifeRpg.Blazor.exe');
  serverProcess = spawn(serverExePath, [], {
    cwd: path.dirname(serverExePath),
    windowsHide: true
  });

  serverProcess.stdout.on('data', (data) => {
    console.log(`[server] ${data}`);
  });

  serverProcess.stderr.on('data', (data) => {
    console.error(`[server error] ${data}`);
  });

  createWindow();
});

app.on('window-all-closed', () => {
  if (serverProcess) serverProcess.kill();
  if (process.platform !== 'darwin') app.quit();
});
```

Open a cmd in this folder and run :
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
