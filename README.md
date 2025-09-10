# Movie Search Application

## Project Overview

A modern movie search application built with cutting-edge technologies and architectural patterns. This project demonstrates expertise in React, TypeScript, Feature-Sliced Design architecture, and comprehensive testing practices.

## 🚀 Technology Stack

- **Frontend Framework**: React 18 + TypeScript
- **Routing**: TanStack Router
- **State Management**: TanStack Query (React Query)
- **Styling**: Tailwind CSS
- **Build Tool**: Vite
- **Testing**: Vitest + React Testing Library
- **Code Quality**: ESLint + Prettier
- **Architecture**: Feature-Sliced Design (FSD)
- **UI Components**: ShadCn components for fast prototyping

## 🏗️ Architecture

The project follows **Feature-Sliced Design** methodology, ensuring:

- **Scalability** - Easy addition of new features
- **Reusability** - Components and logic can be reused across the application
- **Testability** - Each layer can be tested in isolation
- **Maintainability** - Clear structure for developers

### Project Structure

```
src/
├── app/           # Application configuration
├── pages/         # Application pages
├── widgets/       # Composite page blocks
├── features/      # Business features
├── entities/      # Business entities
└── shared/        # Reusable resources
```

## ✨ Features

### Core Functionality

- 🔍 **Movie Search** by title using TMDB API, Abort controller and debounced search
- 📱 **Responsive Design** for all devices
- 📄 **Pagination** of search results
- 🎬 **Detailed Movie Information** with comprehensive data
- ⭐ **Ratings and Reviews** with color-coded indicators
- 🖼️ **Image Handling** with fallbacks for missing posters

## 🛠️ Installation & Setup

### Prerequisites

- Node.js 18+
- npm or yarn
- TMDB API key

### Installation

```bash
# Clone the repository
git clone <repository-url>
cd mews-test-task

# Install dependencies
npm install
```

### Development

```bash
npm run dev
```

### Production Build

```bash
npm run build
npm run preview  # Preview the build
```

## 📋 Code Quality

### Linting & Formatting

```bash
npm run lint      # Run ESLint checks
npm run lint:fix  # Auto-fix issues
npm run format    # Format with Prettier
```
