import * as cp from 'node:child_process';
import * as path from 'node:path';
import * as vscode from 'vscode';
import { buildScaffoldCommand } from './scaffoldCommand';

export function activate(context: vscode.ExtensionContext): void {
  context.subscriptions.push(
    vscode.commands.registerCommand('vxformGenerator.scaffoldForm', scaffoldForm)
  );
}

export function deactivate(): void {
}

async function scaffoldForm(): Promise<void> {
  const workspaceFolder = vscode.workspace.workspaceFolders?.[0];
  if (!workspaceFolder) {
    vscode.window.showErrorMessage('Open a workspace before scaffolding a VxFormGenerator form.');
    return;
  }

  const assemblyUri = await vscode.window.showOpenDialog({
    canSelectFiles: true,
    canSelectFolders: false,
    canSelectMany: false,
    filters: {
      Assemblies: ['dll']
    },
    openLabel: 'Select model assembly'
  });

  if (!assemblyUri?.length) {
    return;
  }

  const typeName = await vscode.window.showInputBox({
    prompt: 'Full model type name',
    placeHolder: 'MyApp.Forms.CustomerFormModel',
    validateInput: value => value.trim().length === 0 ? 'Type name is required.' : undefined
  });

  if (!typeName) {
    return;
  }

  const outputUri = await vscode.window.showSaveDialog({
    defaultUri: vscode.Uri.joinPath(workspaceFolder.uri, 'Forms', `${typeName.split('.').pop()?.replace(/Model$/, '') ?? 'Generated'}Form.razor`),
    filters: {
      Razor: ['razor']
    },
    saveLabel: 'Generate Razor component'
  });

  if (!outputUri) {
    return;
  }

  const componentName = await vscode.window.showInputBox({
    prompt: 'Optional component name',
    placeHolder: path.basename(outputUri.fsPath, '.razor')
  });

  const templateChoice = await vscode.window.showQuickPick(
    [
      { label: 'Include FieldTemplate', noFieldTemplate: false },
      { label: 'Generated default markup only', noFieldTemplate: true }
    ],
    { placeHolder: 'Choose scaffold style' }
  );

  if (!templateChoice) {
    return;
  }

  const config = vscode.workspace.getConfiguration('vxFormGenerator');
  const dotnetCommand = config.get<string>('dotnetCommand', 'dotnet');
  const toolProjectPath = config.get<string>('toolProjectPath', 'VxFormGenerator.Tools/VxFormGenerator.Tools.csproj');
  const vxformCommand = config.get<string>('vxformCommand', 'vxform');
  const { command, args } = buildScaffoldCommand({
    workspacePath: workspaceFolder.uri.fsPath,
    assemblyPath: assemblyUri[0].fsPath,
    typeName,
    outputPath: outputUri.fsPath,
    componentName,
    noFieldTemplate: templateChoice.noFieldTemplate,
    dotnetCommand,
    toolProjectPath,
    vxformCommand
  });

  await runCommand(command, args, workspaceFolder.uri.fsPath);

  const document = await vscode.workspace.openTextDocument(outputUri);
  await vscode.window.showTextDocument(document);
  vscode.window.showInformationMessage(`Generated ${path.basename(outputUri.fsPath)}.`);
}

function runCommand(command: string, args: string[], cwd: string): Promise<void> {
  return new Promise((resolve, reject) => {
    const output = vscode.window.createOutputChannel('VxFormGenerator');
    output.show(true);
    output.appendLine(`> ${command} ${args.map(quoteIfNeeded).join(' ')}`);

    const child = cp.spawn(command, args, { cwd, shell: process.platform === 'win32' });

    child.stdout.on('data', chunk => output.append(chunk.toString()));
    child.stderr.on('data', chunk => output.append(chunk.toString()));
    child.on('error', error => {
      vscode.window.showErrorMessage(`VxFormGenerator scaffold failed: ${error.message}`);
      reject(error);
    });
    child.on('close', code => {
      if (code === 0) {
        resolve();
        return;
      }

      const error = new Error(`VxFormGenerator scaffold exited with code ${code}.`);
      vscode.window.showErrorMessage(error.message);
      reject(error);
    });
  });
}

function quoteIfNeeded(value: string): string {
  return value.includes(' ') ? `"${value}"` : value;
}
