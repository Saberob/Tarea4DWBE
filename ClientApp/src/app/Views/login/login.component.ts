import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Login, NewUser } from 'src/app/interfaces/user';
import { ApiRestService } from 'src/app/Services/api-rest.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  hide = true;
  hideC = true;
  newUser = false;
  login!: FormGroup;
  newUserForm!: FormGroup;

  constructor(private snackBar: MatSnackBar,private router: Router,private api: ApiRestService, private fb: FormBuilder, private cookieService: CookieService) { }

  ngOnInit(): void {
    this.login = this.fb.group({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });

    this.newUserForm = this.fb.group({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      confirmPassword: new FormControl('', Validators.required)
    });
  }

  beginSession(): any {
    const user: Login = {
      username: this.login.value.username,
      password: this.login.value.password
    }

    this.api.login(user).subscribe( data => {
      this.cookieService.set('token', data.token, 0.00347222222)
      this.router.navigate(['/Ingresos'])
    }, error => {
      if(error.status == 404){
        alert("El usuario no existe")
      }else{
        if(error.status == 403){
          alert("El usuario o contraseña es incorrecto")
        }
      }
    });
  }

  postNewUser(){
    if(this.newUserForm.value.confirmPassword != this.newUserForm.value.password){
      alert("Las constraseñas no coinciden");
      return;
    }

    const newUser: NewUser = {
      username: this.newUserForm.value.username,
      password: this.newUserForm.value.password,
      confirmPassword: this.newUserForm.value.confirmPassword
    }

    this.api.postUser(newUser).subscribe( data => {
      this.snackBar.open('El usuario a sido agregado', '', {
        duration: 1500,
        horizontalPosition: 'center', 
        verticalPosition: 'bottom'
      })
    }, error => {
      if(error.status == 404){
        alert("El usuario ya existe")
      }else{
        if(error.status == 403){
          alert("las contraseñas no coinciden")
        }
      }
    });

    this.newUser= false;
  }
}
