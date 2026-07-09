const { defineConfig, devices } = require('@playwright/test');

const port = process.env.VXFORM_DEMO_PORT || '5100';
const baseURL = process.env.VXFORM_DEMO_BASE_URL || `http://127.0.0.1:${port}`;

module.exports = defineConfig({
  testDir: './specs',
  timeout: 180_000,
  expect: {
    timeout: 60_000
  },
  use: {
    baseURL,
    trace: 'on-first-retry'
  },
  webServer: {
    command: `npx serve -s ../../artifacts/playwright-demo/wwwroot -l tcp://127.0.0.1:${port} --no-clipboard`,
    url: baseURL,
    reuseExistingServer: !process.env.CI,
    timeout: 180_000,
    stdout: 'pipe',
    stderr: 'pipe'
  },
  projects: [
    {
      name: 'chromium-desktop',
      use: {
        ...devices['Desktop Chrome'],
        viewport: { width: 1280, height: 900 }
      }
    },
    {
      name: 'chromium-mobile',
      use: {
        ...devices['Pixel 5']
      }
    }
  ]
});
