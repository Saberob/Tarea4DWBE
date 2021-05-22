export interface Empleado {
    id: number,
    Nombre: string,
    ApellidoP: string,
    ApellidoM: string
}

export interface EmpleadoList {
    id: number,
    Nombre: string,
}

export interface EmpleadoSubmit {
    Nombre: string,
    ApellidoP: string,
    ApellidoM: string,
    Rfid: string
}

export interface EmpleadoSubmitWI{
    Id: number,
    Nombre: string,
    ApellidoP: string,
    ApellidoM: string,
    Rfid: string
}

