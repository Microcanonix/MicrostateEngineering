export const STARTER_WORKFLOW_YAML = `name: new-workflow
package_root: "C:\\\\MoleculesDb"
xyzfiles: xyz
gms_input: input
gms_output: output
workflow_status_folder: status
molecule_data: molecules
basisset: B3_21G
molecules:
  - name: example-molecule
    charge: 0
processes:
  - type: moleculeproperties
    steps:
      - id: 0
        type: import_data
        can_execute: true
      - id: 1
        type: geometry_optimization
        can_execute: true
      - id: 2
        type: electronic_structure
        can_execute: true
      - id: 3
        type: fukui_calculation
        can_execute: true
      - id: 4
        type: charge_geodisk
        can_execute: true
      - id: 5
        type: charge_chelpg
        can_execute: true
    dependencies:
      - dependency: import_data
        dependant: geometry_optimization
      - dependency: geometry_optimization
        dependant: electronic_structure
      - dependency: geometry_optimization
        dependant: fukui_calculation
      - dependency: geometry_optimization
        dependant: charge_geodisk
      - dependency: geometry_optimization
        dependant: charge_chelpg
`;
