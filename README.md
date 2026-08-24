# Custom World of Warcraft Launcher
Launcher custom codificado basado en WOWLauncher.
He borrado todos los archivos, reemplazándolos por un comprimido.

### Características
* Descarga cualquier archivo y crea nuevos archivos si existen en el host pero no en la carpeta del wow. Si la carpeta especificada no existe, se crea.
* Interfaz simple simple, archivo ligero.

### Requerimientos
* Windows 10 o superior.

#### Para desarrolladores
* Visual Studio 2022 Community
* uso: cambiar las siguientes líneas en el archivo Form1.cs con tus propios datos:
> 23: la URL completa de tu archivo plist.txt<br/>
> 24: la dirección URL raíz de tus parches<br/>
> 25: la dirección de tu servidor, sin HTTP ni parámetros adicionales
* Compilar y distribuir

* Archivo plist en la web (URL completa):<br/>
> DirectorioRelativo/NombreDeArchivoConExtensión HashMD5<br/>
> NombreDeArchivoODirectorioParaBorrar 0
* No se requiere de modificaciones adicionales.<br/>
Puedes crear/parchar un archivo.<br/>
Puedes borrar un directorio o archivo colocando 0 como hash.<br/>

#### Para usuarios
* .NET Desktop Runtime 6.0+
* En algunos casos, desactivar el antivirus. Pueden revisar el código fuente para comprobar que el programa hace lo que hace y no otra cosa.

### Imágenes
![launcher](./parchando.png)<br/>
_El launcher en acción_

![directorio](./directorioWeb.png)<br/>
_Directorio donde se encuentra la publicación del patchlist_

![archivo](./archivoPLIST.png)<br/>
_Contenido de ejemplo del archivo PLIST.TXT_

### Licencia
Si bien no se menciona ninguna marca comercial,
Todas las marcas comerciales pertenecen a sus respectivos dueños.
