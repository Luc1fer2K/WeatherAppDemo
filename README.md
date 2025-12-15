![Unity](https://img.shields.io/badge/Unity%206-black?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Android-green?logo=android)
![Language](https://img.shields.io/badge/Language-C%23-blue?logo=csharp)
![Status](https://img.shields.io/badge/Status-Portfolio%20Project-brightgreen)

# 🌦 Weather App Demo — Unity

> A clean, production-style Unity application that fetches real-time weather data using device location, designed with **testability, modularity, and mobile readiness** in mind.

This project demonstrates how to build a **non-game, service-driven app inside Unity**, focusing on architecture, API integration, and platform concerns rather than visuals alone.

---

## ✨ Key Highlights

- 📍 **Location-aware weather fetching** (Android-ready)
- 🌐 **REST API integration** with async networking
- 🧱 **Modular, interface-driven architecture** (easy to test & extend)
- 🧪 **Unit-testable core logic**
- 📱 **Mobile-first UI & permissions handling**
- ⚙️ Clean separation between **data, domain, and UI layers**

---

## 🧠 Why This Project Exists

Unity is often used only for games — this project explores using Unity as a **general application framework**, similar to real-world internal tools and utility apps.

The goal was to:
- Build a **maintainable, scalable architecture**
- Handle **real device constraints** (permissions, async APIs)
- Write **production-quality C#**, not prototype code

---

## 🏗 Architecture Overview

```
UI Layer
 └── WeatherUI
       ↓
Application Layer
 └── WeatherAppController
       ↓
Domain / Services
 ├── WeatherApiClient
 ├── ReverseGeocodingClient
 └── LocationServiceWrapper
```

---

## 🛠 Tech Stack

- **Engine**: Unity
- **Language**: C#
- **Networking**: REST APIs
- **Platform**: Android
- **Testing**: Unity Test Framework
- **Version Control**: Git / GitHub

---

## 📂 Project Structure

```
Assets/
 ├── Scripts/
 │    ├── UI/
 │    └── Weather/
 ├── Scenes/
 │    └── MainScene
Packages/
ProjectSettings/
```

---

## 🚀 How to Run

1. Clone the repository
2. Open in **Unity Hub**
3. Load `MainScene`
4. Press **Play**, or build for **Android**

---

## 🧪 Testing

Core logic is written to be **testable outside MonoBehaviours**.  
Editor tests validate API parsing and service behavior.

---

## 📸 Screenshots / Demo

_TODO: Add screenshots or a short GIF here_

---

## 👤 About the Author

**Prikshit Sehrawat**  
Software Engineer / Unity Gameplay & Systems Engineer

---

## ⭐ What This Shows

✔ Clean architecture  
✔ Real-world API usage  
✔ Mobile readiness  
✔ Testability mindset  
✔ Professional Git workflow  

