import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';
import { AoChartMode, aoValues } from '../services/molecule-report-explorer-state.service';
import { AtomOrbitalReport, HEAVY_ATOM_BASIS_ORDER, HYDROGEN_BASIS_ORDER } from '../models/molecule-report.model';

const MODE_LABELS: Record<AoChartMode, string> = {
  ground: 'Ground',
  fractions: 'Fractions',
  homo: 'HOMO',
  lumo: 'LUMO',
};

@Component({
  selector: 'app-ao-config-chart',
  templateUrl: './ao-config-chart.html',
  styleUrl: './ao-config-chart.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AoConfigChart {
  readonly orbital = input.required<AtomOrbitalReport | null>();
  readonly mullikenPopulation = input.required<number>();
  readonly homoShare = input.required<number>();
  readonly lumoShare = input.required<number>();
  readonly mode = input.required<AoChartMode>();
  readonly modeChange = output<AoChartMode>();

  readonly modes: { key: AoChartMode; label: string }[] = (
    Object.entries(MODE_LABELS) as [AoChartMode, string][]
  ).map(([key, label]) => ({ key, label }));
  readonly heavyBasisOrder = HEAVY_ATOM_BASIS_ORDER;
  readonly hydrogenBasisOrder = HYDROGEN_BASIS_ORDER;

  readonly bars = computed(() => {
    const orbital = this.orbital();
    if (!orbital) {
      return [];
    }
    return aoValues(orbital, this.mode());
  });

  readonly maxAbs = computed(() =>
    Math.max(...this.bars().map((b) => Math.abs(b.value)), 0.000001),
  );

  readonly sum = computed(() =>
    this.bars().reduce((acc, b) => acc + b.value, 0),
  );

  readonly isHydrogen = computed(() => this.orbital()?.AtomID?.startsWith('H') ?? false);

  setMode(mode: AoChartMode): void {
    this.modeChange.emit(mode);
  }

  heightPercent(value: number): number {
    return Math.min(100, (Math.abs(value) / this.maxAbs()) * 100);
  }
}
