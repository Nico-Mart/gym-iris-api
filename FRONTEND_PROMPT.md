# 🎯 PROMPT PARA COPILOT FRONTEND (React + Vite)

Copia y pega esto en tu chat de frontend:

---

Hola, estoy trabajando en un frontend React + Vite para un sistema de gestión de gimnasio. Tengo el backend ya funcionando con una Web API en .NET 10.

## 📋 Backend disponible

El backend tiene estos endpoints principales:

### 🔐 Autenticación
- **POST** `/api/admins/validate` - Validar password de admin
  - Password por defecto: `IrisAdmin2026!`

### 👥 Usuarios
- **GET** `/api/users` - Listar todos los usuarios
- **GET** `/api/users/{id}` - Obtener usuario por ID
- **POST** `/api/users` - Crear usuario nuevo (name, email, telephone)
- **PUT** `/api/users/{id}` - Actualizar usuario
- **DELETE** `/api/users/{id}` - Eliminar usuario

Cada usuario tiene:
- `id`, `name`, `email`, `telephone`
- `joinDate` (fecha de alta)
- `status`: "Pending" | "Partial" | "Paid" | "Suspended"
- `payments`: array de pagos históricos

### 💰 Pagos
- **GET** `/api/payments` - Listar todos los pagos
- **GET** `/api/payments/user/{userId}` - Pagos de un usuario
- **POST** `/api/payments` - Crear pago
  - Requiere: `userId`, `membershipType` ("Musculacion" o "Pilates"), `amount`, `month`, `year`, `notes`
  - ⚠️ El `amount` NO puede superar el precio actual de la membresía
- **PUT** `/api/payments/{id}` - Actualizar pago
- **DELETE** `/api/payments/{id}` - Eliminar pago

### 💵 Precios de Membresías
- **GET** `/api/prices` - Obtener precios actuales
- **PUT** `/api/prices/{id}` - Actualizar precio (solo admin)

Precios por defecto (en pesos argentinos ARS):
- **Musculación**: $38.000
- **Pilates**: $42.000

## 🎯 Lo que necesito en el frontend

1. **Panel de Admin** con login (password: `IrisAdmin2026!`)
2. **Gestión de Usuarios** (CRUD completo)
3. **Gestión de Pagos** (registrar pagos mensuales por usuario)
4. **Gestión de Precios** (actualizar precios de Musculación/Pilates)
5. **Dashboard** con estadísticas básicas (usuarios activos, pagos del mes, etc.)

## ⚙️ Configuración

- **Base URL del backend**: `https://localhost:7XXX/api` (ajustar puerto según tu launchSettings.json)
- **CORS**: Ya está configurado para `http://localhost:5173` (puerto por defecto de Vite)
- **Formato**: JSON
- **Swagger**: Disponible en `https://localhost:PUERTO/` para ver la documentación interactiva

## 📦 Tipos de datos importantes

```typescript
// Usuario
interface User {
  id: number;
  name: string;
  email: string;
  telephone: string;
  joinDate: string;
  status: "Pending" | "Partial" | "Paid" | "Suspended";
  payments: Payment[];
}

// Pago
interface Payment {
  id: number;
  userId: number;
  userName: string;
  membershipType: "Musculacion" | "Pilates";
  amount: number;
  paymentDate: string;
  month: number;
  year: number;
  notes?: string;
}

// Precio
interface Price {
  id: number;
  type: "Musculacion" | "Pilates";
  price: number;
  lastUpdated: string;
}
```

## ✅ Ejemplo de request para crear un pago

```javascript
POST /api/payments
{
  "userId": 1,
  "membershipType": "Musculacion",
  "amount": 38000.00,
  "month": 3,
  "year": 2024,
  "notes": "Pago marzo"
}
```

## 🚀 ¿Qué necesitas que te ayude a construir primero?

Tengo toda la documentación completa de la API lista en un archivo Markdown si necesitas más detalles.
