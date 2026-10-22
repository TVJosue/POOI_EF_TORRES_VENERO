Sistema de Gestión de Consumidores

Aplicación web desarrollada con ASP.NET MVC C# y SQL Server para administrar registros de consumidores mediante operaciones CRUD y procedimientos almacenados.

Tecnologías utilizadas

C#

ASP.NET MVC 5

.NET Framework

SQL Server

ADO.NET

HTML y Razor

Funcionalidades

Listar: visualizar los consumidores registrados.

Registrar: agregar consumidores con validación de datos.

Consultar: mostrar los detalles de un consumidor.

Actualizar: modificar los datos registrados.

Eliminar: eliminar consumidores por DNI.

Validaciones: verificar DNI de 8 dígitos, campos obligatorios, DNI duplicado y correo electrónico único.

Base de datos

Base de datos: ExamenFinal

Tablas principales:

tb_Consumidor

tb_paises

Procedimientos almacenados:

usp_agrega_consumidor

usp_eliminar_consumidor

usp_listar_consumidor

usp_obtener_consumidor

usp_actualizar_consumidor

usp_listar_paises

Instalación y ejecución

Clonar o descargar este repositorio.

Abrir la solución en Visual Studio.

Ejecutar el script SQL incluido en el proyecto para crear la base de datos y los procedimientos almacenados.

Configurar la cadena de conexión conexionADatos en el archivo Web.config, utilizando el servidor SQL Server correspondiente.

Verificar que el proyecto tenga instalados los componentes necesarios de ASP.NET MVC 5 y .NET Framework.

Compilar la solución y ejecutar con F5.

Autor

Josue Torres Venero

Proyecto académico de desarrollo web con ASP.NET MVC y SQL Server.
