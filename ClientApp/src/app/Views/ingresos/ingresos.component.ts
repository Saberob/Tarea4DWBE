import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { EmpleadoList } from 'src/app/interfaces/empleado';
import { Ingreso } from 'src/app/interfaces/ingresos';
import { IngresoSubmit } from 'src/app/interfaces/ingresoSubmit';
import { ApiRestService } from 'src/app/Services/api-rest.service';

@Component({
  selector: 'app-ingresos',
  templateUrl: './ingresos.component.html', 
  styleUrls: ['./ingresos.component.css']
})
export class IngresosComponent implements OnInit {
  newIngreso = false;

  displayedColumns: string[] = ['id', 'nombre', 'fecha', 'hora', 'actions'];
  dataSource = new MatTableDataSource<Ingreso>();
  ingresos: Ingreso[] = [];
  empleados: EmpleadoList[] = [];  
  form!: FormGroup;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
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

    this.form = this.fb.group({
      EmpId: ['', Validators.required]
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
    this.dataSource.paginator = this.paginator;
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

  agregarIngreso(){
    console.log(this.form.value.EmpId);
    const ingS: IngresoSubmit = {
      EmpleadoId: this.form.value.EmpId
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

}
