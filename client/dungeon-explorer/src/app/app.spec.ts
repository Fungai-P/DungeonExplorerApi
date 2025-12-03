import { TestBed } from '@angular/core/testing';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { App } from './app';
import { of } from 'rxjs';import {
  DungeonService,
  CreateMapRequest,
  MapResponse,
  PathResponse
} from '../services/dungeon.service';

describe('App', () => {
  beforeEach(async () => {
    const dungeonServiceStub = {
      createMap: (request: CreateMapRequest) => of({
        id: 'test-map-id',
        ...request  } as MapResponse),
      getMap: (id: string) => of({
        id, width: 10, height: 10,
        start: { x: 0, y: 0 },
        goal: { x: 9, y: 9 }, obstacles: [] } as MapResponse),
      getPath: (id: string) => of({
        mapId: id,
        steps: [{ x: 0, y: 0 }, { x: 1, y: 1 }],
        length: 2 } as PathResponse)
    };

    await TestBed.configureTestingModule({
      imports: [App],
      providers: [
        provideHttpClientTesting(),        
        { provide: DungeonService, useValue: dungeonServiceStub }
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
