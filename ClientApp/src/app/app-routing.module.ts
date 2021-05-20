import { componentFactoryName } from '@angular/compiler';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserGuard } from './Guard/user-guard.guard';
import { EmpleadosComponent } from './Views/empleados/empleados.component';
import { IngresosComponent } from './Views/ingresos/ingresos.component';
import { LoginComponent } from './Views/login/login.component';


const routes: Routes = [
  { path:'', component: LoginComponent},
  { path:'Ingresos', canActivate:[UserGuard], component: IngresosComponent},
  { path:'Empleados', canActivate:[UserGuard], component: EmpleadosComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {onSameUrlNavigation: 'reload'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
