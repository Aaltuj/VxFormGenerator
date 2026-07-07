import * as fs from 'node:fs';
import * as path from 'node:path';

export interface ScaffoldCommandInput {
  workspacePath: string;
  assemblyPath: string;
  typeName: string;
  outputPath: string;
  componentName?: string;
  noFieldTemplate: boolean;
  dotnetCommand: string;
  toolProjectPath: string;
  vxformCommand: string;
}

export interface ScaffoldCommand {
  command: string;
  args: string[];
}

export function buildScaffoldCommand(input: ScaffoldCommandInput): ScaffoldCommand {
  const fullToolProjectPath = path.join(input.workspacePath, input.toolProjectPath);
  const useSourceTool = fs.existsSync(fullToolProjectPath);

  const command = useSourceTool ? input.dotnetCommand : input.vxformCommand;
  const args = useSourceTool
    ? ['run', '--project', fullToolProjectPath, '--', 'scaffold']
    : ['scaffold'];

  args.push('--assembly', input.assemblyPath);
  args.push('--type', input.typeName.trim());
  args.push('--output', input.outputPath);

  if (input.componentName?.trim()) {
    args.push('--component', input.componentName.trim());
  }

  if (input.noFieldTemplate) {
    args.push('--no-field-template');
  }

  return { command, args };
}
