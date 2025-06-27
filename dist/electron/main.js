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