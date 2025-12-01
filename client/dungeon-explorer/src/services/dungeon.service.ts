import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Position {
    x: number;
    y: number;
}

export interface CreateMapRequest {
    width: number;
    height: number;
    start: Position;
    goal: Position;
    obstacles: Position[];
}

export interface MapResponse extends CreateMapRequest {
    id: string;
}

export interface PathResponse {
    mapId: string;
    steps: Position[];
    length: number;
}

@Injectable({
    providedIn: 'root'
})
export class DungeonService {
    private baseUrl = '/api/maps';

    constructor(private http: HttpClient) {}

    createMap(request: CreateMapRequest): Observable<MapResponse> {
        return this.http.post<MapResponse>(this.baseUrl, request);
    }

    getMap(id: string): Observable<MapResponse> {
        return this.http.get<MapResponse>(`${this.baseUrl}/${id}`);
    }

    getPath(id: string): Observable<PathResponse> {
        if (!id) {
            return new Observable<PathResponse>((observer) => {
                observer.error({ error: 'Invalid map ID' });
            });
        }
        return this.http.get<PathResponse>(`${this.baseUrl}/${id}/path`);
    }
}
