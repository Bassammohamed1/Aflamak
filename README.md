# 🎬 Aflamak – Streaming Web Application

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-Professional-green?logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red?logo=microsoftsqlserver)
![SignalR](https://img.shields.io/badge/RealTime-NotApplicable-lightgrey)
![License](https://img.shields.io/badge/License-MIT-yellow)
![Tests](https://img.shields.io/badge/Tests-xUnit%20%2B%20FakeItEasy-success)

> A streaming platform for **films, TV shows, and episodes**, with a strong focus on **Arabic content** and **Ramadan specials**.

---

## 📌 Features

### 🎥 User Experience
- Homepage with latest & top-rated content.
- Dedicated sections for **Arabic & Ramadan** titles.
- Advanced filtering by **Year, Language, Category, Type**.
- Bilingual search (Arabic & English) for actors, producers, films, and shows.
- Detailed views: cast & crew, related titles, like/dislike counts.
- TV show breakdown by parts & episodes.

### 🛠 Admin Panel
- Manage **Actors, Producers, Films, TV Shows, Categories, Roles, and Users**.
- Role-based access control.

---

## 🧪 Testing & Quality Assurance
- **xUnit** unit tests for core features.
- **InMemory Database** for safe DB testing.
- **FakeItEasy** for mocking dependencies.

---

## 🛠 Tech Stack
| Layer | Technology |
|-------|------------|
| **Backend** | C#, ASP.NET Core MVC |
| **Database** | SQL Server, EF Core |
| **Testing** | xUnit, FakeItEasy |
| **Frontend** | HTML, CSS, JavaScript |

---

## 📐 Architecture

[Client] → [ASP.NET Core MVC] → [Business Logic] → [Data Access Layer] → [SQL Server]

*(3-Tier Architecture ensures separation of concerns and testability.)*

---

## 🌐 Live Demo

🔗 [Aflamak Live](https://aflamak.runasp.net/)

---

## 📄 License

MIT License
