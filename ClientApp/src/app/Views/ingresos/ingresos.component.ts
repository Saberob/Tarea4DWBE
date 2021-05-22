import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { EmpleadoList } from 'src/app/interfaces/empleado';
import { Ingreso, IngresoSubmit, IngresoSubmitW } from 'src/app/interfaces/ingresos';
import { ApiRestService } from 'src/app/Services/api-rest.service';

@Component({
  selector: 'app-ingresos',
  templateUrl: './ingresos.component.html', 
  styleUrls: ['./ingresos.component.css']
})
export class IngresosComponent implements OnInit {
  newIngreso = false;
  modIngreso = false;
  RegIdtoMod!: number;
  years = [2000,2001,2002,2003,2004,2005,2006,2007,2008,2009,2010,2011,2012,2013,2014,2015,2016,2017,2018,2019,2020,2021];
  days = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30]
  months = [1,2,3,4,5,6,7,8,9,10,11,12];
  hours = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23];
  minutes = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59];


  displayedColumns: string[] = ['id', 'nombre', 'fecha', 'hora', 'actions'];
  dataSource = new MatTableDataSource<Ingreso>();
  ingresos: Ingreso[] = [];
  empleados: EmpleadoList[] = [];  
  newIng!: FormGroup;
  modIng!: FormGroup;

  @ViewChild(MatSort) sort!: MatSort;
  

  constructor(private fb: FormBuilder,private router: Router,private api:ApiRestService, private _snackBar: MatSnackBar) { 

    this.api.getEmpleados().subscribe(data => {
      data.forEach(element => {
        var a: EmpleadoList = {
            id: element.id,
            Nombre: element.Nombre + " " + element.ApellidoP + " " + element.ApellidoM
        };
        this.empleados.push(a);
      });
    });

    this.newIng = this.fb.group({
      EmpId: new FormControl('', Validators.required),
    });
  }

  ngOnInit(): void {
    this.getIngresos();
  }

  getIngresos(){
    this.api.getIngresos().subscribe(data => {
      this.ingresos = data;
      this.dataSource = new MatTableDataSource(this.ingresos);
    }); 
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  deleteIngreso(id: number): void{
    console.log(id);
    if(confirm("¿Esta seguro de eliminar al empleado?")){
      this.api.deleteIngreso(id).subscribe(data => {
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

  addIngreso(){
    console.log(this.newIng.value.EmpId);
    const ingS: IngresoSubmit = {
      EmpleadoId: this.newIng.value.EmpId
    }
    this.api.postIngreso(ingS).subscribe(data => {
      console.log(data);
    });

    this._snackBar.open('El ingreso a sido agregado', '', {
      duration: 1500,
      horizontalPosition: 'center', 
      verticalPosition: 'bottom'
    })

    this.newIngreso = false;
    this.ngOnInit();
  }

  toModifyIngreso(id: number){
    this.modIng = this.fb.group({
      EmpleadoId: new FormControl('', ),
      day: new FormControl(32, ),
      month: new FormControl(1,),
      year: new FormControl(2021,),
      hour: new FormControl(25, ),
      minute: new FormControl(1, ),
    });

    this.modIngreso = true;
    this.RegIdtoMod = id;
  }
  
  modifyIngreso(){
    const ingresoS: IngresoSubmitW = {
      RegistroId: this.RegIdtoMod,
      EmpleadoId: this.modIng.value.EmpleadoId,
      year: this.modIng.value.year,
      month: this.modIng.value.month,
      day: this.modIng.value.day,
      hours: this.modIng.value.hour,
      minutes: this.modIng.value.minute,
      seconds: 0
    }

    this.api.modifyIngreso(ingresoS).subscribe(data => {
      console.log(data);
    }, error => {
      if(error.status == 404){
        alert("El empleado no fue encontrado")
      }
    });

    this._snackBar.open('El ingreso a sido modificado', '', {
      duration: 1500,
      horizontalPosition: 'center', 
      verticalPosition: 'bottom'
    })

    this.modIngreso = false;
    this.ngOnInit();
  }
}
