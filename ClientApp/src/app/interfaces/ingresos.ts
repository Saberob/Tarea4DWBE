export interface Ingreso{
    RegistroId: number,
    Nombre: string,
    Fecha: string,
    Hora: string
}

export interface IngresoSubmit {
    EmpleadoId: number
}

export interface IngresoSubmitW {
    RegistroId: number,
    EmpleadoId: number,
    day: number,
    month: number,
    year: number,
    hours: number,
    minutes: number,
    seconds: number,
}
