# GitHub Community Triage And Response Playbook

## Objective

Give VxFormGenerator a lightweight GitHub community workflow that keeps contributor response times credible without creating maintainer support debt.

## Decision

- Directly responsible agent: CMO owns inbound GitHub community triage and non-technical responses.
- Technical authority: engineering owns product correctness, bug confirmation, API behavior, roadmap commitments, and code changes.
- Operating model: manual triage first, no new hire now, and no public promises that exceed current maintainer capacity.

## Scope

This playbook covers:

- GitHub issues
- Pull request conversation threads
- GitHub Discussions if enabled later

This playbook does not cover:

- Discord moderation
- Production incidents
- Security disclosures
- Release engineering

## Response Targets

These targets fit the current repo scale and part-time operating model.

- First human response to new issue or PR comment: within 2 business days
- Triage classification applied: within 3 business days
- Engineering escalation for plausible bug, API ambiguity, or feature request needing product input: within 3 business days
- Community follow-up after engineering answer lands: within 1 business day

If the team cannot meet these targets for two consecutive weeks, reduce public responsiveness claims and simplify the workflow before adding channels.

## Cadence

- Monday, Wednesday, Friday: CMO checks GitHub inbox and open conversations
- Same pass: acknowledge new items, classify them, and route owner/action
- Weekly: 15-minute review of unresolved items older than 7 days
- Monthly: quick metric readout to CEO on volume, response time, and unresolved engineering escalations

## Intake Categories

Every new thread should be placed in one primary category.

### 1. Usage Question

Examples:

- setup confusion
- documentation gap
- expected usage of metadata rendering
- extension/scaffolding questions

Owner:

- CMO for first response
- engineering only if docs do not answer the question or the answer requires technical confirmation

### 2. Bug Report

Examples:

- broken behavior in `net10.0`
- metadata rendering defect
- Blazor Server/WASM regression
- validation or layout bug

Owner:

- CMO acknowledges and requests reproduction details
- engineering confirms, scopes, and fixes or declines

### 3. Feature Request

Examples:

- new renderer capability
- dynamic metadata enhancement
- package or tooling improvement

Owner:

- CMO acknowledges, tests for audience fit, and avoids roadmap promises
- engineering/product confirms feasibility and priority

### 4. Pull Request Contribution

Examples:

- external bug fix
- documentation contribution
- enhancement PR

Owner:

- CMO thanks contributor, confirms review path, and keeps expectations realistic
- engineering reviews code and requests changes

### 5. Support Noise / Out Of Scope

Examples:

- unrelated framework issues
- duplicate reports
- low-information complaints with no reproducible detail

Owner:

- CMO closes the loop politely
- engineering is not pulled in unless a real product signal appears

### 6. Security / Sensitive Report

Owner:

- do not discuss publicly
- route immediately to engineering/leadership through approved private channel

## Triage Flow

1. Confirm whether the item is understandable enough to route.
2. Send a first response that does one of three things:
   - acknowledges and routes
   - requests missing reproduction detail
   - redirects as out of scope
3. Apply the category and ownership decision in the thread.
4. Escalate only when technical judgment is required.
5. Close the loop after engineering responds so the contributor is not left reading internal handoff language.

## Ownership Boundaries

CMO owns:

- acknowledgment and tone
- expectation setting
- duplicate detection
- asking for missing reproduction detail
- routing and follow-up
- documentation-gap identification
- monthly signal summary

Engineering owns:

- confirming bugs
- explaining technical limitations
- evaluating PR code quality
- making or rejecting roadmap commitments
- security handling
- merge decisions

CEO owns:

- headcount and budget changes
- priority calls when community asks conflict with roadmap

## Escalation Rules

Escalate to engineering when any of the following are true:

- a report includes a plausible reproduction of broken library behavior
- a question requires a technical claim not already documented
- a contributor asks for roadmap commitment, version timing, or compatibility guarantees
- a PR is ready for code review
- a thread suggests security, privacy, or licensing risk

Do not escalate when:

- the answer is already in docs or README
- the issue is a duplicate with a clear canonical thread
- the user has not provided minimum context after one follow-up request
- the request is clearly outside VxFormGenerator scope

## Minimum Information Standard For Bugs

Before asking engineering to engage, CMO should try to collect:

- package and runtime version
- host type: Blazor Server, WebAssembly, or other
- expected behavior
- actual behavior
- minimal reproduction steps
- screenshot or error text if applicable

If this minimum is missing, respond once with a structured ask. If there is no reply after 7 days, close politely or allow stale handling to proceed.

## PR Conversation Workflow

- Acknowledge contributor within 2 business days
- Confirm whether the PR appears in scope
- State that technical review is pending engineering review, not guaranteed merge
- If blocked on tests or scope mismatch, ask for the smallest next change
- If stale, comment before closure so the contributor understands the reason

## Suggested Response Patterns

### Usage Question

Thanks for the report. I am triaging this for the team. Based on what you described, this looks like a usage/docs question rather than a confirmed library defect. If you can share your runtime target and a minimal example, we can confirm whether the current metadata-based path supports your case.

### Bug Intake

Thanks for the detailed report. I am routing this for technical review. To help engineering confirm it quickly, please add the package version, host type, expected behavior, actual behavior, and the smallest reproduction you can share.

### Feature Request

Thanks for the suggestion. I am logging this as a feature request, but we are not making a roadmap commitment in this thread yet. The most useful next detail is the user outcome you need and whether the current metadata-based form path blocks adoption.

### PR Acknowledgment

Thanks for the contribution. I have triaged this and queued it for engineering review. Review timing depends on maintainer capacity, so we will avoid promising merge timing until the technical pass is complete.

### Out Of Scope / Duplicate

Thanks for taking the time to report this. I am closing this in favor of the linked thread because it covers the same underlying request and keeps follow-up in one place.

## Automation Notes

Current repo state:

- there are GitHub Actions workflows already in place
- there is a stale bot config at `.github/stale.yml`
- the stale bot currently uses the `wontfix` label as its stale label

Risk:

- `wontfix` communicates intent, not inactivity, and may erode contributor trust when used automatically

Smallest validation path:

- keep triage manual first
- change the stale label to `stale` in a follow-up engineering-owned repo hygiene task
- add issue templates only after two weeks of manual triage data shows repeated intake gaps

## Metrics

Track monthly:

- number of new issues
- number of external PRs
- median first-response time
- number of items escalated to engineering
- number of issues closed for missing information
- number of documentation-gap signals

Decision rule:

- if median first response exceeds 2 business days for two straight months, either reduce channels, simplify the process, or ask CEO to approve more operating capacity

## Tradeoffs

- Manual triage preserves flexibility and avoids premature process overhead
- Response targets are conservative enough for current maintainer bandwidth, but they will not feel real-time
- Deferring issue templates avoids setup churn now, but it means the CMO must manually normalize low-quality reports at the start

## Immediate Next Action

- CMO begins operating this workflow for GitHub inbound items
- Engineering should take one small follow-up repo hygiene task: replace the stale bot's `wontfix` stale label with `stale`
- CEO only needs to re-open the staffing question if monthly metrics show sustained response target misses or contributor volume growth
