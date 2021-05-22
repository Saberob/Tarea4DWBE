import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Empleado, EmpleadoSubmit, EmpleadoSubmitWI } from '../interfaces/empleado';
import { Ingreso, IngresoSubmit, IngresoSubmitW } from '../interfaces/ingresos';
import { Token } from '../interfaces/login';
import { Login, NewUser } from '../interfaces/user';

@Injectable({
  providedIn: 'root'
})
export class ApiRestService {
  url: string = "https://localhost:5001/api"

  constructor(private http: HttpClient) { }

  login(user: Login): Observable<Token>{
    let dir = this.url + "/login"
    return this.http.post<Token>(dir, user);
  }

  postUser(newUser: NewUser): Observable<Token>{
    let dir = this.url + "/usuarios"
    return this.http.post<Token>(dir, newUser);
  }

  getEmpleados(): Observable<Empleado[]> {
    let dir = this.url + "/empleado"
    return this.http.get<Empleado[]>(dir);
  }

  postEmpleado(data: EmpleadoSubmit): Observable<any> {
    let dir = this.url + "/empleado"
    return this.http.post<any>(dir, data);
  }

  modifyEmpleado(data: EmpleadoSubmitWI): Observable<any> {
    let dir = this.url + "/empleado/" + data.Id.toString()
    return this.http.put<any>(dir, data);
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

  modifyIngreso(data: IngresoSubmitW): Observable<any> {
    let dir = this.url + "/ingresos"
    return this.http.put<any>(dir, data);
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
