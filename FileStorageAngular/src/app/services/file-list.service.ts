import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Dataset } from '../_interfaces/dataset.model';

@Injectable({
  providedIn: 'root',
})
export class FileListService {
  constructor(private http: HttpClient) {}

  rootUrl: string = 'http://localhost:63659/api';
  controllerUpload: string = 'filedetail';

  getData(): Observable<Dataset[]> {
    return this.http.get<Dataset[]>(this.rootUrl + '/' + this.controllerUpload);
  }

  deleteFile(id: number) {
    return this.http.delete(
      this.rootUrl + '/' + this.controllerUpload + '/' + id
    );
  }

  downloadFile(id: number): Observable<any> {
    return this.http.get(
      this.rootUrl + '/' + this.controllerUpload + '/' + id,
      { responseType: 'arraybuffer' }
    );
  }
}
