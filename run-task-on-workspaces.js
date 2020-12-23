#!/usr/bin/env node

if (process.argv.length < 3) {
    console.log('You have to specify what workspace task to run on all')
    console.log('\nUsage: run-task-on-workspaces [task] [arguments]');
    console.log('\nExamples of tasks: build|test|ci');
    process.exit(1);
    return;
}

const path = require('path');
const fs = require('fs');
const spawn = require('child_process').spawnSync;

const workspacesAsString = spawn('yarn', ['workspaces', 'info']).stdout.toString();

let stripped = workspacesAsString.substr(workspacesAsString.indexOf('{'));
stripped = stripped.substr(0, stripped.lastIndexOf('}') + 1);
const workspacesInfo = JSON.parse(stripped);

const task = process.argv[2];
args = process.argv.slice(3, process.argv.length);

console.log(`Performing '${task}' on workspaces`);

if (args.length > 0) {
    console.log(`  Using args : ${args}`);
}

console.log('');

for (const workspaceName in workspacesInfo) {
    const workspace = workspacesInfo[workspaceName];
    const workspaceAbsoluteLocation = path.join(process.cwd(), workspace.location);

    if (task === 'publish-version') {
        if (args.length === 1) {
            console.log(`Publishing workspace '${workspaceName}' at '${workspace.location}'`);
            const result = spawn('yarn', ['publish', '--new-version', args[0]], { cwd: workspaceAbsoluteLocation });
            console.log(result.stdout.toString());
            if (result.status !== 0) {
                process.exit(1);
                return;
            }
        }
    } else {
        const packageJsonFile = path.join(workspaceAbsoluteLocation, 'package.json');

        if (fs.existsSync(packageJsonFile)) {
            const packageJson = JSON.parse(fs.readFileSync(packageJsonFile).toString());

            if (!packageJson.scripts || !packageJson.scripts.hasOwnProperty(task)) {
                console.log(`Skipping workspace '${workspaceName}' - no script with name '${task}'`);
                continue;
            }

            console.log(`Workspace '${workspaceName}' at '${workspace.location}'`);

            const result = spawn('yarn', [task], { cwd: workspaceAbsoluteLocation });
            console.log(result.stdout.toString());
            if (result.status !== 0) {
                process.exit(1);
                return;
            }
        }
    }
}
