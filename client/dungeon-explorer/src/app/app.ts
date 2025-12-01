import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { DungeonService, MapResponse, PathResponse, Position } from '../services/dungeon.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = 'dungeon-explorer';

  // Form model
  width = 10;
  height = 10;
  startX = 0;
  startY = 0;
  goalX = 9;
  goalY = 9;
  obstaclesText = '1,1;2,3;5,5'; // semicolon-separated list like "x,y;x,y"

  // State
  map: MapResponse | null = null;
  path: Position[] = [];
  errorMessage = '';
  isLoading = false;

  constructor(private dungeonService: DungeonService) {}

  onCreateMap() {
    this.errorMessage = '';
    this.isLoading = true;
    this.path = [];

    const obstacles = this.parseObstacles(this.obstaclesText);

    const request = {
      width: this.width,
      height: this.height,
      start: { x: this.startX, y: this.startY },
      goal: { x: this.goalX, y: this.goalY },
      obstacles
    };

    this.dungeonService.createMap(request).subscribe({
      next: (map) => {
        this.map = map;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error ?? 'Failed to create map';
        this.isLoading = false;
      }
    });
  }

  onComputePath() {
    if (!this.map?.id) return;

    this.errorMessage = '';
    this.isLoading = true;
    this.path = [];

    this.dungeonService.getPath(this.map.id).subscribe({
      next: (res: PathResponse) => {
        this.path = res.steps;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err?.error ?? 'Failed to compute path';
        this.isLoading = false;
      }
    });
  }

  parseObstacles(text: string): Position[] {
    if (!text.trim()) return [];
    return text
      .split(';')
      .map(part => part.trim())
      .filter(part => part.length > 0)
      .map(part => {
        const [xStr, yStr] = part.split(',');
        const x = Number(xStr);
        const y = Number(yStr);
        return { x, y };
      })
      .filter(p => !Number.isNaN(p.x) && !Number.isNaN(p.y));
  }

  isObstacle(x: number, y: number): boolean {
    if (!this.map) return false;
    return this.map.obstacles.some(o => o.x === x && o.y === y);
  }

  isStart(x: number, y: number): boolean {
    if (!this.map) return false;
    return this.map.start.x === x && this.map.start.y === y;
  }

  isGoal(x: number, y: number): boolean {
    if (!this.map) return false;
    return this.map.goal.x === x && this.map.goal.y === y;
  }

  isPath(x: number, y: number): boolean {
    return this.path.some(step => step.x === x && step.y === y);
  }

  cellClass(x: number, y: number): string {
    if (!this.map) return 'cell';

    if (this.isStart(x, y)) return 'cell start';
    if (this.isGoal(x, y)) return 'cell goal';
    if (this.isObstacle(x, y)) return 'cell obstacle';
    if (this.isPath(x, y)) return 'cell path';
    return 'cell';
  }

  rowArray(): number[] {
    return this.map ? Array.from({ length: this.map.height }, (_, i) => i) : [];
  }

  colArray(): number[] {
    return this.map ? Array.from({ length: this.map.width }, (_, i) => i) : [];
  }
}
