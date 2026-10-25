# MicrostateEngineering Repository Analysis

**Generated**: 2024-10-25  
**Repository**: https://github.com/Microcanonix/MicrostateEngineering  
**Purpose**: Quantum chemistry research platform for investigating molecular/electronic behavior at the orbital level

---

## Project Overview

Microstate Engineering is both an evolving scientific/research concept and a software platform supporting that research. The long-term ambition is a serious scientific software platform and potentially a high-tech startup.

**Core Focus Areas** (per AGENTS.md):
- **Workflows / Research Definitions**: Define what should be executed (name, basis set, package root, supported process types)
- **Runs**: Execute selected workflows/research definitions, expose execution status, success/failure, errors, incomplete data, processing outcomes, diagnostics

**Scientific Direction**:
- Investigate molecular/electronic behavior at the orbital level
- Research themes: orbital populations, orbital overlaps/couplings, electronic response to adding/removing electrons
- Compare neutral, +1 electron, and -1 electron states
- Discover response patterns algorithmically (not hard-coded classifications)

**Response Graph** (experimental):
- Nodes represent orbitals
- Node properties may represent population changes
- Edges may represent changes in coupling/overlap
- Response patterns should emerge from data

---

## Architecture & Technology Stack

| Layer | Technology | Purpose |
|-------|------------|---------|
| Frontend | Angular 18 + Bootstrap/ng-bootstrap | UI for managing research definitions and workflow runs |
| Backend | .NET 8 WebAPI | REST API, CORS-enabled for Angular |
| Orchestration | .NET Aspire | Development environment (Angular → WebAPI → backend/services) |
| Data Storage | YAML files (primary), MongoDB (experimental) | Research definitions stored as YAML |
| Workflow Engine | Custom graph-based (WorkflowGraph) | Directed graphs for process steps and dependencies |
| Quantum Chemistry | GAMESS integration | RHF, DFT/B3LYP, CHELPG, GEODISK calculations |

---

## Application Structure

### Solution Structure (MicrostateEngineering.slnx)

```
/Common
  /CommonDomain
/WorkflowGraph
  /Engine
/Molecules
  /Molecule (IMoleculeFactory, IMoleculeRepository, IMoleculeServices, Domain, implementations)
  /Process (IMoleculeProcessFactory, IMoleculeProcessServices, Domain, implementations)
  /ResearchDefinition (Domain, Repository, Service, interfaces)
/ScientificReportDomain
/ScientificReportService
/IScientificReportServices
/Utilities
/UnitTests
/Web (Angular frontend)
/WebApi (.NET WebAPI)
/MainConsole (.NET Console with BackgroundService)
/MicrostateEngineering.AppHost (Aspire AppHost)
```

### Key File Counts
- C# files: 174
- TypeScript files: 12
- Total projects: ~30

---

## Frontend (Web/src/app)

### Routing (app.routes.ts)
```typescript
/
/workflows          -> WorkflowList
/workflows/:name    -> WorkflowEditor
/workflows/:name/run -> WorkflowRun
```

### Features Implemented
✅ Research Definition CRUD (list, add, edit, delete)  
✅ Workflow execution with progress reporting  
✅ Structured forms (not YAML editing)  
✅ Models align with backend domain (ResearchDefinition, ProcessType, StepType)  

### Components
- `WorkflowList`: Lists all research definitions, allows deletion
- `WorkflowEditor`: Edit research definitions as structured objects/forms
- `WorkflowRun`: Execute workflows, display results

### Services
- `ResearchDefinitionService`: CRUD operations for research definitions
- `WorkflowRunService`: Execute workflows, get reports

### Models
- **ProcessType**: Dummy = 0, MoleculeProperties = 1
- **StepType**: Dummy, ImportData, GeometryOptimization, ElectronicStructure, FukuiCalculation, ChargeGeodisk, ChargeChelpg
- **ResearchDefinition**: name, packageRoot, xyzfiles, gmsInput, gmsOutput, workflowStatusFolder, moleculeData, moleculeReport, basisset, molecules[], processes[]
- **WorkflowRunReport**: name, moleculeResult[] (with moleculeName, succeeded)

### Issues
⚠️ **Hardcoded API URL**: Both services use `https://localhost:7094` directly
   - Should use Angular environment files (`environment.ts`/`environment.prod.ts`)
   - Example: `API_URL = environment.apiUrl`

⚠️ **Duplicate Code**: `describeHttpError` function exists in both services
   - Should be in a shared utility file

---

## Backend (WebApi)

### Controllers

