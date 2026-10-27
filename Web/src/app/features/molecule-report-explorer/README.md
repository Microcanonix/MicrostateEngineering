# Molecule report explorer

Renders one parsed GAMESS/DFT calculation report (JSON) at a time in the
scientific report screen. Implemented with standalone components, signals,
OnPush change detection and modern template control flow. The app is zoneless:
the three.js render loop is driven explicitly (pointer events, resize observer,
`requestAnimationFrame`) and emits hover/select events into the parent.

## Feature contents

- `models/molecule-report.model.ts` — interfaces for the five report sections
  (`AtomPositionsReport`, `AtomChargeReport`, `AtomPopulationReport`,
  `AtomOrbitalReport`, `MoleculeBondsReport`) and the container
  `MoleculeReportData`; `parseMoleculeReport` normalizes raw report objects
  with either PascalCase (report file convention) or camelCase (ASP.NET Core
  serialization) section keys; `parseAoConfiguration` parses the
  `S(1.9883)X(0.5410)...` AO configuration strings into raw, unnormalized
  per-basis-function populations.
- `services/molecule-report-explorer-state.service.ts` — signal-based state
  (colour modes, hover/selection, verification strip computation) plus pure
  helpers: `atomPropertyValue`, `divergingColor` (red–white–blue),
  `elementColor` (H pale, C slate, O red), `aoValues`.
- `components/molecule-viewer.ts` — three.js ball-and-stick viewer: small atom
  spheres as position markers, bonds as dominant cylinders whose radius scales
  with Mayer bond order; molecule centred on its centroid, chemistry z-up
  frame preserved (proper rotation only, no mirroring); drag to rotate, wheel
  to zoom; raycast hover glow for atoms; bond hover highlight overlay; diverging
  atom colour modes and HOMO/LUMO bond colouring; small gradient legend. No
  persistent atom labels.
- `components/ao-config-chart.ts` — per-AO Mulliken population bar chart with
  Ground / Fractions / HOMO / LUMO modes, raw unnormalized values, displayed
  sum, basis-order note (S S X Y Z S X Y Z for C/O, S S for H) and the atom's
  Mulliken population and HOMO/LUMO shares.
- `components/grouped-bars.ts` — lightweight inline SVG grouped bar chart
  (zero reference line, 4-decimal tooltips) used for the charge-scheme and
  HOMO/LUMO-share comparisons.
- `components/molecule-report-explorer.ts` — composition: atom table (left of
  the 3D view, sized to content), verification chips strip, AO configuration
  section, comparison charts, bonds table, footer semantics notes.

## Feeding a report in

The explorer component takes the raw report object as a single input:

```html
<app-molecule-report-explorer [report]="reportObject" />
```

where `reportObject` is the JSON of one calculation with the five sections
(`AtomPositionsReport`, ... — PascalCase as in the report files, or camelCase
as serialized by the WebAPI; both are accepted). `parseMoleculeReport` returns
`null` when the object does not have the expected shape.

The scientific report screen (`/workflows/:name/report`) feeds the selected
molecule's `report` object into the explorer. A sample report
(`ethanol-report.json`, ethanol / 3-21G, 9 atoms, 8 bonds) is served from
`public/ethanol-report.json` for testing without a running backend:

```
http://localhost:4200/ethanol-report.json
```

## Notes on semantics

- HOMO distribution = response to Lewis acid attack; LUMO distribution =
  response to Lewis base attack (labels reflect this).
- Positive frontier-MO bond overlap = bonding contribution; negative =
  antibonding contribution (green/red in bond colouring and bonds table).
- Mulliken values are conventions; displayed exactly as given, never clamped,
  rescaled or filtered. Lowdin shown as a cross-check.
- Bond membership comes from the GAMESS bond-order printout, not inferred from
  distances.
