import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Empleado, EmpleadoSubmit, EmpleadoSubmitWI  } from 'src/app/interfaces/empleado';
import { ApiRestService } from 'src/app/Services/api-rest.service';

@Component({
  selector: 'app-empleados',
  templateUrl: './empleados.component.html',
  styleUrls: ['./empleados.component.css']
})
export class EmpleadosComponent implements OnInit {
  newEmpleado = false;
  modEmpleado = false;
  EmpToMod!: number;

  displayedColumns: string[] = ['id', 'nombre', 'apellidoP', 'apellidoM', 'actions'];
  dataSource = new MatTableDataSource<Empleado>();
  empleados: Empleado[] = [];
  empleadoToMod!: EmpleadoSubmitWI;
  newEmp!: FormGroup;
  modifiedEmp!: FormGroup;

  @ViewChild(MatSort) sort!: MatSort;

  constructor(private fb: FormBuilder,private router: Router,private api:ApiRestService, private _snackBar: MatSnackBar) { 
    this.newEmp = this.fb.group({
      Nombre: new FormControl('', Validators.required),
      ApellidoP: new FormControl('', Validators.required),
      ApellidoM: new FormControl('', Validators.required),
      Rfid: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(50)])
    });

    this.modifiedEmp = this.fb.group({
      Id: new FormControl('', Validators.required),
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

  addEmpleado(){
    const empleadoS: EmpleadoSubmit = {
      Nombre: this.newEmp.value.Nombre,
      ApellidoP: this.newEmp.value.ApellidoP,
      ApellidoM: this.newEmp.value.ApellidoM,
      Rfid: this.newEmp.value.Rfid
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

  toModifyEmpleado(id: number){
    this.modifiedEmp = this.fb.group({
      Id: new FormControl(id, Validators.required),
      Nombre: new FormControl(''),
      ApellidoP: new FormControl(''),
      ApellidoM: new FormControl(''),
      Rfid: new FormControl('', [Validators.minLength(10),Validators.maxLength(50)])
    });

    this.modEmpleado = true;
    this.EmpToMod = id;
  }
  
  modifyEmpleado(){
    const empleadoS: EmpleadoSubmitWI = {
      Id: this.EmpToMod,
      Nombre: this.modifiedEmp.value.Nombre,
      ApellidoP: this.modifiedEmp.value.ApellidoP,
      ApellidoM: this.modifiedEmp.value.ApellidoM,
      Rfid: this.modifiedEmp.value.Rfid
    }

    this.api.modifyEmpleado(empleadoS).subscribe(data => {
      console.log(data);
    }, error => {
      if(error.status == 400){
        alert("El rfid ya esta registrado bajo otro empleado")
      }
    });

    this._snackBar.open('El empleado a sido modificado', '', {
      duration: 1500,
      horizontalPosition: 'center', 
      verticalPosition: 'bottom'
    })

    this.modEmpleado = false;
    this.ngOnInit();
  }
}
