import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  NgZone,
  OnDestroy,
  ViewChild,
  computed,
  effect,
  inject,
  input,
  output,
  signal,
} from '@angular/core';
import * as THREE from 'three';
import {
  AtomColorMode,
  AtomView,
  BondColorMode,
  atomPropertyValue,
  divergingColor,
  elementColor,
} from '../services/molecule-report-explorer-state.service';
import { MoleculeBondReport } from '../models/molecule-report.model';

const ATOM_SELECT_SCALE = 1.5;
const BOND_MIN_RADIUS = 0.025;

/** Number of parallel cylinders to draw for a bond: 1 single, 2 double, 3 triple. */
function bondMultiplicity(bondOrder: number | null): number {
  if (typeof bondOrder !== 'number') {
    return 1;
  }
  if (bondOrder >= 2.5) {
    return 3;
  }
  if (bondOrder >= 1.5) {
    return 2;
  }
  return 1;
}

interface AtomMesh {
  mesh: THREE.Mesh;
  atom: AtomView;
  baseColor: THREE.Color;
}

interface BondMesh {
  /** All cylinders of the bond (2 for a double, 3 for a triple bond). */
  meshes: THREE.Mesh[];
  bond: MoleculeBondReport;
}

@Component({
  selector: 'app-molecule-viewer',
  templateUrl: './molecule-viewer.html',
  styleUrl: './molecule-viewer.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MoleculeViewer implements AfterViewInit, OnDestroy {
  private readonly zone = inject(NgZone);
  private readonly hostRef = inject(ElementRef<HTMLElement>);

  readonly atoms = input.required<AtomView[]>();
  readonly bonds = input.required<MoleculeBondReport[]>();
  readonly atomColorMode = input.required<AtomColorMode>();
  readonly bondColorMode = input.required<BondColorMode>();
  readonly atomRadius = input.required<number>();
  readonly bondMaxRadius = input.required<number>();
  readonly showAtomLabels = input(false);
  readonly selectedAtomIds = input<ReadonlySet<string>>(new Set());
  readonly selectedBondIds = input<ReadonlySet<string>>(new Set());
  readonly hoveredAtomId = input<string | null>(null);
  readonly hoveredBondId = input<string | null>(null);

  readonly atomHovered = output<string | null>();
  readonly atomClicked = output<string>();
  readonly bondHovered = output<string | null>();
  readonly bondClicked = output<string>();

  @ViewChild('canvasHost', { static: true })
  canvasHost!: ElementRef<HTMLElement>;

  private renderer?: THREE.WebGLRenderer;
  private scene?: THREE.Scene;
  private camera?: THREE.PerspectiveCamera;
  private raycaster = new THREE.Raycaster();
  private atomMeshes: AtomMesh[] = [];
  private bondMeshes: BondMesh[] = [];
  private atomGroup?: THREE.Group;
  private resizeObserver?: ResizeObserver;
  private rendererHost?: HTMLElement;
  private frameHandle: number | null = null;
  private renderRequested = signal(false);
  private disposed = false;
  private needsRebuild = true;
  private readonly pointer = new THREE.Vector2();
  private hoverTarget: 'atom' | 'bond' | null = null;
  private lastHoverAtomId: string | null = null;
  private lastHoverBondId: string | null = null;
  private bondHighlightMeshes: THREE.Mesh[] = [];
  private labelSprites: THREE.Sprite[] = [];
  private labelTextures: THREE.Texture[] = [];

  constructor() {
    effect(() => {
      this.atoms();
      this.bonds();
      this.atomRadius();
      this.bondMaxRadius();
      this.showAtomLabels();
      this.needsRebuild = true;
      this.requestRender();
    });

    effect(() => {
      const mode = this.atomColorMode();
      void mode;
      this.applyAtomColors();
      this.requestRender();
    });

    effect(() => {
      const mode = this.bondColorMode();
      void mode;
      this.applyBondColors();
      this.requestRender();
    });

    effect(() => {
      const selected = this.selectedAtomIds();
      const hovered = this.hoveredAtomId();
      this.applyAtomHighlights(selected, hovered);
      this.requestRender();
    });

    effect(() => {
      const hoveredBond = this.hoveredBondId();
      const selectedBonds = this.selectedBondIds();
      this.applyBondHighlight(hoveredBond, selectedBonds);
      this.requestRender();
    });
  }

  ngAfterViewInit(): void {
    this.rendererHost = this.canvasHost.nativeElement;
    this.setupScene();
    this.setupResizeObserver();
    this.requestRender();
  }

  ngOnDestroy(): void {
    this.disposed = true;
    if (this.frameHandle !== null) {
      cancelAnimationFrame(this.frameHandle);
      this.frameHandle = null;
    }
    this.resizeObserver?.disconnect();
    this.renderer?.dispose();
    this.scene?.traverse((object) => {
      if (object instanceof THREE.Mesh) {
        object.geometry.dispose();
        (object.material as THREE.Material).dispose();
      }
    });
    for (const texture of this.labelTextures) {
      texture.dispose();
    }
    this.labelTextures = [];
    this.labelSprites = [];
  }

  private setupScene(): void {
    const host = this.rendererHost!;
    const width = Math.max(host.clientWidth, 1);
    const height = Math.max(host.clientHeight, 1);

    const renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true });
    renderer.setSize(width, height);
    renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    host.appendChild(renderer.domElement);

    const scene = new THREE.Scene();
    scene.background = null;

    const camera = new THREE.PerspectiveCamera(45, width / height, 0.1, 500);
    camera.position.set(0, -8, 4);
    camera.up.set(0, 0, 1);
    camera.lookAt(0, 0, 0);

    const ambient = new THREE.AmbientLight(0xffffff, 0.55);
    scene.add(ambient);
    const keyLight = new THREE.DirectionalLight(0xffffff, 1.1);
    keyLight.position.set(3, -6, 8);
    scene.add(keyLight);
    const fillLight = new THREE.DirectionalLight(0xffffff, 0.35);
    fillLight.position.set(-4, 5, -3);
    scene.add(fillLight);

    this.renderer = renderer;
    this.scene = scene;
    this.camera = camera;

    this.setupControls(camera, renderer);
  }

  private setupControls(camera: THREE.PerspectiveCamera, renderer: THREE.WebGLRenderer): void {
    const dom = renderer.domElement;
    let dragging = false;
    let lastX = 0;
    let lastY = 0;
    let moved = 0;

    dom.addEventListener('pointerdown', (event) => {
      dragging = true;
      moved = 0;
      lastX = event.clientX;
      lastY = event.clientY;
      dom.setPointerCapture(event.pointerId);
    });

    dom.addEventListener('pointermove', (event) => {
      if (dragging) {
        const dx = event.clientX - lastX;
        const dy = event.clientY - lastY;
        moved += Math.abs(dx) + Math.abs(dy);
        lastX = event.clientX;
        lastY = event.clientY;

        const rotateSpeed = 0.01;
        const group = this.atomGroup;
        if (group) {
          const axisY = new THREE.Vector3(0, 1, 0);
          const axisX = new THREE.Vector3(1, 0, 0);
          group.rotateOnWorldAxis(axisY, dx * rotateSpeed);
          group.rotateOnWorldAxis(axisX, dy * rotateSpeed);
        }
        this.requestRender();
      }
      this.updatePointer(event);
      this.updateHover();
    });

    dom.addEventListener('pointerup', (event) => {
      dragging = false;
      if (moved < 4) {
        this.pickAtom(event, true);
      }
    });

    dom.addEventListener('pointerleave', () => {
      dragging = false;
      if (this.lastHoverAtomId !== null) {
        this.lastHoverAtomId = null;
        this.zone.run(() => this.atomHovered.emit(null));
      }
      if (this.lastHoverBondId !== null) {
        this.lastHoverBondId = null;
        this.zone.run(() => this.bondHovered.emit(null));
      }
    });

    dom.addEventListener('wheel', (event) => {
      event.preventDefault();
      const direction = event.deltaY > 0 ? 1.1 : 0.9;
      camera.position.multiplyScalar(direction);
      if (camera.position.length() > 200) {
        camera.position.setLength(200);
      }
      if (camera.position.length() < 1.5) {
        camera.position.setLength(1.5);
      }
      this.requestRender();
    });
  }

  private updatePointer(event: PointerEvent | WheelEvent): void {
    const dom = this.renderer!.domElement;
    const rect = dom.getBoundingClientRect();
    this.pointer.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
    this.pointer.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;
  }

  private updateHover(): void {
    if (!this.scene || !this.camera) {
      return;
    }

    this.raycaster.setFromCamera(this.pointer, this.camera);

    const atomHits = this.raycaster.intersectObjects(this.atomMeshes.map((a) => a.mesh), false);
    const bondHits = this.raycaster.intersectObjects(
      this.bondMeshes.flatMap((b) => b.meshes),
      false,
    );

    let newAtomId: string | null = null;
    let newBondId: string | null = null;

    if (atomHits.length) {
      const mesh = atomHits[0].object as THREE.Mesh;
      newAtomId = this.atomMeshes.find((a) => a.mesh === mesh)?.atom.id ?? null;
      this.hoverTarget = 'atom';
    } else if (bondHits.length) {
      const mesh = bondHits[0].object as THREE.Mesh;
      newBondId = this.bondMeshes.find((b) => b.meshes.includes(mesh))?.bond.BondID ?? null;
      this.hoverTarget = 'bond';
    } else {
      this.hoverTarget = null;
    }

    if (newAtomId !== this.lastHoverAtomId) {
      this.lastHoverAtomId = newAtomId;
      this.zone.run(() => this.atomHovered.emit(newAtomId));
    }
    if (newBondId !== this.lastHoverBondId) {
      this.lastHoverBondId = newBondId;
      this.zone.run(() => this.bondHovered.emit(newBondId));
    }
  }

  private pickAtom(event: PointerEvent | WheelEvent, click: boolean): void {
    if (!click) {
      return;
    }

    this.updatePointer(event);
    this.updateHover();
    const atomId = this.lastHoverAtomId ?? this.hoveredAtomIdFromTarget();
    if (atomId) {
      this.zone.run(() => this.atomClicked.emit(atomId));
      return;
    }

    const bondId = this.hoverTarget === 'bond' ? this.lastHoverBondId : null;
    if (bondId) {
      this.zone.run(() => this.bondClicked.emit(bondId));
    }
  }

  private hoveredAtomIdFromTarget(): string | null {
    return this.hoverTarget === 'atom' ? this.lastHoverAtomId : null;
  }

  private setupResizeObserver(): void {
    this.resizeObserver = new ResizeObserver(() => this.onResize());
    this.resizeObserver.observe(this.rendererHost!);
  }

  private onResize(): void {
    if (!this.renderer || !this.camera || !this.rendererHost) {
      return;
    }
    const width = Math.max(this.rendererHost.clientWidth, 1);
    const height = Math.max(this.rendererHost.clientHeight, 1);
    this.renderer.setSize(width, height);
    this.camera.aspect = width / height;
    this.camera.updateProjectionMatrix();
    this.requestRender();
  }

  private requestRender(): void {
    if (this.disposed || this.frameHandle !== null) {
      return;
    }
    this.renderRequested.set(true);
    this.frameHandle = requestAnimationFrame(() => {
      this.frameHandle = null;
      this.renderFrame();
    });
  }

  private renderFrame(): void {
    if (this.disposed || !this.renderer || !this.scene || !this.camera) {
      return;
    }

    if (this.needsRebuild) {
      this.rebuildMolecule();
      this.needsRebuild = false;
    }

    this.renderer.render(this.scene, this.camera);
  }

  private rebuildMolecule(): void {
    if (!this.scene) {
      return;
    }

    if (this.atomGroup) {
      this.scene.remove(this.atomGroup);
      this.atomGroup.traverse((object) => {
        if (object instanceof THREE.Mesh) {
          object.geometry.dispose();
          (object.material as THREE.Material).dispose();
        }
      });
    }

    const group = new THREE.Group();
    const atoms = this.atoms();
    const bonds = this.bonds();

    this.atomMeshes = [];
    this.bondMeshes = [];
    this.bondHighlightMeshes = [];
    this.labelSprites = [];
    this.labelTextures = [];

    if (atoms.length) {
      const centroid = new THREE.Vector3();
      for (const atom of atoms) {
        centroid.add(new THREE.Vector3(atom.position.PosX, atom.position.PosY, atom.position.PosZ));
      }
      centroid.divideScalar(atoms.length);

      const atomGeometry = new THREE.SphereGeometry(Math.max(this.atomRadius(), 0.01), 24, 24);
      for (const atom of atoms) {
        const color = this.currentAtomColor(atom);
        const material = new THREE.MeshStandardMaterial({ color });
        const mesh = new THREE.Mesh(atomGeometry, material);
        mesh.position.set(
          atom.position.PosX - centroid.x,
          atom.position.PosY - centroid.y,
          atom.position.PosZ - centroid.z,
        );
        group.add(mesh);
        this.atomMeshes.push({ mesh, atom, baseColor: new THREE.Color(color) });
      }

      const atomsByPos = new Map(atoms.map((a) => [a.pos, a]));
      for (const bond of bonds) {
        // Bonds without a Mayer bond order are not chemical bonds (GAMESS did
        // not print them in its bond-order output); keep them out of the 3D
        // view.
        if (typeof bond.BondOrder !== 'number') {
          continue;
        }
        const atom1 = atomsByPos.get(bond.Atom1Pos);
        const atom2 = atomsByPos.get(bond.Atom2Pos);
        if (!atom1 || !atom2) {
          continue;
        }

        const start = new THREE.Vector3(atom1.position.PosX, atom1.position.PosY, atom1.position.PosZ);
        const end = new THREE.Vector3(atom2.position.PosX, atom2.position.PosY, atom2.position.PosZ);
        start.sub(centroid);
        end.sub(centroid);

        const direction = new THREE.Vector3().subVectors(end, start);
        const length = direction.length();
        const radius = this.bondRadius(bond.BondOrder);

        // Render double/triple bonds as parallel cylinders (perpendicular
        // offset around the bond axis), like a classic ball-and-stick model.
        const cylinderCount = bondMultiplicity(bond.BondOrder);
        const unit = direction.clone().normalize();
        // Any vector perpendicular to the bond axis for the offset direction.
        const perpendicular = Math.abs(unit.z) < 0.9
          ? new THREE.Vector3(0, 0, 1).cross(unit).normalize()
          : new THREE.Vector3(1, 0, 0).cross(unit).normalize();
        const spacing = radius * 2.2;

        const meshes: THREE.Mesh[] = [];
        for (let i = 0; i < cylinderCount; i++) {
          const offsetIndex = cylinderCount === 1 ? 0 : i - (cylinderCount - 1) / 2;
          const offset = perpendicular.clone().multiplyScalar(offsetIndex * spacing);
          const perCylinderRadius = cylinderCount === 1 ? radius : radius * 0.6;

          const geometry = new THREE.CylinderGeometry(perCylinderRadius, perCylinderRadius, length, 20);
          const material = new THREE.MeshStandardMaterial({ color: this.currentBondColor(bond) });
          const mesh = new THREE.Mesh(geometry, material);
          const midpoint = start.clone().add(end).multiplyScalar(0.5);
          mesh.position.copy(midpoint).add(offset);
          mesh.quaternion.setFromUnitVectors(new THREE.Vector3(0, 1, 0), unit);
          group.add(mesh);
          meshes.push(mesh);
        }
        this.bondMeshes.push({ meshes, bond });
      }

      this.applyBondHighlight(this.hoveredBondId(), this.selectedBondIds());

      if (this.showAtomLabels()) {
        for (const atom of atoms) {
          const sprite = this.createLabelSprite(atom.id);
          sprite.position.set(
            atom.position.PosX - centroid.x,
            atom.position.PosY - centroid.y,
            atom.position.PosZ - centroid.z + this.atomRadius() * 1.4,
          );
          group.add(sprite);
          this.labelSprites.push(sprite);
        }
      }
    }

    this.atomGroup = group;
    this.scene.add(group);

    this.applyAtomColors();
    this.applyBondColors();
    this.applyAtomHighlights(this.selectedAtomIds(), this.hoveredAtomId());
  }

  private createLabelSprite(text: string): THREE.Sprite {
    const canvas = document.createElement('canvas');
    const fontPx = 48;
    const context = canvas.getContext('2d')!;
    context.font = `bold ${fontPx}px sans-serif`;
    const width = Math.ceil(context.measureText(text).width) + 16;
    canvas.width = width;
    canvas.height = fontPx + 12;

    const ctx = canvas.getContext('2d')!;
    ctx.font = `bold ${fontPx}px sans-serif`;
    ctx.textBaseline = 'middle';
    ctx.textAlign = 'center';
    ctx.fillStyle = 'rgba(255, 255, 255, 0.75)';
    const padding = 6;
    ctx.beginPath();
    const radius = 10;
    const x = padding;
    const y = padding;
    const w = canvas.width - 2 * padding;
    const h = canvas.height - 2 * padding;
    ctx.moveTo(x + radius, y);
    ctx.arcTo(x + w, y, x + w, y + h, radius);
    ctx.arcTo(x + w, y + h, x, y + h, radius);
    ctx.arcTo(x, y + h, x, y, radius);
    ctx.arcTo(x, y, x + w, y, radius);
    ctx.closePath();
    ctx.fill();
    ctx.fillStyle = '#1e293b';
    ctx.fillText(text, canvas.width / 2, canvas.height / 2 + 2);

    const texture = new THREE.CanvasTexture(canvas);
    texture.needsUpdate = true;
    this.labelTextures.push(texture);

    const material = new THREE.SpriteMaterial({ map: texture, transparent: true });
    const sprite = new THREE.Sprite(material);
    sprite.scale.set(0.42, (0.42 * canvas.height) / canvas.width, 1);
    sprite.userData['isAtomLabel'] = true;
    return sprite;
  }

  private bondRadius(bondOrder: number | null): number {
    const orders = this.bonds()
      .map((b) => b.BondOrder)
      .filter((o): o is number => typeof o === 'number');
    const maxOrder = Math.max(...orders, 0.01);
    const normalized = Math.max(0, Math.min(1, (bondOrder ?? 0) / maxOrder));
    const max = Math.max(this.bondMaxRadius(), BOND_MIN_RADIUS);
    return BOND_MIN_RADIUS + (max - BOND_MIN_RADIUS) * normalized;
  }

  private currentAtomColor(atom: AtomView): number {
    const mode = this.atomColorMode();
    if (mode === 'elements') {
      return elementColor(atom.symbol);
    }
    return this.divergingAtomColor(atom, mode);
  }

  private divergingAtomColor(atom: AtomView, mode: AtomColorMode): number {
    const values = this.atoms()
      .map((a) => atomPropertyValue(a, mode))
      .filter((v): v is number => v !== null);
    const max = Math.max(...values.map(Math.abs), 0.000001);
    const value = atomPropertyValue(atom, mode);
    if (value === null) {
      return 0xffffff;
    }
    const [r, g, b] = divergingColor(value, max);
    return (r << 16) | (g << 8) | b;
  }

  private currentBondColor(bond: MoleculeBondReport): number {
    const mode = this.bondColorMode();
    if (mode === 'none') {
      return 0x9aa4b2;
    }
    const values = this.bonds()
      .map((bondItem) => (mode === 'homo' ? bondItem.OverlapPopulationHOMO : bondItem.OverlapPopulationLUMO))
      .filter((v): v is number => typeof v === 'number');
    const max = Math.max(...values.map(Math.abs), 0.000001);
    const value = mode === 'homo' ? bond.OverlapPopulationHOMO : bond.OverlapPopulationLUMO;
    const [r, g, b] = bondModeColor(typeof value === 'number' ? value : 0, max);
    return (r << 16) | (g << 8) | b;
  }

  private applyAtomColors(): void {
    for (const entry of this.atomMeshes) {
      const color = this.currentAtomColor(entry.atom);
      entry.baseColor.setHex(color);
      (entry.mesh.material as THREE.MeshStandardMaterial).color.setHex(color);
    }
  }

  private applyBondColors(): void {
    for (const entry of this.bondMeshes) {
      const color = this.currentBondColor(entry.bond);
      for (const mesh of entry.meshes) {
        (mesh.material as THREE.MeshStandardMaterial).color.setHex(color);
      }
    }
  }

  private applyAtomHighlights(selected: ReadonlySet<string>, hovered: string | null): void {
    for (const entry of this.atomMeshes) {
      const isSelected = selected.has(entry.atom.id);
      const isHovered = entry.atom.id === hovered;

      const material = entry.mesh.material as THREE.MeshStandardMaterial;
      material.emissive.setHex(0x38bdf8);
      material.emissiveIntensity = isSelected ? 0.9 : isHovered ? 0.55 : 0;
      const scale = isSelected ? ATOM_SELECT_SCALE : 1;
      entry.mesh.scale.setScalar(scale);
    }
  }

  private applyBondHighlight(
    hoveredBondId: string | null,
    selectedBondIds: ReadonlySet<string>,
  ): void {
    for (const mesh of this.bondHighlightMeshes) {
      mesh.visible = false;
      this.atomGroup?.remove(mesh);
      mesh.geometry.dispose();
      (mesh.material as THREE.Material).dispose();
    }
    this.bondHighlightMeshes = [];
    if (!this.atomGroup) {
      return;
    }

    // Highlight the hovered bond and all selected bonds (hover style wins on
    // the single hovered bond; selected bonds get a stronger persistent glow).
    const toHighlight = new Map<string, 'hover' | 'selected'>();
    if (hoveredBondId) {
      toHighlight.set(hoveredBondId, 'hover');
    }
    for (const id of selectedBondIds) {
      if (!toHighlight.has(id)) {
        toHighlight.set(id, 'selected');
      }
    }

    for (const [bondId, kind] of toHighlight) {
      const entry = this.bondMeshes.find((b) => b.bond.BondID === bondId);
      if (!entry) {
        continue;
      }

      const color = kind === 'hover' ? 0x38bdf8 : 0x0284c7;
      const opacity = kind === 'hover' ? 0.35 : 0.5;
      for (const mesh of entry.meshes) {
        const params = mesh.geometry as THREE.CylinderGeometry;
        const highlight = new THREE.Mesh(
          new THREE.CylinderGeometry(
            params.parameters.radiusTop * 1.4,
            params.parameters.radiusBottom * 1.4,
            params.parameters.height,
            12,
            1,
            true,
          ),
          new THREE.MeshBasicMaterial({ color, transparent: true, opacity }),
        );
        highlight.position.copy(mesh.position);
        highlight.quaternion.copy(mesh.quaternion);
        this.atomGroup.add(highlight);
        this.bondHighlightMeshes.push(highlight);
      }
    }
  }

  readonly legend = computed(() => {
    const mode = this.atomColorMode();
    if (mode === 'elements') {
      return null;
    }
    const values = this.atoms()
      .map((a) => atomPropertyValue(a, mode))
      .filter((v): v is number => v !== null);
    if (!values.length) {
      return null;
    }
    const max = Math.max(...values.map(Math.abs));
    return { min: -max, max };
  });
}

function bondModeColor(value: number, max: number): [number, number, number] {
  if (max <= 0) {
    return [150, 150, 150];
  }
  const t = Math.max(-1, Math.min(1, value / max));
  if (t >= 0) {
    // green = bonding contribution, intensity scaled to max
    return [Math.round(220 * (1 - t)), Math.round(255 - 35 * (1 - t)), Math.round(220 * (1 - t))];
  }
  // red = antibonding contribution
  return [255, Math.round(255 * (1 + t)), Math.round(255 * (1 + t))];
}
