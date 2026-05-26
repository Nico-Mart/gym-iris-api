# 🏋️ Backend Gym Iris - Documentación API para Frontend (React + Vite)

## 📋 Información General

- **Base URL**: `https://localhost:7XXX/api` (ajustar según tu puerto)
- **CORS**: Configurado para `http://localhost:5173` (Vite default)
- **Formato**: JSON
- **Swagger UI**: Disponible en `https://localhost:7XXX/` (en desarrollo)

---

## 🔐 Autenticación Admin

### POST `/api/admins/validate`
Valida la contraseña del administrador.

**Credenciales por defecto:**
- Password: `IrisAdmin2026!`

**Request Body:**
```json
{
  "password": "IrisAdmin2026!"
}
```

**Response (200 OK):**
```json
{
  "message": "Contraseña válida"
}
```

**Response (400 Bad Request):**
```json
{
  "message": "Contraseña incorrecta"
}
```

---

## 👥 Gestión de Usuarios

### GET `/api/users`
Obtiene todos los usuarios del gimnasio.

**Response (200 OK):**
```json
[
  {
	"id": 1,
	"name": "Juan Pérez",
	"email": "juan@example.com",
	"telephone": "1234567890",
	"joinDate": "2024-01-15T10:30:00Z",
	"status": "Paid",
	"payments": [
	  {
		"id": 1,
		"month": "2024-01",
		"status": "paid",
		"amount": 38000.00,
		"paidAmount": 38000.00,
		"paidDate": "2024-01-15"
	  }
	]
  }
]
```

### GET `/api/users/{id}`
Obtiene un usuario específico por ID.

**Response (200 OK):** Mismo formato que arriba

**Response (404 Not Found):**
```json
{
  "message": "Usuario con ID X no encontrado"
}
```

### POST `/api/users`
Crea un nuevo usuario.

**Request Body:**
```json
{
  "name": "María González",
  "email": "maria@example.com",
  "telephone": "0987654321"
}
```

**Response (201 Created):**
```json
{
  "id": 2,
  "name": "María González",
  "email": "maria@example.com",
  "telephone": "0987654321",
  "joinDate": "2024-03-20T14:00:00Z",
  "status": "Pending",
  "payments": []
}
```

### PUT `/api/users/{id}`
Actualiza un usuario existente.

**Request Body:**
```json
{
  "name": "María González",
  "email": "maria.nueva@example.com",
  "telephone": "1122334455",
  "status": "Paid"
}
```

**Response (200 OK):** Usuario actualizado

### DELETE `/api/users/{id}`
Elimina un usuario (también elimina sus pagos en cascada).

**Response (204 No Content):** Usuario eliminado exitosamente

---

## 💰 Gestión de Pagos

### GET `/api/payments`
Obtiene todos los pagos registrados.

**Response (200 OK):**
```json
[
  {
	"id": 1,
	"userId": 1,
	"userName": "Juan Pérez",
	"membershipType": "Musculacion",
	"amount": 38000.00,
	"paymentDate": "2024-01-15T10:00:00Z",
	"month": 1,
	"year": 2024,
	"notes": "Pago enero"
  }
]
```

### GET `/api/payments/{id}`
Obtiene un pago específico por ID.

### GET `/api/payments/user/{userId}`
Obtiene todos los pagos de un usuario específico.

### GET `/api/payments/user/{userId}/total`
Obtiene el total pagado por un usuario.

**Response (200 OK):**
```json
{
  "userId": 1,
  "totalPaid": 76000.00
}
```

### GET `/api/payments/user/{userId}/month/{month}/year/{year}/exists`
Verifica si un usuario ya tiene un pago registrado para un mes/año específico.

**Response (200 OK):**
```json
{
  "exists": true
}
```

### POST `/api/payments`
Crea un nuevo pago.

**Request Body:**
```json
{
  "userId": 1,
  "membershipType": "Musculacion",
  "amount": 38000.00,
  "month": 3,
  "year": 2024,
  "notes": "Pago marzo"
}
```

**Validaciones:**
- El `amount` NO puede superar el precio actual configurado para el tipo de membresía
- No puede haber pagos duplicados para el mismo usuario/mes/año
- El usuario debe existir

**Response (201 Created):**
```json
{
  "id": 5,
  "userId": 1,
  "userName": "Juan Pérez",
  "membershipType": "Musculacion",
  "amount": 38000.00,
  "paymentDate": "2024-03-20T14:00:00Z",
  "month": 3,
  "year": 2024,
  "notes": "Pago marzo"
}
```

**Response (400 Bad Request):**
```json
{
  "message": "El monto del pago ($45000) no puede superar el precio actual ($38000) para Musculacion"
}
```

### PUT `/api/payments/{id}`
Actualiza un pago existente (monto, fecha, notas).

**Request Body:**
```json
{
  "amount": 38000.00,
  "paymentDate": "2024-03-20T14:00:00Z",
  "notes": "Pago marzo - actualizado"
}
```

### DELETE `/api/payments/{id}`
Elimina un pago.

---

## 💵 Gestión de Precios de Membresías (Solo Admin)

### GET `/api/prices`
Obtiene todos los precios actuales de membresías.

