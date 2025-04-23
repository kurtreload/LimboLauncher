# Custom World of Warcraft Launcher
Launcher custom codificado basado en WOWLauncher.

### Características
* Descarga los MPQ actualizados y crea nuevos archivos si existen en el host pero no en la carpeta del wow.
* Interfaz simple simple, archivo ligero.

### Requerimientos
* Windows 10 o mejor.

#### Para desarrolladores
* Visual Studio 2019 o 2022 Community
* uso: cambiar las siguientes líneas, por la dirección de tu servidor:
- private string urlPatchlist = "https://limbo.org.pe/PatchHD/plist.txt";
- private string urlParches = "https://limbo.org.pe/PatchHD/";

* Archivo plist:
- NombreDeArchivoConExtensión HashMD5
* No se requiere de modificaciones adicionales, al menos que cambie el código para actualizar realmlist o alguna característica adicional.

#### Para usuarios
* .NET Desktop Runtime 6.0+
* En algunos casos, desactivar el antivirus. Pueden revisar el código fuente para comprobar que el programa hace lo que hace y no otra cosa.

### Licencia
Si bien no se menciona ninguna marca comercial,
Todas las marcas comerciales pertenecen a sus respectivos dueños.
