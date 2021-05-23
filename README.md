# PIA Programación Web: Back-End :computer:
_Back-End de una WEB API CRUD con login más Front-End._

## Tecnología :wrench:
El proyecto fue construido empleando:
- SQLServer Management Studio v18.9.1
- ASP .NET Core v3.1
- Angular v12.0.1

## Descarga :inbox_tray:
Para poder descargar el proyecto es necesario tener instalado [Git](https://git-scm.com/downloads), abrir la consola en la carpeta donde se quiera descargar el proyecto y emplear el siguiete comando en la consola:
```sh
git clone https://github.com/Saberob/Tarea4DWBE.git
```
## Requisitos :electric_plug:
Es muy recomendable tener instalados las siguientes herramientas para poder ejecutar la API y la aplicación:

- [Visual Studio](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2019)
- [Postman](https://www.postman.com/downloads/)
- [SQL Server 2019](https://www.microsoft.com/en-in/sql-server/sql-server-downloads)
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Node.js](https://nodejs.org/en/download/)
- [Angular CLI](https://angular.io/guide/setup-local#install-the-angular-cli)

## Ejecución del servidor :arrows_counterclockwise: :globe_with_meridians:
Teniendo ya instaladas los programas anteriores, y hecho clone al proyecto, tendremos que realizar los siguientes pasos:

1. Abrir el archivo "SQLQuery1.sql" en SQL Server Management Studio (SSMS) y ejecutarlo para que se genere la base de datos. Se recomienda ejecutar el query por partes.
2. Hecho lo anterior, sin cerrar SSMS, abrimos la carpeta RFID y damos clic a "RFID.sln". Esto hará que se abra el código de la API en Visual Studio (VS).
3. Ya abierto la API en VS le damos a ejecutar, y sí no hemos modificado el usuario y contraseña de SSMS, la API empezará a funcionar (puede tardar unos minutos para que se compile).

**Nota:** asegurese que la base de datos esté conectada.
**Nota:** para detener el servidor oprima las teclas `Ctrl` + `C`.

Sí el servidor está funcionando correctamente, debería de emerger la consola de VS tal y como semuestra en la imágen:
![img_servidor](https://github.com/Saberob/Tarea4DWBE/blob/main/imgs/servidor_funcionando.png)

## Ejecución de la aplicación :arrows_counterclockwise: :iphone:
Teniendo ya instalados los programas de la sección de requisitos, y hecho clone al proyecto, tendremos que realizar los siguientes pasos:

1. Abrir en Visual Studio Code (VSC) toda la carpeta de "ClientApp".
2. Ejecutar, en consola cmd o bash los siguientes comandos para descargar los modulos necesarios para la ejecución de la aplicación (la consola debe de indicar la dirección de la carpeta ClientApp):
    ```sh
    npm i -g npm-check-updates
    ncu -u
    npm install
    ```
3. Para que la aplicación se ejecute debemos de escribir en consola el siguiente comando:
    ```sh
    ng serve
    ```
4. Para poder acceder a los datos de la base de datos desde la aplicación, es necesario que el servidor/API esté funcionando (en la sección anterior se especifica como hacer que esto ocurra).

**Nota:** para detener la ejecución de la aplicación oprima las teclas `Ctrl` + `C`.
**Nota:** sí al ejecutar el comando `ng serve` no se despliega la aplicación en una ventana de su navegador, detenga la ejecución de la aplicación y vuelva a internar con el comando `ng serve -o`.

Sí la aplicación está funcionando correctamente, debería de emerger una nueva ventana en el navegador donde semuestre la aplicación como en la siguiente imágen (con las casillas vacías):
![img_aplicacion](https://github.com/Saberob/Tarea4DWBE/blob/main/imgs/aplicacion_funcionando.png)

## Endpoints :envelope: :page_facing_up:
Sí se tiene descargado Postman, por medio de este programa podremos importar el archivo "PIADWBE.postman_collection.json", el cual contiene una colección de todos los endpoints de la API.
La API cuenta con 10 endpoints, las cuales son los siguientes:


## Desarrolladores :hammer:
* ***Alberto Natanael Sánchez Robles***... ... ... ... ... ... ... ... ...*1861608*
* ***Leonardo Román Sáenz Flores***... ... ... ... ... ... ... ... ... ... *1855453*
* ***Andrik de la Cruz Martínez***... ... ... ... ... ... ... ... ... ... ... ...*1863369*