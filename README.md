ChatApp.Api — простой чат-сервис на ASP.NET Core

- JWT-аутентификация  
- REST API для пользователей, чатов и сообщений  
- Real-time через SignalR  
- База данных PostgreSQL с EF Core миграциями  

---

## 🛠️ API Методы

### Пользователи

- POST /api/Users/register — регистрация
- POST /api/Users/authenticate — вход  
  → возвращает { token, expiresAt }
- GET  /api/Users/users — список пользователей (JWT)

### Чаты

- GET  /api/Chats — мои чаты (JWT)
- POST /api/Chats — создать чат  
  Тело: { name, userIds } (JWT)

### Сообщения

- GET    /api/Messages/{chatId} — история чата (JWT)
- POST   /api/Messages/{chatId} — отправить сообщение  
  Тело: { text } (JWT)
- PUT    /api/Messages/{messageId} — редактировать сообщение (JWT)
- DELETE /api/Messages/{messageId} — удалить сообщение (JWT)

### SignalR Hub: /hubs/chat

- JoinChat(int chatId)
- LeaveChat(int chatId)
- SendMessage(int chatId, string text)  
  ⇒ ReceiveMessage(MessageDto)

---

## ▶️ Запуск локально

docker compose up --build


Откройте в браузере:

- Регистрация: http://localhost:5000/register.html
- Вход: http://localhost:5000/login.html
- Чат-тест: http://localhost:5000/signalr-test.html

---
