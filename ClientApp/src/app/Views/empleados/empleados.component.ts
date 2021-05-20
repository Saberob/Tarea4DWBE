import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Empleado } from 'src/app/interfaces/empleado';
import { EmpleadoSubmit, EmpleadoSubmitWI } from 'src/app/interfaces/empleadoSubmit';
import { ApiRestService } from 'src/app/Services/api-rest.service';

@Component({
  selector: 'app-empleados',
  templateUrl: './empleados.component.html',
  styleUrls: ['./empleados.component.css']
})
export class EmpleadosComponent implements OnInit {
  newEmpleado = false;

  displayedColumns: string[] = ['id', 'nombre', 'apellidoP', 'apellidoM', 'actions'];
  dataSource = new MatTableDataSource<Empleado>();
  empleados: Empleado[] = [];
  form!: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private fb: FormBuilder,private router: Router,private api:ApiRestService, private _snackBar: MatSnackBar) { 
    this.form = this.fb.group({
      Nombre: new FormControl('', Validators.required),
      ApellidoP: new FormControl('', Validators.required),
      ApellidoM: new FormControl('', Validators.required),
      Rfid: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(50)])
    });
  }

  ngOnInit(): void {
    this.getEmpleados();
  }

  getEmpleados(){
    this.api.getEmpleados().subscribe(data => {
      this.empleados = data;
      this.dataSource = new MatTableDataSource(this.empleados);
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if(this.dataSource.paginator){
      this.dataSource.paginator.firstPage();
    }
  }

  deleteEmpleado(id: number): void{
    let res = confirm("¿Esta seguro de eliminar al empleado?");

    if(res){
      this.api.deleteEmpleado(id).subscribe(data => {
        console.log(data)
      })

      this._snackBar.open('El empleado a sido eliminado', '', {
        duration: 1500,
        horizontalPosition: 'center',
        verticalPosition: 'bottom'
      })
      this.ngOnInit();
    }
    else{
      this.ngOnInit();
    }
  }

  agregarEmpleado(){
    const empleadoS: EmpleadoSubmit = {
      Nombre: this.form.value.Nombre,
      ApellidoP: this.form.value.ApellidoP,
      ApellidoM: this.form.value.ApellidoM,
      Rfid: this.form.value.Rfid
    }

    this.api.postEmpleado(empleadoS).subscribe(data => {
      console.log(data);
    });

    this._snackBar.open('El empleado a sido agregado', '', {
      duration: 1500,
      horizontalPosition: 'center', 
      verticalPosition: 'bottom'
    })

    this.newEmpleado = false;
    this.ngOnInit();
  }

}
