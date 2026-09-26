# Microstate Engineering — Project Guidance

This is the durable project guidance supplied by the project owner. Apply it to work throughout this repository. It describes the intended direction; inspect current code before assuming a feature or technology is implemented.

## Project goal

Microstate Engineering is both an evolving scientific/research concept and a software platform supporting that research. The long-term ambition is a serious scientific software platform and potentially a high-tech startup. Software and research concepts evolve together. Avoid over-engineering abstractions for experimentally evolving concepts.

## Application scope and current priority

Keep the application focused on two major concepts:

- **Workflows / Research Definitions:** define what should be executed. A Research Definition contains workflow information such as name, basis set, package root, and supported process types. The model will evolve with the research.
- **Runs:** execute a selected workflow/research definition and eventually expose execution status, success/failure, errors, incomplete or missing data, processing outcomes, and diagnostics.

Do not introduce Molecules as a major UI/domain section for now. Molecular data becomes very large and detailed. The application should ultimately concentrate on analysis results rather than exposing all raw molecular information.

## Frontend

- Angular is a real application, not a tutorial or demonstration project.
- Use Bootstrap / ng-bootstrap for UI and REST communication with the .NET WebAPI.
- Angular and .NET live in this repository and should evolve together.
- The first functional UI centers on listing, adding, editing, and deleting Research Definitions.
- Edit Research Definitions as structured objects/forms. Direct YAML editing in the GUI was considered and abandoned.
- Keep frontend architecture straightforward while the domain evolves.

## Backend and development environment

- The backend is .NET, with a WebAPI used by Angular. Research Definition functionality is already exposed; Angular/WebAPI CORS has already been addressed.
- The broader backend contains or has experimented with ASP.NET Core, BackgroundService, MongoDB, Hangfire, RabbitMQ, Dapr, Serilog, and Seq. Do not assume all these technologies are currently present or required for a new feature.
- Prefer the simplest architecture appropriate to the requirement.
- Aspire orchestrates the development environment: Angular → .NET WebAPI → backend/services. It should support launching the relevant applications together and inspecting them through the Aspire development environment.

## Scientific direction

Investigate molecular/electronic behavior at the orbital level rather than starting from predefined chemical concepts. Research themes include orbital populations, orbital overlaps/couplings, electronic response to adding or removing an electron, and comparison of neutral, +1 electron, and −1 electron states. Discover response patterns algorithmically rather than imposing conventional chemical categories beforehand.

The **Response Graph** is experimental: nodes represent orbitals; node properties may represent population changes; edges may represent changes in coupling/overlap; neutral and ±1 electron calculations can be compared. Response patterns should emerge from data. Do not hard-code speculative scientific classifications into the architecture unless explicitly requested.

## Quantum chemistry pipeline

GAMESS is an important computational package. The broader experimental pipeline includes:

XYZ → GAMESS optimization → parse results → JSON representation → generate derived calculations → GAMESS calculations → enrich results → Microstate analysis.

Explored calculations include RHF, DFT/B3LYP, neutral states, +1 electron states, −1 electron states, CHELPG, and GEODISK. Keep GAMESS output parsing and computational infrastructure reasonably decoupled from the Angular UI.

## Development philosophy and working style

- Build a small working feature, validate it, understand its fit with the research, then extend it.
- Inspect the current repository and respect its structure instead of recreating it from assumptions.
- Make focused changes; avoid unrelated refactoring and large amounts of speculative architecture or code.
- Design significant new components at a useful level before implementing large changes. Architectural/design discussions do not automatically authorize implementation.
- Refactoring is expected as research evolves. Favor clarity and maintainability over prematurely generic frameworks.
- Explain significant architectural choices, report the important files added/changed, and state assumptions or uncertainties.
- Treat iterative side questions separately unless the user explicitly changes the current design.
- Prefer concise answers unless more explanation is requested.

## GitHub workflow

Repository: https://github.com/Microcanonix/MicrostateEngineering

For code changes:

1. Read the current default branch.
2. Create a dedicated feature branch (normally `codex/<descriptive-name>`).
3. Make changes only on that branch; never make development changes directly on the default/main branch.
4. Clearly explain what changed.
5. Leave changes for the user to review.
6. The user performs/controls the merge; do not merge on their behalf.