#### ResearchDefinitionController
- GET `/ResearchDefinition` - Get all research definitions
- POST `/ResearchDefinition` - Save a research definition
- DELETE `/ResearchDefinition/{researchDefintionName}` - Delete a research definition

#### WorkflowController
- POST `/Workflow?researchDefintionName={name}` - Run a workflow, returns WorkflowRunReport

#### ScientificReportController
- (Details not inspected, but exists)

### Configuration (Program.cs)
- Serilog for logging (console output)
- CORS: AllowAnyOrigin, AllowAnyHeader, AllowAnyMethod (Development only)
- Swagger/OpenAPI enabled in Development
- ResearchDefinitionSettings configured from appsettings.json

### Service Registration (ServiceRegistration.cs)
Chain of registration methods:
```csharp
services
    .RegisterUtilities(serviceLifetime)
    .RegisterResearchDefinitionRepo(serviceLifetime)
    .RegisterResearchDefinitionSvc(serviceLifetime)
    .RegisterMoleculeRepository(serviceLifetime)
    .RegisterMoleculeFactory(serviceLifetime)
    .RegisterMoleculeService(serviceLifetime)
    .RegisterMoleculeProcessService(serviceLifetime)
    .RegisterMoleculeProcessFactory(serviceLifetime)
    .RegisterScientificReportService(serviceLifetime)
```

---

## Domain Models (Molecules/ResearchDefinition)

### MoleculesResearchDefinition (ResearchDefinitionDomain.GamessCalculation)
```csharp
public sealed class MoleculesResearchDefinition
{
    public required string Name { get; set; }
    public required string PackageRoot { get; set; }
    public required string Xyzfiles { get; set; }
    public required string GmsInput { get; set; }
    public required string GmsOutput { get; set; }
    public required string WorkflowStatusFolder { get; set; }
    public required string MoleculeData { get; set; }
    public required string MoleculeReport { get; set; }
    public required string Basisset { get; set; }
    public MoleculesResearchDefinitionMolecule[] Molecules { get; set; } = [];
    public MoleculesResearchDefinitionProcess[] Processes { get; set; } = [];
}
```

### Supporting Models
- **MoleculesResearchDefinitionMolecule**: name, charge
- **MoleculesResearchDefinitionProcess**: type (ProcessType), steps[], dependencies[]
- **MoleculesResearchDefinitionProcessStep**: id, type (StepType), canExecute
- **MoleculesResearchDefinitionProcessDependency**: dependency (StepType), dependant (StepType)

### ResearchDefinitionSettings
Configured via `appsettings.json` with `MoleculesLocation` property (directory for YAML files).

---

## Repository Layer (Molecules/ResearchDefinition/ResearchDefinitionRepository)

### ResearchDefinitionRepo
- **Storage**: YAML files in configurable directory (`MoleculesLocation`)
- **Pattern**: One YAML file per research definition, named `{name}.yaml`
- **Operations**:
  - `GetMoleculesResearchDefinitions()`: Load all YAML files
  - `GetMoleculesResearchDefinition(name)`: Load specific YAML file
  - `SaveMoleculesResearchDefintion(definition)`: Serialize and save as YAML
  - `DeleteMoleculesResearchDefintion(name)`: Delete YAML file(s)

**Issue**: ⚠️ **Typo in method names**: `researchDefintionName` (missing 'i') is used throughout instead of `researchDefinitionName`

---

## Service Layer (Molecules/ResearchDefinition/ResearchDefinitionService)

### ResearchDefinitionService
Implements `IResearchDefinitionService` with:
- `GetMoleculesResearchDefinitions()`
- `GetMoleculesResearchDefinition(string researchDefinitionName)`
- `SaveMoleculesResearchDefintion(MoleculesResearchDefinition)`
- `DeleteMoleculesResearchDefintion(string researchDefintionName)`

All methods include try-catch with Serilog logging at Critical level.

---

## Workflow Engine (WorkflowGraph)

### Graph Infrastructure
- **IDigraph<TKey>**: Interface for directed graphs
  - AddVertex(TKey)
  - AddEdge(TKey from, TKey to)
  - RemoveEdge(TKey from, TKey to)
  - RemoveVertex(TKey)
- **IReadOnlyDigraph<TKey>**: Read-only graph operations
- **KeyedDigraph<TKey>**: Implementation of IDigraph
- **Edge<TKey>**: Represents a directed edge
- **Extensions**: TopologyExtensions, DigraphExtensions

### Persistence
- **FileInstancePersistence**: File-based persistence for workflow graphs

### Tests
- WorkflowExecutorTests
- TopologyTests
- WorkflowPersistenceTests

