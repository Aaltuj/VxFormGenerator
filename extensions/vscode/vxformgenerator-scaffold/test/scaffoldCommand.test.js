const assert = require('node:assert/strict');
const fs = require('node:fs');
const os = require('node:os');
const path = require('node:path');
const test = require('node:test');
const { buildScaffoldCommand } = require('../out/scaffoldCommand');

test('buildScaffoldCommand uses source tool project when present', () => {
  const workspacePath = fs.mkdtempSync(path.join(os.tmpdir(), 'vxform-vscode-'));
  const toolProjectPath = path.join('VxFormGenerator.Tools', 'VxFormGenerator.Tools.csproj');
  const fullToolProjectPath = path.join(workspacePath, toolProjectPath);
  fs.mkdirSync(path.dirname(fullToolProjectPath), { recursive: true });
  fs.writeFileSync(fullToolProjectPath, '<Project />');

  const result = buildScaffoldCommand({
    workspacePath,
    assemblyPath: '/tmp/models.dll',
    typeName: ' Demo.CustomerModel ',
    outputPath: '/tmp/CustomerForm.razor',
    componentName: 'CustomerForm',
    noFieldTemplate: false,
    dotnetCommand: 'dotnet',
    toolProjectPath,
    vxformCommand: 'vxform'
  });

  assert.equal(result.command, 'dotnet');
  assert.deepEqual(result.args, [
    'run', '--project', fullToolProjectPath, '--', 'scaffold',
    '--assembly', '/tmp/models.dll',
    '--type', 'Demo.CustomerModel',
    '--output', '/tmp/CustomerForm.razor',
    '--component', 'CustomerForm'
  ]);
});

test('buildScaffoldCommand falls back to installed tool', () => {
  const result = buildScaffoldCommand({
    workspacePath: '/tmp/missing-workspace',
    assemblyPath: '/tmp/models.dll',
    typeName: 'Demo.CustomerModel',
    outputPath: '/tmp/CustomerForm.razor',
    noFieldTemplate: true,
    dotnetCommand: 'dotnet',
    toolProjectPath: 'missing.csproj',
    vxformCommand: 'vxform'
  });

  assert.equal(result.command, 'vxform');
  assert.deepEqual(result.args, [
    'scaffold',
    '--assembly', '/tmp/models.dll',
    '--type', 'Demo.CustomerModel',
    '--output', '/tmp/CustomerForm.razor',
    '--no-field-template'
  ]);
});
