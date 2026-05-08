# Frontend task - Search movie app

## 🚀 Project overview

This is a Frontend application where the user can filter movies just typing in the input text.
When the application is loaded, the user can see some of the most popular movies. Once the user start typing, the application filter them. Additionally, a dark/light theme switch was implemented to improve the user experience.

This proyect was created using **React**. For styling, **Styled-Component** was chosen. Additionally, **Vite** was used because it simplifies setup and development by offering minimal configuration by default. It also integrates **TanStack React Query** for data retrieval and caching, **Zustand** for managing the application's global state, and **React Router** for page navigation.

---

## 📦 Dependencies

### **Main dependencies**

- **react-router**: In-app navigation management, facilitating page management
- **@tanstack/react-query**: Data fetching, caching, and synchronization
- **zustand**: Lightweight state management library
- **axios**: Simplified and powerful HTTP client
- **react-icons**: Popular icon library for React components
- **styled-components**: CSS-in-JS styling for component-based design

### **Development dependencies**

- **vite**: A rapid build tool that improves development speed
- **typescript**: Adds typing, improving code reliability and maintainability
- **@vitejs/plugin-react**: Optimize React applications within the Vite ecosystem
- **eslint & eslint-plugin-react-hooks & eslint-plugin-react-refresh**: Ensure code quality by applying good practices and detecting potential errors
- **@eslint/js** & **typescript-eslint**: Improves ESLint support for TypeScript projects
- **@types/react & @types/react-dom**: Provides TypeScript type definitions for React

---

## 🛠 How to Execute the Project locally

### **1️⃣ Clone the repository**

The first step is cloning the repository. Then you should move to the search-movie-app. You will find it inside the path **"search-movie-app/jobs/Frontend"**

```sh
git clone https://github.com/gastoncolaneri/mews-frontend-task.git

cd search-movie-app/jobs/Frontend/search-movie-app
```

### **2️⃣ Install dependencies**

Before running the project, make sure you have **Node.js** installed. Then, install the dependencies by running the command:

```sh
npm install
```

### **3️⃣Setup environment variables**

Create a .env file at the root of the project and add your API token for [TMDb](https://www.themoviedb.org/):

```sh
VITE_API_TOKEN=your_api_token_here
```

Note: The token is injected into the app using Vite's environment variables. Make sure your variable is prefixed with VITE\_.

### **4️⃣ Run in Development Mode**

To start a development server, run the following command:

```sh
npm run dev
```

This will start the application on **`http://localhost:5173`** (or a different port if configured).

To test this project without having to clone the repository, you can use **[this link](https://search-movie-app-task.vercel.app/)**.

---

## 🧪 Testing

To run the tests:

```sh
npm run test
```

To run in watch mode:

```sh
npm run test:watch
```

## 💡 Improvements

- Implement more functionalities, like filters by genre, release date or votes
- Retrieve default movie list, such as "Top rated" or "Upcoming"
- Add more information in the movie details
- Implement the capability to add to favorites
- Add more test scenarios
- Add E2E testing
