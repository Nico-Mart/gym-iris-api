# 🏋️ Backend Gym Iris

API REST para gestión integral de gimnasio desarrollada con .NET 10, diseñada para administrar usuarios, membresías, pagos y precios dinámicos.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=dotnet)](https://dotnet.microsoft.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0+-4479A1?style=flat&logo=mysql&logoColor=white)](https://www.mysql.com/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-9.0-512BD4?style=flat)](https://docs.microsoft.com/en-us/ef/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=flat&logo=swagger&logoColor=black)](https://swagger.io/)

---

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Tecnologías](#-tecnologías)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Migraciones](#-migraciones)
- [Ejecución](#-ejecución)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Endpoints API](#-endpoints-api)
- [Autenticación](#-autenticación)
- [Documentación Interactiva](#-documentación-interactiva)
- [Frontend](#-frontend)
- [Contribuir](#-contribuir)
- [Licencia](#-licencia)

---

## ✨ Características

- ✅ **Gestión de Usuarios**: CRUD completo con estados de membresía (Pending, Partial, Paid, Suspended)
- 💰 **Sistema de Pagos**: Registro y seguimiento de pagos mensuales por usuario
- 💵 **Precios Dinámicos**: Configuración flexible de precios para Musculación y Pilates
- 🔐 **Autenticación Admin**: Validación segura con BCrypt para operaciones administrativas
- 📊 **Historial de Pagos**: Tracking completo de pagos por usuario, mes y año
- 🚫 **Validaciones de Negocio**: 
  - Pagos no pueden exceder el precio configurado
  - Un solo pago por usuario/mes/año
  - Emails únicos por usuario
- 📄 **Swagger/OpenAPI**: Documentación interactiva integrada
- 🔄 **CORS Configurado**: Listo para integración con frontend React/Vite

---

## 🛠️ Tecnologías

- **Framework**: .NET 10
- **ORM**: Entity Framework Core 9.0
- **Base de Datos**: MySQL 8.0+
- **Autenticación**: BCrypt.Net-Next
- **Documentación**: Swashbuckle (Swagger/OpenAPI)
- **Arquitectura**: Repository Pattern + Service Layer
- **DTOs**: Separación de concerns con Data Transfer Objects

### Paquetes NuGet

```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.1.0" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="7.2.0" />
```

---

## 📦 Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MySQL Server 8.0+](https://dev.mysql.com/downloads/mysql/)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

---

## 🚀 Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/backend-gym-iris.git
cd backend-gym-iris
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Configurar base de datos

Edita `appsettings.json` con tus credenciales de MySQL:

```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=localhost;Database=gymiris;User=root;Password=tu_password;"
  }
}
```

---

## ⚙️ Configuración

### appsettings.json

```json
{
  "Logging": {
	"LogLevel": {
	  "Default": "Information",
	  "Microsoft.AspNetCore": "Warning"
	}
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
	"DefaultConnection": "Server=localhost;Database=gymiris;User=root;Password=yourpassword;"
  }
}
```

### Variables de Entorno (Opcional)

```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Database=gymiris;User=root;Password=yourpassword;"
```

---

## 🗄️ Migraciones

### Crear la base de datos

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Aplicar migración de precios de membresías

```bash
dotnet ef migrations add AddMembershipPricesAndPaymentTypes
dotnet ef database update
```

### Datos Seed (Automático al iniciar)

El sistema crea automáticamente:

**Admin por defecto:**
- Password: `IrisAdmin2026!`

**Precios iniciales (ARS):**
- Musculación: $38.000
- Pilates: $42.000

---

## ▶️ Ejecución

### Modo Desarrollo

```bash
dotnet run
```

O desde Visual Studio: `F5`

### Modo Producción

```bash
dotnet publish -c Release
cd bin/Release/net10.0/publish
dotnet Backend-Gym-Iris.dll
```

### Puerto por defecto

La aplicación estará disponible en:
- **HTTPS**: `https://localhost:7XXX`
- **HTTP**: `http://localhost:5XXX`

*(El puerto exacto se muestra en la consola al iniciar)*

---

## 📁 Estructura del Proyecto

```
Backend-Gym-Iris/
├── Controllers/
│   ├── AdminsController.cs       # Validación de admin
│   ├── UsersController.cs        # CRUD de usuarios
│   ├── PaymentsController.cs     # Gestión de pagos
│   └── PricesController.cs       # Gestión de precios
├── Services/
│   ├── IUserService.cs
│   ├── UserService.cs
│   ├── IAdminService.cs
│   ├── AdminService.cs
│   ├── IPaymentService.cs
│   ├── PaymentService.cs
│   ├── IMembershipPriceService.cs
│   └── MembershipPriceService.cs
├── Repositories/
│   ├── IUserRepository.cs
│   ├── UserRepository.cs
│   ├── IPaymentRepository.cs
│   ├── PaymentRepository.cs
│   ├── IMembershipPriceRepository.cs
│   └── MembershipPriceRepository.cs
├── Entities/
│   ├── User.cs                   # Entidad Usuario
│   ├── Admin.cs                  # Entidad Admin
│   ├── Payment.cs                # Entidad Pago
│   └── MembershipPrice.cs        # Entidad Precio
├── DTOs/
│   ├── User/
│   │   └── UserDTOs.cs
│   ├── Admin/
│   │   └── AdminDTOs.cs
│   ├── Payment/
│   │   └── PaymentDTOs.cs
│   └── MembershipPrice/
│       └── MembershipPriceDTOs.cs
├── Data/
│   └── ApplicationDbContext.cs   # Contexto EF Core
├── Enums/
│   ├── MembershipStatus.cs
│   └── MembershipType.cs
├── Migrations/                   # Migraciones EF
├── Program.cs                    # Punto de entrada
├── appsettings.json
└── README.md
```

---

## 🌐 Endpoints API

### 🔐 Autenticación

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/admins/validate` | Validar contraseña de admin |

### 👥 Usuarios

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/users` | Obtener todos los usuarios |
| GET | `/api/users/{id}` | Obtener usuario por ID |
| POST | `/api/users` | Crear nuevo usuario |
| PUT | `/api/users/{id}` | Actualizar usuario |
| DELETE | `/api/users/{id}` | Eliminar usuario |

### 💰 Pagos

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/payments` | Listar todos los pagos |
| GET | `/api/payments/{id}` | Obtener pago por ID |
| GET | `/api/payments/user/{userId}` | Pagos de un usuario |
| GET | `/api/payments/user/{userId}/total` | Total pagado por usuario |
| POST | `/api/payments` | Crear nuevo pago |
| PUT | `/api/payments/{id}` | Actualizar pago |
| DELETE | `/api/payments/{id}` | Eliminar pago |

### 💵 Precios

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/prices` | Obtener precios actuales |
| GET | `/api/prices/{id}` | Obtener precio por ID |
| PUT | `/api/prices/{id}` | Actualizar precio (admin) |

---

## 🔐 Autenticación

### Validar Admin

```bash
POST /api/admins/validate
Content-Type: application/json

{
  "password": "IrisAdmin2026!"
}
```

**Respuesta exitosa (200):**
```json
{
  "message": "Contraseña válida"
}
```

---

## 📖 Documentación Interactiva

Una vez iniciada la aplicación, accede a Swagger UI:

```
https://localhost:7XXX/
```

Swagger proporciona:
- 📋 Lista completa de endpoints
- 🧪 Interfaz para probar cada endpoint
- 📝 Schemas de request/response
- ✅ Validaciones y ejemplos

---

## 🎨 Frontend

Este backend está diseñado para integrarse con un frontend React + Vite.

### Configuración CORS

El backend ya está configurado para aceptar peticiones desde:
```
http://localhost:5173
```

### Documentación para Frontend

Consulta los siguientes archivos en el repositorio:
- `FRONTEND_API_DOCS.md` - Documentación técnica completa
- `FRONTEND_PROMPT.md` - Guía rápida de integración

### Ejemplo de integración (Axios)

```typescript
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7XXX/api',
  headers: {
	'Content-Type': 'application/json',
  },
});

// Obtener usuarios
const getUsers = async () => {
  const response = await api.get('/users');
  return response.data;
};

// Crear pago
const createPayment = async (data) => {
  const response = await api.post('/payments', {
	userId: data.userId,
	membershipType: data.membershipType, // "Musculacion" o "Pilates"
	amount: data.amount,
	month: data.month,
	year: data.year,
	notes: data.notes
  });
  return response.data;
};
```

---

## 🧪 Testing

### Probar con Swagger

1. Inicia la aplicación
2. Navega a `https://localhost:PUERTO/`
3. Usa la interfaz interactiva para probar endpoints

### Probar con cURL

```bash
# Validar admin
curl -X POST https://localhost:7XXX/api/admins/validate \
  -H "Content-Type: application/json" \
  -d '{"password":"IrisAdmin2026!"}'

# Obtener usuarios
curl https://localhost:7XXX/api/users

# Crear usuario
curl -X POST https://localhost:7XXX/api/users \
  -H "Content-Type: application/json" \
  -d '{
	"name": "Juan Pérez",
	"email": "juan@example.com",
	"telephone": "1234567890",
	"activity": "Musculacion"
  }'
```

---

## 📊 Modelos de Datos

### Usuario

```csharp
public class User
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public string Telephone { get; set; }
	public DateTime JoinDate { get; set; }
	public MembershipStatus Status { get; set; }
	public string Activity { get; set; }
	public ICollection<Payment> Payments { get; set; }
}
```

### Pago

```csharp
public class Payment
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public MembershipType MembershipType { get; set; }
	public decimal Amount { get; set; }
	public DateTime PaymentDate { get; set; }
	public int Month { get; set; }
	public int Year { get; set; }
	public string? Notes { get; set; }
	public User User { get; set; }
}
```

### Precio de Membresía

```csharp
public class MembershipPrice
{
	public int Id { get; set; }
	public MembershipType Type { get; set; }
	public decimal Price { get; set; }
	public DateTime LastUpdated { get; set; }
}
```

---

## 🔒 Seguridad

- ✅ Contraseñas hasheadas con **BCrypt**
- ✅ Validación de inputs en DTOs
- ✅ CORS configurado específicamente
- ✅ HTTPS habilitado por defecto
- ✅ Validaciones de negocio en capa de servicio

---

## 🐛 Troubleshooting

### Error de conexión a MySQL

```bash
# Verificar que MySQL esté corriendo
mysql -u root -p

# Verificar la cadena de conexión en appsettings.json
```

### Error en migraciones

```bash
# Eliminar migraciones anteriores
dotnet ef database drop -f
dotnet ef migrations remove

# Recrear migraciones
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### CORS Error desde frontend

Verifica que el puerto del frontend sea `5173` o actualiza en `Program.cs`:

```csharp
options.AddPolicy("AllowFrontend", policy =>
	policy.WithOrigins("http://localhost:TU_PUERTO")
```

---

## 🤝 Contribuir

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

---

## 📝 Roadmap

- [ ] Implementar JWT Authentication
- [ ] Agregar endpoints de reportes y estadísticas
- [ ] Sistema de notificaciones por email
- [ ] Backup automático de base de datos
- [ ] Tests unitarios y de integración
- [ ] Dockerización

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

---

## 👥 Autores

**Gym Iris Team**

---

## 🙏 Agradecimientos

- [.NET Community](https://dotnet.microsoft.com/platform/community)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Pomelo MySQL Provider](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)

---

## 📧 Contacto

Para consultas o soporte:
- 📧 Email: contacto@gymiris.com
- 🌐 Website: [gymiris.com](https://gymiris.com)

---

<div align="center">

**⭐ Si este proyecto te resultó útil, considera darle una estrella ⭐**

</div>
