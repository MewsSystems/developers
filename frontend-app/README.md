# Movie Search App

A modern React application for searching movies, built with cutting-edge technology stack.

## 🎯 Key Features

- **Movie Search**: Intuitive search by title with instant results
- **Detailed Information**: Complete movie information including poster, description, rating, genres
- **Pagination**: Convenient navigation through search results
- **Responsive Design**: Optimized for all devices
- **Modern UI**: Using shadcn/ui components

## 🛠 Technology Stack

- **Frontend**: React 18 + TypeScript
- **Routing**: TanStack Router
- **Styling**: Tailwind CSS + shadcn/ui
- **Build Tool**: Vite
- **API**: The Movie Database (TMDB)
- **Testing**: Jest + React Testing Library
- **Architecture**: Feature-Sliced Design (FSD)

## 📁 Project Structure

The project is organized following Feature-Sliced Design principles:

```
src/
├── entities/movie/          # Business entities
├── features/               # Feature capabilities
│   ├── movie-search/       # Movie search
│   └── pagination/         # Pagination
├── pages/                  # Application pages
├── shared/                 # Reusable components
└── widgets/                # Composite blocks
```

## 🚀 Getting Started

```bash
npm install
npm run dev
```

## ✅ Testing

```bash
npm test
```

The application includes unit tests for all key components and functions.