**Response (200 OK):**
```json
[
  {
	"id": 1,
	"type": "Musculacion",
	"price": 38000.00,
	"lastUpdated": "2024-03-20T10:00:00Z"
  },
  {
	"id": 2,
	"type": "Pilates",
	"price": 42000.00,
	"lastUpdated": "2024-03-20T10:00:00Z"
  }
]
```

### GET `/api/prices/{id}`
Obtiene el precio de una membresía específica por ID.

### PUT `/api/prices/{id}`
Actualiza el precio de una membresía (solo admin).

**Request Body:**
```json
{
  "price": 45000.00
}
```

**Validaciones:**
- El precio debe ser mayor a 0

**Response (200 OK):**
```json
{
  "id": 1,
  "type": "Musculacion",
  "price": 45000.00,
  "lastUpdated": "2024-03-21T15:30:00Z"
}
```

---

## 📊 Enums y Estados

### MembershipStatus (Estado de usuario)
```typescript
enum MembershipStatus {
  Pending = "Pending",     // Sin pagos
  Partial = "Partial",     // Pagos parciales
  Paid = "Paid",           // Al día
  Suspended = "Suspended"  // Suspendido
}
```

### MembershipType (Tipo de membresía)
```typescript
enum MembershipType {
  Musculacion = "Musculacion",
  Pilates = "Pilates"
}
```

---

## 🔧 Configuración de Axios (Ejemplo React)

```typescript
// src/api/axios.ts
import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7XXX/api', // Ajusta el puerto
  headers: {
	'Content-Type': 'application/json',
  },
});

export default api;
```

## 📝 Ejemplo de uso en componentes

### Obtener usuarios
```typescript
import api from './api/axios';

const getUsers = async () => {
  try {
	const response = await api.get('/users');
	console.log(response.data);
  } catch (error) {
	console.error('Error fetching users:', error);
  }
};
```

### Crear un pago
```typescript
const createPayment = async (paymentData) => {
  try {
	const response = await api.post('/payments', {
	  userId: paymentData.userId,
	  membershipType: paymentData.membershipType, // "Musculacion" o "Pilates"
	  amount: paymentData.amount,
	  month: paymentData.month,
	  year: paymentData.year,
	  notes: paymentData.notes
	});
	console.log('Pago creado:', response.data);
  } catch (error) {
	if (error.response?.status === 400) {
	  alert(error.response.data.message);
	}
  }
};
```

### Validar admin
```typescript
const validateAdmin = async (password: string) => {
  try {
	const response = await api.post('/admins/validate', { password });
	return response.status === 200;
  } catch (error) {
	return false;
  }
};
```

### Actualizar precio de membresía
```typescript
const updatePrice = async (priceId: number, newPrice: number) => {
  try {
	const response = await api.put(`/prices/${priceId}`, {
	  price: newPrice
	});
	console.log('Precio actualizado:', response.data);
  } catch (error) {
	console.error('Error:', error);
  }
};
```

---

## 🎯 Datos por Defecto (Seed)

Al ejecutar la migración, se crean automáticamente:

### Admin
- **Password:** `IrisAdmin2026!`

### Precios (ARS - Pesos Argentinos)
- **Musculación:** $38.000
- **Pilates:** $42.000

---

## ⚠️ Validaciones Importantes

1. **Pagos duplicados**: No se permite crear dos pagos para el mismo usuario en el mismo mes/año
2. **Límite de monto**: El monto de un pago NO puede exceder el precio configurado para ese tipo de membresía
3. **Email único**: No pueden existir dos usuarios con el mismo email
4. **Fechas**: Todas las fechas están en formato UTC (ISO 8601)

---

## 🚀 Para empezar en el frontend

1. **Instalar Axios:**
   ```bash
   npm install axios
   ```

2. **Verificar CORS:** El backend ya está configurado para `http://localhost:5173`

3. **Puerto del backend:** Verificar en `Properties/launchSettings.json` el puerto HTTPS

4. **Swagger:** Usar `https://localhost:PUERTO/` para probar endpoints interactivamente

---

## 📞 Endpoints Resumen

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| POST | `/api/admins/validate` | Validar contraseña admin |
| GET | `/api/users` | Listar usuarios |
| GET | `/api/users/{id}` | Obtener usuario |
| POST | `/api/users` | Crear usuario |
| PUT | `/api/users/{id}` | Actualizar usuario |
| DELETE | `/api/users/{id}` | Eliminar usuario |
| GET | `/api/payments` | Listar pagos |
| GET | `/api/payments/{id}` | Obtener pago |
| GET | `/api/payments/user/{userId}` | Pagos de usuario |
| GET | `/api/payments/user/{userId}/total` | Total pagado |
| POST | `/api/payments` | Crear pago |
| PUT | `/api/payments/{id}` | Actualizar pago |
| DELETE | `/api/payments/{id}` | Eliminar pago |
| GET | `/api/prices` | Listar precios |
| GET | `/api/prices/{id}` | Obtener precio |
| PUT | `/api/prices/{id}` | Actualizar precio |

---

## ✅ Listo para el frontend

Todo el backend está configurado con:
- ✅ CORS habilitado para Vite
- ✅ Swagger/OpenAPI para testing
- ✅ Validaciones de negocio
- ✅ Datos seed (admin + precios)
- ✅ DTOs limpios
- ✅ Enums serializados como strings

**¡Solo copia esta documentación al chat del frontend y están listos para conectar!** 🎉
