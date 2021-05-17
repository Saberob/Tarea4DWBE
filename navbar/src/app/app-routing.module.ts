import { componentFactoryName } from '@angular/compiler';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmpleadosComponent } from './Views/empleados/empleados.component';
import { IngresosComponent } from './Views/ingresos/ingresos.component';


const routes: Routes = [
  { path:'Ingresos', component: IngresosComponent},
  { path:'Empleados', component: EmpleadosComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {onSameUrlNavigation: 'reload'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
