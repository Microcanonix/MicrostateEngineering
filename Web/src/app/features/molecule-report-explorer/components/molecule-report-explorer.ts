import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  computed,
  effect,
  inject,
  input,
  signal,
  viewChild,
} from '@angular/core';
import { MoleculeReportData, parseMoleculeReport } from '../models/molecule-report.model';
import {
  ATOM_COLOR_MODES,
  AoChartMode,
  AtomColorMode,
  BondColorMode,
  MoleculeReportExplorerState,
} from '../services/molecule-report-explorer-state.service';
import { MoleculeViewer } from './molecule-viewer';
import { AoConfigChart } from './ao-config-chart';

@Component({
  selector: 'app-molecule-report-explorer',
  templateUrl: './molecule-report-explorer.html',
  styleUrl: './molecule-report-explorer.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [MoleculeViewer, AoConfigChart],
  providers: [MoleculeReportExplorerState],
})
export class MoleculeReportExplorer {
  private readonly state = inject(MoleculeReportExplorerState);

  /** Raw report object: PascalCase (file convention) or camelCase (WebAPI). */
  readonly report = input.required<unknown>();

  readonly atomColorModes = ATOM_COLOR_MODES;

  readonly atoms = this.state.atoms;
  readonly bonds = this.state.bonds;
  readonly moleculeName = this.state.moleculeName;
  readonly selectedAtomIds = this.state.selectedAtomIds;
  readonly hoveredAtomId = this.state.hoveredAtomId;
  readonly hoveredBondId = this.state.hoveredBondId;
  readonly selectedBondIds = this.state.selectedBondIds;
  readonly aoChartAtom = this.state.aoChartAtom;
  readonly verificationChips = computed(() => this.state.verification().chips);

  readonly atomColorMode = this.state.atomColorMode;
  readonly bondColorMode = this.state.bondColorMode;
  readonly aoChartMode = this.state.aoChartMode;
  readonly atomRadius = this.state.atomRadius;
  readonly bondMaxRadius = this.state.bondMaxRadius;
  readonly showAtomLabels = this.state.showAtomLabels;

  /** Whether the 3D row (atom table + viewer) is in fullscreen. */
  readonly rowFullscreen = signal(false);

  private readonly rowElement = viewChild<ElementRef<HTMLElement>>('mreThreeDRow');

  constructor() {
    effect(() => {
      const normalized = parseMoleculeReport(this.report());
      this.state.setReport(normalized);
    });
  }

  onAtomHovered(id: string | null): void {
    this.state.hoveredAtomId.set(id);
  }

  onAtomClicked(id: string): void {
    this.state.selectAtom(id);
  }

  onBondHovered(id: string | null): void {
    this.state.hoveredBondId.set(id);
  }

  onBondClicked(id: string): void {
    this.state.selectBond(id);
  }

  onAtomRowHover(atomId: string | null): void {
    this.state.hoveredAtomId.set(atomId);
  }

  onAtomRowLeave(): void {
    this.state.hoveredAtomId.set(null);
  }

  onAtomRowClick(atomId: string): void {
    this.state.selectAtom(atomId);
  }

  setAtomColorMode(mode: AtomColorMode): void {
    this.state.atomColorMode.set(mode);
  }

  setBondColorMode(mode: BondColorMode): void {
    this.state.bondColorMode.set(mode);
  }

  setAoChartMode(mode: AoChartMode): void {
    this.state.aoChartMode.set(mode);
  }

  setAtomRadius(value: number): void {
    this.state.atomRadius.set(value);
  }

  setBondMaxRadius(value: number): void {
    this.state.bondMaxRadius.set(value);
  }

  toggleAtomLabels(checked: boolean): void {
    this.state.showAtomLabels.set(checked);
  }

  toggleRowFullscreen(): void {
    const element = this.rowElement()?.nativeElement;
    if (!element) {
      return;
    }

    if (document.fullscreenElement === element) {
      void document.exitFullscreen().catch(() => undefined);
    } else {
      void element.requestFullscreen().catch(() => undefined);
    }
  }

  onRowFullscreenChange(): void {
    this.rowFullscreen.set(!!document.fullscreenElement);
  }

  formatNumber(value: number | null | undefined, decimals: number): string {
    return typeof value === 'number' ? value.toFixed(decimals) : '—';
  }

  chargeClass(value: number | null | undefined): string {
    if (typeof value !== 'number') {
      return '';
    }
    if (value > 0) {
      return 'text-primary';
    }
    if (value < 0) {
      return 'text-danger';
    }
    return '';
  }

  frontierClass(value: number | null | undefined): string {
    if (typeof value !== 'number') {
      return '';
    }
    if (value > 0) {
      return 'mre-positive';
    }
    if (value < 0) {
      return 'mre-negative';
    }
    return '';
  }


}
