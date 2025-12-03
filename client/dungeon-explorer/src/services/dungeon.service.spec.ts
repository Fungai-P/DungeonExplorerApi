import { TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';  // 👈 add throwError, keep of
import {
  DungeonService,
  CreateMapRequest,
  MapResponse,
  PathResponse
} from './dungeon.service';

describe('DungeonService (stubbed)', () => {
  let service: DungeonService;
  let dungeonServiceStub: {
    getMap: jasmine.Spy;
    createMap: jasmine.Spy;
    getPath: jasmine.Spy;
  };

  beforeEach(() => {
    // Spy-based stub
    dungeonServiceStub = {
      getMap: jasmine.createSpy('getMap'),
      createMap: jasmine.createSpy('createMap'),
      getPath: jasmine.createSpy('getPath')
    };

    TestBed.configureTestingModule({
      providers: [
        { provide: DungeonService, useValue: dungeonServiceStub }
      ]
    });

    service = TestBed.inject(DungeonService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should delegate to stubbed createMap and return its value', () => {
    const requestBody: CreateMapRequest = {
      width: 10,
      height: 10,
      start: { x: 0, y: 0 },
      goal: { x: 9, y: 9 },
      obstacles: [{ x: 1, y: 1 }]
    };

    const mockResponse: MapResponse = {
      id: '123',
      ...requestBody
    };

    dungeonServiceStub.createMap.and.returnValue(of(mockResponse));

    let actualResponse: MapResponse | undefined;

    service.createMap(requestBody).subscribe(res => {
      actualResponse = res;
    });

    expect(dungeonServiceStub.createMap).toHaveBeenCalledWith(requestBody);
    expect(actualResponse).toEqual(mockResponse);
  });

  it('should delegate to stubbed getMap and return its value', () => {
    const id = '123';

    const mockResponse: MapResponse = {
      id,
      width: 10,
      height: 10,
      start: { x: 0, y: 0 },
      goal: { x: 9, y: 9 },
      obstacles: []
    };

    dungeonServiceStub.getMap.and.returnValue(of(mockResponse));

    let actualResponse: MapResponse | undefined;

    service.getMap(id).subscribe(res => {
      actualResponse = res;
    });

    expect(dungeonServiceStub.getMap).toHaveBeenCalledWith(id);
    expect(actualResponse).toEqual(mockResponse);
  });

  it('should delegate to stubbed getPath and return its value for valid id', () => {
    const id = '123';

    const mockResponse: PathResponse = {
      mapId: id,
      steps: [
        { x: 0, y: 0 },
        { x: 1, y: 0 },
        { x: 2, y: 0 }
      ],
      length: 3
    };

    dungeonServiceStub.getPath.and.returnValue(of(mockResponse));

    let actualResponse: PathResponse | undefined;

    service.getPath(id).subscribe(res => {
      actualResponse = res;
    });

    expect(dungeonServiceStub.getPath).toHaveBeenCalledWith(id);
    expect(actualResponse).toEqual(mockResponse);
  });

  it('can simulate an error from stubbed getPath for empty id', () => {
    const error = { error: 'Invalid map ID' };

    // 👇 Use throwError instead of new Observable(...)
    dungeonServiceStub.getPath.and.returnValue(
      throwError(() => error)
    );

    let errorResponse: any;
    service.getPath('').subscribe({
      next: () => fail('Expected error for empty id'),
      error: err => {
        errorResponse = err;
      }
    });

    expect(dungeonServiceStub.getPath).toHaveBeenCalledWith('');
    expect(errorResponse).toEqual(error);
  });
});
