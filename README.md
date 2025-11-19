# TaskTrackPro

Descripción general

**TaskTrackPro** es un proyecto realizado para la materia **Diseño de Aplicaciones 1 (Universidad ORT Uruguay)**.
El objetivo fue construir una aplicación aplicando **TDD, arquitectura en capas y persistencia con base de datos**, siguiendo buenas prácticas de ingeniería.

El proyecto no está finalizado, pero cuenta con una base sólida de dominio, servicios, acceso a datos y pruebas automatizadas.

**Objetivos del proyecto**
El proyecto se desarrolló con el objetivo de aplicar:
	•	TDD (Test-Driven Development)
	•	Arquitectura en capas (Dominio, Servicios, DataAccess, FrontEnd)
	•	Buenas prácticas de ingeniería
	•	GitFlow
	•	Testing con enfoque en cobertura
	•	Integración continua
	•	Persistencia con base de datos

**Testing (TDD) y cobertura**

TaskTrackPro fue construido siguiendo el ciclo TDD:
🟥 Red → 🟩 Green → ♻️ Refactor
Se desarrollaron pruebas en:
	•	Dominio
	•	Servicios
	•	DataAccess
	•	Pruebas de integración puntuales

Además:
	•	Se buscó asegurar **alta cobertura**, especialmente en lógica de negocio.
	•	Se utilizaron múltiples test suites (DominioTests, ServicesTests, DataAccessTest, etc.).
	•	Se integraron pipelines de **GitHub Actions** para ejecutar los tests automáticamente.

**Base de datos y persistencia**

El proyecto utiliza:
	•	Repositorios para acceder a datos
	•	SQL como motor principal
	•	Archivo docker-compose.yml para levantar la base en un contenedor
	•	Configuración de entorno (data/config, appsettings.json)
	•	Arquitectura desacoplada para permitir testeo y cambios futuros sin romper capas

**Tecnologías utilizadas**
	•	C#
	•	.NET 8
	•	MSTest (testing)
	•	SQL
	•	Docker / docker-compose
	•	GitHub Actions
	•	Git + GitFlow
	•	Arquitectura en capas

**Estado actual del proyecto**

El proyecto no está finalizado, pero incluye:
	•	Arquitectura definida
	•	Tests en todas las capas principales
	•	Alta cobertura en módulos clave
	•	Persistencia funcional
	•	Front-end preliminar
	•	Infraestructura de CI configurada

La base está lista para continuar construcción de funcionalidades futuras.

**Próximos pasos**
	•	Completar las funcionalidades del front-end
	•	Refinar reglas de negocio y casos de uso
	•	Expandir pruebas de integración
	•	Documentación técnica adicional
	•	Mejorar experiencia de usuario
	•	Finalizar capa de servicios con todas las operaciones previstas

## **Cómo ejecutar el proyecto**

**1. Requisitos previos**

Asegurate de tener instalado:
	•	Docker Desktop
	•	.NET 8 SDK

**2. Iniciar la base de datos con Docker**

Desde la raíz del proyecto:
 docker compose up -d
Esto va a:
	•	Crear el contenedor de la base de datos
	•	Exponer los puertos configurados
	•	Levantar el servicio en segundo plano

Podés verificar que está corriendo con:
 docker ps

**3. Configurar la cadena de conexión**

La conexión se define en archivos de configuración como:
	•	appsettings.json
	•	archivos dentro de /data/config

Con Docker, normalmente el host es localhost y el puerto el que figura en docker-compose.yml.

Ejemplo típico:
Server=localhost;Port=5432;Database=tasktrackpro;User Id=postgres;Password=Passw1rd;

**4. Ejecutar la aplicación (.NET)**

dotnet build
dotnet run


**5. Ejecutar tests**
dotnet test

**6. Apagar los contenedores**
docker compose down

**Autores / Integrantes del equipo**

Proyecto realizado dentro del curso Diseño de Aplicaciones 1 - Universidad ORT Uruguay.
- Gonzalo Cabrera
- Nicolás RLL
- Juan Bautista Rey