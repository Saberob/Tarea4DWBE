import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Empleado } from '../interfaces/empleado';
import { EmpleadoSubmit, EmpleadoSubmitWI } from '../interfaces/empleadoSubmit';
import { Ingreso } from '../interfaces/ingresos';
import { IngresoSubmit, IngresoSubmitW } from '../interfaces/ingresoSubmit';

@Injectable({
  providedIn: 'root'
})
export class ApiRestService {
  url: string = "https://localhost:5001/api"

  constructor(private http: HttpClient) { }

  getEmpleados(): Observable<Empleado[]> {
    let dir = this.url + "/empleado"
    return this.http.get<Empleado[]>(dir);
  }

  getEmpleadoById(id: number): Observable<EmpleadoSubmitWI> {
    let dir = this.url + "/empleado/" + id.toString();
    return this.http.get<EmpleadoSubmitWI>(dir);
  }

  postEmpleado(data: EmpleadoSubmit): Observable<any> {
    let dir = this.url + "/empleado"
    return this.http.post<any>(dir, data);
  }

  deleteEmpleado(id: number): Observable<any> {                    
    let dir = this.url + "/empleado/" + id.toString();
    let options = {
      headers: new HttpHeaders({
        'Content-type': 'application/json'
      }),
      body: ''
    }
    return this.http.delete<any>(dir,options);
  }

  getIngresos(): Observable<Ingreso[]> {
    let dir = this.url + "/ingresos"
    return this.http.get<Ingreso[]>(dir)
  }

  postIngreso(data: IngresoSubmit): Observable<any> {
    let dir = this.url + "/ingresos"
    return this.http.post<any>(dir, data);
  }

  deleteIngreso(id: number): Observable<any> {
    let dir = this.url + "/ingresos/" + id.toString();
    let options = {
      headers: new HttpHeaders({
        'Content-type': 'application/json'
      }),
      body: ''
    }
    return this.http.delete<any>(dir, options);
  }

}
