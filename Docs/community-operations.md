# VxFormGenerator Community Operations

This document defines the initial community-hygiene baseline for VxFormGenerator.

## Channel Decision

GitHub Discussions should stay disabled for now.

Rationale:

- Current public support traffic is already routed through GitHub issues and Discord.
- A low-volume project gets more maintainer overhead, not less, from another channel to watch.
- The immediate gap is missing routing and governance, not missing conversation volume.

Revisit enabling Discussions when at least one of these is true for two straight
monthly snapshots:

- Support questions are regularly being redirected out of GitHub issues.
- Discord questions are repeating often enough to justify a searchable FAQ layer.
- Maintainers can name an owner for moderation and a weekly response cadence.

## Public Repo Baseline

Add and maintain these assets:

- `CONTRIBUTING.md` for routing, contribution expectations, and local build/test notes.
- `CODE_OF_CONDUCT.md` for behavior standards and moderation posture.
- `SECURITY.md` for private vulnerability reporting guidance.
- `.github/ISSUE_TEMPLATE/` forms for bug reports and feature requests.
- `.github/ISSUE_TEMPLATE/config.yml` contact links that route support questions away from issue spam.

## Intake Rules

- Bugs belong in GitHub issues when the reporter can describe expected versus actual behavior.
- Feature requests belong in GitHub issues when they describe the user problem and use case.
- Support questions belong in Discord, not in GitHub issues.
- Security reports require a private channel and should never be filed publicly.

## Operating Cadence

Recommended ownership model:

- Decision owner for channel hygiene: CMO
- Technical responder for confirmed bugs and roadmap-bound answers: engineering maintainer
- Escalation owner for product or priority changes: CEO

Cadence:

- GitHub triage: twice weekly
- Discord sweep: twice weekly on the same days as GitHub triage
- Monthly snapshot: first business week of the month

## Monthly Snapshot Format

Capture one short note with:

- Time window covered
- New GitHub issues opened
- New GitHub issues closed
- Number of support questions redirected to Discord
- Repeated questions worth turning into docs
- Any conduct, moderation, or spam incidents
- Recommendation: keep current setup, enable Discussions, expand docs, or adjust triage ownership

## Smallest Validation Path

Before wider community promotion:

1. Publish the governance docs and issue forms.
2. Confirm the contact links render correctly on GitHub.
3. Run one month of triage with the new routing.
4. Review whether issue quality improved and whether Discord became a support bottleneck.