---

## Console Application (MainConsole)

### Program.cs
- .NET Host with Serilog logging
- Configures ResearchDefinitionSettings from appsettings.json
- Registers services with Singleton lifetime
- Runs Application as HostedService

### Application.cs (BackgroundService)
```csharp
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    _logger.LogInformation("Application started");
    
    while (!stoppingToken.IsCancellationRequested)
    {
        _logger.LogInformation("Application running at: {time}", DateTimeOffset.Now);
        foreach (var workflow in _workflowRunners)
        {
            await workflow.RunAsync();
        }
        await Task.Delay(3600000, stoppingToken); // 1 hour
    }
    _logger.LogInformation("Application stopping");
}
```

**Issue**: ⚠️ Runs ALL registered workflow runners every hour without any filtering or specific research definition targeting

---

## Quantum Chemistry Pipeline

The experimental pipeline follows this workflow:

```
XYZ → GAMESS optimization → parse results → JSON representation → 
generate derived calculations → GAMESS calculations → enrich results → Microstate analysis
```

### Supported Calculations
- RHF (Restricted Hartree-Fock)
- DFT/B3LYP (Density Functional Theory with B3LYP functional)
- Neutral states
- +1 electron states
- -1 electron states
- CHELPG (CHarges from ELectrostatic Potentials for Grid based methods)
- GEODISK

---

## Strengths ✅

1. **Clean Architecture**: Good separation of concerns
   - Domain → Services → Repositories → API Controllers
   - Each layer has clear responsibilities

2. **Type Safety**: 
   - TypeScript models mirror C# domain models well
   - Strong typing throughout the stack

3. **Error Handling**: 
   - Both frontend and backend have proper error handling
   - Serilog logging in backend
   - Console error logging in frontend

4. **Workflow Foundation**:
   - Graph-based workflow engine is solid and extensible
   - Supports complex dependencies between steps

5. **Aspire Integration**:
   - Proper development environment orchestration
   - Supports launching relevant applications together

6. **Scientific Focus**:
   - Domain models align with quantum chemistry research needs
   - Flexible for evolving research requirements

7. **Configuration**:
   - Research definition paths configurable via settings
   - Supports different environments

---

## Issues & Recommendations ⚠️

### Critical Issues

1. **⚠️ Typo: `researchDefintionName` (missing 'i')**
   - **Location**: Throughout the codebase
   - **Files affected**:
     - WebApi/Controllers/ResearchDefinitionController.cs
     - WebApi/Controllers/WorkflowController.cs
     - Molecules/ResearchDefinition/ResearchDefinitionService/ResearchDefinitionService.cs
     - Molecules/ResearchDefinition/ResearchDefinitionRepository/ResearchDefinitionRepo.cs
     - Molecules/ResearchDefinition/IResearchDefintionService/IResearchDefinitionService.cs
     - Molecules/ResearchDefinition/IResearchDefinitionRepository/IResearchDefinitionRepo.cs
     - Web/src/app/features/workflows/services/research-definition.service.ts
     - Web/src/app/features/workflows/services/workflow-run.service.ts
   - **Impact**: Inconsistent naming, potential for bugs
   - **Fix**: Standardize on `researchDefinitionName` (with 'i') throughout

2. **⚠️ Hardcoded API URLs in Angular**
   - **Location**: 
     - `ResearchDefinitionService`: `const API_URL = 'https://localhost:7094/ResearchDefinition'`
     - `WorkflowRunService`: `const API_URL = 'https://localhost:7094/Workflow'`
   - **Impact**: Cannot deploy to different environments, breaks in production
   - **Fix**: Use Angular environment files
     ```typescript
     // environment.ts
     export const environment = {
       production: false,
       apiUrl: 'https://localhost:7094'
     };
     
     // environment.prod.ts
     export const environment = {
       production: true,
       apiUrl: 'https://your-production-api.com'
     };
     
     // In services
     const API_URL = `${environment.apiUrl}/ResearchDefinition`;
     ```

3. **⚠️ MainConsole doesn't use researchDefinitionName**
   - **Location**: MainConsole/Application.cs
   - **Issue**: Runs all workflows without filtering
   - **Impact**: Cannot target specific research definitions
   - **Fix**: Accept command-line arguments or configuration to specify which workflows to run

### High Priority Improvements

4. **⚠️ Duplicate Error Handling Code**
   - **Location**: Both Angular services have identical `describeHttpError` functions
   - **Fix**: Create shared utility file `Web/src/app/core/utils/error.utils.ts`

