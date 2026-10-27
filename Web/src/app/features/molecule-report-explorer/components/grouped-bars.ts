import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core';

export interface GroupedBarSeries {
  label: string;
  color: string;
  values: number[];
}

@Component({
  selector: 'app-grouped-bars',
  templateUrl: './grouped-bars.html',
  styleUrl: './grouped-bars.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GroupedBars {
  readonly categories = input.required<string[]>();
  readonly series = input.required<GroupedBarSeries[]>();
  readonly title = input.required<string>();

  readonly width = 720;
  readonly height = 260;
  readonly margin = { top: 24, right: 16, bottom: 34, left: 44 };

  readonly plotHeight = this.height - this.margin.top - this.margin.bottom;
  readonly plotWidth = this.width - this.margin.left - this.margin.right;

  readonly maxAbs = computed(() => {
    let max = 0.000001;
    for (const s of this.series()) {
      for (const v of s.values) {
        max = Math.max(max, Math.abs(v));
      }
    }
    return max;
  });

  readonly zeroY = computed(() => this.margin.top + this.plotHeight / 2);

  readonly groupWidth = computed(() => this.plotWidth / Math.max(this.categories().length, 1));

  readonly barWidth = computed(() =>
    Math.max(3, (this.groupWidth() * 0.72) / Math.max(this.series().length, 1)),
  );

  readonly ticks = computed(() => {
    const max = this.maxAbs();
    const step = niceStep(max / 2);
    const values: number[] = [];
    for (let v = -step; v < max; v += step) {
      values.push(v);
    }
    for (let v = step; v < max; v += step) {
      if (!values.includes(v)) {
        values.push(v);
      }
    }
    values.push(0);
    return values.sort((a, b) => a - b);
  });

  y(value: number): number {
    const half = this.plotHeight / 2;
    return this.zeroY() - (value / this.maxAbs()) * half;
  }

  x(categoryIndex: number, seriesIndex: number): number {
    const groupStart =
      this.margin.left + categoryIndex * this.groupWidth() + this.groupWidth() / 2;
    const seriesCount = this.series().length;
    const total = this.barWidth() * seriesCount;
    return groupStart - total / 2 + seriesIndex * this.barWidth();
  }

  barHeight(value: number): number {
    return Math.abs(this.y(value) - this.zeroY());
  }

  tickText(v: number): string {
    const abs = Math.abs(v);
    if (abs >= 1 || abs === 0) {
      return String(Math.round(v));
    }
    return v.toFixed(2);
  }
}

function niceStep(raw: number): number {
  const exponent = Math.floor(Math.log10(raw));
  const fraction = raw / Math.pow(10, exponent);
  const nice = fraction < 1.5 ? 1 : fraction < 3 ? 2 : fraction < 7 ? 5 : 10;
  return nice * Math.pow(10, exponent);
}
