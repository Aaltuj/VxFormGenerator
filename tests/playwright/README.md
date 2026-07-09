# Playwright Integration Tests

These tests validate the Blazor WebAssembly demo in a real browser.

Run from the repository root with:

```bash
docker build -f tests/playwright/Dockerfile.test -t vxformgenerator-playwright-tests .
docker run --rm vxformgenerator-playwright-tests
```

For local development with Node and Playwright installed:

```bash
npm --prefix tests/playwright install
npx --prefix tests/playwright playwright install chromium
npm --prefix tests/playwright test
```

The test script publishes `VxFormGeneratorDemo.Wasm` to `artifacts/playwright-demo` and serves that static output during the run.