5. **⚠️ API Contract Mismatch**
   - **Issue**: WorkflowController expects `researchDefintionName` as query parameter
   - **Angular**: WorkflowRunService passes it as a param (not query)
   - **Fix**: Align the approach - either use query param consistently or change to body/route param

6. **⚠️ WorkflowRunService parameter handling**
   - Currently: `params: { researchDefintionName: researchDefinitionName }`
   - This sends it as a query parameter
   - But the controller expects `[FromQuery, Required] string researchDefintionName`
   - **Status**: Actually aligned, but the typo is the main issue

### Medium Priority Improvements

7. **Workflow Editor Validation**
   - No validation for step dependencies
   - Can create dependencies on non-existent steps
   - Step IDs could conflict when manually edited
   - **Fix**: Add validation in the editor component

8. **Missing Features (per AGENTS.md)**
   - Run status tracking (currently just returns success/fail counts)
   - No visibility into execution progress
   - No error details from failed runs
   - **Recommendation**: Enhance WorkflowRunReport to include:
     - Execution status (Running, Completed, Failed)
     - Progress percentage
     - Detailed error messages
     - Timestamps
     - Individual step status

9. **Configuration Management**
   - Frontend has no configuration for API base URL
   - Backend settings are configurable but frontend is hardcoded
   - **Fix**: Implement proper environment configuration throughout

10. **CORS Security**
    - Currently: `AllowAnyOrigin()`, `AllowAnyHeader()`, `AllowAnyMethod()`
    - **Risk**: Security risk in production
    - **Fix**: Restrict to specific origins in production

---

## Development Philosophy (from AGENTS.md)

### Guidelines
- Build a small working feature, validate it, understand its fit with the research, then extend it
- Inspect the current repository and respect its structure instead of recreating it from assumptions
- Make focused changes; avoid unrelated refactoring and large amounts of speculative architecture or code
- Design significant new components at a useful level before implementing large changes
- Architectural/design discussions do not automatically authorize implementation
- Favor clarity and maintainability over prematurely generic frameworks
- Explain significant architectural choices, report the important files added/changed, and state assumptions or uncertainties
- Treat iterative side questions separately unless the user explicitly changes the current design

### GitHub Workflow
1. Read the current default branch
2. Create a dedicated feature branch (normally `codex/<descriptive-name>`)
3. Make changes only on that branch; never make development changes directly on the default/main branch
4. Clearly explain what changed
5. Leave changes for the user to review
6. The user performs/controls the merge; do not merge on their behalf

---

## Quick Start Guide

### Prerequisites
- .NET 8 SDK
- Node.js (for Angular)
- Aspire workload (for AppHost)

### Running the Application
1. **Backend**: `dotnet run --project WebApi`
2. **Frontend**: `cd Web && npm install && npm start`
3. **Full Stack**: `dotnet run --project MicrostateEngineering.AppHost`

### Configuration
- Backend: `WebApi/appsettings.json` (Development) or `appsettings.Development.json`
- Frontend: `Web/src/environments/environment.ts` (to be created)

---

## File Locations Cheat Sheet

| Purpose | Location |
|---------|----------|
| Angular App | `Web/src/app` |
| Angular Environments | `Web/src/environments` (needs creation) |
| WebAPI | `WebApi` |
| Research Definition Domain | `Molecules/ResearchDefinition/ResearchDefinitionDomain` |
| Research Definition Service | `Molecules/ResearchDefinition/ResearchDefinitionService` |
| Research Definition Repository | `Molecules/ResearchDefinition/ResearchDefinitionRepository` |
| Workflow Graph Engine | `WorkflowGraph/Engine` |
| Main Console App | `MainConsole` |
| Aspire AppHost | `MicrostateEngineering.AppHost` |
| Unit Tests | `UnitTests` |

---

## Summary Assessment

**Overall**: Well-architected, focused application that aligns with the scientific research goals. The foundation is solid, but there are **configuration and consistency issues** that need addressing before scaling.

**Maturity**: Early stage but with good architectural decisions. The workflow engine and domain models provide a strong foundation for the scientific research.

**Technical Debt**: 
- High: Typo in `researchDefintionName` (pervasive)
- High: Hardcoded API URLs in frontend
- Medium: Duplicate error handling code
- Medium: MainConsole workflow execution logic

**Recommendations for Next Steps**:
1. Fix the `researchDefintionName` typo throughout the codebase
2. Implement proper environment configuration for API URLs
3. Enhance workflow run reporting with progress and error details
4. Add validation to the workflow editor
5. Secure CORS configuration for production

---

*Analysis stored for future sessions. Last updated: 2024-10-25*
